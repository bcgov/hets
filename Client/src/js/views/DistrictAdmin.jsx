import React from 'react';

import { connect } from 'react-redux';

import { PageHeader, Row, Col, Button, ButtonGroup, Glyphicon, Well } from 'react-bootstrap';

import _ from 'lodash';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import TableControl from '../components/TableControl.jsx';
import Spinner from '../components/Spinner.jsx';
import OverlayTrigger from '../components/OverlayTrigger.jsx';
import Confirm from '../components/Confirm.jsx';
import ConditionAddEditDialog from './dialogs/ConditionAddEditDialog.jsx';
import DistrictEquipmentTypeAddEditDialog from './dialogs/DistrictEquipmentTypeAddEditDialog.jsx';

var DistrictAdmin = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
    rentalConditions: React.PropTypes.object,
    districtEquipmentTypes: React.PropTypes.object,
    equipmentTypes: React.PropTypes.object,
    router: React.PropTypes.object,
  },

  getInitialState() {
    return {
      showConditionAddEditDialog: false,
      showDistrictEquipmentTypeAddEditDialog: false,
      condition: {},
      districtEquipmentType: {},
    };
  },

  componentDidMount() {
    Api.getRentalConditions();
    Api.getDistrictEquipmentTypes(this.props.currentUser.district.id);
    Api.getEquipmentTypes();
  },

  addCondition() {
    this.setState({ condition: { id: 0 } }, this.showConditionAddEditDialog());
  },

  editCondition(condition) {
    this.setState({ condition: condition }, this.showConditionAddEditDialog());
  },

  deleteCondition(condition) {
    Api.deleteCondition(condition.id);
  },

  showConditionAddEditDialog() {
    this.setState({ showConditionAddEditDialog: true });
  },

  closeConditionAddEditDialog() {
    this.setState({ showConditionAddEditDialog: false });
  },

  onConditionSave(data) {
    let condition = { ...data, district: { id: this.props.currentUser.district.id } };
    let promise = Api.addCondition(condition);
    if (condition.id !== 0) {
      promise = Api.updateCondition(condition);
    }
    promise.then(() => {
      Api.getRentalConditions();
      this.closeConditionAddEditDialog();
    });
  },

  showDistrictEquipmentTypeAddEditDialog() {
    this.setState({ showDistrictEquipmentTypeAddEditDialog: true });
  },

  closeDistrictEquipmentTypeAddEditDialog() {
    this.setState({ showDistrictEquipmentTypeAddEditDialog: false });
  },

  addDistrictEquipmentType() {
    this.setState({ districtEquipmentType: { id: 0 } }, this.showDistrictEquipmentTypeAddEditDialog());
  },

  editDistrictEquipmentType(equipment) {
    this.setState({ districtEquipmentType: equipment }, this.showDistrictEquipmentTypeAddEditDialog());
  },

  onDistrictEquipmentTypeSave(data) {
    let equipment = { ...data, district: { id: this.props.currentUser.district.id } };
    let promise = Api.addDistrictEquipmentType(equipment);
    if (equipment.id !== 0) {
      promise = Api.updateDistrictEquipmentType(equipment);
    }
    promise.then(() => {
      Api.getDistrictEquipmentTypes(this.props.currentUser.district.id);
      this.closeDistrictEquipmentTypeAddEditDialog();
    });
  },

  deleteDistrictEquipmentType(equipment) {
    Api.deleteDistrictEquipmentType(equipment).then(() => {
      Api.getDistrictEquipmentTypes(this.props.currentUser.district.id); 
    });
  },

  render() {
    var equipmentTypes = _.chain(this.props.equipmentTypes)
      .sortBy('name')
      .value();

    return <div id="district-admin">
      <PageHeader>District Admin</PageHeader>

      {(() => {
        if (this.props.rentalConditions.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }


        var addDistrictEquipmentButton = <Button title="Add District Equipment" bsSize="xsmall" onClick={ this.addDistrictEquipmentType }><Glyphicon glyph="plus" />&nbsp;<strong>Add District Equipment Type</strong></Button>;

        return (
          <Well>
            <h3>Manage District Equipment Types</h3>
            <TableControl headers={[
              { field: 'districtEquipmentName',         title: 'District Equipment Name'  },
              { field: 'equipmentType',                 title: 'Equipment Type'           },
              { field: 'addDistrictEquipmentType', title: 'Add District Equipment Type',  style: { textAlign: 'right'  },
                node: addDistrictEquipmentButton,
              },
            ]}>
              {
                _.map(this.props.districtEquipmentTypes.data, (equipment) => {
                  return <tr key={ equipment.id }>
                    <td>{ equipment.districtEquipmentName }</td>
                    <td>{ equipment.equipmentType.name }</td>
                    <td style={{ textAlign: 'right' }}>
                      <ButtonGroup>
                        <OverlayTrigger trigger="click" placement="top" rootClose overlay={ <Confirm onConfirm={ this.deleteDistrictEquipmentType.bind(this, equipment) }/> }>
                          <Button title="Delete District Equipment Type" bsSize="xsmall"><Glyphicon glyph="trash" /></Button>
                        </OverlayTrigger>
                        <Button title="Edit District Equipment Type" bsSize="xsmall" onClick={ this.editDistrictEquipmentType.bind(this, equipment) }><Glyphicon glyph="edit" /></Button>
                      </ButtonGroup>
                    </td> 
                  </tr>;
                })
              }
            </TableControl>
          </Well>
        );
      })()}


      {(() => {
        if (this.props.rentalConditions.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

        var addConditionButton = <Button title="Add Condition" bsSize="xsmall" onClick={ this.addCondition }><Glyphicon glyph="plus" />&nbsp;<strong>Add Condition</strong></Button>;

        return (
          <Well>
            <h3>Manage Conditions</h3>
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
                      <ButtonGroup>
                        <OverlayTrigger trigger="click" placement="top" rootClose overlay={ <Confirm onConfirm={ this.deleteCondition.bind(this, condition) }/> }>
                          <Button title="Delete User" bsSize="xsmall"><Glyphicon glyph="trash" /></Button>
                        </OverlayTrigger>
                        <Button title="Edit Condition" bsSize="xsmall" onClick={ this.editCondition.bind(this, condition) }><Glyphicon glyph="edit" /></Button>
                      </ButtonGroup>
                    </td>
                  </tr>;
                })
              }
            </TableControl>
          </Well>
        );
      })()}

      { this.state.showConditionAddEditDialog &&
        <ConditionAddEditDialog
          show={this.state.showConditionAddEditDialog}
          onClose={this.closeConditionAddEditDialog}
          onSave={this.onConditionSave}
          condition={this.state.condition}
        />        
      }
      { this.state.showDistrictEquipmentTypeAddEditDialog &&
        <DistrictEquipmentTypeAddEditDialog
          show={this.state.showDistrictEquipmentTypeAddEditDialog}
          onClose={this.closeDistrictEquipmentTypeAddEditDialog}
          onSave={this.onDistrictEquipmentTypeSave}
          districtEquipmentType={this.state.districtEquipmentType}
          equipmentTypes={equipmentTypes}
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
    equipmentTypes: state.lookups.equipmentTypes,
  };
}

export default connect(mapStateToProps)(DistrictAdmin);
