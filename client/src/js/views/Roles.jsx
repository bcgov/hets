import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Alert, Row, Col, ButtonToolbar, Button, ButtonGroup } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Link } from 'react-router-dom';
import _ from 'lodash';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';

import PageHeader from '../components/ui/PageHeader.jsx';
import SearchBar from '../components/ui/SearchBar.jsx';
import DeleteButton from '../components/DeleteButton';
import EditButton from '../components/EditButton';
import SearchControl from '../components/SearchControl.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import PrintButton from '../components/PrintButton.jsx';
import Authorize from '../components/Authorize.jsx';

const Roles = () => {
  const dispatch = useDispatch();
  const roles = useSelector((state) => state.models.roles);
  const currentUser = useSelector((state) => state.user);
  const search = useSelector((state) => state.search.roles);
  const ui = useSelector((state) => state.ui.roles);

  const [loading, setLoading] = useState(true);
  const [searchState, setSearchState] = useState({
    key: search.key || 'name',
    text: search.text || '',
    params: search.params || null,
  });
  const [uiState, setUIState] = useState({
    sortField: ui.sortField || 'name',
    sortDesc: ui.sortDesc === true,
  });

  useEffect(() => {
    fetch();
  }, []);

  const fetch = () => {
    setLoading(true);
    dispatch(Api.searchRoles()).finally(() => {
      setLoading(false);
    });
  };

  const updateSearchState = (state, callback) => {
    setSearchState((prevState) => ({ ...prevState, ...state }));
    dispatch({
      type: Action.UPDATE_ROLES_SEARCH,
      roles: { ...searchState, ...state },
    });
    if (callback) {
      callback();
    }
  };

  const updateUIState = (state, callback) => {
    setUIState((prevState) => ({ ...prevState, ...state }));
    dispatch({ type: Action.UPDATE_ROLES_UI, roles: { ...uiState, ...state } });
    if (callback) {
      callback();
    }
  };

  const deleteRole = (role) => {
    dispatch(Api.deleteRole(role)).then(() => {
      fetch();
    });
  };

  const numRoles = loading ? '...' : Object.keys(roles).length;

  if (
    !currentUser.hasPermission(Constant.PERMISSION_ROLES_AND_PERMISSIONS) &&
    !currentUser.hasPermission(Constant.PERMISSION_ADMIN)
  ) {
    return <div>You do not have permission to view this page.</div>;
  }

  return (
    <div id="roles-list">
      <PageHeader>
        Roles ({numRoles})
        <ButtonGroup id="roles-buttons" className="float-right">
          <PrintButton />
        </ButtonGroup>
      </PageHeader>
      <SearchBar>
        <Row>
          <Col xs={9} sm={10} id="filters">
            <ButtonToolbar>
              <SearchControl
                className="d-flex"
                id="search"
                search={searchState}
                updateState={updateSearchState}
                items={[
                  { id: 'name', name: 'Name' },
                  { id: 'description', name: 'Description' },
                ]}
              />
            </ButtonToolbar>
          </Col>
        </Row>
      </SearchBar>

      {loading ? (
        <div style={{ textAlign: 'center' }}>
          <Spinner />
        </div>
      ) : (
        (() => {
          const addRoleButton = (
            <Link to={`${Constant.ROLES_PATHNAME}/0`}>
              <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
                <Button className="btn-custom" title="Add Role" size="sm">
                  <FontAwesomeIcon icon="plus" />
                  &nbsp;<strong>Add Role</strong>
                </Button>
              </Authorize>
            </Link>
          );

          if (Object.keys(roles).length === 0) {
            return <Alert variant="success">No roles {addRoleButton}</Alert>;
          }

          const filteredRoles = _.sortBy(
            _.filter(roles, (role) => {
              if (!searchState.params) {
                return true;
              }
              return (
                role[searchState.key] &&
                role[searchState.key].toLowerCase().indexOf(searchState.text.toLowerCase()) !== -1
              );
            }),
            uiState.sortField
          );

          if (uiState.sortDesc) {
            _.reverse(filteredRoles);
          }

          return (
            <SortTable
              sortField={uiState.sortField}
              sortDesc={uiState.sortDesc}
              onSort={updateUIState}
              headers={[
                { field: 'name', title: 'Name' },
                { field: 'description', title: 'Description' },
                {
                  field: 'addRole',
                  title: 'Add Role',
                  style: { textAlign: 'right' },
                  node: addRoleButton,
                },
              ]}
            >
              {_.map(filteredRoles, (role) => (
                <tr key={role.id}>
                  <td>{role.name}</td>
                  <td>{role.description}</td>
                  <td style={{ textAlign: 'right' }}>
                    <ButtonGroup>
                      <DeleteButton onConfirm={() => deleteRole(role)} name="Role" />
                      <EditButton pathname={`${Constant.ROLES_PATHNAME}/${role.id}`} name="Role" />
                    </ButtonGroup>
                  </td>
                </tr>
              ))}
            </SortTable>
          );
        })()
      )}
    </div>
  );
};

Roles.propTypes = {
  roles: PropTypes.object,
  currentUser: PropTypes.object,
  search: PropTypes.object,
  ui: PropTypes.object,
};

export default Roles;