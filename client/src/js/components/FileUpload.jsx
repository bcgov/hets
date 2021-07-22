import PropTypes from 'prop-types';
import React from 'react';
import { Button, OverlayTrigger, Tooltip, ProgressBar } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import { request, buildApiPath } from '../utils/http';
import { plural } from '../utils/string';

const FILE_UPLOAD_PATH = '/test-file-upload';

class FileUpload extends React.Component {
  static displayName = 'FileUpload';

  static propTypes = {
    id: PropTypes.string,
    className: PropTypes.string,
    label: PropTypes.string,
    files: PropTypes.array,
    path: PropTypes.string,
    onUploadProgress: PropTypes.func,
    onUploadFinished: PropTypes.func,
  };

  constructor(props) {
    super(props);

    this.state = {
      uploadInProgress: false,
      percentUploaded: null,
      fileUploadError: null,
    };
  }

  uploadFiles = () => {
    this.setState({ uploadInProgress: true, percentUploaded: 0 });

    var options = {
      method: 'POST',
      files: this.props.files,
      onUploadProgress: (percentComplete) => {
        var percent = Math.round(percentComplete);
        this.setState({ percentUploaded: percent });
        if (this.props.onUploadProgress) {
          this.props.onUploadProgress(percent);
        }
      },
    };

    this.uploadPromise = request(buildApiPath(this.props.path || FILE_UPLOAD_PATH), options).then(
      () => {
        this.setState({ uploadInProgress: false, percentUploaded: null });
        if (this.props.onUploadFinished) {
          this.props.onUploadFinished(true);
        }
      },
      (err) => {
        this.setState({ uploadInProgress: false, fileUploadError: err });
        if (this.props.onUploadFinished) {
          this.props.onUploadFinished(err);
        }
      }
    );
  };

  reset = () => {
    this.setState({ uploadInProgress: false, percentUploaded: null, fileUploadError: null });
  };

  render() {
    var classNames = ['file-upload', 'clearfix'];

    if (this.props.className) {
      classNames.push(this.props.className);
    }

    if (this.state.fileUploadError) {
      classNames.push('file-upload-error');
    }

    var files = this.props.files || [];
    var fileUploadButton, uploadProgressBar;
    var error;
    if (!this.state.fileUploadError) {
      var uploadText = `Upload ${files.length} ${plural(files.length, 'File', 'Files')}`;
      if (!this.state.uploadInProgress) {
        var uploadTooltip = <Tooltip id="files-to-upload-tooltip">{uploadText}</Tooltip>;
        var notReady = files.length === 0;

        fileUploadButton = (
          <OverlayTrigger placement="top" overlay={uploadTooltip}>
            <Button
              className="file-upload-button"
              onClick={this.uploadFiles}
              disabled={notReady}
              variant={notReady ? 'default' : 'success'}
            >
              <FontAwesomeIcon icon="file-upload" />
              {` ${uploadText}`}
            </Button>
          </OverlayTrigger>
        );
      } else {
        uploadProgressBar = (
          <ProgressBar active now={this.state.percentUploaded} label={`${this.state.percentUploaded}%`} min={5} />
        );
      }
    } else {
      error = (
        <OverlayTrigger
          placement="top"
          overlay={<Tooltip id="file-upload-error-tooltip">{String(this.state.fileUploadError)}</Tooltip>}
        >
          <p className="file-upload-error-message" onClick={this.reset}>
            Upload Error
            <FontAwesomeIcon icon="times" />
          </p>
        </OverlayTrigger>
      );
    }

    return (
      <div id={this.props.id} className={classNames.join(' ')}>
        {fileUploadButton}
        {uploadProgressBar}
        {error}
      </div>
    );
  }
}

export default FileUpload;
