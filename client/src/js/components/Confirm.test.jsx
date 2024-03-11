import { fireEvent, render, screen } from "@testing-library/react";
import Confirm from "./Confirm";

let hasConfirmed;
let hasCancelled;

const onConfirm = () => {
  hasConfirmed = true;
};

const onCancel = () => {
  hasCancelled = true;
};

beforeEach(() => {
  hasConfirmed = false;
  hasCancelled = false;
});

describe("Confirm performs correct actions", () => {
  test("Confirm executes onConfirm properly", () => {
    // Arrange
    render((
      <Confirm onConfirm={onConfirm}></Confirm>
    ));

    // Act
    fireEvent(screen.getByText("Yes"), new MouseEvent("click", { bubbles: true, cancelable: true }));

    // Assert
    expect(hasConfirmed).toBe(true);
    expect(hasCancelled).toBe(false);
  });

  test("Confirm executes onCancel properly", () => {
    // Arrange
    render((
      <Confirm onConfirm={onConfirm} onCancel={onCancel}></Confirm>
    ));

    fireEvent(screen.getByText("No"), new MouseEvent("click", { bubbles: true, cancelable: true }));

    // Assert
    expect(hasConfirmed).toBe(false);
    expect(hasCancelled).toBe(true);
  });
});
