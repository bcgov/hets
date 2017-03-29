import React from 'react';

import { connect } from 'react-redux';

import { Alert, Button, ButtonGroup, Glyphicon } from 'react-bootstrap';

import _ from 'lodash';

import * as Action from '../../actionTypes';
import * as Api from '../../api';
import * as Constant from '../../constants';
import store from '../../store';

import DeleteButton from '../../components/DeleteButton.jsx';
import FileAttachDialog from '../../components/FileAttachDialog.jsx';
import ModalDialog from '../../components/ModalDialog.jsx';
import SortTable from '../../components/SortTable.jsx';
import Spinner from '../../components/Spinner.jsx';

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

      showAttachmentDialog: false,

      ui : {
        sortField: this.props.ui.sortField || 'timestampSort',
        sortDesc: this.props.ui.sortDesc !== false,
      },
    };
  },

  componentDidMount() {
    this.setState({ loading: true });
    Api.getUsers().then(() => {
      return this.fetch(true);
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
      var documents = _.map(this.props.documents, document => {
        document.userName = this.getUserName(document.lastUpdateUserid);
        document.formattedTimestamp = formatDateTime(document.lastUpdateTimestamp, Constant.DATE_TIME_LOG);
        return document;
      });
      this.setState({
        documents: documents,
      });
    }).finally(() => {
      this.setState({ loading: false });
    });
  },

  attachDocument() {
    this.setState({ showAttachmentDialog: true });
  },

  onDocumentAttached() {
    this.setState({ showAttachmentDialog: false });
    this.fetch();
  },

  closeAttachmentDialog() {
    this.setState({ showAttachmentDialog: false });
  },

  deleteDocument(document) {
    Api.deleteDocument(document).then(() => {
      return this.fetch();
    });
  },

  downloadDocument(document) {
    // Get path to the document and open it in a new browser window to initiate download.
    window.open(Api.getDownloadDocumentURL(document));
  },

  render() {
    var parent = this.props.parent;

    return <ModalDialog id="documents-dialog" backdrop="static" bsSize="lg" show={ this.props.show } onClose={ this.props.onClose }
      title={ <strong>Documents for { parent.name }</strong> }
      footer={ <Button title="Close" onClick={ this.props.onClose }>Close</Button> }
    >
      {(() => {
        return <div>
          <div>
            {(() => {
              if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

              var numDocuments = Object.keys(this.state.documents).length;

              var attachButton = <Button title="Attach Document" onClick={ this.attachDocument } bsStyle={ numDocuments ? 'primary' : 'default' }>
                <Glyphicon glyph="paperclip" />&nbsp;<strong>Attach</strong>
              </Button>;

              if (numDocuments === 0) { return <Alert bsStyle="success">No documents { attachButton }</Alert>; }

              var documents = _.sortBy(this.state.documents, this.state.ui.sortField);
              if (this.state.ui.sortDesc) {
                _.reverse(documents);
              }

              var headers = [
                { field: 'timestampSort',  title: 'Uploaded On' },
                { field: 'userName',       title: 'Uploaded By' },
                { field: 'fileName',       title: 'File Name'   },
                { field: 'fileSize',       title: 'File Size'   },
                { field: 'attachDocument', title: 'Attach Document', style: { textAlign: 'right'  },
                  node: attachButton,
                },
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
          { this.state.showAttachmentDialog &&
            <FileAttachDialog show={ this.state.showAttachmentDialog } onClose={ this.closeAttachmentDialog }
              uploadPath={ parent.uploadDocumentPath } onUpload={ this.onDocumentAttached } parentName={ parent.name }
            />
          }
        </div>;
      })()}
    </ModalDialog>;
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
