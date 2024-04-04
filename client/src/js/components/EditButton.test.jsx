import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import EditButton from "./EditButton";

describe("EditButton display", () => {
  test("EditButton displays properly for view", () => {
    // Arrange and Act
    render((
      <EditButton
        view={true}
        name="Test"
        hide={false}
        onClick={() => {}}
      />
    ));

    const expectedText = "View Test";

    // Assert
    expect(screen.queryAllByTitle(expectedText)).toHaveLength(1);
    const buttonEl = screen.getByTitle(expectedText);
    expect(buttonEl).not.toHaveClass("d-none");
  });

  test("EditButton displays properly for edit", () => {
    // Arrange and Act
    render((
      <EditButton
        view={false}
        name="Test"
        hide={false}
        onClick={() => {}}
      />
    ));

    const expectedText = "Edit Test";

    // Assert
    expect(screen.queryAllByTitle(expectedText)).toHaveLength(1);
    const buttonEl = screen.getByTitle(expectedText);
    expect(buttonEl).not.toHaveClass("d-none");
  });

  test("EditButton properly hides button", () => {
    // Arrange and Act
    render((
      <EditButton
        view={false}
        name="Test"
        hide={true}
        onClick={() => {}}
      />
    ));

    const expectedText = "Edit Test";

    // Assert
    const buttonEl = screen.getByTitle(expectedText);
    expect(buttonEl).toHaveClass("d-none");
  });
});

describe("EditButton behaviour", () => {
  test("EditButton onClick works properly", async () => {
    const user = userEvent.setup();
    
    // Arrange
    let clicked = false;
    const onClick = () => {
      clicked = true;
    };

    render((
      <EditButton
        view={false}
        name="Test"
        hide={false}
        onClick={onClick}
      />
    ));

    // Act
    await user.click(screen.getByTitle("Edit Test"));

    // Assert
    expect(clicked).toBe(true);
  });
});
