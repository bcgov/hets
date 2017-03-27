import React from 'react';
import { Button, Glyphicon, OverlayTrigger, Tooltip, ProgressBar } from 'react-bootstrap';

import { request, buildApiPath } from '../utils/http';

const FILE_UPLOAD_PATH = '/test-file-upload';

var FileUpload = React.createClass({
  propTypes: {
    id: React.PropTypes.string,
    className: React.PropTypes.string,
    label: React.PropTypes.string,
    files: React.PropTypes.array,
    url: React.PropTypes.string,
    onUploadProgress: React.PropTypes.func,
    onUploadFinished: React.PropTypes.func,
  },

  getInitialState() {
    return {
      uploadInProgress: false,
      percentUploaded: null,
      fileUploadError: null,
    };
  },

  uploadFiles() {
    this.setState({ uploadInProgress: true, percentUploaded: 0 });

    var options = {
      method: 'POST',
      files: this.props.files,
      onUploadProgress: (percentComplete) => {
        this.setState({ percentUploaded: percentComplete });
        if(this.props.onUploadProgress) {
          this.props.onUploadProgress(percentComplete);
        }
      },
    };

    this.uploadPromise = request(buildApiPath(this.props.url || FILE_UPLOAD_PATH), options).then(() => {
      this.setState({ uploadInProgress: false, percentUploaded: null });
      if(this.props.onUploadFinished) { this.props.onUploadFinished(true); }
    }, (err) => {
      this.setState({ uploadInProgress: false, fileUploadError: err });
      if(this.props.onUploadFinished) { this.props.onUploadFinished(err); }
    });
  },

  reset() {
    this.setState(this.getInitialState());
  },

  render() {
    var classNames = [ 'file-upload', 'clearfix' ];

    if(this.props.className) {
      classNames.push(this.props.className);
    }

    if(this.state.fileUploadError) {
      classNames.push('file-upload-error');
    }

    var files = this.props.files || [];
    var fileUploadButton, uploadProgressBar;
    var error;
    if(!this.state.fileUploadError) {
      if(!this.state.uploadInProgress) {
        var uploadTooltip = <Tooltip id="files-to-upload-tooltip">
          Upload {files.length} files
        </Tooltip>;

        fileUploadButton = <OverlayTrigger placement="top" overlay={uploadTooltip}>
          <Button className="file-upload-button" onClick={this.uploadFiles} disabled={files.length === 0}>
            <Glyphicon glyph="upload" />
            Upload {files.length} Files
          </Button>
        </OverlayTrigger>;
      } else {
        uploadProgressBar = <ProgressBar now={this.state.percentUploaded} label={`${this.state.percentUploaded}%`} />;
      }
    } else {
      error = <OverlayTrigger placement="top" overlay={<Tooltip id="file-upload-error-tooltip">{String(this.state.fileUploadError)}</Tooltip>}>
        <p className="file-upload-error-message" onClick={this.reset}>Upload Error<Glyphicon glyph="remove"/></p>
      </OverlayTrigger>;
    }

    return <div id={this.props.id} className={classNames.join(' ')}>
        {fileUploadButton}
        {uploadProgressBar}
        {error}
      </div>;
  },
});

export default FileUpload;
