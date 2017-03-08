import React from 'react';

import { connect } from 'react-redux';

import { PageHeader, Well, Alert, Row, Col } from 'react-bootstrap';
import { ButtonToolbar, Button, ButtonGroup, Glyphicon } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';

import _ from 'lodash';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import CheckboxControl from '../components/CheckboxControl.jsx';
import DropdownControl from '../components/DropdownControl.jsx';
import Favourites from '../components/Favourites.jsx';
import FormInputControl from '../components/FormInputControl.jsx';
import MultiDropdown from '../components/MultiDropdown.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import Unimplemented from '../components/Unimplemented.jsx';

/*

TODO:
* Print / Email / Add Project

*/

var Projects = React.createClass({
  propTypes: {
    projects: React.PropTypes.object,
    localAreas: React.PropTypes.object,
    favourites: React.PropTypes.object,
    search: React.PropTypes.object,
    ui: React.PropTypes.object,
    router: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loading: true,

      showAddDialog: false,

      search: {
        selectedLocalAreasIds: this.props.search.selectedLocalAreasIds || [],
        statusCode: this.props.search.statusCode || Constant.PROJECT_STATUS_CODE_ACTIVE,
        hires: this.props.search.hires === true,
        requests: this.props.search.requests === true,
        projectName: this.props.search.projecName,
      },

      ui : {
        sortField: this.props.ui.sortField || 'name',
        sortDesc: this.props.ui.sortDesc === true,
      },
    };
  },

  buildSearchParams() {
    var searchParams = {
      hires: this.state.search.hires,
      requests: this.state.search.requests,
      statusCode: this.state.search.statusCode,
      project: this.state.search.projectName || '',
    };

    if (this.state.search.selectedLocalAreasIds.length > 0) {
      searchParams.localAreas = this.state.search.selectedLocalAreasIds;
    }

    return searchParams;
  },

  componentDidMount() {
    this.setState({ loading: true });

    Api.getFavourites('project').then(() => {
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
    Api.searchProjects(this.buildSearchParams()).finally(() => {
      this.setState({ loading: false });
    });
  },

  updateSearchState(state, callback) {
    this.setState({ search: { ...this.state.search, ...state, ...{ loaded: true } }}, () =>{
      store.dispatch({ type: Action.UPDATE_PROJECTS_SEARCH, projects: this.state.search });
      if (callback) { callback(); }
    });
  },

  updateUIState(state, callback) {
    this.setState({ ui: { ...this.state.ui, ...state }}, () =>{
      store.dispatch({ type: Action.UPDATE_PROJECTS_UI, projects: this.state.ui });
      if (callback) { callback(); }
    });
  },

  loadFavourite(favourite) {
    this.updateSearchState(JSON.parse(favourite.value), this.fetch);
  },

  openAddDialog() {
    // TODO Add Project
    this.setState({ showAddDialog: true });
  },

  closeAddDialog() {
    this.setState({ showAddDialog: false });
  },

  email() {

  },

  print() {

  },

  
  render() {
    var localAreas = _.sortBy(this.props.localAreas, 'name');
    var numProjects = this.state.loading ? '...' : Object.keys(this.props.projects).length;

    return <div id="projects-list">
      <PageHeader>Projects ({ numProjects })
        <ButtonGroup id="projects-buttons">
          <Unimplemented>
            <Button onClick={ this.email }><Glyphicon glyph="envelope" title="E-mail" /></Button>
          </Unimplemented>
          <Unimplemented>
            <Button onClick={ this.print }><Glyphicon glyph="print" title="Print" /></Button>
          </Unimplemented>
        </ButtonGroup>
      </PageHeader>
      <Well id="projects-bar" bsSize="small" className="clearfix">
        <Row>
          <Col md={11}>
            <ButtonToolbar id="projects-filters">
              <MultiDropdown id="selectedLocalAreasIds" placeholder="Local Areas"
                items={ localAreas } selectedIds={ this.state.search.selectedLocalAreasIds } updateState={ this.updateSearchState } showMaxItems={ 2 } />
              <DropdownControl id="statusCode" title={ this.state.search.statusCode } updateState={ this.updateSearchState }
                  items={[ Constant.PROJECT_STATUS_CODE_ACTIVE, Constant.PROJECT_STATUS_CODE_COMPLETED ]} />
              <FormInputControl id="projectName" type="text" placeholder="Project name" value={ this.state.search.projectName } updateState={ this.updateSearchState }></FormInputControl>
              <CheckboxControl inline id="hires" checked={ this.state.search.hires } updateState={ this.updateSearchState }> Hires</CheckboxControl>
              <CheckboxControl inline id="requests" checked={ this.state.search.requests } updateState={ this.updateSearchState }> Requests</CheckboxControl>
              <Button id="search-button" bsStyle="primary" onClick={ this.fetch }>Search</Button>
            </ButtonToolbar>
          </Col>
          <Col md={1}>
            <Row id="projects-faves">
              <Favourites id="projects-faves-dropdown" type="project" favourites={ this.props.favourites } data={ this.state.search } onSelect={ this.loadFavourite } />
            </Row>
          </Col>
        </Row>
      </Well>

      {(() => {
        var addProjectButton = (
          <Unimplemented>
            <Button title="Add Project" bsSize="xsmall" onClick={ this.openAddDialog }><Glyphicon glyph="plus" />&nbsp;<strong>Add Project</strong></Button>
          </Unimplemented>
        );

        if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }
        if (Object.keys(this.props.projects).length === 0) { return <Alert bsStyle="success">No Projects { addProjectButton }</Alert>; }

        var projects = _.sortBy(this.props.projects, this.state.ui.sortField);
        if (this.state.ui.sortDesc) {
          _.reverse(projects);
        }

        return <SortTable sortField={ this.state.ui.sortField } sortDesc={ this.state.ui.sortDesc } onSort={ this.updateUIState } headers={[
          { field: 'localAreaName',          title: 'Local Area'                                     },
          { field: 'name',                   title: 'Project'                                        },
          { field: 'primaryContactName',     title: 'Primary Contact'                                },
          { field: 'primaryContactPhone',    title: 'Contact #'                                      },
          { field: 'numberOfHires',          title: 'Hires',          style: { textAlign: 'center' } },
          { field: 'numberOfRequests',       title: 'Requests',       style: { textAlign: 'center' } },
          { field: 'status',                 title: 'Status',         style: { textAlign: 'center' } },
          { field: 'addProject',             title: 'Add Project',    style: { textAlign: 'right'  },
            node: addProjectButton,
          },
        ]}>
          {
            _.map(projects, (project) => {
              return <tr key={ project.id } className={ project.isActive ? null : 'info' }>
                <td>{ project.localAreaName }</td>
                <td>{ project.name }</td>
                <td>{ project.primaryContactName }</td>
                <td>{ project.primaryContactPhone }</td>
                <td style={{ textAlign: 'center' }}>{ project.numberOfHires }</td>
                <td style={{ textAlign: 'center' }}>{ project.numberOfRequests }</td>
                <td style={{ textAlign: 'center' }}>{ project.status }</td>
                <td style={{ textAlign: 'right' }}>
                  <LinkContainer to={{ pathname: `projects/${ project.id }` }}>
                    <Button title="View Project" bsSize="xsmall"><Glyphicon glyph="edit" /></Button>
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
    projects: state.models.projects,
    localAreas: state.lookups.localAreas,
    favourites: state.models.favourites,
    search: state.search.projects,
    ui: state.ui.projects,
  };
}

export default connect(mapStateToProps)(Projects);
