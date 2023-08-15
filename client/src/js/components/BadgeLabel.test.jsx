import { render, screen } from "@testing-library/react";
import BadgeLabel from "./BadgeLabel";

describe("BadgeLabel displays properly", () => {
  test("BadgeLabel displays text properly", () => {
    // Arrange
    const text = "Test";
  
    // Act
    render((
      <BadgeLabel>{text}</BadgeLabel>
    ));

    // Assert
    expect(screen.getByText(text)).toBeVisible();
  });
});