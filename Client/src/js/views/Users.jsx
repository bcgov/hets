import React from 'react';
import { connect } from 'react-redux';
import { PageHeader, Well, Alert, Row, Col, ButtonToolbar, Button, ButtonGroup, Glyphicon, InputGroup, Form } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';
import _ from 'lodash';

import UsersEditDialog from './dialogs/UsersEditDialog.jsx';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import CheckboxControl from '../components/CheckboxControl.jsx';
import Confirm from '../components/Confirm.jsx';
import Favourites from '../components/Favourites.jsx';
import FormInputControl from '../components/FormInputControl.jsx';
import MultiDropdown from '../components/MultiDropdown.jsx';
import OverlayTrigger from '../components/OverlayTrigger.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import PrintButton from '../components/PrintButton.jsx';


var Users = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
    users: React.PropTypes.object,
    user: React.PropTypes.object,
    districts: React.PropTypes.object,
    favourites: React.PropTypes.object,
    search: React.PropTypes.object,
    ui: React.PropTypes.object,
    router: React.PropTypes.object,
  },

  getInitialState() {
    return {
      showUsersEditDialog: false,

      search: {
        selectedDistrictsIds: this.props.search.selectedDistrictsIds || [],
        surname: this.props.search.surname || '',
        hideInactive: this.props.search.hideInactive || true,
      },

      ui : {
        sortField: this.props.ui.sortField || 'surname',
        sortDesc: this.props.ui.sortDesc === true,
      },
    };
  },

  buildSearchParams() {
    var searchParams = {
      includeInactive: !this.state.search.hideInactive,
      surname: this.state.search.surname,
    };

    if (this.state.search.selectedDistrictsIds.length > 0) {
      searchParams.districts = this.state.search.selectedDistrictsIds;
    }

    return searchParams;
  },

  componentDidMount() {
    // If this is the first load, then look for a default favourite
    if (_.isEmpty(this.props.search)) {
      var defaultFavourite = _.find(this.props.favourites, f => f.isDefault);
      if (defaultFavourite) {
        this.loadFavourite(defaultFavourite);
      }
    }
  },

  fetch() {
    Api.searchUsers(this.buildSearchParams());
  },

  search(e) {
    e.preventDefault();
    this.fetch();
  },

  clearSearch() {
    var defaultSearchParameters = {
      selectedDistrictsIds: [],
      surname: '',
      hideInactive: true,
    };

    this.setState({ search: defaultSearchParameters }, () => {
      store.dispatch({ type: Action.UPDATE_USERS_SEARCH, users: this.state.search });
      store.dispatch({ type: Action.CLEAR_USERS });
    });
  },

  updateSearchState(state, callback) {
    this.setState({ search: { ...this.state.search, ...state, ...{ loaded: true } }}, () =>{
      store.dispatch({ type: Action.UPDATE_USERS_SEARCH, users: this.state.search });
      if (callback) { callback(); }
    });
  },

  updateUIState(state, callback) {
    this.setState({ ui: { ...this.state.ui, ...state }}, () =>{
      store.dispatch({ type: Action.UPDATE_USERS_UI, users: this.state.ui });
      if (callback) { callback(); }
    });
  },

  loadFavourite(favourite) {
    this.updateSearchState(JSON.parse(favourite.value), this.fetch);
  },

  delete(user) {
    Api.deleteUser(user).then(() => {
      this.fetch();
    });
  },

  openUsersEditDialog() {
    this.setState({ showUsersEditDialog: true });
  },

  closeUsersEditDialog() {
    this.setState({ showUsersEditDialog: false });
  },

  onUserSaved(user) {
    this.props.router.push({
      pathname: `${ Constant.USERS_PATHNAME }/${ user.id }`,
    });
  },

  renderResults(addUserButton) {
    if (Object.keys(this.props.users.data).length === 0) {
      return <Alert bsStyle="success">No users { addUserButton }</Alert>;
    }

    var users = _.sortBy(this.props.users.data, this.state.ui.sortField);
    if (this.state.ui.sortDesc) {
      _.reverse(users);
    }

    return <SortTable sortField={ this.state.ui.sortField } sortDesc={ this.state.ui.sortDesc } onSort={ this.updateUIState } headers={[
      { field: 'surname',      title: 'Surname'    },
      { field: 'givenName',    title: 'First Name' },
      { field: 'smUserId',     title: 'User ID'    },
      { field: 'districtName', title: 'District'   },
      { field: 'addUser',      title: 'Add User',  style: { textAlign: 'right'  },
        node: addUserButton,
      },
    ]}>
      {
        _.map(users, (user) => {
          return <tr key={ user.id } className={ user.active ? null : 'info' }>
            <td>{ user.surname }</td>
            <td>{ user.givenName }</td>
            <td>{ user.smUserId }</td>
            <td>{ user.districtName }</td>
            <td style={{ textAlign: 'right' }}>
              <ButtonGroup>
                <OverlayTrigger trigger="click" placement="top" rootClose overlay={ <Confirm onConfirm={ this.delete.bind(this, user) }/> }>
                  <Button className={ user.canDelete ? '' : 'hidden' } title="Delete User" bsSize="xsmall"><Glyphicon glyph="trash" /></Button>
                </OverlayTrigger>
                <LinkContainer to={{ pathname: `${ Constant.USERS_PATHNAME }/${ user.id }` }}>
                  <Button className={ user.canEdit ? '' : 'hidden' } title="View User" bsSize="xsmall"><Glyphicon glyph="edit" /></Button>
                </LinkContainer>
              </ButtonGroup>
            </td>
          </tr>;
        })
      }
    </SortTable>;
  },

  render() {
    var districts = _.sortBy(this.props.districts, 'name');

    if (!this.props.currentUser.hasPermission(Constant.PERMISSION_USER_MANAGEMENT) && !this.props.currentUser.hasPermission(Constant.PERMISSION_ADMIN)) {
      return (
        <div>You do not have permission to view this page.</div>
      );
    }

    var resultCount = '';
    if (this.props.users.loaded) {
      resultCount = '(' + Object.keys(this.props.users.data).length + ')';
    }

    return <div id="users-list">
      <PageHeader>Users { resultCount }
        <ButtonGroup id="users-buttons">
          <PrintButton disabled={!this.props.users.loaded}/>
        </ButtonGroup>
      </PageHeader>
      <div>
        <Well id="users-bar" bsSize="small" className="clearfix">
          <Row>
            <Col sm={10}>
              <Form onSubmit={ this.search }>
                <ButtonToolbar id="users-search">
                  <MultiDropdown id="selectedDistrictsIds" placeholder="Districts"
                    items={ districts } selectedIds={ this.state.search.selectedDistrictsIds } updateState={ this.updateSearchState } showMaxItems={ 2 } />
                  <InputGroup>
                    <InputGroup.Addon>Surname</InputGroup.Addon>
                    <FormInputControl id="surname" type="text" value={ this.state.search.surname } updateState={ this.updateSearchState }/>
                  </InputGroup>
                  <CheckboxControl inline id="hideInactive" checked={ this.state.search.hideInactive } updateState={ this.updateSearchState }>Hide Inactive</CheckboxControl>
                  <Button id="search-button" bsStyle="primary" type="submit">Search</Button>
                  <Button id="clear-search-button" onClick={ this.clearSearch }>Clear</Button>
                </ButtonToolbar>
              </Form>
            </Col>
            <Col sm={2}>
              <Row id="users-faves">
                <Favourites id="users-faves-dropdown" type="user" favourites={ this.props.favourites } data={ this.state.search } onSelect={ this.loadFavourite } pullRight />
              </Row>
            </Col>
          </Row>
        </Well>

        {(() => {
          if (this.props.users.loading) {
            return <div style={{ textAlign: 'center' }}><Spinner/></div>;
          }

          var addUserButton = <Button title="Add User" bsSize="xsmall" onClick={ this.openUsersEditDialog }>
            <Glyphicon glyph="plus" />&nbsp;<strong>Add User</strong>
          </Button>;

          if (this.props.users.loaded) {
            return this.renderResults(addUserButton);
          }

          return <div id="add-button-container">{ addUserButton }</div>;
        })()}
      </div>
      { this.state.showUsersEditDialog &&
        <UsersEditDialog
          show={ this.state.showUsersEditDialog }
          onSave={ this.onUserSaved }
          onClose= { this.closeUsersEditDialog }
          isNew
        />
      }
    </div>;
  },
});

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
