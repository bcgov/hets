import PropTypes from 'prop-types';
import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

const FilePicker = ({ id, className, label, mimeTypes, onFilesSelected }) => {
  const filesPicked = (e) => {
    if (onFilesSelected) onFilesSelected(e.target.files);
  };

  const classNames = ['file-picker'];

  if (className) {
    classNames.push(className);
  }

  return (
    <span id={id} className={classNames.join(' ')}>
      <label>
        <span className="btn btn-custom" title="Pick files to upload">
          <FontAwesomeIcon icon="folder-open" />
          {label ? ` ${label}` : null}
        </span>
        <input type="file" multiple onChange={filesPicked} />
      </label>
    </span>
  );
};
FilePicker.propTypes = {
  id: PropTypes.string,
  className: PropTypes.string,
  label: PropTypes.string,
  mimeTypes: PropTypes.array,
  onFilesSelected: PropTypes.func,
};
export default FilePicker;
