import React from 'react';
import { Button, Glyphicon } from 'react-bootstrap';

import ModalDialog from './ModalDialog.jsx';
import FilePicker from './FilePicker.jsx';
import FileUpload from './FileUpload.jsx';
import {titleCase} from '../utils/string';

var FileAttachDialog = React.createClass({
  propTypes: {
    id: React.PropTypes.string,
    className: React.PropTypes.string,
    attachToName: React.PropTypes.string,
    onClose: React.PropTypes.func.isRequired,
  },

  getInitialState() {
    return {
      files: [],
    };
  },

  filesPicked(files) {
    var existingFiles = this.state.files.slice();
    existingFiles.push.apply(existingFiles, files);
    this.setState({ files: existingFiles });
  },

  removeFile(file) {
    var pos = this.state.files.indexOf(file);
    var files = this.state.files.slice();
    files.splice(pos, 1);
    this.setState({ files: files });
  },

  filesUploaded(result) {
    if(!(result instanceof Error)) {
      this.setState({ files: [] });
    }
  },

  render() {
    var fileList;
    if(this.state.files.length > 0) {
      fileList = <ol className="file-list">
        {this.state.files.map((file, i) => {
          return <li key={`${file.name}-${i}`} className="clearfix">
            {file.name}
            <Glyphicon glyph="remove-sign" onClick={this.removeFile.bind(this, file)} />
          </li>;
        })}
      </ol>;
    }

    var footer = <span>
      <Button onClick={ this.props.onClose } className="pull-left">Close</Button>
      <FileUpload files={this.state.files} onUploadFinished={this.filesUploaded}/>
    </span>;

    var titleAttr = `Attach files ${this.props.attachToName ? `to ${titleCase(this.props.attachToName)}` : ''}`;

    return <ModalDialog show={true} id={this.props.id} className={ `file-upload-dialog ${this.props.className || ''}` } title={titleAttr} onClose={this.props.onClose} footer={footer}>
      {fileList}
      <p className="note">
        <FilePicker onFilesSelected={this.filesPicked}/><br/>
        Select one or more files {this.props.attachToName ? `to attach to ${this.props.attachToName}` : null}
      </p>
    </ModalDialog>;
  },
});

export default FileAttachDialog;
