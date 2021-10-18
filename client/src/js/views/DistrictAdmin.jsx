import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Button, ButtonGroup, Alert, Row, Col, OverlayTrigger } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import _ from 'lodash';

import * as Api from '../api';
import * as Constant from '../constants';
import * as Action from '../actionTypes';

import PageHeader from '../components/ui/PageHeader.jsx';
import SubHeader from '../components/ui/SubHeader.jsx';
import ModalDialog from '../components/ModalDialog.jsx';
import SortTable from '../components/SortTable.jsx';
import TableControl from '../components/TableControl.jsx';
import Spinner from '../components/Spinner.jsx';
import Confirm from '../components/Confirm.jsx';
import Authorize from '../components/Authorize.jsx';

import ConditionAddEditDialog from './dialogs/ConditionAddEditDialog.jsx';
import DistrictEquipmentTypeAddEditDialog from './dialogs/DistrictEquipmentTypeAddEditDialog.jsx';
import EquipmentTransferDialog from './dialogs/EquipmentTransferDialog.jsx';

import { caseInsensitiveSort, sort } from '../utils/array';

class DistrictAdmin extends React.Component {
  static propTypes = {
    currentUser: PropTypes.object,
    rentalConditions: PropTypes.object,
    districtEquipmentTypes: PropTypes.object,
    equipmentTypes: PropTypes.object,
    router: PropTypes.object,
    uiEquipment: PropTypes.object,
    dispatch: PropTypes.func,
  };

  constructor(props) {
    super(props);

    this.state = {
      showConditionAddEditDialog: false,
      showDistrictEquipmentTypeAddEditDialog: false,
      showEquipmentTransferDialog: false,
      condition: {},
      districtEquipmentType: {},

      // Equipment
      uiEquipment: {
        sortField: props.uiEquipment.sortField || 'districtEquipmentName',
        sortDesc: props.uiEquipment.sortDesc === true,
      },
    };
  }

  componentDidMount() {
    Api.getRentalConditions();
    Api.getDistrictEquipmentTypes();
  }

  updateEquipmentUIState = (state, callback) => {
    this.setState({ uiEquipment: { ...this.state.uiEquipment, ...state } }, () => {
      this.props.dispatch({ type: Action.UPDATE_DISTRICT_EQUIPMENT_UI, districtEquipment: this.state.uiEquipment });
      if (callback) {
        callback();
      }
    });
  };

  addCondition = () => {
    this.setState({ condition: { id: 0 } }, this.showConditionAddEditDialog);
  };

  editCondition = (condition) => {
    this.setState({ condition: condition }, this.showConditionAddEditDialog);
  };

  deleteCondition = (condition) => {
    Api.deleteCondition(condition.id).then(() => {
      Api.getRentalConditions();
    });
  };

  showConditionAddEditDialog = () => {
    this.setState({ showConditionAddEditDialog: true });
  };

  closeConditionAddEditDialog = () => {
    this.setState({ showConditionAddEditDialog: false });
  };

  showEquipmentTransferDialog = () => {
    this.setState({ showEquipmentTransferDialog: true });
  };

  closeEquipmentTransferDialog = () => {
    this.setState({ showEquipmentTransferDialog: false });
  };

  conditionSaved = () => {
    Api.getRentalConditions();
  };

  showDistrictEquipmentTypeAddEditDialog = () => {
    this.setState({ showDistrictEquipmentTypeAddEditDialog: true });
  };

  closeDistrictEquipmentTypeAddEditDialog = () => {
    this.setState({ showDistrictEquipmentTypeAddEditDialog: false });
  };

  closeDistrictEquipmentTypeErrorDialog = () => {
    this.setState({ showDistrictEquipmentTypeErrorDialog: false });
  };

  addDistrictEquipmentType = () => {
    this.setState({ districtEquipmentType: { id: 0 } }, this.showDistrictEquipmentTypeAddEditDialog);
  };

  editDistrictEquipmentType = (equipment) => {
    this.setState({ districtEquipmentType: equipment }, this.showDistrictEquipmentTypeAddEditDialog);
  };

