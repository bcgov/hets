export const RUNTIME_OPENSHIFT_BUILD_COMMIT = window.RUNTIME_OPENSHIFT_BUILD_COMMIT ?? '';

// Paths
export const HOME_PATHNAME = '/home';
export const OWNERS_PATHNAME = '/owners';
export const EQUIPMENT_PATHNAME = '/equipment';
export const PROJECTS_PATHNAME = '/projects';
export const CONTACTS_PATHNAME = '/contacts';
export const RENTAL_REQUESTS_PATHNAME = '/rental-requests';
export const RENTAL_AGREEMENTS_PATHNAME = '/rental-agreements';
export const OVERTIME_RATES_PATHNAME = '/overtime-rates';
export const USERS_PATHNAME = '/users';
export const ROLES_PATHNAME = '/roles';
export const ROLLOVER_PATHNAME = '/roll-over';
export const DISTRICT_ADMIN_PATHNAME = '/district-admin';
export const SENIORITY_LIST_PATHNAME = '/reports/seniority-list';
export const STATUS_LETTERS_REPORT_PATHNAME = '/reports/status-letters';
export const HIRING_REPORT_PATHNAME = '/reports/owners-equipment-reason';
export const OWNERS_COVERAGE_PATHNAME = '/reports/wcb-cgl-coverage';
export const TIME_ENTRY_PATHNAME = '/time-entry';
export const VERSION_PATHNAME = '/version';
export const BUSINESS_PORTAL_PATHNAME = '/business';
export const BUSINESS_DETAILS_PATHNAME = '/business/details';
export const AIT_REPORT_PATHNAME = '/reports/rental-agreement-summary';
export const UNAUTHORIZED_PATHNAME = '/unauthorized';
//temporary fix to fix import error RENTAL_CONDITIONS_PATHNAME and RENTAL_RATES_PATHNAME. Two pathnames below are used in the API.
export const RENTAL_CONDITIONS_PATHNAME = 'temporaryfix';
export const RENTAL_RATES_PATHNAME = 'temporaryfix';

// Permissions
export const PERMISSION_LOGIN = 'Login';
export const PERMISSION_BUSINESS_LOGIN = 'BusinessLogin';
export const PERMISSION_ADMIN = 'Admin';
export const PERMISSION_USER_MANAGEMENT = 'UserManagement';
export const PERMISSION_ROLES_AND_PERMISSIONS = 'RolesAndPermissions';
export const PERMISSION_IMPORT_DATA = 'ImportData';
export const PERMISSION_CODE_TABLE_MANAGEMENT = 'CodeTableManagement';
export const PERMISSION_DISTRICT_CODE_TABLE_MANAGEMENT = 'DistrictCodeTableManagement';
export const PERMISSION_DISTRICT_ROLLOVER = 'DistrictRollover';
export const PERMISSION_VERSION = 'Version';
export const PERMISSION_WRITE_ACCESS = 'WriteAccess';

// Roles
export const ADMINISTRATOR_ROLE = '4-HETS System Administrator';

// Equipments
export const EQUIPMENT_DAYS_SINCE_VERIFIED_WARNING = 270;
export const EQUIPMENT_DAYS_SINCE_VERIFIED_CRITICAL = 365;

export const EQUIPMENT_STATUS_CODE_PENDING = 'Unapproved';
export const EQUIPMENT_STATUS_CODE_APPROVED = 'Approved';
export const EQUIPMENT_STATUS_CODE_ARCHIVED = 'Archived';

// Owners
export const OWNER_STATUS_CODE_PENDING = 'Unapproved';
export const OWNER_STATUS_CODE_APPROVED = 'Approved';
export const OWNER_STATUS_CODE_ARCHIVED = 'Archived';

// Projects
export const PROJECT_STATUS_CODE_ACTIVE = 'Active';
export const PROJECT_STATUS_CODE_COMPLETED = 'Completed';

// Rental Requests
export const RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS = 'In Progress';
export const RENTAL_REQUEST_STATUS_CODE_COMPLETED = 'Complete';
export const RENTAL_REQUEST_STATUS_CODE_CANCELLED = 'Cancelled';

