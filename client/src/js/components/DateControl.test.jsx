import { fireEvent, render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import DateControl from "./DateControl";

const mockConsoleLog = jest.spyOn(console, "log");
mockConsoleLog.mockImplementation(() => {});

describe("DateControl component display", () => {
  test("DateControl displays correct date and format", () => {
    // Arrange
    const format = "YYYY-MM-DD";
    const dateStr = "2023-01-01";

    // Act
    render((
      <DateControl
        id="testdate"
        date={dateStr}
        format={format}
        label="Test Date"
        title="Test Date"
      />
    ));

    // Assert
    expect(screen.getByDisplayValue(dateStr)).toBeVisible();
  });
});

describe("DateControl component updating dates", () => {
  test("DateControl updates date correctly", async () => {
    // Arrange
    const format = "YYYY-MM-DD";
    const dateStr = "2023-01-01";
    const id = "testdate";
    let currDateStr = dateStr;
    let currId = id;
    const onChange = (newDateStr, componentId) => {
      currDateStr = newDateStr;
      currId = componentId;
    };
    const updateState = (state) => {
      currDateStr = state[id];
    };
    const updatedDateStr = "2023-01-02";

    render((
      <DateControl
        id={currId}
        date={currDateStr}
        format={format}
        label="Test Date"
        title="Test Date"
        onChange={onChange}
        updateState={updateState}
      />
    ));

    const dateInput = screen.getByDisplayValue(currDateStr);

    // Act
    fireEvent.change(dateInput, { target: { value: updatedDateStr } });
    
    // Assert
    expect(currDateStr).toEqual(updatedDateStr);
  });

  test("DateControl date selection works correctly", async () => {
    const user = userEvent.setup();

    // Arrange
    const format = "YYYY-MM-DD";
    const dateStr = "2022-01-01";
    const id = "testdate";
    let currDateStr = dateStr;
    let currId = id;
    const onChange = (newDateStr, componentId) => {
      currDateStr = newDateStr;
      currId = componentId;
    };
    const updateState = (state) => {
      currDateStr = state[id];
    };
    const updatedDateStr = "2023-03-02";

    render((
      <DateControl
        id={currId}
        date={currDateStr}
        format={format}
        label="Test Date"
        title="Test Date"
        onChange={onChange}
        updateState={updateState}
      />
    ));

    const dateInput = screen.getByDisplayValue(currDateStr);

    // Act
    await user.click(dateInput);
    const monthYearSel = await screen.findByText("January 2022");
    await user.click(monthYearSel);
    const yearSel = await screen.findByText("2022");
    await user.click(yearSel);
    const yearEl = await screen.findByText("2023");
    await user.click(yearEl);
    const monthEl = await screen.findByText("Mar");
    await user.click(monthEl);
    const dayEl = await screen.findAllByText("2");
    await user.click(dayEl[0]);
    
    // Assert
    expect(currDateStr).toEqual(updatedDateStr);
  });
});
