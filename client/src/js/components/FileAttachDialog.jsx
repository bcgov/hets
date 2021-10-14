import PropTypes from 'prop-types';
import React from 'react';
import { Button } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import ModalDialog from './ModalDialog.jsx';
import FilePicker from './FilePicker.jsx';
import FileUpload from './FileUpload.jsx';

class FileAttachDialog extends React.Component {
  static propTypes = {
    id: PropTypes.string,
    className: PropTypes.string,
    parentName: PropTypes.string,
    uploadPath: PropTypes.string,
    show: PropTypes.bool.isRequired,
    onClose: PropTypes.func.isRequired,
    onUpload: PropTypes.func,
  };

  constructor(props) {
    super(props);

    this.state = {
      files: [],
    };
  }

  filesPicked = (files) => {
    var existingFiles = this.state.files.slice();
    existingFiles.push.apply(existingFiles, files);
    this.setState({ files: existingFiles });
  };

  removeFile = (file) => {
    var pos = this.state.files.indexOf(file);
    var files = this.state.files.slice();
    files.splice(pos, 1);
    this.setState({ files: files });
  };

  filesUploaded = (result) => {
    if (!(result instanceof Error)) {
      this.setState({ files: [] });
      if (this.props.onUpload) {
        this.props.onUpload();
      }
    }
  };

  render() {
    var fileList;
    if (this.state.files.length > 0) {
      fileList = (
        <ol className="file-list">
          {' '}
          {this.state.files.map((file, i) => {
            return (
              <li key={`${file.name} - ${i}`} className="clearfix">
                {file.name}
                <FontAwesomeIcon icon="times" onClick={this.removeFile.bind(this, file)} />
              </li>
            );
          })}
        </ol>
      );
    }

    var footer = (
      <span>
        <Button onClick={this.props.onClose} className="pull-left">
          Close
        </Button>
        <FileUpload files={this.state.files} path={this.props.uploadPath} onUploadFinished={this.filesUploaded} />
      </span>
    );

    var titleAttr = `Attach files${this.props.parentName ? ` to ${this.props.parentName}` : ''}`;

    return (
      <ModalDialog
        backdrop="static"
        show={this.props.show}
        id={this.props.id}
        className={`file-upload-dialog ${this.props.className || ''}`}
        title={<b>{titleAttr}</b>}
        onClose={this.props.onClose}
        footer={footer}
      >
        {fileList}
        <p className="note">
          <FilePicker onFilesSelected={this.filesPicked} />
          <br />
          Select one or more files{this.props.parentName ? ` to attach to ${this.props.parentName}` : null}
        </p>
      </ModalDialog>
    );
  }
}

export default FileAttachDialog;
