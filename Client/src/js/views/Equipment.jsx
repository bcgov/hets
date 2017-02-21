import React from 'react';

import { connect } from 'react-redux';

import { Well, Alert, Row, Col } from 'react-bootstrap';
import { ButtonToolbar, Button, ButtonGroup, Glyphicon } from 'react-bootstrap';
import { ControlLabel } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';

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
import FilterDropdown from '../components/FilterDropdown.jsx';
import MultiDropdown from '../components/MultiDropdown.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';

import { formatDateTime } from '../utils/date';

/*

TODO:
* Print

*/

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
        ownerId: this.props.search.ownerId || 0,
        ownerName: this.props.search.ownerName || 'Owner',
        lastVerifiedDate: this.props.search.lastVerifiedDate || '',
        hired: this.props.search.hired !== false,
        statusCode: this.props.search.statusCode || Constant.EQUIPMENT_STATUS_CODE_APPROVED,
      },

      ui : {
        sortField: this.props.ui.sortField || 'organizationName',
        sortDesc: this.props.ui.sortDesc === true,
      },
    };
  },

  buildSearchParams() {
    var searchParams = {
      hired: this.state.search.hired,
      owner: this.state.search.ownerId || '',
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
    // TODO Implement
  },

  render() {
    var localAreas = _.sortBy(this.props.localAreas, 'name');
    var owners = _.sortBy(this.props.owners, 'name');
    var equipmentTypes = _.sortBy(this.props.equipmentTypes, 'description');
    var attachmentTypes = _.sortBy(this.props.physicalAttachmentTypes, 'name');

    return <div id="equipment-list">
      <Well id="equipment-bar" bsSize="small" className="clearfix">
        <Row>
          <Col md={11}>
            <Row>
              <ButtonToolbar id="equipment-search-row-1">
                <MultiDropdown id="selectedLocalAreasIds" placeholder="Local Areas"
                  items={ localAreas } selectedIds={ this.state.search.selectedLocalAreasIds } updateState={ this.updateSearchState } showMaxItems={ 2 } />
                <DropdownControl id="statusCode" title={ this.state.search.statusCode } updateState={ this.updateSearchState }
                  items={[ Constant.EQUIPMENT_STATUS_CODE_APPROVED, Constant.EQUIPMENT_STATUS_CODE_PENDING, Constant.EQUIPMENT_STATUS_CODE_ARCHIVED ]}
                />
                <CheckboxControl inline id="hired" checked={ this.state.search.hired } updateState={ this.updateSearchState }>Hired</CheckboxControl>
                <MultiDropdown id="selectedEquipmentTypesIds" placeholder="Equipment Types" fieldName="description"
                  items={ equipmentTypes } selectedIds={ this.state.search.selectedEquipmentTypesIds } updateState={ this.updateSearchState } showMaxItems={ 2 } />
                <FilterDropdown id="ownerId" placeholder="Owner" blankLine 
                  items={ owners } selectedId={ this.state.search.ownerId } updateState={ this.updateSearchState } />
              </ButtonToolbar>
            </Row>
            <Row>
              <ButtonToolbar id="equipment-search-row-2">
                <DateControl id="lastVerifiedDate" date={ this.state.search.lastVerifiedDate } updateState={ this.updateSearchState } placeholder="mm/dd/yyyy" label="Not Verified Since:" title="last verified date"/>
                <div id="equipment-attachments">
                  <ControlLabel>Attachment:</ControlLabel>
                  <MultiDropdown id="selectedPhysicalAttachmentsIds" placeholder="Select Attachments"
                    items={ attachmentTypes } selectedIds={ this.state.search.selectedPhysicalAttachmentsIds } updateState={ this.updateSearchState } showMaxItems={ 2 } />
                </div>
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
              <Favourites id="equipment-faves-dropdown" type="equipment" favourites={ this.props.favourites } data={ this.state.search } onSelect={ this.loadFavourite } pullRight />
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
        return <SortTable sortField={ this.state.ui.sortField } sortDesc={ this.state.ui.sortDesc } onSort={ this.updateUIState } headers={[
            { field: 'equipmentCode',        title: 'ID'            },
            { field: 'typeName',             title: 'Type'          },
            { field: 'organizationName',     title: 'Owner'         },
            { field: 'seniorityText',        title: 'Seniority'     },
            { field: 'isWorking',            title: 'Hired'         },
            { field: 'make',                 title: 'Make'          },
            { field: 'model',                title: 'Model'         },
            { field: 'size',                 title: 'Size'          },
            { field: 'attachments',          title: 'Attachments'   },
            { field: 'lastVerifiedDate',     title: 'Last Verified' },
            { field: 'blank' },
        ]}>
          {
            _.map(equipmentList, (equip) => {
              return <tr key={ equip.id }>
                <td>{ equip.equipmentCode }</td>
                <td>{ equip.typeName }</td>
                <td><a href={ equip.ownerPath }>{ equip.organizationName }</a></td>
                <td>{ equip.seniorityText }</td>
                <td>{ equip.isWorking ? equip.currentWorkDescription : 'N' }</td>
                <td>{ equip.make }</td>
                <td>{ equip.model }</td>
                <td>{ equip.size }</td>
                <td>{ Object.keys(equip.equipmentAttachments).length }</td>
                <td>{ equip.isApproved ? formatDateTime(equip.lastVerifiedDate, 'YYYY-MMM-DD') : 'Not Approved' }</td>
                <td style={{ textAlign: 'right' }}>
                  <LinkContainer to={{ pathname: 'equipment/' + equip.id }}>
                    <Button title="edit" bsSize="xsmall"><Glyphicon glyph="edit" /></Button>
                  </LinkContainer>
                </td>
              </tr>;
            })
          }
        </SortTable>;
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
