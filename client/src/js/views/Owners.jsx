import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Alert, Row, Col, ButtonToolbar, Button, ButtonGroup } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import _ from 'lodash';
import OwnersAddDialog from './dialogs/OwnersAddDialog.jsx';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';

import AddButtonContainer from '../components/ui/AddButtonContainer.jsx';
import PageHeader from '../components/ui/PageHeader.jsx';
import SearchBar from '../components/ui/SearchBar.jsx';
import DropdownControl from '../components/DropdownControl.jsx';
import EditButton from '../components/EditButton.jsx';
import Favourites from '../components/Favourites.jsx';
import FormInputControl from '../components/FormInputControl.jsx';
import MultiDropdown from '../components/MultiDropdown.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import Form from '../components/Form.jsx';
import PrintButton from '../components/PrintButton.jsx';
import Authorize from '../components/Authorize.jsx';

import { sort, caseInsensitiveSort } from '../utils/array.js';

class Owners extends React.Component {
  static propTypes = {
    currentUser: PropTypes.object,
    ownerList: PropTypes.object,
    localAreas: PropTypes.object,
    favourites: PropTypes.object,
    search: PropTypes.object,
    ui: PropTypes.object,
    history: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      showAddDialog: false,

      search: {
        selectedLocalAreasIds: props.search.selectedLocalAreasIds || [],
        ownerCode: props.search.ownerCode || '',
        ownerName: props.search.ownerName || '',
        statusCode: props.search.statusCode || Constant.OWNER_STATUS_CODE_APPROVED,
      },

      ui: {
        sortField: props.ui.sortField || 'organizationName',
        sortDesc: props.ui.sortDesc === true,
      },
    };
  }

  buildSearchParams = () => {
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
  };

  componentDidMount() {
    // If this is the first load, then look for a default favourite
    if (_.isEmpty(this.props.search)) {
      var defaultFavourite = _.find(this.props.favourites, (f) => f.isDefault);
      if (defaultFavourite) {
        this.loadFavourite(defaultFavourite);
      }
    }
  }

  fetch = () => {
    this.props.dispatch(Api.searchOwners(this.buildSearchParams()));
  };

  search = () => {
    this.fetch();
  };

  clearSearch = () => {
    var defaultSearchParameters = {
      selectedLocalAreasIds: [],
      ownerCode: '',
      ownerName: '',
      statusCode: Constant.OWNER_STATUS_CODE_APPROVED,
    };

    this.setState({ search: defaultSearchParameters }, () => {
      this.props.dispatch({ type: Action.UPDATE_OWNERS_SEARCH, owners: this.state.search });
      this.props.dispatch({ type: Action.CLEAR_OWNERS });
    });
  };

  updateSearchState = (state, callback) => {
    this.setState({ search: { ...this.state.search, ...state, ...{ loaded: true } } }, () => {
      this.props.dispatch({ type: Action.UPDATE_OWNERS_SEARCH, owners: this.state.search });
      if (callback) {
        callback();
      }
    });
  };

  updateUIState = (state, callback) => {
    this.setState({ ui: { ...this.state.ui, ...state } }, () => {
      this.props.dispatch({ type: Action.UPDATE_OWNERS_UI, owners: this.state.ui });
      if (callback) {
        callback();
      }
    });
  };

  loadFavourite = (favourite) => {
    this.updateSearchState(JSON.parse(favourite.value), this.fetch);
  };

  openAddDialog = () => {
    this.setState({ showAddDialog: true });
  };

  closeAddDialog = () => {
    this.setState({ showAddDialog: false });
  };

  ownerSaved = (owner) => {
    this.fetch();
    this.props.history.push(`${Constant.OWNERS_PATHNAME}/${owner.id}`);
  };

  renderResults = (ownerList, addOwnerButton) => {
    if (Object.keys(this.props.ownerList.data).length === 0) {
      return <Alert variant="success">No owners {addOwnerButton}</Alert>;
    }

    return (
      <SortTable
        sortField={this.state.ui.sortField}
        sortDesc={this.state.ui.sortDesc}
        onSort={this.updateUIState}
        headers={[
          { field: 'ownerCode', title: 'Owner Code' },
          { field: 'localAreaName', title: 'Local Area' },
          { field: 'organizationName', title: 'Company Name', style: { width: '15%' } },
          { field: 'primaryContactName', title: 'Primary Contact Name' },
          { field: 'workPhoneNumber', title: 'Phone' },
          { field: 'mobilePhoneNumber', title: 'Cell Phone' },
          { field: 'equipmentCount', title: 'Equipment', style: { textAlign: 'center' } },
          { field: 'status', title: 'Status', style: { textAlign: 'center' } },
          {
            field: 'addOwner',
            title: 'Add Owner',
            style: { textAlign: 'right', width: '100px' },
            node: addOwnerButton,
          },
        ]}
      >
        {_.map(ownerList, (owner) => {
          return (
            <tr key={owner.id} className={owner.status === Constant.OWNER_STATUS_CODE_APPROVED ? null : 'bg-info'}>
              <td>{owner.ownerCode}</td>
              <td>{owner.localAreaName}</td>
              <td>{owner.organizationName}</td>
              <td>{owner.primaryContactName}</td>
              <td>{owner.workPhoneNumber}</td>
              <td>{owner.mobilePhoneNumber}</td>
              <td style={{ textAlign: 'center' }}>{owner.equipmentCount}</td>
              <td style={{ textAlign: 'center' }}>{owner.status}</td>
              <td style={{ textAlign: 'right' }}>
                <ButtonGroup>
                  <EditButton name="Owner" view pathname={`${Constant.OWNERS_PATHNAME}/${owner.id}`} />
                </ButtonGroup>
              </td>
            </tr>
          );
        })}
      </SortTable>
    );
  };

  render() {
    // Constrain the local area drop downs to those in the District of the current logged in user
    var localAreas = _.chain(this.props.localAreas).sortBy('name').value();

    var resultCount = '';
    if (this.props.ownerList.loaded) {
      resultCount = '(' + Object.keys(this.props.ownerList.data).length + ')';
    }

    var ownerList = sort(
      this.props.ownerList.data,
      this.state.ui.sortField,
      this.state.ui.sortDesc,
      caseInsensitiveSort
    );

    return (
      <div id="owners-list">
        <PageHeader>
          Owners {resultCount}
          <ButtonGroup>
            <PrintButton disabled={!this.props.ownerList.loaded} />
          </ButtonGroup>
        </PageHeader>
        <SearchBar>
          <Form onSubmit={this.search}>
            <Row>
              <Col xs={9} sm={10} id="filters">
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
                      Constant.OWNER_STATUS_CODE_APPROVED,
                      Constant.OWNER_STATUS_CODE_PENDING,
                      Constant.OWNER_STATUS_CODE_ARCHIVED,
                    ]}
                  />
                  <FormInputControl
                    id="ownerCode"
                    type="text"
                    placeholder="Owner Code"
                    value={this.state.search.ownerCode}
                    updateState={this.updateSearchState}
                  />
                  <FormInputControl
                    id="ownerName"
                    type="text"
                    placeholder="Company Name or DBA"
                    title="Searches Company Name And Doing Business As Fields."
                    value={this.state.search.ownerName}
                    updateState={this.updateSearchState}
                  />
                  <Button id="search-button" variant="primary" type="submit">
                    Search
                  </Button>
                  <Button className="btn-custom" id="clear-search-button" onClick={this.clearSearch}>
                    Clear
                  </Button>
                </ButtonToolbar>
              </Col>
              <Col xs={3} sm={2} id="search-buttons">
                <Row className="float-right">
                  <Favourites
                    id="faves-dropdown"
                    type="owner"
                    favourites={this.props.favourites}
                    data={this.state.search}
                    onSelect={this.loadFavourite}
                  />
                </Row>
              </Col>
            </Row>
          </Form>
        </SearchBar>

        {(() => {
          if (this.props.ownerList.loading) {
            return (
              <div style={{ textAlign: 'center' }}>
                <Spinner />
              </div>
            );
          }

          var addOwnerButton = (
            <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
              <Button className="btn-custom" title="Add Owner" size="sm" onClick={this.openAddDialog}>
                <FontAwesomeIcon icon="plus" />
                &nbsp;<strong>Add Owner</strong>
              </Button>
            </Authorize>
          );

          if (this.props.ownerList.loaded) {
            return this.renderResults(ownerList, addOwnerButton);
          }

          return <AddButtonContainer>{addOwnerButton}</AddButtonContainer>;
        })()}
        {this.state.showAddDialog && (
          <OwnersAddDialog show={this.state.showAddDialog} onSave={this.ownerSaved} onClose={this.closeAddDialog} />
        )}
      </div>
    );
  }
}

const mapStateToProps = (state) => ({
  currentUser: state.user,
  ownerList: state.models.owners,
  localAreas: state.lookups.localAreas,
  favourites: state.models.favourites.owner,
  search: state.search.owners,
  ui: state.ui.owners,
});

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(mapStateToProps, mapDispatchToProps)(Owners);
