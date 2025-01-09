import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Button } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import * as Api from '../api';
import * as Constant from '../constants';

import PageHeader from '../components/ui/PageHeader.jsx';
import ColDisplay from '../components/ColDisplay.jsx';
import Spinner from '../components/Spinner.jsx';
import Unimplemented from '../components/Unimplemented.jsx';
import PrintButton from '../components/PrintButton.jsx';

import { formatDateTime } from '../utils/date';
import { request } from '../utils/http';

class Version extends React.Component {
  static propTypes = {
    version: PropTypes.object,
  };

  // Constructor to initialize the component state
  constructor(props) {
    super(props);

    this.state = {
      loading: false,           // Indicates if the data is being loaded
      showRawSection: false,    // Toggles visibility of raw version data
      buildtime: '',            // Holds build time from local data
      version: '',              // Holds version information from local data
      commit: '',               // Holds commit hash from local data
    };
  }

  // Component lifecycle method that runs when the component mounts
  componentDidMount() {
    this.setState({ loading: true });
    // Dispatching API request to get version info and then fetching local version info
    this.props.dispatch(Api.getVersion())
      .finally(() => {
        this.fetchLocal().finally(() => {
          this.setState({ loading: false });
        });
      });
  }

  // Fetch local version information from 'buildinfo.html'
  fetchLocal = async () => {
    try {
      const xhr = await this.props.dispatch(request('buildinfo.html', { silent: true }));
      if (xhr.status === 200) {
        const parser = new DOMParser();
        const doc = parser.parseFromString(xhr.responseText, 'text/html');

        // Setting the state with parsed build info
        this.setState({
          buildTime: doc.getElementById('buildtime').dataset.buildtime,
          version: doc.getElementById('version').textContent,
          commit: doc.getElementById('commit').textContent,
        });
      }
    } catch (err) {
      console.error('Failed to find buildinfo: ', err);
    }
  };

  // Toggle visibility of raw version data
  showRaw = (e) => {
    if (this.state.showRawSection) {
      this.setState({ showRawSection: false });
      e.target.textContent = 'Show Raw Versions';
    } else {
      this.setState({ showRawSection: true });
      e.target.textContent = 'Hide Raw Versions';
    }
  };

  // Placeholder email function (currently not implemented)
  email = () => {};

  render() {
    return (
      <div id="version">
        <PageHeader id="version-header">
          Version
          <div id="version-buttons" style={{ float: 'right' }}>
            {/* Button for email (currently unimplemented) */}
            <Unimplemented>
              <Button className="mr-2 btn-custom" onClick={this.email}>
                <FontAwesomeIcon icon="envelope" title="E-mail" />
              </Button>
            </Unimplemented>
            {/* Print Button for version details */}
            <PrintButton />
          </div>
        </PageHeader>

        {(() => {
          if (this.state.loading) {
            // Show spinner while loading version data
            return (
              <div style={{ textAlign: 'center' }}>
                <Spinner />
              </div>
            );
          }

          // Retrieve version information from props (redux state)
          let applicationVersion = {};
          if (this.props.version.applicationVersions && this.props.version.applicationVersions.length > 0) {
            applicationVersion = this.props.version.applicationVersions[0];
          }
          let databaseVersion = {};
          if (this.props.version.databaseVersions && this.props.version.databaseVersions.length > 0) {
            databaseVersion = this.props.version.databaseVersions[0];
          }

          return (
            <div id="version-details">
              {/* Client Version Details */}
              <div className="well">
                <h3>Client</h3>
                <div className="clearfix">
                  <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="Name">
                    MOTI Hired Equipment Tracking System
                  </ColDisplay>
                  <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="Build Time">
                    {formatDateTime(this.state.buildTime, Constant.DATE_TIME_READABLE)}
                  </ColDisplay>
                  <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="Git Commit">
                    {Constant.RUNTIME_OPENSHIFT_BUILD_COMMIT}
                  </ColDisplay>
                  <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="User Agent">
                    {navigator.userAgent}
                  </ColDisplay>
                </div>
              </div>

              {/* Application Version Details */}
              <div className="well">
                <h3>Application</h3>
                <div className="clearfix">
                  <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="Name">
                    {applicationVersion.title}
                  </ColDisplay>
                  <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="Build Time">
                    {formatDateTime(applicationVersion.fileCreationTime, Constant.DATE_TIME_READABLE)}
                  </ColDisplay>
                  <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="Git Commit">
                    {applicationVersion.commit}
                  </ColDisplay>
                  <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="Version">
                    {applicationVersion.informationalVersion}
                  </ColDisplay>
                  <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="Framework">
                    {applicationVersion.targetFramework}
                  </ColDisplay>
                  <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="HETS Release">
                    {applicationVersion.buildVersion}
                  </ColDisplay>
                  <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="HETS Environment">
                    {applicationVersion.environment}
                  </ColDisplay>
                </div>
              </div>

              {/* Database Version Details */}
              <div className="well">
                <h3>Database</h3>
                <div className="clearfix">
                  <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="Name">
                    {databaseVersion.database}
                  </ColDisplay>
                  <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="Server">
                    {databaseVersion.server}
                  </ColDisplay>
                  <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="Postgres Version">
                    {databaseVersion.version}
                  </ColDisplay>
                  <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="HETS Release">
                    {databaseVersion.buildVersion}
                  </ColDisplay>
                  <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="HETS Environment">
                    {databaseVersion.environment}
                  </ColDisplay>
                </div>
              </div>

              {/* Toggle raw version data visibility */}
              <Button className="btn-custom" onClick={this.showRaw}>
                Show Raw Versions
              </Button>

              {/* Raw version data (hidden initially) */}
              <div
                style={{ marginTop: '20px', wordWrap: 'break-word' }}
                className={this.state.showRawSection ? 'well' : 'd-none'}
              >
                <div>{JSON.stringify(this.props.version)}</div>
              </div>
            </div>
          );
        })()}
      </div>
    );
  }
}

// Mapping state to props to get version information from Redux store
const mapStateToProps = (state) => ({
  version: state.version,
});

// Mapping dispatch to props to allow dispatching actions from the component
const mapDispatchToProps = (dispatch) => ({ dispatch });

// Connecting the component to Redux store
export default connect(mapStateToProps, mapDispatchToProps)(Version);
