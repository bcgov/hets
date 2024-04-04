import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
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

class Roles extends React.Component {
  static propTypes = {
    roles: PropTypes.object,
    currentUser: PropTypes.object,
    search: PropTypes.object,
    ui: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      loading: true,

      search: {
        key: props.search.key || 'name',
        text: props.search.text || '',
        params: props.search.params || null,
      },

      ui: {
        sortField: props.ui.sortField || 'name',
        sortDesc: props.ui.sortDesc === true,
      },
    };
  }

  componentDidMount() {
    this.fetch();
  }

  fetch = () => {
    this.setState({ loading: true });
    this.props.dispatch(Api.searchRoles()).finally(() => {
      this.setState({ loading: false });
    });
  };

  updateSearchState = (state, callback) => {
    this.setState({ search: { ...this.state.search, ...state } }, () => {
      this.props.dispatch({
        type: Action.UPDATE_ROLES_SEARCH,
        roles: this.state.search,
      });
      if (callback) {
        callback();
      }
    });
  };

  updateUIState = (state, callback) => {
    this.setState({ ui: { ...this.state.ui, ...state } }, () => {
      this.props.dispatch({ type: Action.UPDATE_ROLES_UI, roles: this.state.ui });
      if (callback) {
        callback();
      }
    });
  };

  delete = (role) => {
    this.props.dispatch(Api.deleteRole(role)).then(() => {
      this.fetch();
    });
  };

  render() {
    var numRoles = this.state.loading ? '...' : Object.keys(this.props.roles).length;

    if (
      !this.props.currentUser.hasPermission(Constant.PERMISSION_ROLES_AND_PERMISSIONS) &&
      !this.props.currentUser.hasPermission(Constant.PERMISSION_ADMIN)
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
                  search={this.state.search}
                  updateState={this.updateSearchState}
                  items={[
                    { id: 'name', name: 'Name' },
                    { id: 'description', name: 'Description' },
                  ]}
                />
              </ButtonToolbar>
            </Col>
          </Row>
        </SearchBar>

        {(() => {
          if (this.state.loading) {
            return (
              <div style={{ textAlign: 'center' }}>
                <Spinner />
              </div>
            );
          }

          var addRoleButton = (
            <Link to={`${Constant.ROLES_PATHNAME}/0`}>
              <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
                <Button className="btn-custom" title="Add Role" size="sm">
                  <FontAwesomeIcon icon="plus" />
                  &nbsp;<strong>Add Role</strong>
                </Button>
              </Authorize>
            </Link>
          );
          if (Object.keys(this.props.roles).length === 0) {
            return <Alert variant="success">No roles {addRoleButton}</Alert>;
          }

          var roles = _.sortBy(
            _.filter(this.props.roles, (role) => {
              if (!this.state.search.params) {
                return true;
              }
              return (
                role[this.state.search.key] &&
                role[this.state.search.key].toLowerCase().indexOf(this.state.search.text.toLowerCase()) !== -1
              );
            }),
            this.state.ui.sortField
          );

          if (this.state.ui.sortDesc) {
            _.reverse(roles);
          }

          return (
            <SortTable
              sortField={this.state.ui.sortField}
              sortDesc={this.state.ui.sortDesc}
              onSort={this.updateUIState}
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
              {_.map(roles, (role) => {
                return (
                  <tr key={role.id}>
                    <td>{role.name}</td>
                    <td>{role.description}</td>
                    <td style={{ textAlign: 'right' }}>
                      <ButtonGroup>
                        <DeleteButton onConfirm={this.delete.bind(this, role)} name="Role" />
                        <EditButton pathname={`${Constant.ROLES_PATHNAME}/${role.id}`} name="Role" />
                      </ButtonGroup>
                    </td>
                  </tr>
                );
              })}
            </SortTable>
          );
        })()}
      </div>
    );
  }
}

const mapStateToProps = (state) => ({
  currentUser: state.user,
  roles: state.models.roles,
  search: state.search.roles,
  ui: state.ui.roles,
});

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(mapStateToProps, mapDispatchToProps)(Roles);
