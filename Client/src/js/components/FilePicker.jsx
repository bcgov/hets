import React from 'react';
import { Glyphicon } from 'react-bootstrap';


class FilePicker extends React.Component {
  static propTypes = {
    id: React.PropTypes.string,
    className: React.PropTypes.string,
    label: React.PropTypes.string,
    mimeTypes: React.PropTypes.array,
    onFilesSelected: React.PropTypes.func,
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
