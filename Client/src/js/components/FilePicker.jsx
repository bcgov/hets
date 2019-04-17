import PropTypes from 'prop-types';
import React from 'react';
import { Glyphicon } from 'react-bootstrap';


class FilePicker extends React.Component {
  static propTypes = {
    id: PropTypes.string,
    className: PropTypes.string,
    label: PropTypes.string,
    mimeTypes: PropTypes.array,
    onFilesSelected: PropTypes.func,
  };

  filesPicked = (e) => {
    this.props.onFilesSelected(e.target.files);
  };

  render() {
    var classNames = [ 'file-picker' ];

    if(this.props.className) {
      classNames.push(this.props.className);
    }

    return <span id={ this.props.id } className={ classNames.join(' ') }>
      <label>
        <span className="btn btn-default" title="Pick files to upload">
          <Glyphicon glyph="folder-open" />{ this.props.label ? ` ${ this.props.label }` : null }
        </span>
        <input type="file" multiple onChange={ this.filesPicked }/>
      </label>
    </span>;
  }
}

export default FilePicker;
