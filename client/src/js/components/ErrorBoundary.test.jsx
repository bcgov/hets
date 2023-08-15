import { render, screen } from "@testing-library/react";
import ErrorBoundary from "./ErrorBoundary";

const mockedConsoleLog = jest.spyOn(console, "log");
mockedConsoleLog.mockImplementation(() => {});

const mockedConsoleError = jest.spyOn(console, "error");
mockedConsoleError.mockImplementation(() => {});

const TestComponent = () => {
  throw new Error("Test Error");
};

describe("ErrorBoundary capture", () => {
  test("ErrorBoundary captures child components errors", () => {
    // Arrange and Act
    render((
      <ErrorBoundary>
        <TestComponent />
      </ErrorBoundary>
    ));

    // Assert
    expect(screen.queryAllByText(/something went wrong/i)).toHaveLength(1);
    expect(mockedConsoleLog).toHaveBeenCalledWith("error captured");
  });
});