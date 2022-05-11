import PropTypes from 'prop-types';
import React from 'react';
import { saveAs } from 'file-saver';

import { connect } from 'react-redux';

import { Alert, Button, ButtonGroup, ProgressBar, FormText } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import _ from 'lodash';

import { request, buildApiPath } from '../../utils/http';

import * as Action from '../../actionTypes';
import * as Api from '../../api';
import * as Constant from '../../constants';
import store from '../../store';

import DeleteButton from '../../components/DeleteButton.jsx';
import ModalDialog from '../../components/ModalDialog.jsx';
import SortTable from '../../components/SortTable.jsx';
import Spinner from '../../components/Spinner.jsx';
import FilePicker from '../../components/FilePicker.jsx';
import Authorize from '../../components/Authorize.jsx';

import { formatDateTime } from '../../utils/date';

class DocumentsListDialog extends React.Component {
  static propTypes = {
    parent: PropTypes.object.isRequired,
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool,

    documents: PropTypes.object,
    users: PropTypes.object,
    ui: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      loading: false,
      documents: [],
      uploadInProgress: false,
      percentUploaded: 0,
      showAttachmentDialog: false,
      ui: {
        sortField: props.ui.sortField || 'timestampSort',
        sortDesc: props.ui.sortDesc !== false,
      },
      uploadError: '',
    };
  }

  componentDidMount() {
    this.setState({ loading: true });
    this.formatDocuments();
    this.setState({ loading: false });
  }

  updateUIState = (state, callback) => {
    this.setState({ ui: { ...this.state.ui, ...state } }, () => {
      store.dispatch({ type: Action.UPDATE_DOCUMENTS_UI, documents: this.state.ui });
      if (callback) {
        callback();
      }
    });
  };

  fetch = () => {
    this.setState({ loading: true });
    return this.props.parent
      .getDocumentsPromise(this.props.parent.id)
      .then(() => {
        this.formatDocuments();
      })
      .finally(() => {
        this.setState({ loading: false });
      });
  };

  formatDocuments = () => {
    var documents = _.map(this.props.documents, (document) => {
      return {
        ...document,
        formattedTimestamp: formatDateTime(document.lastUpdateTimestamp, Constant.DATE_TIME_LOG),
      };
    });
    this.setState({
      documents: documents,
    });
  };

  deleteDocument = (document) => {
    Api.deleteDocument(document).then(() => {
      this.props.parent.documentDeleted(this.props.parent, document);
      return this.fetch();
    });
  };

  downloadDocument = (document) => {
    let fName = '';
    Api.getDownloadDocument(document)
      .getBlob()
      .then((res) => {
        //use Header content-disposition for the file name
        //looks like ""attachment; filename=adobe.pdf; filename*=UTF-8''adobe.pdf""
        console.log(res.getResponseHeader('content-disposition'));
        fName = res.getResponseHeader('content-disposition').match(/(?<=filename=)(.*)(?=; filename)/)[0];
        console.log(fName);
        saveAs(res.response, fName);
      })
      .catch((error) => {
        console.log("file name error");
        console.log(error);
        let message = error.message.split(`"`)[0].replace('"', ''); //extracts error message without HTML text.Odd issue where I can't detect \ so need to rely on "
        this.setState({ uploadError: message });
      });
  };

  uploadFiles = (files) => {
    this.setState({ uploadError: '' });

    var invalidFiles = _.filter(files, (file) => file.size > Constant.MAX_ATTACHMENT_FILE_SIZE);
    if (invalidFiles.length > 0) {
      this.setState({ uploadError: 'One of the selected files is too large.' });
      return;
    }

    this.setState({ uploadInProgress: true, percentUploaded: 0 });

    var options = {
      method: 'POST',
      files: [...files],
      onUploadProgress: (percentComplete) => {
        var percent = Math.round(percentComplete);
        this.setState({ percentUploaded: percent });
      },
    };
    this.uploadPromise = request(buildApiPath(this.props.parent.uploadDocumentPath), options).then(
      () => {
        _.map(files, (file) => {
          this.props.parent.documentAdded(this.props.parent, file.name);
        });
        this.setState({ uploadInProgress: false, percentUploaded: null });
        this.fetch();
      },
      (err) => {
        let message = err.message.split(`"`)[0].replace('"', ''); //extracts error message without HTML text.Odd issue where I can't detect \ so need to rely on "
        this.setState({ uploadInProgress: false, uploadError: message });
      }
    );
  };

  render() {
    var parent = this.props.parent;

    return (
      <ModalDialog
        id="documents-dialog"
        backdrop="static"
        size="lg"
        show={this.props.show}
        onClose={this.props.onClose}
        title={<strong>Documents for {parent.name}</strong>}
        footer={
          <Button title="Close" onClick={this.props.onClose}>
            Close
          </Button>
        }
      >
        <div>
          {this.state.uploadInProgress ? (
            <ProgressBar active now={this.state.percentUploaded} min={5} />
          ) : (
            <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
              <div className="file-picker-container">
                <FilePicker onFilesSelected={this.uploadFiles} />
                <div>Select one or more files{parent.name ? ` to attach to ${parent.name}` : null}</div>
                <FormText>The maximum size of each file is {Constant.MAX_ATTACHMENT_FILE_SIZE_READABLE}.</FormText>
                {this.state.uploadError && (
                  <div className="has-error">
                    <FormText>{this.state.uploadError}</FormText>
                  </div>
                )}
              </div>
            </Authorize>
          )}
          <div>
            {(() => {
              if (this.state.loading) {
                return (
                  <div style={{ textAlign: 'center' }}>
                    <Spinner />
                  </div>
                );
              }

              var numDocuments = Object.keys(this.state.documents).length;

              if (numDocuments === 0) {
                return <Alert variant="success">No documents</Alert>;
              }

              var documents = _.sortBy(this.state.documents, this.state.ui.sortField);
              if (this.state.ui.sortDesc) {
                _.reverse(documents);
              }

              var headers = [
                { field: 'timestampSort', title: 'Uploaded On' },
                { field: 'userName', title: 'Uploaded By' },
                { field: 'fileName', title: 'File Name' },
                { field: 'fileSize', title: 'File Size' },
                { field: 'attachDocument', title: 'Attach Document', style: { textAlign: 'right' } },
              ];

              return (
                <SortTable
                  id="documents-list"
                  sortField={this.state.ui.sortField}
                  sortDesc={this.state.ui.sortDesc}
                  onSort={this.updateUIState}
                  headers={headers}
                >
                  {_.map(documents, (document) => {
                    return (
                      <tr key={document.id}>
                        <td>{document.formattedTimestamp}</td>
                        <td>{document.userName}</td>
                        <td>{document.fileName}</td>
                        <td>{document.fileSizeDisplay}</td>
                        <td style={{ textAlign: 'right' }}>
                          <ButtonGroup>
                            <Button
                              title="Download Document"
                              onClick={this.downloadDocument.bind(this, document)}
                              size="sm"
                            >
                              <FontAwesomeIcon icon="download" />
                            </Button>
                            <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
                              <DeleteButton
                                name="Document"
                                hide={!document.canDelete}
                                onConfirm={this.deleteDocument.bind(this, document)}
                              />
                            </Authorize>
                          </ButtonGroup>
                        </td>
                      </tr>
                    );
                  })}
                </SortTable>
              );
            })()}
          </div>
        </div>
      </ModalDialog>
    );
  }
}

function mapStateToProps(state) {
  return {
    documents: state.models.documents,
    users: state.lookups.users,
    ui: state.ui.documents,
  };
}

export default connect(mapStateToProps)(DocumentsListDialog);
