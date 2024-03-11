import { render, screen } from "@testing-library/react";
import Countdown from "./Countdown";

describe("Countdown component display", () => {
  test("Countdown correctly displays time greater than 1min", () => {
    // Arrange
    const time = 61;

    // Act
    render(<Countdown time={time} />);

    // Assert
    expect(screen.getByText("1m 01s")).toBeVisible();
  });

  test("Countdown correctly displays time less than 1min", () => {
    // Arrange
    const time = 59;

    // Act
    render(<Countdown time={time} />);

    // Assert
    expect(screen.getByText("59s")).toBeVisible();
  });

  test("Countdown correctly displays time less than 10sec", () => {
    // Arrange
    const time = 9;

    // Act
    render(<Countdown time={time} />);

    // Assert
    expect(screen.getByText("09s")).toBeVisible();
  });
});

describe("Countdown component counting down", () => {
  test("Countdown counts down correctly", () => {
    // Arrange
    jest.useFakeTimers();
    render(<Countdown time={5} />); 
    
    expect(screen.getByText("05s")).toBeVisible();

    // Act
    jest.advanceTimersToNextTimer(2);

    // Assert
    expect(screen.getByText("03s")).toBeVisible();
  });

  test("Countdown executes callback when countdown finishes", () => {
    // Arrange
    jest.useFakeTimers();
    let finished = false;
    const onEnd = () => {
      finished = true;
    };
    render(<Countdown time={1} onEnd={onEnd} />); 
    
    // Act
    jest.advanceTimersToNextTimer(2);

    // Assert
    expect(finished).toBe(true);
  });
});
