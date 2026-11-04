import dayjs from 'dayjs';
import quarterOfYear from 'dayjs/plugin/quarterOfYear';
import utc from 'dayjs/plugin/utc';

dayjs.extend(quarterOfYear);
dayjs.extend(utc);

export default dayjs;

