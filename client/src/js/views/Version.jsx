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

  constructor(props) {
    super(props);

    this.state = {
      loading: false,
      showRawSection: false,
      buildtime: '',
      version: '',
      commit: '',
    };
  }

  componentDidMount() {
    this.setState({ loading: true });
    this.props.dispatch(Api.getVersion()).finally(() => {
      this.fetchLocal().finally(() => {
        this.setState({ loading: false });
      });
    });
  }

  fetchLocal = async () => {
    try {
      const xhr = await this.props.dispatch(request('buildinfo.html', { silent: true }));
      if (xhr.status === 200) {
        const parser = new DOMParser();
        const doc = parser.parseFromString(xhr.responseText, 'text/html');

        this.setState({
          buildTime: doc.getElementById('buildtime').dataset.buildtime,
          version: doc.getElementById('version').textContent,
          commit: doc.getElementById('commit').textContent,
        });
      }
    } catch(err) {
      console.error('Failed to find buildinfo: ', err);
    };
  };

  showRaw = (e) => {
    if (this.state.showRawSection) {
      this.setState({ showRawSection: false });
      e.target.textContent = 'Show Raw Versions';
    } else {
      this.setState({ showRawSection: true });
      e.target.textContent = 'Hide Raw Versions';
    }
  };

  email = () => {};

  render() {
    return (
      <div id="version">
        <PageHeader id="version-header">
          Version
          <div id="version-buttons" style={{ float: 'right' }}>
            <Unimplemented>
              <Button className="mr-2 btn-custom" onClick={this.email}>
                <FontAwesomeIcon icon="envelope" title="E-mail" />
              </Button>
            </Unimplemented>
            <PrintButton />
          </div>
        </PageHeader>

        {(() => {
          if (this.state.loading) {
            return (
              <div style={{ textAlign: 'center' }}>
                <Spinner />
              </div>
            );
          }

          var applicationVersion = {};
          if (this.props.version.applicationVersions && this.props.version.applicationVersions.length > 0) {
            applicationVersion = this.props.version.applicationVersions[0];
          }
          var databaseVersion = {};
          if (this.props.version.databaseVersions && this.props.version.databaseVersions.length > 0) {
            databaseVersion = this.props.version.databaseVersions[0];
          }

          return (
            <div id="version-details">
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
              <Button className="btn-custom" onClick={this.showRaw}>
                Show Raw Versions
              </Button>
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

const mapStateToProps = (state) => ({
  version: state.version,
});

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(mapStateToProps, mapDispatchToProps)(Version);
