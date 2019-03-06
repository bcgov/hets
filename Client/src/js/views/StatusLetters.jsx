import React from 'react';
import { connect } from 'react-redux';
import { Well, PageHeader, Row, Col, ButtonToolbar, Button, ButtonGroup, Glyphicon } from 'react-bootstrap';
import _ from 'lodash';

import * as Api from '../api';
import * as Constant from '../constants';

import MultiDropdown from '../components/MultiDropdown.jsx';
import SortTable from '../components/SortTable.jsx';
import DeleteButton from '../components/DeleteButton.jsx';
import Spinner from '../components/Spinner.jsx';

import { formatDateTimeUTCToLocal } from '../utils/date';
import { sortDir } from '../utils/array';


var StatusLetters = React.createClass({
  propTypes: {
    localAreas: React.PropTypes.object,
    owners: React.PropTypes.object,
    batchReports: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loading: true,
      localAreaIds: [],
      ownerIds: [],
      ui : {
        sortField: 'startDate',
        sortDesc: false,
      },
    };
  },

  componentDidMount() {
    Api.getOwnersLite();
    this.fetch();
  },

  fetch() {
    this.setState({ loading: true });
    return Api.getBatchReports().then(() => this.setState({ loading: false }));
  },

  updateState(state, callback) {
    this.setState(state, () => {
      if (callback) { callback(); }
    });
  },

  updateUIState(state, callback) {
    this.setState({ ui: { ...this.state.ui, ...state }}, () =>{
      // store.dispatch({ type: Action.UPDATE_HISTORY_UI, history: this.state.ui });
      if (callback) { callback(); }
    });
  },

  downloadPdf(promise, filename) {
    promise.then((response) => {
      var blob;
      if (window.navigator.msSaveBlob) {
        blob = window.navigator.msSaveBlob(response, filename);
      } else {
        blob = new Blob([response], {type: 'image/pdf'});
      }
      //Create a link element, hide it, direct
      //it towards the blob, and then 'click' it programatically
      let a = document.createElement('a');
      a.style.cssText = 'display: none';
      document.body.appendChild(a);
      //Create a DOMString representing the blob
      //and point the link element towards it
      let url = window.URL.createObjectURL(blob);
      a.href = url;
      a.download = filename;
      //programatically click the link to trigger the download
      a.click();
      //release the reference to the file by revoking the Object URL
      window.URL.revokeObjectURL(url);
    });
  },

  getStatusLetters() {
    this.setState({ loading: true });
    Api.scheduleStatusLettersPdf({ localAreas: this.state.localAreaIds, owners: this.state.ownerIds }).then(() => {
      return this.fetch();
    });
  },

  getMailingLabels() {
    var promise = Api.getMailingLabelsPdf({ localAreas: this.state.localAreaIds, owners: this.state.ownerIds });
    var filename = 'MailingLabels-' + formatDateTimeUTCToLocal(new Date(), Constant.DATE_TIME_FILENAME) + '.pdf';

    this.downloadPdf(promise, filename);
  },

  downloadStatusLetterPdf(reportId) {
    var promise = Api.getStatusLettersPdf(reportId);
    var filename = 'StatusLetters-' + formatDateTimeUTCToLocal(new Date(), Constant.DATE_TIME_FILENAME) + '.pdf';
    this.downloadPdf(promise, filename);
  },

  deleteBatchReport(reportId) {
    Api.deleteBatchReport(reportId);
  },

  matchesLocalAreaFilter(localAreaId) {
    if (this.state.localAreaIds.length == 0) {
      return true;
    }

    return _.includes(this.state.localAreaIds, localAreaId);
  },

  updateLocalAreaState(state) {
    this.updateState(state, this.filterSelectedOwners);
  },

  filterSelectedOwners() {
    var acceptableOwnerIds = _.map(this.getFilteredOwners(), 'id');
    var ownerIds = _.intersection(this.state.ownerIds, acceptableOwnerIds);
    this.updateState({ ownerIds: ownerIds });
  },

  getFilteredOwners() {
    return _.chain(this.props.owners.data)
      .filter(x => this.matchesLocalAreaFilter(x.localAreaId))
      .sortBy('organizationName')
      .value();
  },

  renderBatchReports() {
    const { loading } = this.state;
    const { batchReports } = this.props;

    if (!batchReports.loaded) {
      return <div style={{ textAlign: 'center' }}><Spinner/></div>;
    }

    var reports = _.orderBy(batchReports.data, [this.state.ui.sortField], sortDir(this.state.ui.sortDesc));

    var headers = [
      { field: 'startDate',           title: 'Time Started' },
      { field: 'complete',            title: 'Completed', style: { textAlign: 'center' }},
      { field: 'showMore',            title: '', style: { textAlign: 'right' },
        node: (
          <Button bsSize="xsmall" disabled={loading} onClick={ this.fetch }>
            <Glyphicon glyph="refresh" title="Refresh reports" />
          </Button>
        ),
      },
    ];

    return (
      <SortTable id="batch-reports" sortField={ this.state.ui.sortField } sortDesc={ this.state.ui.sortDesc } onSort={ this.updateUIState } headers={ headers }>
        {
          reports.map((report) => {
            const reportStatus = report.complete ?
              <Button title="Download Report" onClick={() => this.downloadStatusLetterPdf(report.id) } bsSize="xsmall"><Glyphicon glyph="download-alt" /></Button> :
              <Glyphicon glyph="hourglass"/>;
            return <tr key={ report.id }>
              <td>{ formatDateTimeUTCToLocal(report.startDate, Constant.DATE_TIME_LOG) }</td>
              <td style={{textAlign: 'center'}}>{reportStatus}</td>
              <td style={{ textAlign: 'right' }}>
                <ButtonGroup>
                  <DeleteButton name="Report" hide={ report.complete } onConfirm={ () => this.deleteBatchReport(report.id) }/>
                </ButtonGroup>
              </td>
            </tr>;
          })
        }
      </SortTable>
    );
  },

  render() {
    var localAreas = _.sortBy(this.props.localAreas, 'name');
    var owners = this.getFilteredOwners();

    return (
      <div id="status-letters">
        <PageHeader>Status Letters / Mailing Labels</PageHeader>
        <Well bsSize="small" className="clearfix">
          <Row>
            <Col md={12}>
              <ButtonToolbar className="btn-container">
                <MultiDropdown
                  id="localAreaIds"
                  placeholder="Local Areas"
                  items={localAreas}
                  selectedIds={this.state.localAreaIds}
                  updateState={this.updateLocalAreaState}
                  showMaxItems={2} />
                <MultiDropdown
                  id="ownerIds"
                  placeholder="Companies"
                  fieldName="organizationName"
                  items={owners}
                  disabled={!this.props.owners.loaded}
                  selectedIds={this.state.ownerIds}
                  updateState={this.updateState}
                  showMaxItems={2} />
                <Button onClick={ this.getStatusLetters } bsStyle="primary">Status Letters</Button>
                <Button onClick={ this.getMailingLabels } bsStyle="primary">Mailing Labels</Button>
              </ButtonToolbar>
            </Col>
          </Row>
        </Well>
        {this.renderBatchReports()}
    </div>
    );
  },
});

function mapStateToProps(state) {
  return {
    localAreas: state.lookups.localAreas,
    owners: state.lookups.owners.lite,
    batchReports: state.models.batchReports,
  };
}

export default connect(mapStateToProps)(StatusLetters);
