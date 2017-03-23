// Paths
export const HOME_PATHNAME = 'home';
export const OWNERS_PATHNAME = 'owners';
export const EQUIPMENT_PATHNAME = 'equipment';
export const RENTAL_REQUESTS_PATHNAME = 'rental-requests';
export const PROJECTS_PATHNAME = 'projects';
export const USERS_PATHNAME = 'users';
export const ROLES_PATHNAME = 'roles';
export const VERSION_PATHNAME = 'version';
export const CONTACTS_PATHNAME = 'contacts';

// Permissions
export const PERMISSION_LOGIN = 'LOGIN';
export const PERMISSION_ADMIN = 'ADMIN';
export const PERMISSION_ASSIGN_INSPECTORS = 'ASSIGN_INSPECTORS';
export const PERMISSION_RECEIVE_NOTIFICATIONS = 'RECEIVE_NOTIFICATIONS';
export const PERMISSION_ROLES_AND_PERMISSIONS = 'ROLES_AND_PERMISSIONS';
export const PERMISSION_USER_MANAGEMENT = 'USER_MANAGEMENT';

// Equipments
export const EQUIPMENT_DAYS_SINCE_VERIFIED_WARNING = 270;
export const EQUIPMENT_DAYS_SINCE_VERIFIED_CRITICAL = 365;

export const EQUIPMENT_STATUS_CODE_PENDING = 'Pending';
export const EQUIPMENT_STATUS_CODE_APPROVED = 'Approved';
export const EQUIPMENT_STATUS_CODE_ARCHIVED = 'Archived';

// Owners
export const OWNER_STATUS_CODE_PENDING = 'Pending';
export const OWNER_STATUS_CODE_APPROVED = 'Approved';
export const OWNER_STATUS_CODE_ARCHIVED = 'Archived';

// Projects
export const PROJECT_STATUS_CODE_ACTIVE = 'Active';
export const PROJECT_STATUS_CODE_COMPLETED = 'Completed';

// Rental Requests
export const RENTAL_REQUEST_STATUS_CODE_IN_PROGRESS = 'In Progress';
export const RENTAL_REQUEST_STATUS_CODE_COMPLETED = 'Completed';
export const RENTAL_REQUEST_STATUS_CODE_CANCELLED = 'Cancelled';

// Rental Agreements
export const RENTAL_AGREEMENT_STATUS_CODE_ACTIVE = 'Active';
export const RENTAL_AGREEMENT_STATUS_CODE_COMPLETED = 'Completed';

// Users
export const USER_STATUS_ACTIVE = 'Active';
export const USER_STATUS_ARCHIVED = 'Archived';

// Date Formats
export const DATE_FULL_MONTH_DAY_YEAR = 'MMMM D, YYYY';
export const DATE_SHORT_MONTH_DAY_YEAR = 'MMM D, YYYY';
export const DATE_YEAR_SHORT_MONTH_DAY = 'YYYY-MMM-DD';
export const DATE_ISO_8601 = 'YYYY-MM-DD';

export const DATE_ZULU = 'YYYY-MM-DDT00:00:00Z';

export const DATE_TIME_ISO_8601 = 'YYYY-MM-DDTHH:mm:ss';
export const DATE_TIME_READABLE = 'MMMM D, YYYY [at] h:mm:ss A';

// RegEx
export const EMAIL_REGEX = /\S+@\S+\.\S+/;
export const NANP_REGEX = /^(?:(?:\+?1\s*(?:[.-]\s*)?)?(?:\(\s*([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9])\s*\)|([2-9]1[02-9]|[2-9][02-8]1|[2-9][02-8][02-9]))\s*(?:[.-]\s*)?)?([2-9]1[02-9]|[2-9][02-9]1|[2-9][02-9]{2})\s*(?:[.-]\s*)?([0-9]{4})(?:\s*(?:#|x\.?|ext\.?|extension)\s*(\d+))?$/;
