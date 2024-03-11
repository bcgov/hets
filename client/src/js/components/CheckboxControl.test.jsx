import { fireEvent, render, screen } from "@testing-library/react";
import CheckboxControl from "./CheckboxControl";

const initialState = {};

let state;
let id;

const updateState = (newState) => {
  state = { ...newState };
};
const onChange = (e) => {
  id = e.target.id;
};

beforeEach(() => {
  state = { ...initialState };
  id = undefined;
});

describe("CheckboxControl updates correctly", () => {
  test("CheckboxControl toggles state", () => {
    // Arrange
    const label = "Checkbox1";
    render(<CheckboxControl label={label} updateState={updateState} onChange={onChange} />);
    const input = screen.getByLabelText(label);

    // Act
    fireEvent(input, new MouseEvent("click", { bubbles: true, cancelable: true }));

    // Assert
    const expectedId = `checkboxControl-${label}`;
    expect(id).toBe(expectedId);
    expect(state[id]).toBe(true);

    fireEvent(input, new MouseEvent("click", { bubbles: true, cancelable: true }));

    expect(id).toBe(expectedId);
    expect(state[id]).toBe(false);
  });
});