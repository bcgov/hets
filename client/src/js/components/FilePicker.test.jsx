import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import FilePicker from "./FilePicker";

describe("FilePicker file selection", () => {
  test("FilePicker selects file properly", async () => {
    const user = userEvent.setup();

    // Arrange
    const expectedFiles = [
      new File(["test1"], "test1.png", { type: "image/png" }),
      new File(["test2"], "test2.png", { type: "image/png" }),
      new File(["test3"], "test3.png", { type: "image/png" }),
    ];
    const selectedFiles = [];
    const onFilesSelected = (files) => {
      selectedFiles.push.apply(selectedFiles, files);
    };

    render((
      <FilePicker
        id="file-picker"
        className="file-picker"
        label="File Picker"
        onFilesSelected={onFilesSelected}
      />
    ));

    const pickerEl = screen.getByLabelText(/File Picker/i);

    // Act
    await user.upload(pickerEl, expectedFiles);

    // Assert
    expect(selectedFiles).toHaveLength(expectedFiles.length);
    expect(selectedFiles[0]).toStrictEqual(expectedFiles[0]);
    expect(selectedFiles[1]).toStrictEqual(expectedFiles[1]);
    expect(selectedFiles[2]).toStrictEqual(expectedFiles[2]);
  });
});