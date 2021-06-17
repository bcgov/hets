import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Alert, Row, Col, ButtonToolbar, Button, ButtonGroup, Glyphicon } from 'react-bootstrap';
import { Link } from 'react-router-dom';
import _ from 'lodash';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import PageHeader from '../components/ui/PageHeader.jsx';
import SearchBar from '../components/ui/SearchBar.jsx';
import Confirm from '../components/Confirm.jsx';
import OverlayTrigger from '../components/OverlayTrigger.jsx';
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
    Api.searchRoles().finally(() => {
      this.setState({ loading: false });
    });
  };

  updateSearchState = (state, callback) => {
    this.setState({ search: { ...this.state.search, ...state } }, () => {
      store.dispatch({
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
      store.dispatch({ type: Action.UPDATE_ROLES_UI, roles: this.state.ui });
      if (callback) {
        callback();
      }
    });
  };

  delete = (role) => {
    Api.deleteRole(role).then(() => {
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
          <ButtonGroup id="roles-buttons" className="pull-right">
            <PrintButton />
          </ButtonGroup>
        </PageHeader>
        <SearchBar>
          <Row>
            <Col xs={9} sm={10} id="filters">
              <ButtonToolbar>
                <SearchControl
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
              <Authorize>
                <Button title="Add Role" bsSize="xsmall">
                  <Glyphicon glyph="plus" />
                  &nbsp;<strong>Add Role</strong>
                </Button>
              </Authorize>
            </Link>
          );
          if (Object.keys(this.props.roles).length === 0) {
            return <Alert bsStyle="success">No roles {addRoleButton}</Alert>;
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
                        <OverlayTrigger
                          trigger="click"
                          placement="top"
                          rootClose
                          overlay={<Confirm onConfirm={this.delete.bind(this, role)} />}
                        >
                          <Button className={role.canDelete ? '' : 'hidden'} title="Delete Role" bsSize="xsmall">
                            <Glyphicon glyph="trash" />
                          </Button>
                        </OverlayTrigger>
                        <Link to={`${Constant.ROLES_PATHNAME}/${role.id}`}>
                          <Button className={role.canEdit ? '' : 'hidden'} title="Edit Role" bsSize="xsmall">
                            <Glyphicon glyph="pencil" />
                          </Button>
                        </Link>
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

function mapStateToProps(state) {
  return {
    currentUser: state.user,
    roles: state.models.roles,
    search: state.search.roles,
    ui: state.ui.roles,
  };
}

export default connect(mapStateToProps)(Roles);
