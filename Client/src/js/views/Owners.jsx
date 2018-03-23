import React from 'react';

import { connect } from 'react-redux';

import { PageHeader, Well, Alert, Row, Col, ButtonToolbar, Button, ButtonGroup, Glyphicon, Form } from 'react-bootstrap';

import _ from 'lodash';
import Promise from 'bluebird';

import OwnersAddDialog from './dialogs/OwnersAddDialog.jsx';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import * as Log from '../history';
import store from '../store';

import CheckboxControl from '../components/CheckboxControl.jsx';
import DropdownControl from '../components/DropdownControl.jsx';
import EditButton from '../components/EditButton.jsx';
import Favourites from '../components/Favourites.jsx';
import FilterDropdown from '../components/FilterDropdown.jsx';
import MultiDropdown from '../components/MultiDropdown.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';

var Owners = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
    ownerList: React.PropTypes.object,
    owner: React.PropTypes.object,
    localAreas: React.PropTypes.object,
    districtEquipmentTypes: React.PropTypes.object,
    owners: React.PropTypes.object,
    favourites: React.PropTypes.object,
    search: React.PropTypes.object,
    ui: React.PropTypes.object,
    router: React.PropTypes.object,
  },

  getInitialState() {
    return {
      showAddDialog: false,

      search: {
        selectedLocalAreasIds: this.props.search.selectedLocalAreasIds || [],
        selectedEquipmentTypesIds: this.props.search.selectedEquipmentTypesIds || [],
        ownerId: this.props.search.ownerId || 0,
        ownerName: this.props.search.ownerName || 'Owner',
        hired: this.props.search.hired || false,
        statusCode: this.props.search.statusCode || Constant.OWNER_STATUS_CODE_APPROVED,
      },

      ui : {
        sortField: this.props.ui.sortField || 'organizationName',
        sortDesc: this.props.ui.sortDesc === true,
      },
    };
  },

  buildSearchParams() {
    var searchParams = {};

    if (this.state.search.ownerId) {
      searchParams.owner = this.state.search.ownerId;
    }

    if (this.state.search.hired) {
      searchParams.hired = this.state.search.hired;
    }

    if (this.state.search.statusCode) {
      searchParams.status = this.state.search.statusCode;
    }

    if (this.state.search.selectedLocalAreasIds.length > 0) {
      searchParams.localareas = this.state.search.selectedLocalAreasIds;
    }
    if (this.state.search.selectedEquipmentTypesIds.length > 0) {
      searchParams.equipmenttypes = this.state.search.selectedEquipmentTypesIds;
    }

    return searchParams;
  },

  componentDidMount() {
    var equipmentTypesPromise = Api.getDistrictEquipmentTypes(this.props.currentUser.district.id);
    var ownersPromise = Api.getOwnersByDistrict(this.props.currentUser.district.id);
    var favouritesPromise = Api.getFavourites('owner');

    Promise.all([equipmentTypesPromise, ownersPromise, favouritesPromise]).then(() => {
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
    Api.searchOwners(this.buildSearchParams());
  },

  search(e) {
    e.preventDefault(); 
    this.fetch();
  },

  updateSearchState(state, callback) {
    this.setState({ search: { ...this.state.search, ...state, ...{ loaded: true } }}, () => {
      store.dispatch({ type: Action.UPDATE_OWNERS_SEARCH, owners: this.state.search });
      if (callback) { callback(); }
    });
  },

  updateUIState(state, callback) {
    this.setState({ ui: { ...this.state.ui, ...state }}, () => {
      store.dispatch({ type: Action.UPDATE_OWNERS_UI, owners: this.state.ui });
      if (callback) { callback(); }
    });
  },

  loadFavourite(favourite) {
    this.updateSearchState(JSON.parse(favourite.value), this.fetch);
  },

  openAddDialog() {
    this.setState({ showAddDialog: true });
  },

  closeAddDialog() {
    this.setState({ showAddDialog: false });
  },

  saveNewOwner(owner) {
    Api.addOwner(owner).then(() => {
      Log.ownerAdded(this.props.owner);
      // Open it up
      this.props.router.push({
        pathname: `${ Constant.OWNERS_PATHNAME }/${ this.props.owner.id }`,
      });
    });
  },
  
  print() {
    window.print();
  },

  verifyOwners(ownerList) {
    var owners = _.map(ownerList, owner => {
      return owner.id;
    });
    Api.verifyOwners(owners).then((response) => {
      var blob;
      if (window.navigator.msSaveBlob) {
        blob = window.navigator.msSaveBlob(response, 'owner_status_letters.pdf');
      } else {
        blob = new Blob([response], {type: 'image/pdf'}); 
      }
      //Create a link element, hide it, direct 
      //it towards the blob, and then 'click' it programatically
      let a = document.createElement('a');
      a.style = 'display: none';
      document.body.appendChild(a);
      //Create a DOMString representing the blob 
      //and point the link element towards it
      let url = window.URL.createObjectURL(blob);
      a.href = url;
      a.download = 'ownersVerificationLetters.pdf';
      //programatically click the link to trigger the download
      a.click();
      //release the reference to the file by revoking the Object URL
      window.URL.revokeObjectURL(url);
    });
  },

  render() {
    // Constrain the local area drop downs to those in the District of the current logged in user
    var localAreas = _.chain(this.props.localAreas)
      .sortBy('name')
      .value();

    var owners = _.chain(this.props.owners.data)
      .sortBy('organizationName')
      .value();

    var districtEquipmentTypes = _.chain(this.props.districtEquipmentTypes.data)
      .filter(type => type.district.id == this.props.currentUser.district.id)
      .sortBy('districtEquipmentName')
      .value();

    var numOwners = this.props.ownerList.loading ? '...' : Object.keys(this.props.ownerList.data).length;

    var ownerList = _.sortBy(this.props.ownerList.data, owner => {
      if (typeof owner[this.state.ui.sortField] === 'string') {
        return owner[this.state.ui.sortField].toLowerCase();
      }
      return owner[this.state.ui.sortField];
    });
    
    if (this.state.ui.sortDesc) {
      _.reverse(ownerList);
    }

    return <div id="owners-list">
      <PageHeader>Owners ({ numOwners })
        <div id="owners-buttons">
          <Button onClick={ this.verifyOwners.bind(this, ownerList) }>Status Letters</Button>
          <ButtonGroup>
            <Button onClick={ this.print }><Glyphicon glyph="print" title="Print" /></Button>
          </ButtonGroup>
        </div>
      </PageHeader>
      <Well id="owners-bar" bsSize="small" className="clearfix">
        <Row>
          <Col sm={10}>
            <Form onSubmit={ this.search }>
              <ButtonToolbar id="owners-filters">
                <MultiDropdown id="selectedLocalAreasIds" placeholder="Local Areas"
                  items={ localAreas } selectedIds={ this.state.search.selectedLocalAreasIds } updateState={ this.updateSearchState } showMaxItems={ 2 } />
                <DropdownControl id="statusCode" title={ this.state.search.statusCode } updateState={ this.updateSearchState } blankLine="(All)" placeholder="Status"
                  items={[ Constant.OWNER_STATUS_CODE_APPROVED, Constant.OWNER_STATUS_CODE_PENDING, Constant.OWNER_STATUS_CODE_ARCHIVED ]} />
                <MultiDropdown id="selectedEquipmentTypesIds" placeholder="Equipment Types" fieldName="districtEquipmentName"
                  items={ districtEquipmentTypes } selectedIds={ this.state.search.selectedEquipmentTypesIds } updateState={ this.updateSearchState } showMaxItems={ 2 } />
                <FilterDropdown id="ownerId" placeholder="Owner" fieldName="organizationName" blankLine="(All)"
                  items={ owners } selectedId={ this.state.search.ownerId } updateState={ this.updateSearchState } />
                <CheckboxControl inline id="hired" checked={ this.state.search.hired } updateState={ this.updateSearchState }>Hired</CheckboxControl>
                <Button id="search-button" bsStyle="primary" type="submit">Search</Button>
              </ButtonToolbar>
            </Form>
          </Col>
          <Col sm={2}>
            <Row id="owners-faves">
              <Favourites id="owners-faves-dropdown" type="owner" favourites={ this.props.favourites.data } data={ this.state.search } onSelect={ this.loadFavourite } pullRight />
            </Row>
          </Col>
        </Row>
      </Well>

      {(() => {
        var addOwnerButton = <Button title="Add Owner" bsSize="xsmall" onClick={ this.openAddDialog }>
          <Glyphicon glyph="plus" />&nbsp;<strong>Add Owner</strong>
        </Button>;
        if (this.props.owners.loading || this.props.ownerList.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }
        if (Object.keys(this.props.ownerList.data).length === 0) { return <Alert bsStyle="success">No owners { addOwnerButton }</Alert>; }

        return <SortTable sortField={ this.state.ui.sortField } sortDesc={ this.state.ui.sortDesc } onSort={ this.updateUIState } headers={[
          { field: 'localAreaName',          title: 'Local Area'                                      },
          { field: 'organizationName',       title: 'Company'                                         },
          { field: 'primaryContactName',     title: 'Primary Contact'                                 },
          { field: 'equipmentCount',         title: 'Equipment',       style: { textAlign: 'center' } },
          { field: 'status',                 title: 'Status',          style: { textAlign: 'center' } },
          { field: 'addOwner',               title: 'Add Owner',       style: { textAlign: 'right'  },
            node: addOwnerButton,
          },
        ]}>
          {
            _.map(ownerList, (owner) => {
              return <tr key={ owner.id } className={ owner.status === Constant.OWNER_STATUS_CODE_APPROVED ? null : 'info' }>
                <td>{ owner.localAreaName }</td>
                <td>{ owner.organizationName }</td>
                <td>{ owner.primaryContactName }</td>
                <td style={{ textAlign: 'center' }}>{ owner.equipmentCount }</td>
                <td style={{ textAlign: 'center' }}>{ owner.status }</td>
                <td style={{ textAlign: 'right' }}>
                  <ButtonGroup>
                    <EditButton name="Owner" view pathname={ `${ Constant.OWNERS_PATHNAME }/${ owner.id }` }/>
                  </ButtonGroup>
                </td>
              </tr>;
            })
          }
        </SortTable>;
      })()}
      { this.state.showAddDialog &&
        <OwnersAddDialog show={ this.state.showAddDialog } onSave={ this.saveNewOwner } onClose={ this.closeAddDialog } />
      }
    </div>;
  },
});

function mapStateToProps(state) {
  return {
    currentUser: state.user,
    ownerList: state.models.owners,
    owner: state.models.owner,
    localAreas: state.lookups.localAreas,
    districtEquipmentTypes: state.lookups.districtEquipmentTypes,
    owners: state.lookups.owners,
    favourites: state.models.favourites,
    search: state.search.owners,
    ui: state.ui.owners,
  };
}

export default connect(mapStateToProps)(Owners);