  districtEquipmentTypeSaved = () => {
    Api.getDistrictEquipmentTypes();
  };

  deleteDistrictEquipmentType = (equipment) => {
    Api.deleteDistrictEquipmentType(equipment)
      .then(() => {
        return Api.getDistrictEquipmentTypes();
      })
      .catch((error) => {
        if (error.status === 400 && error.errorCode === 'HETS-37') {
          this.setState({
            showDistrictEquipmentTypeErrorDialog: true,
            districtEquipmentTypeError: error.errorDescription,
          });
        } else {
          throw error;
        }
      });
  };

  render() {
    if (
      !this.props.currentUser.hasPermission(Constant.PERMISSION_DISTRICT_CODE_TABLE_MANAGEMENT) &&
      !this.props.currentUser.hasPermission(Constant.PERMISSION_ADMIN)
    ) {
      return <div>You do not have permission to view this page.</div>;
    }

    return (
      <div id="district-admin">
        <PageHeader>District Admin</PageHeader>

        <div className="well">
          <SubHeader title="Manage District Equipment Types" />
          {(() => {
            if (!this.props.districtEquipmentTypes.loaded) {
              return (
                <div style={{ textAlign: 'center' }}>
                  <Spinner />
                </div>
              );
            }

            var addDistrictEquipmentButton = (
              <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
                <Button
                  className="btn-custom"
                  title="Add District Equipment"
                  size="xsmall"
                  onClick={this.addDistrictEquipmentType}
                >
                  <FontAwesomeIcon icon="plus" />
                  &nbsp;<strong>Add District Equipment Type</strong>
                </Button>
              </Authorize>
            );

            var equipmentTypes = this.props.districtEquipmentTypes.data;

            if (Object.keys(equipmentTypes).length === 0) {
              return <Alert variant="success">No equipment types {addDistrictEquipmentButton}</Alert>;
            }

            var sortedEquipmentTypes = sort(
              equipmentTypes,
              this.state.uiEquipment.sortField,
              this.state.uiEquipment.sortDesc,
              caseInsensitiveSort
            );

            var headers = [
              { field: 'districtEquipmentName', title: 'Equipment Type/Description' },
              { field: 'equipmentType.blueBookSection', title: 'Blue Book Section Number' },
              { field: 'equipmentType.name', title: 'Blue Book Section Name' },
              {
                field: 'addDistrictEquipmentType',
                title: 'Add District Equipment Type',
                style: { textAlign: 'right' },
                node: addDistrictEquipmentButton,
              },
            ];

            return (
              <SortTable
                id="district-equipment-types"
                sortField={this.state.uiEquipment.sortField}
                sortDesc={this.state.uiEquipment.sortDesc}
                onSort={this.updateEquipmentUIState}
                headers={headers}
              >
                {_.map(sortedEquipmentTypes, (equipment) => {
                  return (
                    <tr key={equipment.id}>
                      <td>{equipment.districtEquipmentName}</td>
                      <td>{equipment.equipmentType.blueBookSection}</td>
                      <td>{equipment.equipmentType.name}</td>
                      <td style={{ textAlign: 'right' }}>
                        <ButtonGroup>
                          <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
                            <OverlayTrigger
                              trigger="click"
                              placement="top"
                              rootClose
                              overlay={<Confirm onConfirm={this.deleteDistrictEquipmentType.bind(this, equipment)} />}
                            >
                              <Button className="btn-custom" title="Delete District Equipment Type" size="sm">
                                <FontAwesomeIcon icon="trash-alt" />
                              </Button>
                            </OverlayTrigger>
                          </Authorize>
                          <Button
                            className="btn-custom"
                            title="Edit District Equipment Type"
                            size="sm"
                            onClick={this.editDistrictEquipmentType.bind(this, equipment)}
                          >
                            <FontAwesomeIcon icon="edit" />
                          </Button>
                        </ButtonGroup>
                      </td>
                    </tr>
                  );
                })}
              </SortTable>
            );
          })()}
        </div>

        <div className="well">
          <SubHeader title="Manage Conditions" />
          {(() => {
            if (this.props.rentalConditions.loading) {
              return (
                <div style={{ textAlign: 'center' }}>
                  <Spinner />
                </div>
              );
            }

            var addConditionButton = (
              <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
                <Button className="btn-custom" title="Add Condition" size="sm" onClick={this.addCondition}>
                  <FontAwesomeIcon icon="plus" />
                  &nbsp;<strong>Add Condition</strong>
                </Button>
              </Authorize>
            );

            if (Object.keys(this.props.rentalConditions.data).length === 0) {
              return <Alert variant="success">No users {addConditionButton}</Alert>;
            }

            return (
              <TableControl
                headers={[
                  { field: 'code', title: 'Condition Code' },
                  { field: 'description', title: 'Description' },
                  {
                    field: 'addCondition',
                    title: 'Add Condition',
                    style: { textAlign: 'right' },
                    node: addConditionButton,
                  },
                ]}
              >
                {_.map(this.props.rentalConditions.data, (condition) => {
                  return (
                    <tr key={condition.id}>
                      <td>{condition.conditionTypeCode}</td>
                      <td>{condition.description}</td>
                      <td style={{ textAlign: 'right' }}>
                        <ButtonGroup>
                          <OverlayTrigger
                            trigger="focus"
                            placement="top"
                            rootClose
                            overlay={<Confirm onConfirm={this.deleteCondition.bind(this, condition)} />}
                          >
                            <Button className="btn-custom" title="Delete User" size="sm">
                              <FontAwesomeIcon icon="trash-alt" />
                            </Button>
                          </OverlayTrigger>
                          <Button
                            className="btn-custom"
                            title="Edit Condition"
                            size="sm"
                            onClick={this.editCondition.bind(this, condition)}
                          >
                            <FontAwesomeIcon icon="edit" />
                          </Button>
                        </ButtonGroup>
                      </td>
                    </tr>
                  );
                })}
              </TableControl>
            );
          })()}
        </div>
        <Authorize requires={Constant.PERMISSION_WRITE_ACCESS}>
          <div className="clearfix well">
            <SubHeader title="Equipment Transfer (Bulk)" />
            <Row>
              <Col xs={9}>
                Bulk transfer will enable the user to transfer equipment associated with one owner code to another owner
                code.
              </Col>
              <Col xs={3}>
                <span className="float-right">
                  <Button className="btn-custom" onClick={this.showEquipmentTransferDialog}>
                    Equipment Transfer
                  </Button>
                </span>
              </Col>
            </Row>
          </div>
        </Authorize>
        {this.state.showEquipmentTransferDialog && (
          <EquipmentTransferDialog
            show={this.state.showEquipmentTransferDialog}
            onClose={this.closeEquipmentTransferDialog}
          />
        )}
        {this.state.showConditionAddEditDialog && (
          <ConditionAddEditDialog
            show={this.state.showConditionAddEditDialog}
            onClose={this.closeConditionAddEditDialog}
            onSave={this.conditionSaved}
            condition={this.state.condition}
          />
        )}
        {this.state.showDistrictEquipmentTypeAddEditDialog && (
          <DistrictEquipmentTypeAddEditDialog
            show={this.state.showDistrictEquipmentTypeAddEditDialog}
            onClose={this.closeDistrictEquipmentTypeAddEditDialog}
            onSave={this.districtEquipmentTypeSaved}
            districtEquipmentType={this.state.districtEquipmentType}
          />
        )}
        {this.state.showDistrictEquipmentTypeErrorDialog && (
          <ModalDialog
            title="Error"
            show={this.state.showDistrictEquipmentTypeErrorDialog}
            onClose={this.closeDistrictEquipmentTypeErrorDialog}
            footer={
              <span>
                <Button onClick={this.closeDistrictEquipmentTypeErrorDialog}>Close</Button>
              </span>
            }
          >
            <div>{this.state.districtEquipmentTypeError}</div>
          </ModalDialog>
        )}
      </div>
    );
  }
}

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
