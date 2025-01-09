import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Alert, Row, Col, ButtonToolbar, Button, ButtonGroup, Form } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import _ from 'lodash';

import ProjectsAddDialog from './dialogs/ProjectsAddDialog.jsx';

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
import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';
import PrintButton from '../components/PrintButton.jsx';
import Authorize from '../components/Authorize.jsx';

const Projects = ({ history }) => {
  const dispatch = useDispatch();
  const fiscalYears = useSelector((state) => state.lookups.fiscalYears);
  const projects = useSelector((state) => state.models.projects);
  const favourites = useSelector((state) => state.models.favourites.project);
  const search = useSelector((state) => state.search.projects);
  const ui = useSelector((state) => state.ui.projects);

  const [showAddDialog, setShowAddDialog] = useState(false);
  const [searchState, setSearchState] = useState({
    statusCode: search.statusCode || Constant.PROJECT_STATUS_CODE_ACTIVE,
    projectName: search.projectName || '',
    projectNumber: search.projectNumber || '',
    fiscalYear: search.fiscalYear || '',
  });
  const [uiState, setUIState] = useState({
    sortField: ui.sortField || 'name',
    sortDesc: ui.sortDesc === true,
  });

  useEffect(() => {
    if (_.isEmpty(search)) {
      const defaultFavourite = _.find(favourites, (f) => f.isDefault);
      if (defaultFavourite) {
        loadFavourite(defaultFavourite);
      }
    }
  }, [search, favourites]);

  const buildSearchParams = () => {
    const searchParams = {};

    if (searchState.projectName) {
      searchParams.project = searchState.projectName;
    }

    if (searchState.statusCode) {
      searchParams.status = searchState.statusCode;
    }

    if (searchState.projectNumber) {
      searchParams.projectNumber = searchState.projectNumber;
    }

    if (searchState.fiscalYear) {
      searchParams.fiscalYear = searchState.fiscalYear;
    }

    return searchParams;
  };

  const fetch = () => {
    dispatch(Api.searchProjects(buildSearchParams()));
  };

  const searchProjects = (e) => {
    e.preventDefault();
    fetch();
  };

  const clearSearch = () => {
    const defaultSearchParameters = {
      statusCode: Constant.PROJECT_STATUS_CODE_ACTIVE,
      projectName: '',
      projectNumber: '',
      fiscalYear: '',
    };

    setSearchState(defaultSearchParameters);
    dispatch({ type: Action.UPDATE_PROJECTS_SEARCH, projects: defaultSearchParameters });
    dispatch({ type: Action.CLEAR_PROJECTS });
  };

  const updateSearchState = (state, callback) => {
    setSearchState((prevState) => ({ ...prevState, ...state, ...{ loaded: true } }));
    dispatch({ type: Action.UPDATE_PROJECTS_SEARCH, projects: { ...searchState, ...state } });
    if (callback) {
      callback();
    }
  };

  const updateUIState = (state, callback) => {
    setUIState((prevState) => ({ ...prevState, ...state }));
    dispatch({ type: Action.UPDATE_PROJECTS_UI, projects: { ...uiState, ...state } });
    if (callback) {
      callback();
    }
  };

  const loadFavourite = (favourite) => {
    updateSearchState(JSON.parse(favourite.value), fetch);
  };

  const openAddDialog = () => {
    setShowAddDialog(true);
  };

  const closeAddDialog = () => {
    setShowAddDialog(false);
  };

  const projectAdded = (project) => {
    fetch();
    history.push(`${Constant.PROJECTS_PATHNAME}/${project.id}`);
  };

  const renderResults = (addProjectButton) => {
    if (Object.keys(projects.data).length === 0) {
      return <Alert variant="success">No Projects {addProjectButton}</Alert>;
    }

    const sortedProjects = _.sortBy(projects.data, (project) => {
      const sortValue = project[uiState.sortField];
      if (typeof sortValue === 'string') {
        return sortValue.toLowerCase();
      }
      return sortValue;
    });

    if (uiState.sortDesc) {
      _.reverse(sortedProjects);
    }

    return (
      <SortTable
        sortField={uiState.sortField}
        sortDesc={uiState.sortDesc}
        onSort={updateUIState}
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
        {_.map(sortedProjects, (project) => (
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
        ))}
      </SortTable>
    );
  };

  let resultCount = '';
  if (projects.loaded) {
    resultCount = `(${Object.keys(projects.data).length})`;
  }

  return (
    <div id="projects-list">
      <PageHeader>
        Projects {resultCount}
        <ButtonGroup>
          <PrintButton disabled={!projects.loaded} />
        </ButtonGroup>
      </PageHeader>
      <SearchBar>
        <Form onSubmit={searchProjects}>
          <Row>
            <Col xs={9} sm={10} id="filters">
              <ButtonToolbar>
                <DropdownControl
                  id="statusCode"
                  title={searchState.statusCode}
                  updateState={updateSearchState}
                  blankLine="(All)"
                  placeholder="Status"
                  items={[Constant.PROJECT_STATUS_CODE_ACTIVE, Constant.PROJECT_STATUS_CODE_COMPLETED]}
                />
                <FormInputControl
                  id="projectName"
                  type="text"
                  placeholder="Project name"
                  value={searchState.projectName}
                  updateState={updateSearchState}
                />
                <FormInputControl
                  id="projectNumber"
                  type="text"
                  placeholder="Project number"
                  value={searchState.projectNumber}
                  updateState={updateSearchState}
                />
                <DropdownControl
                  id="fiscalYear"
                  placeholder="Fiscal year"
                  blankLine="(All)"
                  title={searchState.fiscalYear}
                  updateState={updateSearchState}
                  items={fiscalYears}
                />
                <Button id="search-button" variant="primary" type="submit">
                  Search
                </Button>
                <Button className="btn-custom" id="clear-search-button" onClick={clearSearch}>
                  Clear
                </Button>
              </ButtonToolbar>
            </Col>
            <Col xs={3} sm={2} id="search-buttons">
              <Row className="float-right">
                <Favourites
                  id="faves-dropdown"
                  type="project"
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
        if (projects.loading) {
          return (
            <div style={{ textAlign: 'center' }}>
              <Spinner />
            </div>
          );
        }

        const addProjectButton = (
          <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
            <Button className="btn-custom" title="Add Project" size="sm" onClick={openAddDialog}>
              <FontAwesomeIcon icon="plus" />
              &nbsp;<strong>Add Project</strong>
            </Button>
          </Authorize>
        );

        if (projects.loaded) {
          return renderResults(addProjectButton);
        }

        return <AddButtonContainer>{addProjectButton}</AddButtonContainer>;
      })()}
      {showAddDialog && (
        <ProjectsAddDialog show={showAddDialog} onSave={projectAdded} onClose={closeAddDialog} />
      )}
    </div>
  );
};

Projects.propTypes = {
  history: PropTypes.object,
};

export default Projects;