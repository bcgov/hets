import React from 'react';

import { connect } from 'react-redux';

import { PageHeader, Button, ButtonGroup, Glyphicon, Well, Alert, Row, Col } from 'react-bootstrap';

import _ from 'lodash';

import * as Api from '../api';
import * as Constant from '../constants';
import * as Action from '../actionTypes';
// import store from '../store';

import ModalDialog from '../components/ModalDialog.jsx';
import SortTable from '../components/SortTable.jsx';
import TableControl from '../components/TableControl.jsx';
import Spinner from '../components/Spinner.jsx';
import OverlayTrigger from '../components/OverlayTrigger.jsx';
import Confirm from '../components/Confirm.jsx';
import ConditionAddEditDialog from './dialogs/ConditionAddEditDialog.jsx';
import DistrictEquipmentTypeAddEditDialog from './dialogs/DistrictEquipmentTypeAddEditDialog.jsx';
import EquipmentTransferDialog from './dialogs/EquipmentTransferDialog.jsx';

import SubHeader from '../components/ui/SubHeader.jsx';

import { caseInsensitiveSort, sortDir } from '../utils/array';

var DistrictAdmin = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
    rentalConditions: React.PropTypes.object,
    districtEquipmentTypes: React.PropTypes.object,
    equipmentTypes: React.PropTypes.object,
    router: React.PropTypes.object,
    uiEquipment: React.PropTypes.object,
    dispatch: React.PropTypes.func,
  },

  getInitialState() {
    return {
      showConditionAddEditDialog: false,
      showDistrictEquipmentTypeAddEditDialog: false,
      showEquipmentTransferDialog: false,
      condition: {},
      districtEquipmentType: {},

      // Equipment
      uiEquipment : {
        sortField: this.props.uiEquipment.sortField || 'districtEquipmentName',
        sortDesc: this.props.uiEquipment.sortDesc  === true,
      },
    };
  },

  componentDidMount() {
    Api.getRentalConditions();
    Api.getDistrictEquipmentTypes(this.props.currentUser.district.id);
    Api.getEquipmentTypes();
  },

  updateEquipmentUIState(state, callback) {
    this.setState({ uiEquipment: { ...this.state.uiEquipment, ...state }}, () => {
      this.props.dispatch({ type: Action.UPDATE_DISTRICT_EQUIPMENT_UI, districtEquipment: this.state.uiEquipment });
      if (callback) { callback(); }
    });
  },

  addCondition() {
    this.setState({ condition: { id: 0 } }, this.showConditionAddEditDialog());
  },

  editCondition(condition) {
    this.setState({ condition: condition }, this.showConditionAddEditDialog());
  },

  deleteCondition(condition) {
    Api.deleteCondition(condition.id).then(() => {
      Api.getRentalConditions();
    });
  },

  showConditionAddEditDialog() {
    this.setState({ showConditionAddEditDialog: true });
  },

  closeConditionAddEditDialog() {
    this.setState({ showConditionAddEditDialog: false });
  },

  showEquipmentTransferDialog() {
    this.setState({ showEquipmentTransferDialog: true });
  },

  closeEquipmentTransferDialog() {
    this.setState({ showEquipmentTransferDialog: false });
  },

  onConditionSave(data) {
    let condition = { ...data, district: { id: this.props.currentUser.district.id } };
    let promise = Api.addCondition;
    if (condition.id !== 0) {
      promise = Api.updateCondition;
    }
    promise(condition).then(() => {
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

  closeDistrictEquipmentTypeErrorDialog() {
    this.setState({ showDistrictEquipmentTypeErrorDialog: false });
  },

  addDistrictEquipmentType() {
    this.setState({ districtEquipmentType: { id: 0 } }, this.showDistrictEquipmentTypeAddEditDialog());
  },

  editDistrictEquipmentType(equipment) {
    this.setState({ districtEquipmentType: equipment }, this.showDistrictEquipmentTypeAddEditDialog());
  },

  onDistrictEquipmentTypeSave(data) {
    let equipment = { ...data, district: { id: this.props.currentUser.district.id } };
    let promise = Api.addDistrictEquipmentType;
    if (equipment.id !== 0) {
      promise = Api.updateDistrictEquipmentType;
    }
    promise(equipment).then(() => {
      Api.getDistrictEquipmentTypes(this.props.currentUser.district.id);
      this.closeDistrictEquipmentTypeAddEditDialog();
    });
  },

  deleteDistrictEquipmentType(equipment) {
    Api.deleteDistrictEquipmentType(equipment).then(() => {
      Api.getDistrictEquipmentTypes(this.props.currentUser.district.id);
    }).catch(err => {
      this.setState({ showDistrictEquipmentTypeErrorDialog: true, districtEquipmentTypeError: err.message });
    });
  },

  render() {
    var equipmentTypes = _.chain(this.props.equipmentTypes)
      .sortBy('blueBookSection')
      .value();

    if (!this.props.currentUser.hasPermission(Constant.PERMISSION_DISTRICT_CODE_TABLE_MANAGEMENT) && !this.props.currentUser.hasPermission(Constant.PERMISSION_ADMIN)) {
      return (
        <div>You do not have permission to view this page.</div>
      );
    }

    return <div id="district-admin">
      <PageHeader>District Admin</PageHeader>

      <Well>
        <SubHeader title="Manage District Equipment Types"/>
        {(() => {
          if (this.props.districtEquipmentTypes.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

          var addDistrictEquipmentButton = <Button title="Add District Equipment" bsSize="xsmall" onClick={ this.addDistrictEquipmentType }><Glyphicon glyph="plus" />&nbsp;<strong>Add District Equipment Type</strong></Button>;

          var equipmentTypes = this.props.districtEquipmentTypes.data;

          if (Object.keys(equipmentTypes).length === 0) { return <Alert bsStyle="success">No equipment types { addDistrictEquipmentButton }</Alert>; }

          var sortedEquipmentTypes = caseInsensitiveSort(equipmentTypes, [this.state.uiEquipment.sortField], [sortDir(this.state.uiEquipment.sortDesc)]);

          var headers = [
            { field: 'districtEquipmentName',         title: 'Equipment Type/Description'  },
            { field: 'serviceAreaId',                 title: 'Service Area'  },
            { field: 'equipmentType.blueBookSection', title: 'Blue Book Section Number'  },
            { field: 'equipmentType.name',            title: 'Blue Book Section Name'  },
            { field: 'addDistrictEquipmentType',      title: 'Add District Equipment Type',  style: { textAlign: 'right'  },
              node: addDistrictEquipmentButton,
            },
          ];

          return (
            <SortTable id="district-equipment-types"  sortField={ this.state.uiEquipment.sortField } sortDesc={ this.state.uiEquipment.sortDesc } onSort={ this.updateEquipmentUIState } headers={headers}>
              {
                _.map(sortedEquipmentTypes, (equipment) => {
                  return <tr key={ equipment.id }>
                    <td>{ equipment.districtEquipmentName }</td>
                    <td>{ equipment.serviceAreaId }</td>
                    <td>{ equipment.equipmentType.blueBookSection }</td>
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
            </SortTable>
          );
        })()}
      </Well>

      <Well>
        <SubHeader title="Manage Conditions"/>
        {(() => {
          if (this.props.rentalConditions.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

          var addConditionButton = <Button title="Add Condition" bsSize="xsmall" onClick={ this.addCondition }><Glyphicon glyph="plus" />&nbsp;<strong>Add Condition</strong></Button>;

          if (Object.keys(this.props.rentalConditions.data).length === 0) { return <Alert bsStyle="success">No users { addConditionButton }</Alert>; }

          return (
            <TableControl headers={[
              { field: 'code',         title: 'Condition Code'  },
              { field: 'description',  title: 'Description'     },
              { field: 'addCondition', title: 'Add Condition',  style: { textAlign: 'right'  },
                node: addConditionButton,
              },
            ]}>
              {
                _.map(this.props.rentalConditions.data, (condition) => {
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
          );
        })()}
      </Well>

      <Well className="clearfix">
        <SubHeader title="Equipment Transfer (Bulk)"/>
        <Row>
          <Col xs={9}>Bulk transfer will enable the user to transfer equipment associated with one owner code to another owner code.</Col>
          <Col xs={3}><span className="pull-right"><Button onClick={ this.showEquipmentTransferDialog }>Equipment Transfer</Button></span></Col>
        </Row>
      </Well>

      { this.state.showEquipmentTransferDialog &&
        <EquipmentTransferDialog
          show={ this.state.showEquipmentTransferDialog }
          onClose={ this.closeEquipmentTransferDialog }
        />
      }
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
      { this.state.showDistrictEquipmentTypeErrorDialog &&
        <ModalDialog
          title="Error"
          show={this.state.showDistrictEquipmentTypeErrorDialog}
          onClose={this.closeDistrictEquipmentTypeErrorDialog}
          footer={
            <span>
              <Button onClick={ this.closeDistrictEquipmentTypeErrorDialog }>Close</Button>
            </span>
          }
        >
          <div>{ this.state.districtEquipmentTypeError }</div>
        </ModalDialog>
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
    uiEquipment: state.ui.districtEquipment,
  };
}

export default connect(mapStateToProps)(DistrictAdmin);
