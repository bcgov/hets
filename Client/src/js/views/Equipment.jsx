import React from 'react';

import { connect } from 'react-redux';

import { Well, Alert, Table, Row, Col } from 'react-bootstrap';
import { ButtonToolbar, DropdownButton, MenuItem, Button, ButtonGroup, Glyphicon } from 'react-bootstrap';
import { Form, ControlLabel, Checkbox } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';

import _ from 'lodash';
import Moment from 'moment';
import Promise from 'bluebird';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import DateControl from '../components/DateControl.jsx';
import Favourites from '../components/Favourites.jsx';
import MultiDropdown from '../components/MultiDropdown.jsx';
import Spinner from '../components/Spinner.jsx';

import { formatDateTime } from '../utils/date';

/*

TODO:
* Print

*/

// Status code drop-down items
var Equipment = React.createClass({
  propTypes: {
    equipmentList: React.PropTypes.object,
    localAreas: React.PropTypes.object,
    equipmentTypes: React.PropTypes.object,
    physicalAttachmentTypes: React.PropTypes.object,
    owners: React.PropTypes.object,
    favourites: React.PropTypes.object,
    search: React.PropTypes.object,
    ui: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loading: false,

      search: {
        selectedLocalAreasIds: this.props.search.selectedLocalAreasIds || [],
        selectedEquipmentTypesIds: this.props.search.selectedEquipmentTypesIds || [],
        selectedPhysicalAttachmentsIds: this.props.search.selectedPhysicalAttachmentsIds || [],
        ownerId: this.props.search.ownerId || '',
        ownerName: this.props.search.ownerName || 'Owner',
        lastVerifiedDate: this.props.search.lastVerifiedDate || '',
        hired: this.props.search.hired !== false,
        statusCode: this.props.search.statusCode || Constant.EQUIPMENT_STATUS_CODE_APPROVED,
      },

      ui : {
        sortField: this.props.ui.sortField || 'ownerName',
        sortDesc: this.props.ui.sortDesc === true,
      },
    };
  },

  buildSearchParams() {
    var searchParams = {
      hired: this.state.search.hired,
      owner: this.state.search.ownerId,
      statusCode: this.state.search.statusCode,
    };

    if (this.state.search.selectedLocalAreasIds.length > 0) {
      searchParams.localAreas = this.state.search.selectedLocalAreasIds;
    }
    if (this.state.search.selectedEquipmentTypesIds.length > 0) {
      searchParams.types = this.state.search.selectedEquipmentTypesIds;
    }
    if (this.state.search.selectedPhysicalAttachmentsIds.length > 0) {
      searchParams.attachments = this.state.search.selectedPhysicalAttachmentsIds;
    }

    var notVerifiedSinceDate = Moment(this.state.search.lastVerifiedDate);
    if (notVerifiedSinceDate && notVerifiedSinceDate.isValid()) {
      searchParams.notverifiedsincedate = notVerifiedSinceDate.format('YYYY-MM-DDT00:00:00');
    }

    return searchParams;
  },

  componentDidMount() {
    this.setState({ loading: true });

    var ownersPromise = Api.getOwners();
    var favouritesPromise = Api.getFavourites('equipment');

    Promise.all([ownersPromise, favouritesPromise]).then(() => {
      // If this is the first load, then look for a default favourite
      if (!this.props.search.loaded) {
        var favourite = _.find(this.props.favourites, (favourite) => { return favourite.isDefault; });
        if (favourite) {
          this.loadFavourite(favourite);
          return;
        }
      }
      this.fetch();
    });
  },

  fetch() {
    this.setState({ loading: true });    
    Api.searchEquipmentList(this.buildSearchParams()).finally(() => {
      this.setState({ loading: false });
    });
  },

  updateSearchState(state, callback) {
    this.setState({ search: { ...this.state.search, ...state }}, () =>{
      store.dispatch({ type: Action.UPDATE_EQUIPMENT_LIST_SEARCH, equipmentList: this.state.search });
      if (callback) { callback(); }
    });
  },

  updateUIState(state, callback) {
    this.setState({ ui: { ...this.state.ui, ...state }}, () =>{
      store.dispatch({ type: Action.UPDATE_EQUIPMENT_LIST_UI, equipmentList: this.state.ui });
      if (callback) { callback(); }
    });
  },

  localAreasChanged(selected) {
    var selectedIds = _.map(selected, 'id');
    this.updateSearchState({
      selectedLocalAreasIds: selectedIds,
    });
  },

  statusCodeSelected(eventKey) {
    this.updateSearchState({
      statusCode: eventKey,
    });
  },

  hiredChanged(e) {
    this.updateSearchState({
      hired: e.target.checked,
    });
  },

  equipmentTypesChanged(selected) {
    var selectedIds = _.map(selected, 'id');
    this.updateSearchState({
      selectedEquipmentTypesIds: selectedIds,
    });
  },

  ownerSelected(eventKey, e) {
    this.updateSearchState({
      ownerId: eventKey || '',
      ownerName: eventKey !== 0 ? e.target.text : 'Owner',
    });
  },

  lastVerifiedDateChanged(date) {
    this.updateSearchState({
      lastVerifiedDate: date,
    });
  },

  saveFavourite(favourite) {
    favourite.value = JSON.stringify(this.state.search);
  },

  loadFavourite(favourite) {
    this.updateSearchState(JSON.parse(favourite.value), this.fetch);
  },

  sort(e) {
    var newState = {};
    if (this.state.ui.sortField !== e.currentTarget.id) {
      newState.sortField = e.currentTarget.id;
      newState.sortDesc = false;
    } else {
      newState.sortDesc = !this.state.ui.sortDesc;
    }

    this.updateUIState(newState);
  },

  print() {
    // TODO Implement
  },

  render() {
    var localAreas = _.sortBy(this.props.localAreas, 'name');
    var equipmentTypes = _.sortBy(this.props.equipmentTypes, 'name');
    var owners = _.sortBy(this.props.owners, 'name');
    var attachmentTypes = _.sortBy(this.props.physicalAttachmentTypes, 'name');

    return <div id="equipment-list">
      <Well id="equipment-bar" bsSize="small" className="clearfix">
        <Row>
          <Col md={11}>
            <Row>
              <ButtonToolbar id="equipment-search-row-1">
                <MultiDropdown id="local-area-dropdown" placeholder="Local Areas"
                  items={ localAreas } selectedIds={ this.state.search.selectedLocalAreasIds } onChange={ this.localAreasChanged } showMaxItems={ 2 } />
                <DropdownButton id="status-code-dropdown" title={ this.state.search.statusCode } onSelect={ this.statusCodeSelected }>
                  <MenuItem key={ Constant.EQUIPMENT_STATUS_CODE_APPROVED } eventKey={ Constant.EQUIPMENT_STATUS_CODE_APPROVED }>{ Constant.EQUIPMENT_STATUS_CODE_APPROVED }</MenuItem>
                  <MenuItem key={ Constant.EQUIPMENT_STATUS_CODE_NEW } eventKey={ Constant.EQUIPMENT_STATUS_CODE_NEW }>{ Constant.EQUIPMENT_STATUS_CODE_NEW }</MenuItem>
                  <MenuItem key={ Constant.EQUIPMENT_STATUS_CODE_ARCHIVED } eventKey={ Constant.EQUIPMENT_STATUS_CODE_ARCHIVED }>{ Constant.EQUIPMENT_STATUS_CODE_ARCHIVED }</MenuItem>
                </DropdownButton>
                <Checkbox inline checked={ this.state.search.hired } onChange={ this.hiredChanged }>Hired</Checkbox>
                <MultiDropdown id="equipment-type-dropdown" placeholder="Equipment Types"
                  items={ equipmentTypes } selectedIds={ this.state.search.selectedEquipmentTypesIds } onChange={ this.equipmentTypesChanged } showMaxItems={ 2 } />
                <DropdownButton id="owner-dropdown" title={ this.state.search.ownerName } onSelect={ this.ownerSelected }>
                  <MenuItem key={ 0 } eventKey={ 0 }>&nbsp;</MenuItem>
                  {
                    _.map(owners, (owner) => {
                      return <MenuItem key={ owner.id } eventKey={ owner.id }>{ owner.name }</MenuItem>;
                    })
                  }
                </DropdownButton>
              </ButtonToolbar>
            </Row>
            <Row>
              <ButtonToolbar id="equipment-search-row-2">
                <DateControl date={ this.state.search.lastVerifiedDate } onChange={ this.lastVerifiedDateChanged } placeholder="mm/dd/yyyy" label="Not Verified Since:" title="last verified date"/>
                <Form id="equipment-attachments" inline>
                  <ControlLabel>Attachment:</ControlLabel>
                  <MultiDropdown id="attachment-type-dropdown" placeholder="Select Attachments"
                    items={ attachmentTypes } selectedIds={ this.state.search.selectedPhysicalAttachmentsIds } onChange={ this.equipmentTypesChanged } showMaxItems={ 2 } />
                </Form>
                <Button id="search-button" bsStyle="primary" onClick={ this.fetch }>Search</Button>
              </ButtonToolbar>
            </Row>
          </Col>
          <Col md={1}>
            <Row id="equipment-buttons">
              <ButtonGroup>
                <Button onClick={ this.print }><Glyphicon glyph="print" title="Print" /></Button>
              </ButtonGroup>
            </Row>
            <Row id="equipment-faves">
              <Favourites id="equipment-faves-dropdown" type="equipment" favourites={ this.props.favourites } onAdd={ this.saveFavourite } onSelect={ this.loadFavourite } pullRight />
            </Row>
          </Col>
        </Row>
      </Well>

      {(() => {
        if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }
        if (Object.keys(this.props.equipmentList).length === 0) { return <Alert bsStyle="success">No equipment</Alert>; }

        var equipmentList = _.sortBy(this.props.equipmentList, this.state.ui.sortField);
        if (this.state.ui.sortDesc) {
          _.reverse(equipmentList);
        }

        var buildHeader = (id, title) => {
          var sortGlyph = '';
          if (this.state.ui.sortField === id) {
            sortGlyph = <span>&nbsp;<Glyphicon glyph={ this.state.ui.sortDesc ? 'sort-by-attributes-alt' : 'sort-by-attributes' }/></span>;
          }
          return <th id={ id } onClick={ this.sort }>{ title }{ sortGlyph }</th>;
        };

        return <Table condensed striped>
          <thead>
            <tr>
              { buildHeader('equipCode', 'ID') }
              { buildHeader('typeName', 'Type') }
              { buildHeader('ownerName', 'Owner') }
              { buildHeader('seniorityNumber', 'Seniority') }
              { buildHeader('hiredStatus', 'Hired') }
              { buildHeader('make', 'Make') }
              { buildHeader('model', 'Model') }
              { buildHeader('size', 'Size') }
              { buildHeader('attachments', 'Attachments') }
              { buildHeader('lastVerifiedDate', 'Last Verified') }
              <th></th>
            </tr>
          </thead>
          <tbody>
          {
            _.map(equipmentList, (equip) => {
              return <tr key={ equip.id }>
                <td>{ equip.equipCd }</td>
                <td>{ equip.typeName }</td>
                <td><a href={ equip.ownerPath }>{ equip.ownerName }</a></td>
                <td>{ equip.seniorityDisplayNumber }</td>
                <td>{ equip.hiredStatus }</td>
                <td>{ equip.make }</td>
                <td>{ equip.model }</td>
                <td>{ equip.size }</td>
                <td>{ Object.keys(equip.equipmentAttachments).length }</td>
                <td>{ formatDateTime(equip.lastVerifiedDate, 'MM/DD/YYYY') }</td>
                <td style={{ textAlign: 'right' }}>
                  <LinkContainer to={{ pathname: 'equipment/' + equip.id }}>
                    <Button title="edit" bsSize="xsmall"><Glyphicon glyph="edit" /></Button>
                  </LinkContainer>
                </td>
              </tr>;
            })
          }
          </tbody>
        </Table>;
      })()}

    </div>;
  },
});


function mapStateToProps(state) {
  return {
    equipmentList: state.models.equipmentList,
    localAreas: state.lookups.localAreas,
    equipmentTypes: state.lookups.equipmentTypes,
    physicalAttachmentTypes: state.lookups.physicalAttachmentTypes,
    owners: state.lookups.owners,
    favourites: state.models.favourites,
    search: state.search.equipmentList,
    ui: state.ui.equipmentList,
  };
}

export default connect(mapStateToProps)(Equipment);