// Hiring Refusal Reasons
export const HIRING_REFUSAL_EQUIPMENT_NOT_AVAILABLE = 'Equipment Not Available';
export const HIRING_REFUSAL_EQUIPMENT_NOT_SUITABLE = 'Equipment Not Suitable';
export const HIRING_REFUSAL_NO_RESPONSE = 'No Response';
export const HIRING_REFUSAL_MAXIMUM_HOURS_REACHED = 'Maximum Hours Reached';
export const HIRING_REFUSAL_MAINTENANCE_CONTRACTOR = 'Maintenance Contractor';
export const HIRING_REFUSAL_OTHER = 'Other (Reason to be mentioned in note)';

// Rental Agreements
export const RENTAL_AGREEMENT_STATUS_CODE_ACTIVE = 'Active';
export const RENTAL_AGREEMENT_STATUS_CODE_COMPLETED = 'Completed';
export const RENTAL_RATE_PERIOD_HOURLY = 'Hr';
export const RENTAL_RATE_PERIOD_DAILY = 'Daily';
export const RENTAL_RATE_PERIOD_WEEKLY = 'Weekly';
export const RENTAL_RATE_PERIOD_MONTHLY = 'Monthly';
export const RENTAL_RATE_PERIOD_NEGOTIATED = 'Negotiated';
export const RENTAL_RATE_PERIOD_SET = 'Set';

// Users
export const USER_STATUS_ACTIVE = 'Active';
export const USER_STATUS_ARCHIVED = 'Archived';

// Date Formats
export const DATE_FULL_MONTH_DAY_YEAR = 'MMMM D, YYYY';
export const DATE_SHORT_MONTH_DAY_YEAR = 'MMM D, YYYY';
export const DATE_YEAR_SHORT_MONTH_DAY = 'YYYY-MMM-DD';
export const DATE_ISO_8601 = 'YYYY-MM-DD';

export const DATE_ZULU = 'YYYY-MM-DDThh:mm:ss[Z]';

export const DATE_TIME_ISO_8601 = 'YYYY-MM-DDTHH:mm:ss';
export const DATE_TIME_READABLE = 'MMMM D, YYYY [at] h:mm:ss A';
export const DATE_TIME_LOG = 'YYYY/MM/DD HH:mm:ss';
export const DATE_TIME_FILENAME = 'YYYY-MM-DD-HHmmss';

// RegEx
export const EMAIL_REGEX = /\S+@\S+\.\S+/;
export const NANP_REGEX =
  /^(?:(?:\+?1\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})(?:\s*(?:#|x\.?|ext\.?|extension)\s*(\d+))?$/;
export const MONEY_REGEX = /^\d+(\.\d\d?)?$/;
export const POSTAL_CODE_REGEX = /^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$/;

// Conditions
export const NON_STANDARD_CONDITION = 'Other';

// ResponseTypes

export const RESPONSE_TYPE_BLOB = 'blob';

// Cloning
export const BY_PROJECT = 'By Project';
export const BY_EQUIPMENT = 'By Equipment';

export var headerHeight = 0;
export function setHeaderHeight(num) {
  headerHeight = num;
}

// History
export const HISTORY_OWNER = 'Owner';
export const HISTORY_PROJECT = 'Project';
export const HISTORY_EQUIPMENT = 'Equipment';
export const HISTORY_REQUEST = 'Request';
export const HISTORY_USER = 'User';
export const HISTORY_ROLE = 'Role';
export const HISTORY_CONTACT = 'Contact';
export const HISTORY_DOCUMENT = 'Document';

// Session
export const SESSION_TIMEOUT = 7200000; // 120 minutes
export const SESSION_KEEP_ALIVE_INTERVAL = 240000; // 4 minutes

// Max Field Lengths
export const MAX_LENGTH_CGL_COMPANY_NAME = 150;
export const MAX_LENGTH_NOTE_TEXT = 2048;
export const MAX_LENGTH_PHONE_NUMBER = 20;
export const MAX_LENGTH_RENTAL_AGREEMENT_NOTE = 150;
export const MAX_LENGTH_STATUS_COMMENT = 255;

// Max File Sizes
export const MAX_ATTACHMENT_FILE_SIZE = 5242880; // 5 MB
export const MAX_ATTACHMENT_FILE_SIZE_READABLE = '5 MB';
