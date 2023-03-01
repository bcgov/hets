// Import currently not working - update needed in pipeline
import {
  toString,
  dasherize, 
  plural,
  titleCase,
  concat,
  firstLastName,
  lastFirstName,
  isBlank,
  notBlank,
  isBlankOrZero,
  padLeft,
  onlyLetters,
  formatCurrency,
} from "./string";

describe("toString", () => {
  test("null and undefined becomes empty string", () => {
    expect(toString(null)).toBe("");
    expect(toString(undefined)).toBe("");
  });

  test("primitive value becomes a string", () => {
    expect(toString(4)).toBe("4");
    expect(toString(4.4)).toBe("4.4");
    expect(toString("4")).toBe("4");
    expect(toString(true)).toBe("true");
  });
});

describe("dasherize", () => {
  test("adding dashes in front of capital letters", () => {
    expect(dasherize("fooBarBaz")).toBe("foo-bar-baz");
  });

  test("replacing underscores with dashes", () => {
    expect(dasherize("foo_bar_baz")).toBe("foo-bar-baz");
  });

  test("replacing whitespaces with dashes", () => {
    expect(dasherize("foo bar baz")).toBe("foo-bar-baz");
  });

  test("replacing combinations of characters with dashes", () => {
    expect(dasherize("foo Bar_-__Baz")).toBe("foo-bar-baz");
  });
});

describe("titleCase", () => {
  test("capital letter for first character of string", () => {
    expect(titleCase("foo bar baz")).toBe("Foo Bar Baz");
  });
});

describe("plural", () => {
  test("not showing plural for singular item", () => {
    expect(plural(1, "cat", "cats")).toBe("cat");
  });

  test("showing plural for zero or multiple items", () => {
    expect(plural(0, "cat", "cats")).toBe("cats");
    expect(plural(2, "cat", "cats")).toBe("cats");
  });
});

describe("concat", () => {
  test("concatenation of two empty strings", () => {
    expect(concat("", null, ",")).toBe("");
  });

  test("concatenation of a string and an empty string", () => {
    expect(concat("a", null, ",")).toBe("a");
    expect(concat(undefined, "b", ",")).toBe("b");
  });

  test("concatenation of two strings", () => {
    expect(concat("a", "b", ",")).toBe("a,b");
    expect(concat("a", "b")).toBe("a b");
  });
});

describe("firstLastName", () => {
  test("displaying empty string for missing both first and last names", () => {
    expect(firstLastName()).toBe("");
  });

  test("displaying only first name", () => {
    expect(firstLastName("Jane")).toBe("Jane");
  });

  test("displaying only last name", () => {
    expect(firstLastName("", "Doe")).toBe("Doe");
  });

  test("displaying both first and last name", () => {
    expect(firstLastName("Jane", "Doe")).toBe("Jane Doe");
  });
});

describe("lastFirstName", () => {
  test("displaying empty string for missing both first and last names", () => {
    expect(lastFirstName()).toBe("");
  });

  test("displaying only first name", () => {
    expect(lastFirstName("", "Jane")).toBe("Jane");
  });

  test("displaying only last name", () => {
    expect(lastFirstName("Doe")).toBe("Doe");
  });

  test("displaying both first and last name", () => {
    expect(lastFirstName("Doe", "Jane")).toBe("Doe, Jane");
  });
});

describe("isBlank", () => {
  test("empty string is blank", () => {
    expect(isBlank()).toBe(true);
    expect(isBlank("    ")).toBe(true);
    expect(isBlank(null)).toBe(true);
  });

  test("non empty string is not blank", () => {
    expect(isBlank("a")).toBe(false);
  });
});

describe("isBlankOrZero", () => {
  test("empty string is blank or zero", () => {
    expect(isBlankOrZero()).toBe(true);
    expect(isBlankOrZero("    ")).toBe(true);
    expect(isBlankOrZero(null)).toBe(true);
  });

  test("zero string is blank or zero", () => {
    expect(isBlankOrZero("0")).toBe(true);
  });

  test("zero is blank or zero", () => {
    expect(isBlankOrZero(0)).toBe(true);
  });

  test("non-zero string is not blank nor zero", () => {
    expect(isBlankOrZero("1")).toBe(false);
    expect(isBlankOrZero("a")).toBe(false);
  });

  test("non-zero number is not blank nor zero", () => {
    expect(isBlankOrZero(1)).toBe(false);
  });
});

describe("notBlank", () => {
  test("empty string is blank", () => {
    expect(notBlank()).toBe(false);
    expect(notBlank("    ")).toBe(false);
    expect(notBlank(null)).toBe(false);
  });

  test("non empty string is not blank", () => {
    expect(notBlank("a")).toBe(true);
  });
});

describe("padLeft", () => {
  test("proper padding for string shorter than threshold", () => {
    expect(padLeft("1", "0", 4)).toBe("0001");
  });

  test("no padding for strings with length greater than threshold", () => {
    expect(padLeft("11111", "0", 4)).toBe("11111");
    expect(padLeft("1111", "0", 4)).toBe("1111");
  });

  test("showing original string for invalid or empty padding characters and invalid thresholds", () => {
    expect(padLeft("1", null, 4)).toBe("1");
    expect(padLeft("1", "00", 4)).toBe("1");
    expect(padLeft("1", "", 4)).toBe("1");
    expect(padLeft("1", "0", -1)).toBe("1");
    expect(padLeft("1", "0", "4")).toBe("1");
  });

  test("showing empty string for invalid input strings", () => {
    expect(padLeft(null, "0", 4)).toBe("");
    expect(padLeft(undefined, "0", 4)).toBe("");
  });
});

describe("onlyLetters", () => {
  test("string contains only letters", () => {
    expect(onlyLetters("abcdeABCDE")).toBe(true);
    expect(onlyLetters(" abcdeABCDE   ")).toBe(true);
  });

  test("string contains non-letter characters", () => {
    expect(onlyLetters("abc1def")).toBe(false);
    expect(onlyLetters(" abc Def  ")).toBe(false);
  });

  test("string does not contain only letters when empty", () => {
    expect(onlyLetters("")).toBe(false);
  });
});

describe("formatCurrency", () => {
  test("format to empty string on invalid input number", () => {
    expect(formatCurrency("abc")).toBe("");
    expect(formatCurrency("")).toBe("");
    expect(formatCurrency(" ")).toBe("");
  });

  test("format to proper currency format on numeric input", () => {
    expect(formatCurrency(4)).toBe("$4.00");
    expect(formatCurrency("4")).toBe("$4.00");
    expect(formatCurrency("4.32")).toBe("$4.32");
    expect(formatCurrency(4.32)).toBe("$4.32");
    expect(formatCurrency(0)).toBe("$0.00");
    expect(formatCurrency(-1)).toBe("-$1.00");
  });
});
