import _ from "lodash";
import * as Constant from "../constants";

export function toString(str) {
  if (_.isNil(str)) {
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
  sep = toString(sep);
  if (sep === "") {
    sep = " ";
  }
  const a = toString(left).trim();
  const b = toString(right).trim();
  if (a !== "" && b !== "") {
    return `${a}${sep}${b}`;
  }
  if (a !== "") {
    return a;
  }
  if (b !== "") {
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
  return isBlank(str) || Number(toString(str).trim()) === 0;
}

export function notBlank(str) {
  return !isBlank(str);
}

export function padLeft(str, padChar, len) {
  if (_.isNil(str)) {
    return "";
  }

  str = toString(str);
  padChar = toString(padChar);

  if (padChar.length !== 1) {
    return str;
  }

  const threshold = !_.isNumber(len) || len <= 0 ? 0 : Number(len);

  if (str.length >= threshold) {
    return str;
  }
  const pad = Array(len + 1).join(padChar);
  return pad.substring(str.length) + str;
}

export function formatPhoneNumber(str) {
  const phoneNumber = toString(str);
  const match = phoneNumber.match(Constant.NANP_REGEX);
  if (match) {
    match.shift();

    const extension = match.pop();
    const number = match
      .filter((x) => {
        return x;
      })
      .join("-");
    return extension ? number + "x" + extension : number;
  }
  return phoneNumber;
}

export function onlyLetters(str) {
  const a = toString(str).trim();
  return /^[a-zA-Z]+$/.test(a);
}

export function formatCurrency(number) {
  if (_.isNil(number)) {
    return "";
  }
  if (!_.isNumber(number)) {
    const numberStr = toString(number).trim();
    if (numberStr === "" || _.isNaN(Number(numberStr))) {
      return "";
    }
    number = Number(numberStr);
  }
  
  return new Intl.NumberFormat("en-CA", {
    style: "currency",
    currency: "CAD",
  }).format(number);
}

export function formatHours(number) {
  return (number || 0).toFixed(2);
}
