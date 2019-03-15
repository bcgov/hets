import React from 'react';
import { connect } from 'react-redux';
import { Well, PageHeader, Row, Col, ButtonToolbar, Button } from 'react-bootstrap';
import _ from 'lodash';

import * as Api from '../api';

import MultiDropdown from '../components/MultiDropdown.jsx';


class SeniorityList extends React.Component {
  static propTypes = {
    currentUser: React.PropTypes.object,
    districtEquipmentTypes: React.PropTypes.object,
    localAreas: React.PropTypes.object,
  };

  state = {
    selectedEquipmentTypeIds: [],
    selectedLocalAreaIds: [],
  };

  componentDidMount() {
    this.fetch();
  }

  fetch = () => {
    Api.getDistrictEquipmentTypes();
  };

  updateState = (state) => {
    this.setState(state);
  };

  onLocalAreasChanged = () => {
    this.setState({ selectedEquipmentTypeIds: [] });
  };

  getFilteredEquipmentTypes = (localAreaIds) => {
    return _.chain(this.props.districtEquipmentTypes.data)
      .filter(type => type.equipmentCount > 0 && localAreaIds.length === 0 || _.filter(type.localAreas, localArea => _.includes(localAreaIds, localArea.id) && localArea.equipmentCount > 0).length > 0)
      .sortBy('districtEquipmentName')
      .value();
  };

  getRotationList = (counterCopy) => {
    Api.equipmentSeniorityListPdf(this.state.selectedLocalAreaIds, this.state.selectedEquipmentTypeIds, counterCopy).then(response => {
      var filename = counterCopy ? 'counter_copy.pdf' : 'seniority_list.pdf';

      var blob = new Blob([response], {type: 'image/pdf'});
      if (window.navigator.msSaveBlob) {
        // ie11
        window.navigator.msSaveBlob(blob, filename);
        return;
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
  };

  render() {
    var localAreas = _.chain(this.props.localAreas)
      .sortBy('name')
      .value();

    var districtEquipmentTypes = this.getFilteredEquipmentTypes(this.state.selectedLocalAreaIds);

    return <div id="seniority-list">
      <PageHeader>Seniority List</PageHeader>
      <Well bsSize="small" className="clearfix">
        <Row>
          <Col md={12}>
            <ButtonToolbar className="btn-container">
              <MultiDropdown id="selectedLocalAreaIds" className="fixed-width small" placeholder="Local Areas" items={ localAreas }
                selectedIds={ this.state.selectedLocalAreaIds } updateState={ this.updateState } onChange={ this.onLocalAreasChanged } showMaxItems={ 2 } />
              <MultiDropdown
                id="selectedEquipmentTypeIds"
                className="fixed-width"
                placeholder="Equipment Types"
                fieldName="districtEquipmentName"
                items={districtEquipmentTypes}
                disabled={!this.props.districtEquipmentTypes.loaded}
                selectedIds={this.state.selectedEquipmentTypeIds}
                updateState={this.updateState}
                showMaxItems={2}/>
              <Button onClick={ () => this.getRotationList(false) } bsStyle="primary">Seniority List</Button>
              <Button onClick={ () => this.getRotationList(true) } bsStyle="primary">Seniority List (Counter Copy)</Button>
            </ButtonToolbar>
          </Col>
        </Row>
      </Well>
    </div>;
  }
}


function mapStateToProps(state) {
  return {
    currentUser: state.user,
    districtEquipmentTypes: state.lookups.districtEquipmentTypes,
    localAreas: state.lookups.localAreas,
  };
}

export default connect(mapStateToProps)(SeniorityList);
