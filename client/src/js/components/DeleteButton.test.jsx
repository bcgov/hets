import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import DeleteButton from "./DeleteButton";

describe("DeleteButton display", () => {
  test("DeleteButton displays normally", () => {
    // Arrange and Act
    render((
      <DeleteButton
        name="Test"
        onConfirm={() => {}}
      />
    ));

    const deleteButtonEl = screen.getByTitle("Delete Test");

    // Assert
    expect(deleteButtonEl).not.toHaveClass("d-none");
  });

  test("DeleteButton does not show when hidden", () => {
    // Arrange and Act
    render((
      <DeleteButton
        name="Test"
        hide={true}
        onConfirm={() => {}}
      />
    ));

    const deleteButtonEl = screen.getByTitle("Delete Test");

    // Assert
    expect(deleteButtonEl).toHaveClass("d-none");
  });
});

describe("DeleteButton behaviour", () => {
  test("DeleteButton onConfirm works properly", async () => {
    const user = userEvent.setup();
    
    // Arrange
    let confirmDelete = false;
    const onConfirm = () => {
      confirmDelete = true;
    };

    render((
      <DeleteButton
        name="Test"
        onConfirm={onConfirm}
      />
    ));

    const deleteButtonEl = screen.getByTitle("Delete Test");

    // Act
    await user.click(deleteButtonEl);
    expect(screen.getByText("Yes")).toBeInTheDocument();
    expect(screen.getByText("No")).toBeInTheDocument();
    const confirmYesButtonEl = await screen.findByText("Yes");
    await user.click(confirmYesButtonEl);

    // Assert
    expect(confirmDelete).toBe(true);
  });

  test("DeleteButton onCancel works properly", async () => {
    const user = userEvent.setup();

    // Arrange
    let confirmDelete = false;
    const onConfirm = () => {
      confirmDelete = true;
    };

    render((
      <DeleteButton
        name="Test"
        onConfirm={onConfirm}
      />
    ));

    const deleteButtonEl = screen.getByTitle("Delete Test");

    // Act
    await user.click(deleteButtonEl);
    expect(screen.getByText("Yes")).toBeInTheDocument();
    expect(screen.getByText("No")).toBeInTheDocument();
    const confirmNoButtonEl = await screen.findByText("No");
    await user.click(confirmNoButtonEl);

    // Assert
    expect(confirmDelete).toBe(false);
  });

  test("DeleteButton does not do anything when disabled", async () => {
    const user = userEvent.setup();
    
    // Arrange
    let confirmDelete = false;
    const onConfirm = () => {
      confirmDelete = true;
    };

    render((
      <DeleteButton
        name="Test"
        disabled={true}
        onConfirm={onConfirm}
      />
    ));

    const deleteButtonEl = screen.getByTitle("Delete Test");

    // Act
    await user.click(deleteButtonEl);

    // Assert
    expect(screen.queryByText("Yes")).toBeNull();
    expect(screen.queryByText("No")).toBeNull();
    expect(confirmDelete).toBe(false);
  });
});