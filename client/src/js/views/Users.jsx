import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';
import { connect, useDispatch } from 'react-redux';
import { Alert, Row, Col, ButtonToolbar, Button, ButtonGroup, InputGroup, Form } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import _ from 'lodash';

import UsersEditDialog from './dialogs/UsersEditDialog.jsx';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';

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

const Users = ({ currentUser, users, favourites, search, districts, ui, history }) => {
  const [showUsersEditDialog, setShowUsersEditDialog] = useState(false);
  const [searchState, setSearchState] = useState({
    selectedDistrictsIds: search.selectedDistrictsIds || [],
    surname: search.surname || '',
    hideInactive: search.hideInactive || true,
  });
  const [uiState, setUiState] = useState({
    sortField: ui.sortField || 'surname',
    sortDesc: ui.sortDesc === true,
  });

  const dispatch = useDispatch();

  const buildSearchParams = () => {
    let searchParams = {
      includeInactive: !searchState.hideInactive,
      surname: searchState.surname,
    };

    if (searchState.selectedDistrictsIds.length > 0) {
      searchParams.districts = searchState.selectedDistrictsIds;
    }

    return searchParams;
  };

  useEffect(() => {
    if (_.isEmpty(search)) {
      const defaultFavourite = _.find(favourites, (f) => f.isDefault);
      if (defaultFavourite) {
        loadFavourite(defaultFavourite);
      }
    }
  }, [search, favourites]);

  const fetch = () => {
    dispatch(Api.searchUsers(buildSearchParams()));
  };

  const searchHandler = (e) => {
    e.preventDefault();
    fetch();
  };

  const clearSearch = () => {
    const defaultSearchParameters = {
      selectedDistrictsIds: [],
      surname: '',
      hideInactive: true,
    };

    setSearchState(defaultSearchParameters, () => {
      dispatch({ type: Action.UPDATE_USERS_SEARCH, users: searchState });
      dispatch({ type: Action.CLEAR_USERS });
    });
  };

  const updateSearchState = (state, callback) => {
    setSearchState({ ...searchState, ...state, loaded: true }, () => {
      dispatch({ type: Action.UPDATE_USERS_SEARCH, users: searchState });
      if (callback) callback();
    });
  };

  const updateUIState = (state, callback) => {
    setUiState({ ...uiState, ...state }, () => {
      dispatch({ type: Action.UPDATE_USERS_UI, users: uiState });
      if (callback) callback();
    });
  };

  const loadFavourite = (favourite) => {
    updateSearchState(JSON.parse(favourite.value), fetch);
  };

  const deleteUser = (user) => {
    dispatch(Api.deleteUser(user)).then(() => {
      fetch();
    });
  };

  const openUsersEditDialog = () => {
    setShowUsersEditDialog(true);
  };

  const closeUsersEditDialog = () => {
    setShowUsersEditDialog(false);
  };

  const onUserSaved = (user) => {
    closeUsersEditDialog();
    history.push(`${Constant.USERS_PATHNAME}/${user.id}`);
  };

  const renderResults = (addUserButton) => {
    if (Object.keys(users.data).length === 0) {
      return <Alert variant="success">No users {addUserButton}</Alert>;
    }

    let sortedUsers = _.sortBy(users.data, uiState.sortField);
    if (uiState.sortDesc) {
      _.reverse(sortedUsers);
    }

    return (
      <SortTable
        sortField={uiState.sortField}
        sortDesc={uiState.sortDesc}
        onSort={updateUIState}
        headers={[
          { field: 'surname', title: 'Surname' },
          { field: 'givenName', title: 'First Name' },
          { field: 'smUserId', title: 'User ID' },
          { field: 'districtName', title: 'District' },
          { field: 'addUser', title: 'Add User', style: { textAlign: 'right' }, node: addUserButton },
        ]}
      >
        {_.map(sortedUsers, (user) => (
          <tr key={user.id} className={user.active ? null : 'bg-info'}>
            <td>{user.surname}</td>
            <td>{user.givenName}</td>
            <td>{user.smUserId}</td>
            <td>{user.districtName}</td>
            <td style={{ textAlign: 'right' }}>
              <ButtonGroup>
                <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
                  <DeleteButton onConfirm={() => deleteUser(user)} name="User" />
                  <EditButton pathname={`${Constant.USERS_PATHNAME}/${user.id}`} name="User" />
                </Authorize>
              </ButtonGroup>
            </td>
          </tr>
        ))}
      </SortTable>
    );
  };

  const districtsSorted = _.sortBy(districts, 'name');

  if (
    !currentUser.hasPermission(Constant.PERMISSION_USER_MANAGEMENT) &&
    !currentUser.hasPermission(Constant.PERMISSION_ADMIN)
  ) {
    return <div>You do not have permission to view this page.</div>;
  }

  let resultCount = '';
  if (users.loaded) {
    resultCount = `(${Object.keys(users.data).length})`;
  }

  return (
    <div id="users-list">
      <PageHeader>
        Users {resultCount}
        <ButtonGroup id="users-buttons">
          <PrintButton disabled={!users.loaded} />
        </ButtonGroup>
      </PageHeader>
      <SearchBar>
        <Form onSubmit={searchHandler}>
          <Row>
            <Col sm={10} id="filters">
              <ButtonToolbar>
                <MultiDropdown
                  id="selectedDistrictsIds"
                  placeholder="Districts"
                  items={districtsSorted}
                  selectedIds={searchState.selectedDistrictsIds}
                  updateState={updateSearchState}
                  showMaxItems={2}
                />
                <InputGroup>
                  <InputGroup.Prepend>
                    <InputGroup.Text>Surname</InputGroup.Text>
                  </InputGroup.Prepend>
                  <FormInputControl
                    id="surname"
                    type="text"
                    value={searchState.surname}
                    updateState={updateSearchState}
                  />
                </InputGroup>
                <CheckboxControl
                  inline
                  id="hideInactive"
                  checked={searchState.hideInactive}
                  updateState={updateSearchState}
                  label="Hide Inactive"
                />

                <Button id="search-button" variant="primary" type="submit">
                  Search
                </Button>
                <Button className="btn-custom" id="clear-search-button" onClick={clearSearch}>
                  Clear
                </Button>
              </ButtonToolbar>
            </Col>
            <Col sm={2} id="search-buttons">
              <Row className="float-right">
                <Favourites
                  id="users-faves-dropdown"
                  type="user"
                  favourites={favourites}
                  data={searchState}
                  onSelect={loadFavourite}
                />
              </Row>
            </Col>
          </Row>
        </Form>
      </SearchBar>

      {(() => {
        if (users.loading) {
          return (
            <div style={{ textAlign: 'center' }}>
              <Spinner />
            </div>
          );
        }

        const addUserButton = (
          <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
            <Button className="btn-custom" title="Add User" size="sm" onClick={openUsersEditDialog}>
              <FontAwesomeIcon icon="plus" />
              &nbsp;<strong>Add User</strong>
            </Button>
          </Authorize>
        );

        if (users.loaded) {
          return renderResults(addUserButton);
        }

        return <AddButtonContainer>{addUserButton}</AddButtonContainer>;
      })()}

      {showUsersEditDialog && (
        <UsersEditDialog
          isNew
          show={showUsersEditDialog}
          onSave={onUserSaved}
          onClose={closeUsersEditDialog}
        />
      )}
    </div>
  );
};

Users.propTypes = {
  currentUser: PropTypes.object,
  users: PropTypes.object,
  favourites: PropTypes.object,
  search: PropTypes.object,
  districts: PropTypes.object,
  ui: PropTypes.object,
  history: PropTypes.object,
};

const mapStateToProps = (state) => ({
  currentUser: state.user,
  users: state.models.users,
  favourites: state.models.favourites.user,
  search: state.search.users,
  ui: state.ui.users,
  districts: state.lookups.districts,
});

export default connect(mapStateToProps)(Users);
