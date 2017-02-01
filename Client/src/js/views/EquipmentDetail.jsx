import React from 'react';

import { connect } from 'react-redux';

import { PageHeader, Row, Col } from 'react-bootstrap';
import { Button, Glyphicon } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';

import * as Api from '../api';

import ColField from '../components/ColField.jsx';
import ColLabel from '../components/ColLabel.jsx';
import Spinner from '../components/Spinner.jsx';

import { formatDateTime } from '../utils/date';

var EquipmentDetail = React.createClass({
  propTypes: {
    equipment: React.PropTypes.object,
    params: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loading: false,
    };
  },

  componentDidMount() {
    this.fetch();
  },

  fetch() {
    this.setState({ loading: true });
    var equipId = this.props.params.equipmentId;
    // Make several calls here
    Api.getEquipment(equipId).finally(() => {
      this.setState({ loading: false });
    });
  },

  render() {
    var equipment = this.props.equipment;

    return <div id="owners-detail">
      <PageHeader>Equipment Details</PageHeader>

      {(() => {
        if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner /></div>; }

        return <div>
          <Row>
            <Col md={10}></Col>
            <Col md={2}>
              <div className="pull-right">
                <Button><Glyphicon glyph="print" title="Print" /></Button>
                <LinkContainer to={{ pathname: 'equipment' }}>
                  <Button title="Return to List"><Glyphicon glyph="arrow-left" /> Return to List</Button>
                </LinkContainer>
              </div>
            </Col>
          </Row>
          <Row>
            <ColLabel md={4}>Equip Id</ColLabel>
            <ColField md={8}>{ equipment.equipCd }</ColField>
          </Row>
          <Row>
            <ColLabel md={4}>Make</ColLabel>
            <ColField md={8}>{ equipment.make }</ColField>
          </Row>
          <Row>
            <ColLabel md={4}>Model</ColLabel>
            <ColField md={8}>{ equipment.model }</ColField>
          </Row>
          <Row>
            <ColLabel md={4}>Year</ColLabel>
            <ColField md={8}>{ equipment.year }</ColField>
          </Row>
          <Row>
            <ColLabel md={4}>Size</ColLabel>
            <ColField md={8}>{ equipment.size }</ColField>
          </Row>
          <Row>
            <ColLabel md={4}>Type</ColLabel>
            <ColField md={8}>{ equipment.typeName }</ColField>
          </Row>
          <Row>
            <ColLabel md={4}>Last Verified Date</ColLabel>
            <ColField md={8}>{ formatDateTime(equipment.lastVerifiedDate, 'YYYY-MMM-DD') }</ColField>
          </Row>
        </div>;
      })()}

    </div>;
  },
});


function mapStateToProps(state) {
  return {
    equipment: state.models.equipment,
  };
}

export default connect(mapStateToProps)(EquipmentDetail);
