import React from 'react';

import { connect } from 'react-redux';

import { PageHeader, Well, Alert, Row, Col, ButtonToolbar, Button, ButtonGroup, Glyphicon, Form } from 'react-bootstrap';

import _ from 'lodash';

import OwnersAddDialog from './dialogs/OwnersAddDialog.jsx';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import * as Log from '../history';
import store from '../store';

import DropdownControl from '../components/DropdownControl.jsx';
import EditButton from '../components/EditButton.jsx';
import Favourites from '../components/Favourites.jsx';
import FormInputControl from '../components/FormInputControl.jsx';
import MultiDropdown from '../components/MultiDropdown.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import TooltipButton from '../components/TooltipButton.jsx';

import { formatDateTimeUTCToLocal } from '../utils/date';

var Owners = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
    ownerList: React.PropTypes.object,
    owner: React.PropTypes.object,
    localAreas: React.PropTypes.object,
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
        ownerCode: this.props.search.ownerCode || '',
        ownerName: this.props.search.ownerName || '',
        statusCode:  this.props.search.statusCode || Constant.OWNER_STATUS_CODE_APPROVED,
      },

      ui : {
        sortField: this.props.ui.sortField || 'organizationName',
        sortDesc: this.props.ui.sortDesc === true,
      },
    };
  },

  buildSearchParams() {
    var searchParams = {};

    if (this.state.search.ownerCode) {
      searchParams.ownerCode = this.state.search.ownerCode;
    }

    if (this.state.search.ownerName) {
      searchParams.ownerName = this.state.search.ownerName;
    }

    if (this.state.search.statusCode) {
      searchParams.status = this.state.search.statusCode;
    }

    if (this.state.search.selectedLocalAreasIds.length > 0) {
      searchParams.localareas = this.state.search.selectedLocalAreasIds;
    }

    return searchParams;
  },

  componentDidMount() {
    Api.getFavourites('owner').then(() => {
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
    Api.searchOwners(this.buildSearchParams());
  },

  search(e) {
    e.preventDefault(); 
    this.fetch();
  },

  clearSearch() {
    var defaultSearchParameters = {
      selectedLocalAreasIds: [],
      ownerCode: '',
      ownerName: '',
      statusCode: Constant.OWNER_STATUS_CODE_APPROVED,
    };

    this.setState({ search: defaultSearchParameters }, () => {
      store.dispatch({ type: Action.UPDATE_OWNERS_SEARCH, owners: this.state.search });
      store.dispatch({ type: Action.CLEAR_OWNERS });
    });
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

      var filename = 'StatusLetters-' + formatDateTimeUTCToLocal(new Date(), Constant.DATE_TIME_FILENAME) + '.pdf';

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

  renderResults(ownerList, addOwnerButton) {
    if (Object.keys(this.props.ownerList.data).length === 0) { return <Alert bsStyle="success">No owners { addOwnerButton }</Alert>; }

    return <SortTable sortField={ this.state.ui.sortField } sortDesc={ this.state.ui.sortDesc } onSort={ this.updateUIState } headers={[
      { field: 'ownerCode',              title: 'Owner Code'                                      },
      { field: 'localAreaName',          title: 'Local Area'                                      },
      { field: 'organizationName',       title: 'Company Name'                                    },
      { field: 'primaryContactName',     title: 'Primary Contact Name'                            },
      { field: 'primaryContactNumber',   title: 'Primary Contact Number'                          },
      { field: 'equipmentCount',         title: 'Equipment',       style: { textAlign: 'center' } },
      { field: 'status',                 title: 'Status',          style: { textAlign: 'center' } },
      { field: 'addOwner',               title: 'Add Owner',       style: { textAlign: 'right'  },
        node: addOwnerButton,
      },
    ]}>
      {
        _.map(ownerList, (owner) => {
          return <tr key={ owner.id } className={ owner.status === Constant.OWNER_STATUS_CODE_APPROVED ? null : 'info' }>
            <td>{ owner.ownerCode }</td>
            <td>{ owner.localAreaName }</td>
            <td>{ owner.organizationName }</td>
            <td>{ owner.primaryContactName }</td>
            <td>{ owner.primaryContactNumber }</td>
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
  },

  render() {
    // Constrain the local area drop downs to those in the District of the current logged in user
    var localAreas = _.chain(this.props.localAreas)
      .sortBy('name')
      .value();

    var resultCount = '';
    if (this.props.ownerList.loaded) {
      resultCount = '(' + Object.keys(this.props.ownerList.data).length + ')';
    }

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
      <PageHeader>Owners { resultCount }
        <div id="owners-buttons">
          <TooltipButton className="mr-5" onClick={ this.verifyOwners.bind(this, ownerList) } disabled={ !this.props.ownerList.loaded } disabledTooltip={ 'Please complete the search to enable this function.' }>
            Status Letters
          </TooltipButton>
          <ButtonGroup>
            <TooltipButton onClick={ this.print } disabled={ !this.props.ownerList.loaded } disabledTooltip={ 'Please complete the search to enable this function.' }>
              <Glyphicon glyph="print" title="Print" />
            </TooltipButton>
          </ButtonGroup>
        </div>
      </PageHeader>
      <Well id="owners-bar" bsSize="small" className="clearfix">
        <Row>
          <Col xs={9} sm={10}>
            <Form onSubmit={ this.search }>
              <ButtonToolbar id="owners-filters">
                <MultiDropdown id="selectedLocalAreasIds" placeholder="Local Areas"
                  items={ localAreas } selectedIds={ this.state.search.selectedLocalAreasIds } updateState={ this.updateSearchState } showMaxItems={ 2 } />
                <DropdownControl id="statusCode" title={ this.state.search.statusCode } updateState={ this.updateSearchState } blankLine="(All)" placeholder="Status"
                  items={[ Constant.OWNER_STATUS_CODE_APPROVED, Constant.OWNER_STATUS_CODE_PENDING, Constant.OWNER_STATUS_CODE_ARCHIVED ]} />
                <FormInputControl id="ownerCode" type="text" placeholder="Owner Code" value={ this.state.search.ownerCode } updateState={ this.updateSearchState } />
                <FormInputControl id="ownerName" type="text" placeholder="Company Name" value={ this.state.search.ownerName } updateState={ this.updateSearchState } />
                <Button id="search-button" bsStyle="primary" type="submit">Search</Button>
                <Button id="clear-search-button" onClick={ this.clearSearch }>Clear</Button>
              </ButtonToolbar>
            </Form>
          </Col>
          <Col xs={3} sm={2}>
            <Favourites id="owners-faves-dropdown" type="owner" favourites={ this.props.favourites.data } data={ this.state.search } onSelect={ this.loadFavourite } pullRight />
          </Col>
        </Row>
      </Well>

      {(() => {
        if (this.props.ownerList.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

        var addOwnerButton = <Button title="Add Owner" bsSize="xsmall" onClick={ this.openAddDialog }>
          <Glyphicon glyph="plus" />&nbsp;<strong>Add Owner</strong>
        </Button>;
        
        if (this.props.ownerList.loaded) {
          return this.renderResults(ownerList, addOwnerButton);
        }

        return <div id="add-button-container">{ addOwnerButton }</div>;
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
    owners: state.lookups.owners,
    favourites: state.models.favourites,
    search: state.search.owners,
    ui: state.ui.owners,
  };
}

export default connect(mapStateToProps)(Owners);
