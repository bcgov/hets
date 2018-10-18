import React from 'react';

import { connect } from 'react-redux';

import { PageHeader, Well, Alert, Row, Col, ButtonToolbar, Button, ButtonGroup, Glyphicon, Form  } from 'react-bootstrap';

import _ from 'lodash';

import ProjectsAddDialog from './dialogs/ProjectsAddDialog.jsx';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import * as Log from '../history';
import store from '../store';

import DropdownControl from '../components/DropdownControl.jsx';
import EditButton from '../components/EditButton.jsx';
import Favourites from '../components/Favourites.jsx';
import FormInputControl from '../components/FormInputControl.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import TooltipButton from '../components/TooltipButton.jsx';

var Projects = React.createClass({
  propTypes: {
    projects: React.PropTypes.object,
    project: React.PropTypes.object,
    favourites: React.PropTypes.object,
    search: React.PropTypes.object,
    ui: React.PropTypes.object,
    router: React.PropTypes.object,
  },

  getInitialState() {
    // if the search prop has the 'clear' property set, clear out existing search results and use default search parameters
    // otherwise, display previous search results and initialize search parameters from the store
    var clear = true;

    if (this.props.search.clear) {
      // clear existing search results
      store.dispatch({ type: Action.CLEAR_PROJECTS });
    } else {
      clear = false;
      // restore default 'clear' value for future visits to the page
      store.dispatch({ type: Action.UPDATE_PROJECTS_SEARCH, projects: { ...this.props.search, clear: true }});
    }

    return {
      showAddDialog: false,
      search: {
        statusCode: !clear && this.props.search.statusCode || Constant.PROJECT_STATUS_CODE_ACTIVE,
        projectName: !clear && this.props.search.projectName || '',
        projectNumber: !clear && this.props.search.projectNumber || '',
      },
      ui : {
        sortField: this.props.ui.sortField || 'name',
        sortDesc: this.props.ui.sortDesc === true,
      },
    };
  },

  buildSearchParams() {
    var searchParams = {};

    if (this.state.search.projectName) {
      searchParams.project = this.state.search.projectName;
    }

    if (this.state.search.statusCode) {
      searchParams.status = this.state.search.statusCode;
    }

    if (this.state.search.projectNumber) {
      searchParams.projectNumber = this.state.search.projectNumber;
    }

    return searchParams;

  },

  componentDidMount() {
    Api.getFavourites('project').then(() => {
      // If this is the first load, then look for a default favourite
      if (!this.props.search.loaded) {
        var favourite = _.find(this.props.favourites, (favourite) => { return favourite.isDefault; });
        if (favourite) {
          this.loadFavourite(favourite);
          return;
        }
      }
    });
  },

  fetch() {
    Api.searchProjects(this.buildSearchParams());
  },

  search(e) {
    e.preventDefault();
    this.fetch(); 
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
    this.setState({ showAddDialog: true });
  },

  closeAddDialog() {
    this.setState({ showAddDialog: false });
  },

  saveNewProject(project) {
    Api.addProject(project).then(() => {
      Log.projectAdded(this.props.project);
      // Open it up
      this.props.router.push({
        pathname: `${ Constant.PROJECTS_PATHNAME }/${ this.props.project.id }`,
      });
    });
  },

  print() {
    window.print();
  },

  renderResults() {
    var addProjectButton = <Button title="Add Project" bsSize="xsmall" onClick={ this.openAddDialog }>
      <Glyphicon glyph="plus" />&nbsp;<strong>Add Project</strong>
    </Button>;

    if (Object.keys(this.props.projects.data).length === 0) { 
      return <Alert bsStyle="success">No Projects { addProjectButton }</Alert>; 
    }

    var projects = _.sortBy(this.props.projects.data, project => {
      var sortValue = project[this.state.ui.sortField];
      if (typeof sortValue === 'string') {
        return sortValue.toLowerCase();
      }
      return sortValue;
    });
    
    if (this.state.ui.sortDesc) {
      _.reverse(projects);
    }

    return <SortTable sortField={ this.state.ui.sortField } sortDesc={ this.state.ui.sortDesc } onSort={ this.updateUIState } headers={[
      { field: 'name',                   title: 'Project'                                        },
      { field: 'primaryContactName',     title: 'Primary Contact'                                },
      { field: 'primaryContactPhone',    title: 'Contact #'                                      },
      { field: 'hires',          title: 'Hires',          style: { textAlign: 'center' } },
      { field: 'requests',       title: 'Requests',       style: { textAlign: 'center' } },
      { field: 'status',                 title: 'Status',         style: { textAlign: 'center' } },
      { field: 'addProject',             title: 'Add Project',    style: { textAlign: 'right'  },
        node: addProjectButton,
      },
    ]}>
      {
        _.map(projects, (project) => {
          return <tr key={ project.id } className={ project.isActive ? null : 'info' }>
            <td>{ project.name }</td>
            <td>{ project.primaryContactName }</td>
            <td>{ project.primaryContactPhone }</td>
            <td style={{ textAlign: 'center' }}>{ project.hires }</td>
            <td style={{ textAlign: 'center' }}>{ project.requests }</td>
            <td style={{ textAlign: 'center' }}>{ project.status }</td>
            <td style={{ textAlign: 'right' }}>
              <ButtonGroup>
                <EditButton name="Project" hide={ !project.canView } view pathname={ `${ Constant.PROJECTS_PATHNAME }/${ project.id }` }/>
              </ButtonGroup>
            </td>
          </tr>;
        })
      }
    </SortTable>;
  },

  render() {
    var resultCount = '';
    if (this.props.projects.loaded) {
      resultCount = '(' + Object.keys(this.props.projects.data).length + ')';
    }

    return <div id="projects-list">
      <PageHeader>Projects { resultCount }
        <ButtonGroup id="projects-buttons">
          <TooltipButton onClick={ this.print } disabled={ !this.props.projects.loaded } disabledTooltip={ 'Please complete the search to enable this function.' }>
            <Glyphicon glyph="print" title="Print" />
          </TooltipButton>
        </ButtonGroup>
      </PageHeader>
      <Well id="projects-bar" bsSize="small" className="clearfix">
        <Row>
          <Form onSubmit={ this.search }>
            <Col sm={10}>
              <ButtonToolbar id="projects-filters">
                <DropdownControl id="statusCode" title={ this.state.search.statusCode } updateState={ this.updateSearchState } blankLine="(All)" placeholder="Status"
                  items={[ Constant.PROJECT_STATUS_CODE_ACTIVE, Constant.PROJECT_STATUS_CODE_COMPLETED ]} />
                <FormInputControl id="projectName" type="text" placeholder="Project name" value={ this.state.search.projectName } updateState={ this.updateSearchState }></FormInputControl>
                <FormInputControl id="projectNumber" type="text" placeholder="Project number" value={ this.state.search.projectNumber } updateState={ this.updateSearchState }></FormInputControl>
                <Button id="search-button" bsStyle="primary" type="submit">Search</Button>
              </ButtonToolbar>
            </Col>
          </Form>
          <Col sm={2}>
            <Row id="projects-faves">
              <Favourites id="projects-faves-dropdown" type="project" favourites={ this.props.favourites.data } data={ this.state.search } onSelect={ this.loadFavourite } pullRight />
            </Row>
          </Col>
        </Row>
      </Well>

      {(() => {

        if (this.props.projects.loading || this.props.favourites.loading) { 
          return <div style={{ textAlign: 'center' }}><Spinner/></div>; 
        }

        if (this.props.projects.loaded) {
          return this.renderResults();
        }

      })()}
      { this.state.showAddDialog &&
        <ProjectsAddDialog show={ this.state.showAddDialog } onSave={ this.saveNewProject } onClose={ this.closeAddDialog } />
      }
    </div>;
  },
});


function mapStateToProps(state) {
  return {
    projects: state.models.projects,
    project: state.models.project,
    favourites: state.models.favourites,
    search: state.search.projects,
    ui: state.ui.projects,
  };
}

export default connect(mapStateToProps)(Projects);
