import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Alert, Row, Col, ButtonToolbar, Button, ButtonGroup, InputGroup, Form } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import _ from 'lodash';

import UsersEditDialog from './dialogs/UsersEditDialog.jsx';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import AddButtonContainer from '../components/ui/AddButtonContainer.jsx';
import PageHeader from '../components/ui/PageHeader.jsx';
import SearchBar from '../components/ui/SearchBar.jsx';
import CheckboxControl from '../components/CheckboxControl.jsx';
import EditButton from '../components/EditButton';
import DeleteButton from '../components/DeleteButton.jsx';
import Favourites from '../components/Favourites.jsx';
import FormInputControl from '../components/FormInputControl.jsx';
import MultiDropdown from '../components/MultiDropdown.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import PrintButton from '../components/PrintButton.jsx';
import Authorize from '../components/Authorize.jsx';

class Users extends React.Component {
  static propTypes = {
    currentUser: PropTypes.object,
    users: PropTypes.object,
    user: PropTypes.object,
    districts: PropTypes.object,
    favourites: PropTypes.object,
    search: PropTypes.object,
    ui: PropTypes.object,
    history: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      showUsersEditDialog: false,

      search: {
        selectedDistrictsIds: props.search.selectedDistrictsIds || [],
        surname: props.search.surname || '',
        hideInactive: props.search.hideInactive || true,
      },

      ui: {
        sortField: props.ui.sortField || 'surname',
        sortDesc: props.ui.sortDesc === true,
      },
    };
  }

  buildSearchParams = () => {
    var searchParams = {
      includeInactive: !this.state.search.hideInactive,
      surname: this.state.search.surname,
    };

    if (this.state.search.selectedDistrictsIds.length > 0) {
      searchParams.districts = this.state.search.selectedDistrictsIds;
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
    Api.searchUsers(this.buildSearchParams());
  };

  search = (e) => {
    e.preventDefault();
    this.fetch();
  };

  clearSearch = () => {
    var defaultSearchParameters = {
      selectedDistrictsIds: [],
      surname: '',
      hideInactive: true,
    };

    this.setState({ search: defaultSearchParameters }, () => {
      store.dispatch({ type: Action.UPDATE_USERS_SEARCH, users: this.state.search });
      store.dispatch({ type: Action.CLEAR_USERS });
    });
  };

  updateSearchState = (state, callback) => {
    this.setState({ search: { ...this.state.search, ...state, ...{ loaded: true } } }, () => {
      store.dispatch({ type: Action.UPDATE_USERS_SEARCH, users: this.state.search });
      if (callback) {
        callback();
      }
    });
  };

  updateUIState = (state, callback) => {
    this.setState({ ui: { ...this.state.ui, ...state } }, () => {
      store.dispatch({ type: Action.UPDATE_USERS_UI, users: this.state.ui });
      if (callback) {
        callback();
      }
    });
  };

  loadFavourite = (favourite) => {
    this.updateSearchState(JSON.parse(favourite.value), this.fetch);
  };

  delete = (user) => {
    Api.deleteUser(user).then(() => {
      this.fetch();
    });
  };

  openUsersEditDialog = () => {
    this.setState({ showUsersEditDialog: true });
  };

  closeUsersEditDialog = () => {
    this.setState({ showUsersEditDialog: false });
  };

  onUserSaved = (user) => {
    this.closeUsersEditDialog();
    this.props.history.push(`${Constant.USERS_PATHNAME}/${user.id}`);
  };

  renderResults = (addUserButton) => {
    if (Object.keys(this.props.users.data).length === 0) {
      return <Alert variant="success">No users {addUserButton}</Alert>;
    }

    var users = _.sortBy(this.props.users.data, this.state.ui.sortField);
    if (this.state.ui.sortDesc) {
      _.reverse(users);
    }

    return (
      <SortTable
        sortField={this.state.ui.sortField}
        sortDesc={this.state.ui.sortDesc}
        onSort={this.updateUIState}
        headers={[
          { field: 'surname', title: 'Surname' },
          { field: 'givenName', title: 'First Name' },
          { field: 'smUserId', title: 'User ID' },
          { field: 'districtName', title: 'District' },
          { field: 'addUser', title: 'Add User', style: { textAlign: 'right' }, node: addUserButton },
        ]}
      >
        {_.map(users, (user) => {
          return (
            <tr key={user.id} className={user.active ? null : 'bg-info'}>
              <td>{user.surname}</td>
              <td>{user.givenName}</td>
              <td>{user.smUserId}</td>
              <td>{user.districtName}</td>
              <td style={{ textAlign: 'right' }}>
                <ButtonGroup>
                  <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
                    <DeleteButton onConfirm={this.delete.bind(this, user)} name="User" />
                    <EditButton pathname={`${Constant.USERS_PATHNAME}/${user.id}`} name="User" />
                  </Authorize>
                </ButtonGroup>
              </td>
            </tr>
          );
        })}
      </SortTable>
    );
  };

  render() {
    var districts = _.sortBy(this.props.districts, 'name');

    if (
      !this.props.currentUser.hasPermission(Constant.PERMISSION_USER_MANAGEMENT) &&
      !this.props.currentUser.hasPermission(Constant.PERMISSION_ADMIN)
    ) {
      return <div>You do not have permission to view this page.</div>;
    }

    var resultCount = '';
    if (this.props.users.loaded) {
      resultCount = '(' + Object.keys(this.props.users.data).length + ')';
    }

    return (
      <div id="users-list">
        <PageHeader>
          Users {resultCount}
          <ButtonGroup id="users-buttons">
            <PrintButton disabled={!this.props.users.loaded} />
          </ButtonGroup>
        </PageHeader>
        <SearchBar>
          <Form onSubmit={this.search}>
            <Row>
              <Col sm={10} id="filters">
                <ButtonToolbar>
                  <MultiDropdown
                    id="selectedDistrictsIds"
                    placeholder="Districts"
                    items={districts}
                    selectedIds={this.state.search.selectedDistrictsIds}
                    updateState={this.updateSearchState}
                    showMaxItems={2}
                  />
                  <InputGroup>
                    <InputGroup.Prepend>
                      <InputGroup.Text>Surname</InputGroup.Text>
                    </InputGroup.Prepend>
                    <FormInputControl
                      id="surname"
                      type="text"
                      value={this.state.search.surname}
                      updateState={this.updateSearchState}
                    />
                  </InputGroup>
                  <CheckboxControl
                    inline
                    id="hideInactive"
                    checked={this.state.search.hideInactive}
                    updateState={this.updateSearchState}
                    label="Hide Inactive"
                  />

                  <Button id="search-button" variant="primary" type="submit">
                    Search
                  </Button>
                  <Button className="btn-custom" id="clear-search-button" onClick={this.clearSearch}>
                    Clear
                  </Button>
                </ButtonToolbar>
              </Col>
              <Col sm={2} id="search-buttons">
                <Row className="float-right">
                  <Favourites
                    id="users-faves-dropdown"
                    type="user"
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
          if (this.props.users.loading) {
            return (
              <div style={{ textAlign: 'center' }}>
                <Spinner />
              </div>
            );
          }

          var addUserButton = (
            <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
              <Button className="btn-custom" title="Add User" size="sm" onClick={this.openUsersEditDialog}>
                <FontAwesomeIcon icon="plus" />
                &nbsp;<strong>Add User</strong>
              </Button>
            </Authorize>
          );

          if (this.props.users.loaded) {
            return this.renderResults(addUserButton);
          }

          return <AddButtonContainer>{addUserButton}</AddButtonContainer>;
        })()}

        {this.state.showUsersEditDialog && (
          <UsersEditDialog
            isNew
            show={this.state.showUsersEditDialog}
            onSave={this.onUserSaved}
            onClose={this.closeUsersEditDialog}
          />
        )}
      </div>
    );
  }
}

function mapStateToProps(state) {
  return {
    currentUser: state.user,
    users: state.models.users,
    user: state.models.user,
    districts: state.lookups.districts,
    favourites: state.models.favourites.user,
    search: state.search.users,
    ui: state.ui.users,
  };
}

export default connect(mapStateToProps)(Users);
