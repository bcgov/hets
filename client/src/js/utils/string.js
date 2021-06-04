import * as Constant from "../constants";

function toString(str) {
  if (str === null || str === undefined) {
    return "";
  }
  return String(str);
}

export function dasherize(str) {
  return toString(str)
    .trim()
    .replace(/([A-Z])/g, "-$1")
    .replace(/[-_\s]+/g, "-")
    .toLowerCase();
}

export function titleCase(str) {
  return toString(str).replace(/\b\w/g, (l) => l.toUpperCase());
}

export function plural(num, singular, plural) {
  return num === 1 ? singular : plural;
}

export function concat(left, right, sep) {
  if (!sep) {
    sep = " ";
  }
  var a = toString(left).trim();
  var b = toString(right).trim();
  if (a && b) {
    return `${a}${sep}${b}`;
  }
  if (a) {
    return a;
  }
  if (b) {
    return b;
  }
  return "";
}

export function firstLastName(first, last) {
  return concat(first, last);
}

export function lastFirstName(last, first) {
  return concat(last, first, ", ");
}

export function isBlank(str) {
  return toString(str).trim().length === 0;
}

export function isBlankOrZero(str) {
  return toString(str).trim() === 0;
}

export function notBlank(str) {
  return !isBlank(str);
}

export function padLeft(str, padChar, len) {
  if (!str || !padChar || !len) {
    return "";
  }
  if (str.length >= len) {
    return str;
  }
  var pad = Array(len + 1).join(padChar);
  return pad.substring(str.length) + str;
}

export function formatPhoneNumber(str) {
  var phoneNumber = toString(str);
  var match = phoneNumber.match(Constant.NANP_REGEX);
  if (match) {
    match.shift();

    var extension = match.pop();
    var number = match
      .filter((x) => {
        return x;
      })
      .join("-");
    return extension ? number + "x" + extension : number;
  }
  return phoneNumber;
}

export function onlyLetters(str) {
  var a = toString(str).trim();
  return /^[a-zA-Z]+$/.test(a);
}

export function formatCurrency(number) {
  if (number === null || number === undefined) {
    return "";
  }
  return new Intl.NumberFormat("en-CA", {
    style: "currency",
    currency: "CAD",
  }).format(number);
}

export function formatHours(number) {
  return (number || 0).toFixed(2);
}
