import React from 'react';

import { connect } from 'react-redux';

import { Alert, Button, ButtonGroup, Glyphicon, ProgressBar, HelpBlock } from 'react-bootstrap';

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

import { formatDateTime } from '../../utils/date';


var DocumentsListDialog = React.createClass({
  propTypes: {
    parent: React.PropTypes.object.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,

    documents: React.PropTypes.object,
    users: React.PropTypes.object,
    ui: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loading: false,
      documents: [],
      uploadInProgress: false,
      percentUploaded: 0,
      showAttachmentDialog: false,
      ui : {
        sortField: this.props.ui.sortField || 'timestampSort',
        sortDesc: this.props.ui.sortDesc !== false,
      },
      uploadError: '',
    };
  },

  componentDidMount() {
    this.setState({ loading: true });
    Api.getUsers().then(() => {
      return this.formatDocuments();
    }).finally(() => {
      this.setState({ loading: false });
    });
  },

  getUserName(smUserId) {
    var user = _.find(this.props.users, user => { return user.smUserId === smUserId; });
    return user ? user.name : smUserId;
  },

  updateUIState(state, callback) {
    this.setState({ ui: { ...this.state.ui, ...state }}, () =>{
      store.dispatch({ type: Action.UPDATE_DOCUMENTS_UI, documents: this.state.ui });
      if (callback) { callback(); }
    });
  },

  fetch() {
    this.setState({ loading: true });
    return this.props.parent.getDocumentsPromise(this.props.parent.id).then(() => {
      this.formatDocuments();
    }).finally(() => {
      this.setState({ loading: false });
    });
  },

  formatDocuments() {
    var documents = _.map(this.props.documents, document => {
      return {
        ...document,
        userName: this.getUserName(document.lastUpdateUserid),
        formattedTimestamp: formatDateTime(document.lastUpdateTimestamp, Constant.DATE_TIME_LOG),
      };
    });
    this.setState({
      documents: documents,
    });
  },

  deleteDocument(document) {
    Api.deleteDocument(document).then(() => {
      this.props.parent.documentDeleted(this.props.parent, document);
      return this.fetch();
    });
  },

  downloadDocument(document) {
    // Get path to the document and open it in a new browser window to initiate download.
    window.open(Api.getDownloadDocumentURL(document));
  },

  uploadFiles(files) {
    this.setState({ uploadError: '' });

    var invalidFiles = _.filter(files, file => file.size > Constant.MAX_ATTACHMENT_FILE_SIZE);
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

    this.uploadPromise = request(buildApiPath(this.props.parent.uploadDocumentPath), options).then(() => {
      _.map(files, ((file) => {
        this.props.parent.documentAdded(this.props.parent, file.name);
      }));
      this.setState({ uploadInProgress: false, percentUploaded: null });
      this.fetch();
    }, (err) => {
      this.setState({ uploadInProgress: false, fileUploadError: err });
    });
  },

  render() {
    var parent = this.props.parent;

    return (
      <ModalDialog id="documents-dialog" backdrop="static" bsSize="lg" show={ this.props.show } onClose={ this.props.onClose }
        title={ <strong>Documents for { parent.name }</strong> }
        footer={ <Button title="Close" onClick={ this.props.onClose }>Close</Button> }
      >
        <div>
          {this.state.uploadInProgress ?
            <ProgressBar active now={ this.state.percentUploaded } min={ 5 }/>
            :
            <div className="file-picker-container">
              <FilePicker onFilesSelected={ this.uploadFiles }/>
              <div>Select one or more files{ parent.name ? ` to attach to ${ parent.name }` : null }</div>
              <HelpBlock>The maximum size of each file is { Constant.MAX_ATTACHMENT_FILE_SIZE_READABLE }.</HelpBlock>
              { this.state.uploadError && <div className="has-error"><HelpBlock>{ this.state.uploadError }</HelpBlock></div> }
            </div>
          }
          <div>
            {(() => {
              if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

              var numDocuments = Object.keys(this.state.documents).length;

              if (numDocuments === 0) { return <Alert bsStyle="success">No documents</Alert>; }

              var documents = _.sortBy(this.state.documents, this.state.ui.sortField);
              if (this.state.ui.sortDesc) {
                _.reverse(documents);
              }

              var headers = [
                { field: 'timestampSort',  title: 'Uploaded On' },
                { field: 'userName',       title: 'Uploaded By' },
                { field: 'fileName',       title: 'File Name'   },
                { field: 'fileSize',       title: 'File Size'   },
                { field: 'attachDocument', title: 'Attach Document', style: { textAlign: 'right'  } },
              ];

              return <SortTable id="documents-list" sortField={ this.state.ui.sortField } sortDesc={ this.state.ui.sortDesc } onSort={ this.updateUIState } headers={ headers }>
                {
                  _.map(documents, (document) => {
                    return <tr key={ document.id }>
                      <td>{ document.formattedTimestamp }</td>
                      <td>{ document.userName }</td>
                      <td>{ document.fileName }</td>
                      <td>{ document.fileSizeDisplay }</td>
                      <td style={{ textAlign: 'right' }}>
                        <ButtonGroup>
                          <Button title="Download Document" onClick={ this.downloadDocument.bind(this, document) } bsSize="xsmall"><Glyphicon glyph="download-alt" /></Button>
                          <DeleteButton name="Document" hide={ !document.canDelete } onConfirm={ this.deleteDocument.bind(this, document) }/>
                        </ButtonGroup>
                      </td>
                    </tr>;
                  })
                }
              </SortTable>;
            })()}
          </div>
        </div>
      </ModalDialog>
    );
  },
});

function mapStateToProps(state) {
  return {
    documents: state.models.documents,
    users: state.lookups.users,
    ui: state.ui.documents,
  };
}

export default connect(mapStateToProps)(DocumentsListDialog);
