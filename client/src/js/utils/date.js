import Dayjs from './dayjs';

import * as Constant from "../constants";

export function dateIsBetween(date, startDate, endDate) {
  if (startDate && date.isBefore(startDate)) {
    return false;
  }
  if (endDate && date.isAfter(endDate)) {
    return false;
  }
  return true;
}

export function formatDateTime(dateTime, format) {
  if (!dateTime) {
    return "";
  }
  var dt = Dayjs.utc(dateTime);
  if (!dt || !dt.isValid()) {
    return "";
  }
  if (!format) {
    format = Constant.DATE_TIME_ISO_8601;
  }
  return dt.format(format);
}

export function formatDateTimeUTCToLocal(dateTime, format) {
  if (!dateTime) {
    return "";
  }
  var dt = Dayjs.utc(dateTime).local();
  if (!dt || !dt.isValid()) {
    return "";
  }
  if (!format) {
    format = Constant.DATE_TIME_ISO_8601;
  }
  return dt.format(format);
}

export function sortableDateTime(dateTime) {
  if (!dateTime) {
    return 0;
  }
  var dt = Dayjs.utc(dateTime);
  if (!dt || !dt.isValid()) {
    return 0;
  }
  return dt.unix();
}

export function daysFromToday(dateTime) {
  var dt = Dayjs.utc(dateTime);
  if (!dt || !dt.isValid()) {
    return 0;
  }
  var today = Dayjs.utc().startOf("d");
  return dt.startOf("d").diff(today, "d");
}

export function daysAgo(dateTime) {
  var dt = Dayjs.utc(dateTime);
  if (!dt || !dt.isValid()) {
    return 0;
  }
  var today = Dayjs().startOf("d");
  return today.diff(dt.startOf("d"), "d");
}

export function hoursAgo(dateTime) {
  var dt = Dayjs.utc(dateTime);
  if (!dt || !dt.isValid()) {
    return 0;
  }
  var now = Dayjs();
  return now.diff(dt, "h");
}

export function today(format) {
  if (!format) {
    format = Constant.DATE_TIME_ISO_8601;
  }
  var dt = Dayjs.utc().startOf("d");
  return dt.format(format);
}

export function businessDayOnOrBefore(dateTime, format) {
  var dt = Dayjs.utc(dateTime);
  if (!dt || !dt.isValid()) {
    return "";
  }
  if (dt.day() === 6) {
    // Saturday
    dt = dt.subtract(1, "d");
  } else if (dt.day() === 0) {
    // Sunday
    dt = dt.subtract(2, "d");
  }
  // TODO: Holidays
  if (!format) {
    format = Constant.DATE_TIME_ISO_8601;
  }
  return dt.format(format);
}

export function isValidDate(dateTime) {
  var dt = Dayjs.utc(dateTime);
  return dt && dt.isValid();
}

export function toZuluTime(dateTime) {
  var dt = Dayjs.utc(dateTime);
  if (!dt || !dt.isValid()) {
    return "";
  }
  return dt.utc().format(Constant.DATE_ZULU);
}

export function startOfCurrentFiscal(dateTime) {
  var dt = Dayjs.utc(dateTime);
  if (!dt || !dt.isValid()) {
    return "";
  }
  // January-March
  if (dt.quarter() === 1) {
    dt = dt.subtract(1, "year");
  }
  return dt.month(3).startOf("month");
}

export function endOfCurrentFiscal(dateTime) {
  var dt = Dayjs.utc(dateTime);
  if (!dt || !dt.isValid()) {
    return "";
  }
  // April-December
  if (dt.quarter() > 1) {
    dt = dt.add(1, "year");
  }
  return dt.month(2).endOf("month");
}

export function startOfPreviousFiscal(dateTime) {
  var fiscalStart = Dayjs.utc(startOfCurrentFiscal(dateTime));
  if (!fiscalStart || !fiscalStart.isValid()) {
    return "";
  }
  return fiscalStart.subtract(1, "year").month(3).startOf("month");
}

export function endOfPreviousFiscal(dateTime) {
  var fiscalEnd = Dayjs.utc(endOfCurrentFiscal(dateTime));
  if (!fiscalEnd || !fiscalEnd.isValid()) {
    return "";
  }
  return fiscalEnd.subtract(1, "year").month(2).endOf("month");
}

export function isValidYear(year) {
  let currentYear = new Date().getFullYear();
  return (
    year.length === 4 &&
    year.match(/\d{4}/) &&
    parseInt(year, 10) <= currentYear + 1 &&
    parseInt(year, 10) >= 1900
  );
}
