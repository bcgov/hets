import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Alert, Row, Col, ButtonToolbar, Button, ButtonGroup, OverlayTrigger, Tooltip } from 'react-bootstrap';
import _ from 'lodash';
import Moment from 'moment';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';

import PageHeader from '../components/ui/PageHeader.jsx';
import SearchBar from '../components/ui/SearchBar.jsx';
import CheckboxControl from '../components/CheckboxControl.jsx';
import DateControl from '../components/DateControl.jsx';
import DropdownControl from '../components/DropdownControl.jsx';
import Favourites from '../components/Favourites.jsx';
import FormInputControl from '../components/FormInputControl.jsx';
import MultiDropdown from '../components/MultiDropdown.jsx';
import Spinner from '../components/Spinner.jsx';
import Form from '../components/Form.jsx';
import EquipmentTable from './EquipmentTable.jsx';
import PrintButton from '../components/PrintButton.jsx';

import { toZuluTime } from '../utils/date';

class Equipment extends React.Component {
  static propTypes = {
    currentUser: PropTypes.object,
    equipmentList: PropTypes.object,
    localAreas: PropTypes.object,
    districtEquipmentTypes: PropTypes.object,
    owners: PropTypes.object,
    favourites: PropTypes.object,
    search: PropTypes.object,
    ui: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      showAddDialog: false,
      search: {
        selectedLocalAreasIds: props.search.selectedLocalAreasIds || [],
        selectedEquipmentTypesIds: props.search.selectedEquipmentTypesIds || [],
        equipmentAttachment: props.search.equipmentAttachment || '',
        ownerName: props.search.ownerName || '',
        lastVerifiedDate: props.search.lastVerifiedDate || '',
        hired: props.search.hired || false,
        twentyYears: props.search.twentyYears || false,
        statusCode: props.search.statusCode || Constant.EQUIPMENT_STATUS_CODE_APPROVED,
        equipmentId: props.search.equipmentId || '',
        projectName: props.search.projectName || '',
      },
      ui: {
        sortField: props.ui.sortField || 'seniorityText',
        sortDesc: props.ui.sortDesc === true,
      },
    };
  }

  buildSearchParams = () => {
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
  };

  componentDidMount() {
    this.props.dispatch(Api.getDistrictEquipmentTypes());

    // If this is the first load, then look for a default favourite
    if (_.isEmpty(this.props.search)) {
      var defaultFavourite = _.find(this.props.favourites, (f) => f.isDefault);
      if (defaultFavourite) {
        this.loadFavourite(defaultFavourite);
      }
    }
  }

  fetch = () => {
    this.props.dispatch(Api.searchEquipmentList(this.buildSearchParams()));
  };

  search = (e) => {
    e.preventDefault();
    this.fetch();
  };

  clearSearch = () => {
    var defaultSearchParameters = {
      selectedLocalAreasIds: [],
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
      this.props.dispatch({
        type: Action.UPDATE_EQUIPMENT_LIST_SEARCH,
        equipmentList: this.state.search,
      });
      this.props.dispatch({ type: Action.CLEAR_EQUIPMENT_LIST });
    });
  };

  updateSearchState = (state, callback) => {
    this.setState({ search: { ...this.state.search, ...state } }, () => {
      this.props.dispatch({
        type: Action.UPDATE_EQUIPMENT_LIST_SEARCH,
        equipmentList: this.state.search,
      });
      if (callback) {
        callback();
      }
    });
  };

  updateUIState = (state, callback) => {
    this.setState({ ui: { ...this.state.ui, ...state } }, () => {
      this.props.dispatch({
        type: Action.UPDATE_EQUIPMENT_LIST_UI,
        equipmentList: this.state.ui,
      });
      if (callback) {
        callback();
      }
    });
  };

  loadFavourite = (favourite) => {
    this.updateSearchState(JSON.parse(favourite.value), this.fetch);
  };

  renderResults = () => {
    if (Object.keys(this.props.equipmentList.data).length === 0) {
      return <Alert variant="success">No equipment</Alert>;
    }

    return (
      <EquipmentTable
        ui={this.state.ui}
        updateUIState={this.updateUIState}
        equipmentList={this.props.equipmentList.data}
      />
    );
  };

  render() {
    // Constrain the local area drop downs to those in the District of the current logged in user
    var localAreas = _.chain(this.props.localAreas).sortBy('name').value();

    var districtEquipmentTypes = _.chain(this.props.districtEquipmentTypes.data)
      .filter((type) => type.district.id === this.props.currentUser.district.id)
      .sortBy('districtEquipmentName')
      .value();

    var resultCount = '';
    if (this.props.equipmentList.loaded) {
      resultCount = '(' + Object.keys(this.props.equipmentList.data).length + ')';
    }

    return (
      <div id="equipment-list">
        <PageHeader>
          Equipment {resultCount}
          <ButtonGroup>
            <PrintButton disabled={!this.props.equipmentList.loaded} />
          </ButtonGroup>
        </PageHeader>
        <SearchBar>
          <Form onSubmit={this.search}>
            <Row>
              <Col xs={9} sm={10} id="filters">
                <Row>
                  <ButtonToolbar>
                    <MultiDropdown
                      id="selectedLocalAreasIds"
                      placeholder="Local Areas"
                      items={localAreas}
                      selectedIds={this.state.search.selectedLocalAreasIds}
                      updateState={this.updateSearchState}
                      showMaxItems={2}
                    />
                    <DropdownControl
                      id="statusCode"
                      title={this.state.search.statusCode}
                      updateState={this.updateSearchState}
                      blankLine="(All)"
                      placeholder="Status"
                      items={[
                        Constant.EQUIPMENT_STATUS_CODE_APPROVED,
                        Constant.EQUIPMENT_STATUS_CODE_PENDING,
                        Constant.EQUIPMENT_STATUS_CODE_ARCHIVED,
                      ]}
                    />
                    <MultiDropdown
                      id="selectedEquipmentTypesIds"
                      placeholder="Equipment Types"
                      fieldName="districtEquipmentName"
                      items={districtEquipmentTypes}
                      disabled={!this.props.districtEquipmentTypes.loaded}
                      selectedIds={this.state.search.selectedEquipmentTypesIds}
                      updateState={this.updateSearchState}
                      showMaxItems={2}
                    />
                    <FormInputControl
                      id="ownerName"
                      type="text"
                      placeholder="Company Name or DBA"
                      title="Searches Company Name And Doing Business As Fields."
                      value={this.state.search.ownerName}
                      updateState={this.updateSearchState}
                    />
                    <CheckboxControl
                      inline
                      id="hired"
                      checked={this.state.search.hired}
                      updateState={this.updateSearchState}
                      label="Hired"
                    />

                    <OverlayTrigger
                      placement="top"
                      trigger={['hover', 'focus']}
                      rootClose
                      overlay={<Tooltip id="old-equipment-tooltip">Equipment 20 years or older</Tooltip>}
                    >
                      <CheckboxControl
                        inline
                        id="twentyYears"
                        checked={this.state.search.twentyYears}
                        updateState={this.updateSearchState}
                        label="20+ Years"
                      />
                    </OverlayTrigger>
                  </ButtonToolbar>
                </Row>
                <Row>
                  <ButtonToolbar>
                    <DateControl
                      id="lastVerifiedDate"
                      date={this.state.search.lastVerifiedDate}
                      label="Not Verified Since:"
                      title="Last Verified Date"
                      updateState={this.updateSearchState}
                    />
                    <FormInputControl
                      id="equipmentAttachment"
                      placeholder="Attachment"
                      type="text"
                      value={this.state.search.equipmentAttachment}
                      updateState={this.updateSearchState}
                    />
                    <FormInputControl
                      id="equipmentId"
                      placeholder="Equipment Id"
                      type="text"
                      value={this.state.search.equipmentId}
                      updateState={this.updateSearchState}
                    />
                    <FormInputControl
                      id="projectName"
                      placeholder="Project Name"
                      type="text"
                      value={this.state.search.projectName}
                      updateState={this.updateSearchState}
                    />
                  </ButtonToolbar>
                </Row>
              </Col>
              <Col xs={3} sm={2} id="search-buttons">
                <Row className="float-right">
                  <Favourites
                    id="faves-dropdown"
                    type="equipment"
                    favourites={this.props.favourites}
                    data={this.state.search}
                    onSelect={this.loadFavourite}
                  />
                </Row>
                <Row className="float-right">
                  <Button id="search-button" variant="primary" type="submit">
                    Search
                  </Button>
                </Row>
                <Row className="float-right">
                  <Button id="clear-search-button" className="btn-custom" onClick={this.clearSearch}>
                    Clear
                  </Button>
                </Row>
              </Col>
            </Row>
          </Form>
        </SearchBar>

        {(() => {
          if (this.props.equipmentList.loading) {
            return (
              <div style={{ textAlign: 'center' }}>
                <Spinner />
              </div>
            );
          }

          if (this.props.equipmentList.loaded) {
            return this.renderResults();
          }
        })()}
      </div>
    );
  }
}

const mapStateToProps = (state) => ({
  currentUser: state.user,
  equipmentList: state.models.equipmentList,
  localAreas: state.lookups.localAreas,
  districtEquipmentTypes: state.lookups.districtEquipmentTypes,
  favourites: state.models.favourites.equipment,
  search: state.search.equipmentList,
  ui: state.ui.equipmentList,
});

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(mapStateToProps, mapDispatchToProps)(Equipment);
