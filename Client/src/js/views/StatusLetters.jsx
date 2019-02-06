import React from 'react';

import { connect } from 'react-redux';

import { Well, PageHeader, Row, Col, ButtonToolbar, Button } from 'react-bootstrap';

import _ from 'lodash';

import * as Api from '../api';
import * as Constant from '../constants';

import MultiDropdown from '../components/MultiDropdown.jsx';

import { formatDateTimeUTCToLocal } from '../utils/date';

var StatusLetters = React.createClass({
  propTypes: {
    localAreas: React.PropTypes.object,
    owners: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loaded: false,
      localAreaIds: [],
      ownerIds: [],
    };
  },

  componentDidMount() {
    Api.getOwnersLite().then(() => {
      this.setState({ loaded: true });
    });
  },

  updateState(state, callback) {
    this.setState(state, () => {
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
    var promise = Api.getStatusLettersPdf({ localAreas: this.state.localAreaIds, owners: this.state.ownerIds });
    var filename = 'StatusLetters-' + formatDateTimeUTCToLocal(new Date(), Constant.DATE_TIME_FILENAME) + '.pdf';

    this.downloadPdf(promise, filename);
  },

  getMailingLabels() {
    var promise = Api.getMailingLabelsPdf({ localAreas: this.state.localAreaIds, owners: this.state.ownerIds });
    var filename = 'MailingLabels-' + formatDateTimeUTCToLocal(new Date(), Constant.DATE_TIME_FILENAME) + '.pdf';

    this.downloadPdf(promise, filename);
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
    return _.chain(this.props.owners)
      .filter(x => this.matchesLocalAreaFilter(x.localAreaId))
      .sortBy('organizationName')
      .value();
  },

  render() {
    var localAreas = _.sortBy(this.props.localAreas, 'name');
    var owners = this.getFilteredOwners();

    return <div id="status-letters">
      <PageHeader>Status Letters / Mailing Labels</PageHeader>
      <Well bsSize="small" className="clearfix">
        <Row>
          <Col md={12}>
            <ButtonToolbar className="btn-container">
              <MultiDropdown id="localAreaIds" placeholder="Local Areas"
                items={ localAreas } selectedIds={ this.state.localAreaIds } updateState={ this.updateLocalAreaState } showMaxItems={ 2 } />
              <MultiDropdown id="ownerIds" placeholder="Companies" fieldName="organizationName"
                items={ owners } selectedIds={ this.state.ownerIds } updateState={ this.updateState } showMaxItems={ 2 } />
              <Button onClick={ this.getStatusLetters } bsStyle="primary">Status Letters</Button>
              <Button onClick={ this.getMailingLabels } bsStyle="primary">Mailing Labels</Button>
            </ButtonToolbar>
          </Col>
        </Row>
      </Well>
    </div>;
  },
});

function mapStateToProps(state) {
  return {
    localAreas: state.lookups.localAreas,
    owners: state.models.ownersLite.data,
  };
}

export default connect(mapStateToProps)(StatusLetters);
