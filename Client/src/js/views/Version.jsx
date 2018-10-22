import React from 'react';

import { connect } from 'react-redux';

import { PageHeader, Well, Row } from 'react-bootstrap';
import { Button, Glyphicon } from 'react-bootstrap';

import $ from 'jquery';

import * as Api from '../api';
import * as Constant from '../constants';

import ColDisplay from '../components/ColDisplay.jsx';
import Spinner from '../components/Spinner.jsx';
import Unimplemented from '../components/Unimplemented.jsx';

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
          buildTime : $(xhr.responseText).find('#buildtime').text(),
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

  print() {
    window.print();
  },

  render: function() {
    return <div id="version">
      <PageHeader id="version-header">Version
        <div id="version-buttons" style={ { float: 'right' } }>
          <Unimplemented>
            <Button className="mr-5" onClick={ this.email }><Glyphicon glyph="envelope" title="E-mail" /></Button>
          </Unimplemented>
          <Button onClick={ this.print }><Glyphicon glyph="print" title="Print" /></Button>
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
            <Row>
              <ColDisplay labelProps={{ md: 2 }} label="Name">MOTI Hired Equipment Tracking System</ColDisplay>
            </Row>
            <Row>
              <ColDisplay labelProps={{ md: 2 }} label="Build Time">{ formatDateTime(this.state.buildTime, Constant.DATE_TIME_READABLE) }</ColDisplay>
            </Row>            
            <Row>
              <ColDisplay labelProps={{ md: 2 }} label="User Agent">{ navigator.userAgent }</ColDisplay>
            </Row>
          </Well>
          <Well>
            <h3>Application</h3>
            <Row>
              <ColDisplay labelProps={{ md: 2 }} label="Name">{ applicationVersion.title }</ColDisplay>
            </Row>
            <Row>
              <ColDisplay labelProps={{ md: 2 }} label="Build Time">{ formatDateTime(applicationVersion.fileCreationTime, Constant.DATE_TIME_READABLE) }</ColDisplay>
            </Row>
            <Row>
              <ColDisplay labelProps={{ md: 2 }} label="Version">{ applicationVersion.informationalVersion }</ColDisplay>
            </Row>            
            <Row>
              <ColDisplay labelProps={{ md: 2 }} label="Framework">{ applicationVersion.targetFramework }</ColDisplay>
            </Row>
			<Row>
              <ColDisplay labelProps={{ md: 2 }} label="HETS Release">{ applicationVersion.buildVersion }</ColDisplay>
            </Row>
			<Row>
              <ColDisplay labelProps={{ md: 2 }} label="HETS Environment">{ applicationVersion.environment }</ColDisplay>
            </Row>
          </Well>
          <Well>
            <h3>Database</h3>
            <Row>
              <ColDisplay labelProps={{ md: 2 }} label="Name">{ databaseVersion.database }</ColDisplay>
            </Row>
            <Row>
              <ColDisplay labelProps={{ md: 2 }} label="Server">{ databaseVersion.server }</ColDisplay>
            </Row>
            <Row>
              <ColDisplay labelProps={{ md: 2 }} label="Postgres Version">{ databaseVersion.version }</ColDisplay>
            </Row>
			<Row>
              <ColDisplay labelProps={{ md: 2 }} label="HETS Release">{ databaseVersion.buildVersion }</ColDisplay>
            </Row>
			<Row>
              <ColDisplay labelProps={{ md: 2 }} label="HETS Environment">{ databaseVersion.environment }</ColDisplay>
            </Row>
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

