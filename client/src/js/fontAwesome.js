import { library } from '@fortawesome/fontawesome-svg-core';
import {
  faArrowLeft,
  faAsterisk,
  faCalendarAlt,
  faCheck,
  faDownload,
  faEdit,
  faEnvelope,
  faExclamationCircle,
  faFileUpload,
  faFolderOpen,
  faLink,
  faMinus,
  faPencilAlt,
  faPlus,
  faPrint,
  faSortAmountDown,
  faSortAmountDownAlt,
  faStar,
  faSyncAlt,
  faTimes,
  faTrashAlt,
  faUser,
} from '@fortawesome/free-solid-svg-icons';
import { faCheckCircle, faTimesCircle, faCheckSquare } from '@fortawesome/free-regular-svg-icons';

const addIconsToLibrary = () => {
  //solid icons
  library.add(faArrowLeft);
  library.add(faAsterisk);
  library.add(faCalendarAlt);
  library.add(faCheck);
  library.add(faDownload);
  library.add(faEdit);
  library.add(faEnvelope);
  library.add(faExclamationCircle);
  library.add(faFileUpload);
  library.add(faFolderOpen);
  library.add(faLink);
  library.add(faMinus);
  library.add(faPencilAlt);
  library.add(faPlus);
  library.add(faPrint);
  library.add(faSortAmountDown);
  library.add(faSortAmountDownAlt);
  library.add(faStar);
  library.add(faSyncAlt);
  library.add(faTimes);
  library.add(faTrashAlt);
  library.add(faUser);

  //regular icons
  library.add(faCheckCircle);
  library.add(faTimesCircle);
  library.add(faCheckSquare);
};

export default addIconsToLibrary;
