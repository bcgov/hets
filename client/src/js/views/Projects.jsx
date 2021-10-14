import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Alert, Row, Col, ButtonToolbar, Button, ButtonGroup, Form } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import _ from 'lodash';

import ProjectsAddDialog from './dialogs/ProjectsAddDialog.jsx';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import AddButtonContainer from '../components/ui/AddButtonContainer.jsx';
import PageHeader from '../components/ui/PageHeader.jsx';
import SearchBar from '../components/ui/SearchBar.jsx';
import DropdownControl from '../components/DropdownControl.jsx';
import EditButton from '../components/EditButton.jsx';
import Favourites from '../components/Favourites.jsx';
import FormInputControl from '../components/FormInputControl.jsx';
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import PrintButton from '../components/PrintButton.jsx';
import Authorize from '../components/Authorize.jsx';

class Projects extends React.Component {
  static propTypes = {
    fiscalYears: PropTypes.array,
    projects: PropTypes.object,
    favourites: PropTypes.object,
    search: PropTypes.object,
    ui: PropTypes.object,
    history: PropTypes.object,
    // router: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      showAddDialog: false,
      search: {
        statusCode: props.search.statusCode || Constant.PROJECT_STATUS_CODE_ACTIVE,
        projectName: props.search.projectName || '',
        projectNumber: props.search.projectNumber || '',
        fiscalYear: props.search.fiscalYear || '',
      },
      ui: {
        sortField: props.ui.sortField || 'name',
        sortDesc: props.ui.sortDesc === true,
      },
    };
  }

  buildSearchParams = () => {
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

    if (this.state.search.fiscalYear) {
      searchParams.fiscalYear = this.state.search.fiscalYear;
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
    Api.searchProjects(this.buildSearchParams());
  };

  search = (e) => {
    e.preventDefault();
    this.fetch();
  };

  clearSearch = () => {
    var defaultSearchParameters = {
      statusCode: Constant.PROJECT_STATUS_CODE_ACTIVE,
      projectName: '',
      projectNumber: '',
      fiscalYear: '',
    };

    this.setState({ search: defaultSearchParameters }, () => {
      store.dispatch({ type: Action.UPDATE_PROJECTS_SEARCH, projects: this.state.search });
      store.dispatch({ type: Action.CLEAR_PROJECTS });
    });
  };

  updateSearchState = (state, callback) => {
    this.setState({ search: { ...this.state.search, ...state, ...{ loaded: true } } }, () => {
      store.dispatch({ type: Action.UPDATE_PROJECTS_SEARCH, projects: this.state.search });
      if (callback) {
        callback();
      }
    });
  };

  updateUIState = (state, callback) => {
    this.setState({ ui: { ...this.state.ui, ...state } }, () => {
      store.dispatch({ type: Action.UPDATE_PROJECTS_UI, projects: this.state.ui });
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

  projectAdded = (project) => {
    this.fetch();
    this.props.history.push(`${Constant.PROJECTS_PATHNAME}/${project.id}`);
  };

  renderResults = (addProjectButton) => {
    if (Object.keys(this.props.projects.data).length === 0) {
      return <Alert variant="success">No Projects {addProjectButton}</Alert>;
    }

    var projects = _.sortBy(this.props.projects.data, (project) => {
      var sortValue = project[this.state.ui.sortField];
      if (typeof sortValue === 'string') {
        return sortValue.toLowerCase();
      }
      return sortValue;
    });

    if (this.state.ui.sortDesc) {
      _.reverse(projects);
    }

    return (
      <SortTable
        sortField={this.state.ui.sortField}
        sortDesc={this.state.ui.sortDesc}
        onSort={this.updateUIState}
        headers={[
          { field: 'name', title: 'Project' },
          { field: 'fiscalYear', title: 'Fiscal Year' },
          { field: 'provincialProjectNumber', title: 'Project Number' },
          { field: 'primaryContactName', title: 'Primary Contact' },
          { field: 'primaryContactPhone', title: 'Contact #' },
          { field: 'hires', title: 'Hires', style: { textAlign: 'center' } },
          { field: 'requests', title: 'Requests', style: { textAlign: 'center' } },
          { field: 'status', title: 'Status', style: { textAlign: 'center' } },
          { field: 'addProject', title: 'Add Project', style: { textAlign: 'right' }, node: addProjectButton },
        ]}
      >
        {_.map(projects, (project) => {
          return (
            <tr key={project.id} className={project.isActive ? null : 'bg-info'}>
              <td>{project.name}</td>
              <td>{project.fiscalYear}</td>
              <td>{project.provincialProjectNumber}</td>
              <td>{project.primaryContactName}</td>
              <td>{project.primaryContactPhone}</td>
              <td style={{ textAlign: 'center' }}>{project.hires}</td>
              <td style={{ textAlign: 'center' }}>{project.requests}</td>
              <td style={{ textAlign: 'center' }}>{project.status}</td>
              <td style={{ textAlign: 'right' }}>
                <ButtonGroup>
                  <EditButton
                    name="Project"
                    hide={!project.canView}
                    view
                    pathname={`${Constant.PROJECTS_PATHNAME}/${project.id}`}
                  />
                </ButtonGroup>
              </td>
            </tr>
          );
        })}
      </SortTable>
    );
  };

  render() {
    var resultCount = '';
    if (this.props.projects.loaded) {
      resultCount = '(' + Object.keys(this.props.projects.data).length + ')';
    }

    return (
      <div id="projects-list">
        <PageHeader>
          Projects {resultCount}
          <ButtonGroup>
            <PrintButton disabled={!this.props.projects.loaded} />
          </ButtonGroup>
        </PageHeader>
        <SearchBar>
          <Form onSubmit={this.search}>
            <Row>
              <Col xs={9} sm={10} id="filters">
                <ButtonToolbar>
                  <DropdownControl
                    id="statusCode"
                    title={this.state.search.statusCode}
                    updateState={this.updateSearchState}
                    blankLine="(All)"
                    placeholder="Status"
                    items={[Constant.PROJECT_STATUS_CODE_ACTIVE, Constant.PROJECT_STATUS_CODE_COMPLETED]}
                  />
                  <FormInputControl
                    id="projectName"
                    type="text"
                    placeholder="Project name"
                    value={this.state.search.projectName}
                    updateState={this.updateSearchState}
                  ></FormInputControl>
                  <FormInputControl
                    id="projectNumber"
                    type="text"
                    placeholder="Project number"
                    value={this.state.search.projectNumber}
                    updateState={this.updateSearchState}
                  ></FormInputControl>
                  <DropdownControl
                    id="fiscalYear"
                    placeholder="Fiscal year"
                    blankLine="(All)"
                    title={this.state.search.fiscalYear}
                    updateState={this.updateSearchState}
                    items={this.props.fiscalYears}
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
                    type="project"
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
          if (this.props.projects.loading) {
            return (
              <div style={{ textAlign: 'center' }}>
                <Spinner />
              </div>
            );
          }

          var addProjectButton = (
            <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
              <Button className="btn-custom" title="Add Project" size="sm" onClick={this.openAddDialog}>
                <FontAwesomeIcon icon="plus" />
                &nbsp;<strong>Add Project</strong>
              </Button>
            </Authorize>
          );

          if (this.props.projects.loaded) {
            return this.renderResults(addProjectButton);
          }

          return <AddButtonContainer>{addProjectButton}</AddButtonContainer>;
        })()}
        {this.state.showAddDialog && (
          <ProjectsAddDialog show={this.state.showAddDialog} onSave={this.projectAdded} onClose={this.closeAddDialog} />
        )}
      </div>
    );
  }
}

function mapStateToProps(state) {
  return {
    fiscalYears: state.lookups.fiscalYears,
    projects: state.models.projects,
    favourites: state.models.favourites.project,
    search: state.search.projects,
    ui: state.ui.projects,
  };
}

export default connect(mapStateToProps)(Projects);
