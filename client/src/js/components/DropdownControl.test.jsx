import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import DropdownControl from "./DropdownControl";

describe("DropdownControl display", () => {
  test("DropdownControl displays selected item properly", () => {
    // Arrange and Act
    const dropdownId = "dropdown";
    const items = [
      { id: 1, itemName: "Item 1" },
      { id: 2, itemName: "Item 2" },
      { id: 3, itemName: "Item 3" },
    ];
    let selectedItem = items[0];

    render((
      <DropdownControl
        id={dropdownId}
        items={items}
        selectedId={selectedItem.id}
        fieldName="itemName"
      />
    ));

    // Assert
    expect(screen.queryAllByText(/Item [0-9]+/i)).toHaveLength(1);
    expect(screen.queryAllByText(selectedItem.itemName)).toHaveLength(1);
  });

  test("DropdownControl displays list of selections", async () => {
    const user = userEvent.setup();

    // Arrange
    const dropdownId = "dropdown";
    const items = [
      { id: 1, itemName: "Item 1" },
      { id: 2, itemName: "Item 2" },
      { id: 3, itemName: "Item 3" },
    ];
    render((
      <DropdownControl
        id={dropdownId}
        items={items}
        fieldName="itemName"
      />
    ));

    expect(screen.queryAllByText(/Item [0-9]+/i)).toHaveLength(0);
    const dropdownEl = screen.getByText("Select item");
    
    // Act
    await user.click(dropdownEl);

    // Assert
    expect(screen.queryAllByText(/Item [0-9]+/i)).toHaveLength(items.length);
  });
});

describe("DropdownControl behaviour", () => {
  test("DropdownControl properly selects options", async () => {
    const user = userEvent.setup();

    // Arrange
    const dropdownId = "dropdown";
    const items = [
      { id: 1, itemName: "Item 1" },
      { id: 2, itemName: "Item 2" },
      { id: 3, itemName: "Item 3" },
    ];
    let selectedItem = items[0];
    const updateState = (newItem) => {
      const selectedId = newItem[dropdownId];
      const item = items.find(item => item.id === selectedId) ?? { itemName: "Not found" };
      selectedItem = {
        id: selectedId,
        itemName: item.itemName,
      };
    };
    const updatedItem = items[1];

    render((
      <DropdownControl
        id={dropdownId}
        items={items}
        selectedId={selectedItem.id}
        fieldName="itemName"
        updateState={updateState}
      />
    ));

    const dropdownEl = screen.getByText(selectedItem.itemName);

    // Act
    await user.click(dropdownEl);
    await user.click(screen.getByText(updatedItem.itemName));

    // Assert
    expect(selectedItem).toEqual(updatedItem);
    expect(screen.queryAllByText(/Item [0-9]+/i)[0]).toHaveTextContent(updatedItem.itemName);
  });
});
