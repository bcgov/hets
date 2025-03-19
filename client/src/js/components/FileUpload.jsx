import PropTypes from 'prop-types';
import React, { useState } from 'react';
import { connect } from 'react-redux';
import { Button, OverlayTrigger, Tooltip, ProgressBar } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import { request, buildApiPath } from '../utils/http';
import { plural } from '../utils/string';

const FILE_UPLOAD_PATH = '/test-file-upload';

const FileUpload = ({ id, className, label, files, path, onUploadProgress, onUploadFinished, dispatch }) => {
  const [uploadInProgress, setUploadInProgress] = useState(false);
  const [percentUploaded, setPercentUploaded] = useState(null);
  const [fileUploadError, setFileUploadError] = useState(null);

  const uploadFiles = async () => {
    setUploadInProgress(true);
    setPercentUploaded(0);

    const options = {
      method: 'POST',
      files,
      onUploadProgress: (percentComplete) => {
        const percent = Math.round(percentComplete);
        setPercentUploaded(percent);
        if (onUploadProgress) {
          onUploadProgress(percent);
        }
      },
    };

    try {
      await dispatch(request(buildApiPath(path || FILE_UPLOAD_PATH), options));
      setUploadInProgress(false);
      setPercentUploaded(null);
      if (onUploadFinished) {
        onUploadFinished(true);
      }
    } catch (err) {
      setUploadInProgress(false);
      setFileUploadError(err);
      if (onUploadFinished) {
        onUploadFinished(err);
      }
    }
  };

  const reset = () => {
    setUploadInProgress(false);
    setPercentUploaded(null);
    setFileUploadError(null);
  };

  const classNames = ['file-upload', 'clearfix'];
  if (className) classNames.push(className);
  if (fileUploadError) classNames.push('file-upload-error');

  const fileUploadButton = !fileUploadError && !uploadInProgress && files.length > 0 && (
    <OverlayTrigger
      placement="top"
      overlay={
        <Tooltip id="files-to-upload-tooltip">
          Upload {files.length} {plural(files.length, 'File', 'Files')}
        </Tooltip>
      }
    >
      <Button
        className="file-upload-button"
        onClick={uploadFiles}
        disabled={files.length === 0}
        variant={files.length === 0 ? 'default' : 'success'}
      >
        <FontAwesomeIcon icon="file-upload" />
        {` Upload ${files.length} ${plural(files.length, 'File', 'Files')}`}
      </Button>
    </OverlayTrigger>
  );

  const uploadProgressBar = uploadInProgress && (
    <ProgressBar now={percentUploaded} label={`${percentUploaded}%`} min={5} />
  );

  const error = fileUploadError && (
    <OverlayTrigger
      placement="top"
      overlay={<Tooltip id="file-upload-error-tooltip">{String(fileUploadError)}</Tooltip>}
    >
      <p className="file-upload-error-message" onClick={reset}>
        Upload Error <FontAwesomeIcon icon="times" />
      </p>
    </OverlayTrigger>
  );

  return (
    <div id={id} className={classNames.join(' ')}>
      {fileUploadButton}
      {uploadProgressBar}
      {error}
    </div>
  );
};

FileUpload.propTypes = {
  id: PropTypes.string,
  className: PropTypes.string,
  label: PropTypes.string,
  files: PropTypes.array,
  path: PropTypes.string,
  onUploadProgress: PropTypes.func,
  onUploadFinished: PropTypes.func,
  dispatch: PropTypes.func.isRequired,
};

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(null, mapDispatchToProps)(FileUpload);
