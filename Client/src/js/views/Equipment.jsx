import React from 'react';

import { connect } from 'react-redux';

import { PageHeader, Well, Alert, Row, Col, ButtonToolbar, Button, ButtonGroup, Glyphicon, ControlLabel, Form, OverlayTrigger, Tooltip } from 'react-bootstrap';

import _ from 'lodash';
import Moment from 'moment';
import Promise from 'bluebird';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import CheckboxControl from '../components/CheckboxControl.jsx';
import DateControl from '../components/DateControl.jsx';
import DropdownControl from '../components/DropdownControl.jsx';
import Favourites from '../components/Favourites.jsx';
import FormInputControl from '../components/FormInputControl.jsx';
import MultiDropdown from '../components/MultiDropdown.jsx';
import Spinner from '../components/Spinner.jsx';
import TooltipButton from '../components/TooltipButton.jsx';

import EquipmentTable from './EquipmentTable.jsx';

import { toZuluTime } from '../utils/date';

var Equipment = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
    equipmentList: React.PropTypes.object,
    localAreas: React.PropTypes.object,
    districtEquipmentTypes: React.PropTypes.object,
    owners: React.PropTypes.object,
    favourites: React.PropTypes.object,
    search: React.PropTypes.object,
    ui: React.PropTypes.object,
  },

  getInitialState() {
    return {
      showAddDialog: false,
      search: {
        selectedLocalAreasIds: this.props.search.selectedLocalAreasIds || [],
        selectedEquipmentTypesIds: this.props.search.selectedEquipmentTypesIds || [],
        equipmentAttachment: this.props.search.equipmentAttachment || '',
        ownerName: this.props.search.ownerName || '',
        lastVerifiedDate: this.props.search.lastVerifiedDate || '',
        hired: this.props.search.hired || false,
        twentyYears: this.props.search.twentyYears || false,
        statusCode: this.props.search.statusCode || Constant.EQUIPMENT_STATUS_CODE_APPROVED,
        equipmentId: this.props.search.equipmentId || '',
        projectName: this.props.search.projectName || '',
      },
      ui : {
        sortField: this.props.ui.sortField || 'seniorityText',
        sortDesc: this.props.ui.sortDesc === true,
      },
    };
  },

  buildSearchParams() {
    var searchParams = {};

    if (this.state.search.equipmentAttachment) {
      searchParams.equipmentAttachment = this.state.search.equipmentAttachment;
    }

    if (this.state.search.ownerName) {
      searchParams.ownerName = this.state.search.ownerName;
    }

    if (this.state.search.hired) {
      searchParams.hired = this.state.search.hired;
    }

    if (this.state.search.twentyYears) {
      searchParams.twentyYears = this.state.search.twentyYears;
    }

    if (this.state.search.statusCode) {
      searchParams.status = this.state.search.statusCode;
    }

    if (this.state.search.selectedLocalAreasIds.length > 0) {
      searchParams.localareas = this.state.search.selectedLocalAreasIds;
    }

    if (this.state.search.selectedEquipmentTypesIds.length > 0) {
      searchParams.types = this.state.search.selectedEquipmentTypesIds;
    }

    if (this.state.search.equipmentId) {
      searchParams.equipmentId = this.state.search.equipmentId;
    }

    if (this.state.search.projectName) {
      searchParams.projectName = this.state.search.projectName;
    }

    var notVerifiedSinceDate = Moment(this.state.search.lastVerifiedDate);
    if (notVerifiedSinceDate && notVerifiedSinceDate.isValid()) {
      searchParams.notverifiedsincedate = toZuluTime(notVerifiedSinceDate.startOf('day'));
    }

    return searchParams;
  },

  componentDidMount() {
    var equipmentTypesPromise = Api.getDistrictEquipmentTypes(this.props.currentUser.district.id);
    var favouritesPromise = Api.getFavourites('equipment');

    Promise.all([equipmentTypesPromise, favouritesPromise]).then(() => {
      // If this is the first load, then look for a default favourite
      if (_.isEmpty(this.props.search)) {
        var defaultFavourite = _.find(this.props.favourites.data, f => f.isDefault);
        if (defaultFavourite) {
          this.loadFavourite(defaultFavourite);
          return;
        }
      }
    });
  },

  fetch() {
    Api.searchEquipmentList(this.buildSearchParams());
  },

  search(e) {
    e.preventDefault();
    this.fetch();
  },

  clearSearch() {
    var defaultSearchParameters = {
      selectedLocalAreasIds:[],
      selectedEquipmentTypesIds: [],
      equipmentAttachment: '',
      ownerName: '',
      lastVerifiedDate: '',
      hired: false,
      twentyYears: false,
      statusCode: Constant.EQUIPMENT_STATUS_CODE_APPROVED,
      equipmentId: '',
      projectName: '',
    };

    this.setState({ search: defaultSearchParameters }, () => {
      store.dispatch({ type: Action.UPDATE_EQUIPMENT_LIST_SEARCH, equipmentList: this.state.search });
      store.dispatch({ type: Action.CLEAR_EQUIPMENT_LIST });
    });
  },

  updateSearchState(state, callback) {
    this.setState({ search: { ...this.state.search, ...state }}, () => {
      store.dispatch({ type: Action.UPDATE_EQUIPMENT_LIST_SEARCH, equipmentList: this.state.search });
      if (callback) { callback(); }
    });
  },

  updateUIState(state, callback) {
    this.setState({ ui: { ...this.state.ui, ...state }}, () => {
      store.dispatch({ type: Action.UPDATE_EQUIPMENT_LIST_UI, equipmentList: this.state.ui });
      if (callback) { callback(); }
    });
  },

  loadFavourite(favourite) {
    this.updateSearchState(JSON.parse(favourite.value), this.fetch);
  },

  print() {
    window.print();
  },

  renderResults() {
    if (Object.keys(this.props.equipmentList.data).length === 0) {
      return <Alert bsStyle="success">No equipment</Alert>;
    }

    return (
      <EquipmentTable
        ui={this.state.ui}
        updateUIState={this.updateUIState}
        equipmentList={this.props.equipmentList.data}
      />
    );
  },

  render() {
    // Constrain the local area drop downs to those in the District of the current logged in user
    var localAreas = _.chain(this.props.localAreas)
      .sortBy('name')
      .value();

    var districtEquipmentTypes = _.chain(this.props.districtEquipmentTypes.data)
      .filter(type => type.district.id == this.props.currentUser.district.id)
      .sortBy('districtEquipmentName')
      .value();

    var resultCount = '';
    if (this.props.equipmentList.loaded) {
      resultCount = '(' + Object.keys(this.props.equipmentList.data).length + ')';
    }

    return <div id="equipment-list">
      <PageHeader>Equipment { resultCount }
        <ButtonGroup id="equipment-buttons">
          <TooltipButton onClick={ this.print } disabled={ !this.props.equipmentList.loaded } disabledTooltip={ 'Please complete the search to enable this function.' }>
            <Glyphicon glyph="print" title="Print" />
          </TooltipButton>
        </ButtonGroup>
      </PageHeader>
      <Well id="equipment-bar" bsSize="small" className="clearfix">
        <Form onSubmit={ this.search }>
          <Row>
            <Col xs={9} sm={10} id="equipment-filters">
              <Row>
                <ButtonToolbar id="equipment-filters-first-row">
                  <MultiDropdown id="selectedLocalAreasIds" placeholder="Local Areas"
                    items={ localAreas } selectedIds={ this.state.search.selectedLocalAreasIds } updateState={ this.updateSearchState } showMaxItems={ 2 } />
                  <DropdownControl id="statusCode" title={ this.state.search.statusCode } updateState={ this.updateSearchState } blankLine="(All)" placeholder="Status"
                    items={[ Constant.EQUIPMENT_STATUS_CODE_APPROVED, Constant.EQUIPMENT_STATUS_CODE_PENDING, Constant.EQUIPMENT_STATUS_CODE_ARCHIVED ]}
                  />
                  <MultiDropdown id="selectedEquipmentTypesIds" placeholder="Equipment Types" fieldName="districtEquipmentName"
                    items={ districtEquipmentTypes } selectedIds={ this.state.search.selectedEquipmentTypesIds } updateState={ this.updateSearchState } showMaxItems={ 2 } />
                  <FormInputControl id="ownerName" type="text" placeholder="Company Name" value={ this.state.search.ownerName } updateState={ this.updateSearchState } />
                  <CheckboxControl inline id="hired" checked={ this.state.search.hired } updateState={ this.updateSearchState }>Hired</CheckboxControl>
                  <OverlayTrigger placement="top" rootClose overlay={ <Tooltip id="old-equipment-tooltip">Equipment 20 years or older</Tooltip> }>
                    <span>
                      <CheckboxControl inline id="twentyYears" checked={ this.state.search.twentyYears } updateState={ this.updateSearchState }>20+ Years</CheckboxControl>
                    </span>
                  </OverlayTrigger>
                </ButtonToolbar>
              </Row>
              <Row>
                <ButtonToolbar id="equipment-filters-second-row">
                  <DateControl id="lastVerifiedDate" date={ this.state.search.lastVerifiedDate } updateState={ this.updateSearchState } label="Not Verified Since:" title="Last Verified Date"/>
                  <div className="input-container">
                    <ControlLabel>Attachment:</ControlLabel>
                    <FormInputControl id="equipmentAttachment" type="text" value={ this.state.search.equipmentAttachment } updateState={ this.updateSearchState } />
                  </div>
                  <div className="input-container">
                    <ControlLabel>Equipment Id:</ControlLabel>
                    <FormInputControl id="equipmentId" type="text" value={ this.state.search.equipmentId } updateState={ this.updateSearchState } />
                  </div>
                  <div className="input-container">
                    <ControlLabel>Project Name:</ControlLabel>
                    <FormInputControl id="projectName" type="text" value={ this.state.search.projectName } updateState={ this.updateSearchState } />
                  </div>
                </ButtonToolbar>
              </Row>
            </Col>
            <Col xs={3} sm={2} id="equipment-search-buttons">
              <Row>
                <Favourites id="equipment-faves-dropdown" type="equipment" favourites={ this.props.favourites.data } data={ this.state.search } onSelect={ this.loadFavourite } pullRight />
              </Row>
              <Row>
                <Button id="search-button" className="pull-right" bsStyle="primary" type="submit">Search</Button>
              </Row>
              <Row>
                <Button id="clear-search-button" className="pull-right" onClick={ this.clearSearch }>Clear</Button>
              </Row>
            </Col>
          </Row>
        </Form>
      </Well>

      {(() => {

        if (this.props.equipmentList.loading) {
          return <div style={{ textAlign: 'center' }}><Spinner/></div>;
        }

        if (this.props.equipmentList.loaded) {
          return this.renderResults();
        }

      })()}

    </div>;
  },
});


function mapStateToProps(state) {
  return {
    currentUser: state.user,
    equipmentList: state.models.equipmentList,
    localAreas: state.lookups.localAreas,
    districtEquipmentTypes: state.lookups.districtEquipmentTypes,
    favourites: state.models.favourites,
    search: state.search.equipmentList,
    ui: state.ui.equipmentList,
  };
}

export default connect(mapStateToProps)(Equipment);
