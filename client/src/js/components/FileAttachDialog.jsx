import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { Button } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import ModalDialog from './ModalDialog.jsx';
import FilePicker from './FilePicker.jsx';
import FileUpload from './FileUpload.jsx';

const FileAttachDialog = ({ id, className, parentName, uploadPath, show, onClose, onUpload }) => {
  const [files, setFiles] = useState([]);

  const filesPicked = (selectedFiles) => {
    setFiles((prevFiles) => [...prevFiles, ...selectedFiles]);
  };

  const removeFile = (file) => {
    setFiles((prevFiles) => prevFiles.filter((f) => f !== file));
  };

  const filesUploaded = (result) => {
    if (!(result instanceof Error)) {
      setFiles([]);
      if (onUpload) {
        onUpload();
      }
    }
  };

  const fileList = files.length > 0 && (
    <ol className="file-list">
      {files.map((file, i) => (
        <li key={`${file.name} - ${i}`} className="clearfix">
          {file.name}
          <FontAwesomeIcon icon="times" onClick={() => removeFile(file)} />
        </li>
      ))}
    </ol>
  );

  const footer = (
    <span>
      <Button onClick={onClose} className="pull-left">
        Close
      </Button>
      <FileUpload files={files} path={uploadPath} onUploadFinished={filesUploaded} />
    </span>
  );

  const titleAttr = `Attach files${parentName ? ` to ${parentName}` : ''}`;

  return (
    <ModalDialog
      backdrop="static"
      show={show}
      id={id}
      className={`file-upload-dialog ${className || ''}`}
      title={<b>{titleAttr}</b>}
      onClose={onClose}
      footer={footer}
    >
      {fileList}
      <p className="note">
        <FilePicker onFilesSelected={filesPicked} />
        <br />
        Select one or more files{parentName ? ` to attach to ${parentName}` : null}
      </p>
    </ModalDialog>
  );
};

FileAttachDialog.propTypes = {
  id: PropTypes.string,
  className: PropTypes.string,
  parentName: PropTypes.string,
  uploadPath: PropTypes.string,
  show: PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  onUpload: PropTypes.func,
};

export default FileAttachDialog;
