import React from 'react';

import { connect } from 'react-redux';

import { PageHeader, Row, Col, Button, Glyphicon } from 'react-bootstrap';

import _ from 'lodash';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import TableControl from '../components/TableControl.jsx';
import Spinner from '../components/Spinner.jsx';
import OverlayTrigger from '../components/OverlayTrigger.jsx';
import Confirm from '../components/Confirm.jsx';
import ConditionAddDialog from './dialogs/ConditionAddDialog.jsx';

var DistrictAdmin = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
    rentalConditions: React.PropTypes.object,
    districtEquipmentTypes: React.PropTypes.object,
    router: React.PropTypes.object,
  },

  getInitialState() {
    return {
      showConditionAddDialog: false,
      conditionId: 0,
    };
  },

  componentDidMount() {
    Api.getRentalConditions();
    Api.getDistrictEquipmentTypes(this.props.currentUser.district.id);
  },

  addCondition() {
    this.setState({ condition: { id: 0 } }, this.showConditionAddDialog());
  },

  editCondition(condition) {
    this.setState({ condition: condition }, this.showConditionAddDialog());
  },

  deleteCondition(condition) {
    Api.deleteCondition(condition.id);
  },

  showConditionAddDialog() {
    this.setState({ showConditionAddDialog: true });
  },

  closeConditionAddDialog() {
    this.setState({ showConditionAddDialog: false });
  },

  onConditionSave(data) {
    let condition = { ...data, district: { id: this.props.currentUser.district.id } };
    let promise = Api.addCondition(condition);
    if (condition.id !== 0) {
      promise = Api.updateCondition(condition);
    }
    promise.then(() => {
      Api.getRentalConditions();
      this.closeConditionAddDialog();
    });
  },

  addDistrictEquipmentType() {

  },

  editDistrictEquipmentType(equipment) {

  },

  deleteDistrictEquipmentType(equipment) {

  },

  render() {
    return <div id="home">
      <PageHeader>District Admin</PageHeader>

      {(() => {
        if (this.props.rentalConditions.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

        var addDistrictEquipmentButton = <Button title="Add District Equipment" bsSize="xsmall" onClick={ this.addDistrictEquipmentType }><Glyphicon glyph="plus" />&nbsp;<strong>Add District Equipment Type</strong></Button>;

        return (
          <TableControl headers={[
            { field: 'districtEquipmentName',         title: 'District Equipment Name'  },
            { field: 'addDistrictEquipmentType', title: 'Add District Equipment Type',  style: { textAlign: 'right'  },
              node: addDistrictEquipmentButton,
            },
          ]}>
            {
              _.map(this.props.districtEquipmentTypes.data, (equipment) => {
                console.log(equipment);
                return <tr key={ equipment.id }>
                  <td>{ equipment.districtEquipmentName }</td>
                   <td style={{ textAlign: 'right' }}>
                    <OverlayTrigger trigger="click" placement="top" rootClose overlay={ <Confirm onConfirm={ this.deleteDistrictEquipmentType.bind(this, equipment) }/> }>
                      <Button title="Delete District Equipment Type" bsSize="xsmall"><Glyphicon glyph="trash" /></Button>
                    </OverlayTrigger>
                    <Button title="Edit District Equipment Type" bsSize="xsmall" onClick={ this.editDistrictEquipmentType.bind(this, equipment) }><Glyphicon glyph="edit" /></Button>
                  </td> 
                </tr>;
              })
            }
          </TableControl>
        );
      })()}


      {(() => {
        if (this.props.rentalConditions.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

        var addConditionButton = <Button title="Add Condition" bsSize="xsmall" onClick={ this.addCondition }><Glyphicon glyph="plus" />&nbsp;<strong>Add Condition</strong></Button>;

        return (
          <TableControl headers={[
            { field: 'code',         title: 'Condition Code'  },
            { field: 'description',  title: 'Description'     },
            { field: 'addCondition', title: 'Add Condition',  style: { textAlign: 'right'  },
              node: addConditionButton,
            },
          ]}>
            {
              _.map(this.props.rentalConditions, (condition) => {
                return <tr key={ condition.id }>
                  <td>{ condition.conditionTypeCode }</td>
                  <td>{ condition.description }</td>
                  <td style={{ textAlign: 'right' }}>
                    <OverlayTrigger trigger="click" placement="top" rootClose overlay={ <Confirm onConfirm={ this.deleteCondition.bind(this, condition) }/> }>
                      <Button title="Delete User" bsSize="xsmall"><Glyphicon glyph="trash" /></Button>
                    </OverlayTrigger>
                    <Button title="Edit Condition" bsSize="xsmall" onClick={ this.editCondition.bind(this, condition) }><Glyphicon glyph="edit" /></Button>
                  </td>
                </tr>;
              })
            }
          </TableControl>
        );
      })()}

      { this.state.showConditionAddDialog &&
        <ConditionAddDialog
          show={this.state.showConditionAddDialog}
          onClose={this.closeConditionAddDialog}
          onSave={this.onConditionSave}
          condition={this.state.condition}
        />        
      }
    </div>;
  },
});


function mapStateToProps(state) {
  return {
    currentUser: state.user,
    rentalConditions: state.lookups.rentalConditions,
    districtEquipmentTypes: state.lookups.districtEquipmentTypes,
  };
}

export default connect(mapStateToProps)(DistrictAdmin);
