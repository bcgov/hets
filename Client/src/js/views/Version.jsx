import React from 'react';
import { connect } from 'react-redux';
import { PageHeader, Well } from 'react-bootstrap';
import { Button, Glyphicon } from 'react-bootstrap';
import $ from 'jquery';

import * as Api from '../api';
import * as Constant from '../constants';

import ColDisplay from '../components/ColDisplay.jsx';
import Spinner from '../components/Spinner.jsx';
import Unimplemented from '../components/Unimplemented.jsx';
import PrintButton from '../components/PrintButton.jsx';

import { formatDateTime } from '../utils/date';
import { request } from '../utils/http';


var Version = React.createClass({
  propTypes: {
    version: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loading: false,
      showRawSection: false,
      buildtime : '',
      version : '',
      commit : '',
    };
  },

  componentDidMount() {
    this.setState({ loading: true });
    Api.getVersion().finally(() => {
      this.fetchLocal().finally(() => {
        this.setState({ loading: false });
      });
    });
  },

  fetchLocal() {
    return request('buildinfo.html', { silent: true }).then(xhr => {
      if (xhr.status === 200) {
        this.setState({
          buildTime : $(xhr.responseText).find('#buildtime').data('buildtime'),
          version : $(xhr.responseText).find('#version').text(),
          commit : $(xhr.responseText).find('#commit').text(),
        });
      }
    }).catch(err => {
      console.err('Failed to find buildinfo: ', err);
    });
  },

  showRaw(e) {
    if (this.state.showRawSection) {
      this.setState({ showRawSection: false });
      e.target.textContent = 'Show Raw Versions';
    } else {
      this.setState({ showRawSection: true });
      e.target.textContent = 'Hide Raw Versions';
    }
  },

  email() {

  },

  render: function() {
    return <div id="version">
      <PageHeader id="version-header">Version
        <div id="version-buttons" style={ { float: 'right' } }>
          <Unimplemented>
            <Button className="mr-5" onClick={ this.email }><Glyphicon glyph="envelope" title="E-mail" /></Button>
          </Unimplemented>
          <PrintButton/>
        </div>
      </PageHeader>

      {(() => {
        if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

        var applicationVersion = {};
        if (this.props.version.applicationVersions && this.props.version.applicationVersions.length > 0) {
          applicationVersion = this.props.version.applicationVersions[0];
        }
        var databaseVersion = {};
        if (this.props.version.databaseVersions && this.props.version.databaseVersions.length > 0) {
          databaseVersion = this.props.version.databaseVersions[0];
        }

        return <div id="version-details">
          <Well>
            <h3>Client</h3>
            <div className="clearfix">
              <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="Name">MOTI Hired Equipment Tracking System</ColDisplay>
              <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="Build Time">{ formatDateTime(this.state.buildTime, Constant.DATE_TIME_READABLE) }</ColDisplay>
              <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="User Agent">{ navigator.userAgent }</ColDisplay>
            </div>
          </Well>
          <Well>
            <h3>Application</h3>
            <div className="clearfix">
              <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="Name">{ applicationVersion.title }</ColDisplay>
              <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="Build Time">{ formatDateTime(applicationVersion.fileCreationTime, Constant.DATE_TIME_READABLE) }</ColDisplay>
              <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="Version">{ applicationVersion.informationalVersion }</ColDisplay>
              <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="Framework">{ applicationVersion.targetFramework }</ColDisplay>
              <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="HETS Release">{ applicationVersion.buildVersion }</ColDisplay>
              <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="HETS Environment">{ applicationVersion.environment }</ColDisplay>
            </div>
          </Well>
          <Well>
            <h3>Database</h3>
            <div className="clearfix">
              <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="Name">{ databaseVersion.database }</ColDisplay>
              <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="Server">{ databaseVersion.server }</ColDisplay>
              <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="Postgres Version">{ databaseVersion.version }</ColDisplay>
              <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="HETS Release">{ databaseVersion.buildVersion }</ColDisplay>
              <ColDisplay labelProps={{ xs: 2 }} fieldProps={{ xs: 10 }} label="HETS Environment">{ databaseVersion.environment }</ColDisplay>
            </div>
          </Well>
          <Button onClick={ this.showRaw }>Show Raw Versions</Button>
          <Well style={{ marginTop: '20px', wordWrap: 'break-word' }} className={ this.state.showRawSection ? '' : 'hide' }>
            <div>{ JSON.stringify(this.props.version) }</div>
          </Well>
        </div>;
      })()}
    </div>;
  },
});


function mapStateToProps(state) {
  return {
    version: state.version,
  };
}

export default connect(mapStateToProps)(Version);
