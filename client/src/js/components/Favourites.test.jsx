import { setupServer } from "msw/lib/node";
import { rest } from "msw";

import { keycloak } from "../Keycloak";
import { setupStore } from "../store";
import * as Action from "../actionTypes";
import { getCurrentUser } from "../mock/api/getCurrentUser";
import { renderWithProviders } from "../renderWithProviders";
import Favourites from "./Favourites";
import { getFavourites } from "../mock/api/getFavourites";
import { screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";

// Mock Keycloak service
const mockKeycloakUpdateToken = jest.spyOn(keycloak, "updateToken");
mockKeycloakUpdateToken.mockResolvedValue(Promise.resolve(false));

const server = setupServer(
  rest.post("/api/users/current/favourites/:favouriteId/delete", async (req, res, ctx) => {
    const id = Number(req.params.favouriteId);
    const fav = Object.values(getFavourites().customFav).find(f => f.id === id);
    if (!fav) {
      return res(ctx.status(404));
    }
    return res(ctx.json({
      data: fav,
    }));
  }),
  rest.post("/api/users/current/favourites", async (req, res, ctx) => {
    const reqBody = await req.json();
    const favName = reqBody.name;
    const resFav = Object.values(getFavourites().customFav).find(f => f.name === favName);
    if (!resFav) {
      return res(ctx.status(404));
    }
    return res(ctx.json({
      data: resFav,
    }));
  }),
  rest.put("/api/users/current/favourites", async (req, res, ctx) => {
    const reqBody = await req.json();
    const { id, name, isDefault } = reqBody;
    const fav = Object.values(getFavourites().customFav).find(f => f.id === id);
    if (!fav) {
      return res(ctx.status(404));
    }

    const resFav = {
      ...fav,
      name,
      isDefault,
    };
    return res(ctx.json({
      data: resFav,
    }));
  }),
);

let store;

beforeAll(() => {
  server.listen();
});

beforeEach(() => {
  store = setupStore();

  const authorizedUser = getCurrentUser().data;
  authorizedUser.hasPermission = (_) => true;

  store.dispatch({
    type: Action.UPDATE_CURRENT_USER,
    user: authorizedUser,
  });
})

afterEach(() => {
  server.resetHandlers();
});

afterAll(() => {
  server.close();
});

const setup = (component, store) => ({
  user: userEvent.setup(),
  ...renderWithProviders(component, { store }),
});

describe("Favourites component", () => {
  test("Favourites show empty favourites list properly", async () => {
    const favourites = {};
    let searchParams = {};
    const onSelect = (favourite) => {
      searchParams = JSON.parse(favourite.value);
    };

    const { user } = setup((
      <Favourites
        id="faves-dropdown"
        type="customFav"
        favourites={favourites}
        data={searchParams}
        onSelect={onSelect}
      />
    ), store);

    const favButton = await screen.findByText("Favourites");
    await user.click(favButton);

    const displayedFavEls = screen.queryAllByText(/Custom Fav \d+/i);
    expect(displayedFavEls).toHaveLength(0);
  });

  test("Favourites show multiple favourites properly", async () => {
    const favourites = getFavourites().customFav;
    let searchParams = {};
    const onSelect = (favourite) => {
      searchParams = JSON.parse(favourite.value);
    };

    const { user } = setup((
      <Favourites
        id="faves-dropdown"
        type="customFav"
        favourites={favourites}
        data={searchParams}
        onSelect={onSelect}
      />
    ), store);

    const favButton = await screen.findByText("Favourites");
    await user.click(favButton);

    const displayedFavEls = await screen.findAllByText(/Custom Fav \d+/i);
    expect(displayedFavEls).toHaveLength(3);
  });

  test("Favourites can be selected properly", async () => {
    const favourites = getFavourites().customFav;
    let searchParams = {};
    const onSelect = (favourite) => {
      searchParams = JSON.parse(favourite.value);
    };

    const { user } = setup((
      <Favourites
        id="faves-dropdown"
        type="customFav"
        favourites={favourites}
        data={searchParams}
        onSelect={onSelect}
      />
    ), store);

    const favButton = await screen.findByText("Favourites");
    await user.click(favButton);
    const displayedFavEls = await screen.findAllByText(/Custom Fav \d+/i);
    await user.click(displayedFavEls[0]);

    expect(searchParams).toEqual(JSON.parse(favourites[1].value));
  });

  test("Favourites can be added successfully", async () => {
    const sampleFavourite = getFavourites().customFav[1];
    const favourites = {};
    let searchParams = JSON.parse(sampleFavourite.value);
    const onSelect = (favourite) => {
      searchParams = JSON.parse(favourite.value);
    };
    
    const { user } = setup((
      <Favourites
        id="faves-dropdown"
        type="customFav"
        favourites={favourites}
        data={searchParams}
        onSelect={onSelect}
      />
    ), store);

    const favButton = await screen.findByText("Favourites");
    await user.click(favButton);
    const addFavButton = await screen.findByText("Favourite Current Selection");
    await user.click(addFavButton);
    const favNameTextField = await screen.findByLabelText(/Name/i);
    await user.type(favNameTextField, sampleFavourite.name);
    const saveButton = await screen.findByText("Save");
    await user.click(saveButton);
    await waitFor(() => {
      expect(store.getState().models.favourites.customFav).toEqual({ 1: sampleFavourite });
    });
  });

  test("Favourites can be edited successfully", async () => {
    const sampleFavourite = getFavourites().customFav[1];
    const favourites = { [sampleFavourite.id]: sampleFavourite };
    let searchParams = { customSearchParam: "Custom Search Arg" };
    const onSelect = (favourite) => {
      searchParams = JSON.parse(favourite.value);
    };

    const newFavName = "New Fav Name";
    const newIsDefault = !sampleFavourite.isDefault;

    store.dispatch({
      type: Action.ADD_FAVOURITE,
      favourite: sampleFavourite,
    });
    
    const { user } = setup((
      <Favourites
        id="faves-dropdown"
        type="customFav"
        favourites={favourites}
        data={searchParams}
        onSelect={onSelect}
      />
    ), store);

    expect(store.getState().models.favourites.customFav[1]).toEqual(sampleFavourite);
    
    const favButton = screen.getByText("Favourites");
    await user.click(favButton);
    const editFavButton = screen.getByTitle("Edit Favourite");
    await user.click(editFavButton);
    const favNameTextField = screen.getByLabelText(/Name/i);
    await user.clear(favNameTextField);
    await user.type(favNameTextField, newFavName, { initialSelectionStart: 0, initialSelectionEnd: sampleFavourite.name.length });
    const favDefaultCheckBox = screen.getByLabelText(/Default/i);
    await user.click(favDefaultCheckBox);
    const saveButton = screen.getByText("Save");
    await user.click(saveButton);

    // Edit should only change name and isDefault of favourite, not value
    await waitFor(() => {
      expect(store.getState().models.favourites.customFav[1].name).toBe(newFavName);
    });
    expect(store.getState().models.favourites.customFav[1].isDefault).toBe(newIsDefault);
    expect(store.getState().models.favourites.customFav[1].value).toBe(sampleFavourite.value); // value should stay the same
  });

  test("Favourites can be deleted successfully", async () => {
    const sampleFavourite = getFavourites().customFav[1];
    const favourites = { [sampleFavourite.id]: sampleFavourite };
    let searchParams = JSON.parse(sampleFavourite.value);
    const onSelect = (favourite) => {
      searchParams = JSON.parse(favourite.value);
    };

    store.dispatch({
      type: Action.ADD_FAVOURITE,
      favourite: sampleFavourite,
    });
    
    const { user } = setup((
      <Favourites
        id="faves-dropdown"
        type="customFav"
        favourites={favourites}
        data={searchParams}
        onSelect={onSelect}
      />
    ), store);
    
    const favButton = screen.getByText("Favourites");
    await user.click(favButton);
    const deleteFavButton = screen.getByTitle("Delete Favourite");
    await user.click(deleteFavButton);
    const confirmDeleteButton = screen.getByText(/Yes/i);
    await user.click(confirmDeleteButton);

    await waitFor(() => {
      expect(store.getState().models.favourites.customFav).toEqual({});
    });
  });
});