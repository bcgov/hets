import { render, screen } from "@testing-library/react";
import ColDisplay from "./ColDisplay";

describe("ColDisplay displays properly", () => {
  test("ColDisplay display label and text properly", () => {
    // Arrange
    const label = "Test Label";
    const text = "Test text";

    // Act
    render((
      <ColDisplay label={label}>{text}</ColDisplay>
    ));

    // Assert
    expect(screen.getByText(label)).toBeVisible();
    expect(screen.getByText(text)).toBeVisible();
  });
});