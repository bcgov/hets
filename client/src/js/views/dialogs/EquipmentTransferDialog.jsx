import PropTypes from 'prop-types';
import React from 'react';

import { connect } from 'react-redux';

import { Button, Row, Col, FormGroup, FormLabel, FormText, FormCheck } from 'react-bootstrap';

import _ from 'lodash';

import * as Api from '../../api';

import DropdownControl from '../../components/DropdownControl.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';
import ModalDialog from '../../components/ModalDialog.jsx';
import Spinner from '../../components/Spinner.jsx';
import TableControl from '../../components/TableControl.jsx';

import { isBlank } from '../../utils/string';

const STAGE_SELECT_OWNER = 1;
const STAGE_SELECT_EQUIPMENT = 2;
const STAGE_CONFIRM = 3;
const STAGE_COMPLETE = 4;

const OPTION_EQUIPMENT_ONLY = { id: 1, name: 'Transfer Equipment Only' };
const OPTION_EQUIPMENT_AND_SENIORITY = {
  id: 2,
  name: 'Transfer Equipment and Seniority',
};

class EquipmentTransferDialog extends React.Component {
  static propTypes = {
    equipment: PropTypes.object.isRequired,
    owners: PropTypes.object.isRequired,
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool.isRequired,
  };

  constructor(props) {
    super(props);

    this.state = {
      stage: STAGE_SELECT_OWNER,
      donorOwnerCode: '',
      recipientOwnerCode: '',
      seniorityOption: 1,
      donorOwnerCodeError: '',
      recipientOwnerCodeError: '',
      seniorityOptionError: '',
      selectedEquipmentIds: [],
      selectAllEquipment: false,
      waitingForResponse: false,
      equipmentTransferError: '',
    };
  }

  componentDidMount() {
    Api.getOwnersLite();
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  getOwner = (code) => {
    var owner = _.find(this.props.owners.data, { ownerCode: code });
    return owner;
  };

  validateOwnerCode = (code, errorState) => {
    if (isBlank(code)) {
      this.setState({ [errorState]: 'This owner code is required' });
      return false;
    }

    if (this.getOwner(code) === undefined) {
      this.setState({
        [errorState]: 'This owner code is either not valid or not approved',
      });
      return false;
    }

    return true;
  };

  validateSelectOwner = () => {
    this.setState({
      donorOwnerCodeError: '',
      recipientOwnerCodeError: '',
      seniorityOptionError: '',
    });

    var valid = this.validateOwnerCode(this.state.donorOwnerCode, 'donorOwnerCodeError');
    valid &= this.validateOwnerCode(this.state.recipientOwnerCode, 'recipientOwnerCodeError');

    if (!valid) {
      return false;
    }

    if (this.state.donorOwnerCode === this.state.recipientOwnerCode) {
      this.setState({
        recipientOwnerCodeError: 'The owner codes must be different',
      });
      return false;
    }

    var donor = this.getOwner(this.state.donorOwnerCode);
    var recipient = this.getOwner(this.state.recipientOwnerCode);

    if (
      this.state.seniorityOption === OPTION_EQUIPMENT_AND_SENIORITY.id &&
      donor.localAreaId !== recipient.localAreaId
    ) {
      this.setState({
        seniorityOptionError: 'Both Transfer From and Transfer To Owners/Companies should be from the same local area',
      });
      return false;
    }

    return true;
  };

  renderSelectOwner = () => {
    var seniorityOptions = [OPTION_EQUIPMENT_ONLY, OPTION_EQUIPMENT_AND_SENIORITY];

    return (
      <div id="select-owner">
        <Row>
          <Col xs={4}>
            <FormLabel>
              Transfer Equipment From (Owner Code) <sup>*</sup>
            </FormLabel>
          </Col>
          <Col xs={4}>
            <FormLabel>
              Action <sup>*</sup>
            </FormLabel>
          </Col>
          <Col xs={4}>
            <FormLabel>
              Transfer Equipment To (Owner Code) <sup>*</sup>
            </FormLabel>
          </Col>
        </Row>
        <Row>
          <Col xs={4}>
            <FormGroup controlId="donorOwnerCode">
              <FormInputControl
                type="text"
                defaultValue={this.state.donorOwnerCode}
                updateState={this.updateState}
                isInvalid={this.state.donorOwnerCodeError}
              />
              <FormText>{this.state.donorOwnerCodeError}</FormText>
            </FormGroup>
          </Col>
          <Col xs={4}>
            <FormGroup controlId="seniorityOption">
              <DropdownControl
                id="seniorityOption"
                updateState={this.updateState}
                selectedId={this.state.seniorityOption}
                items={seniorityOptions}
                isInvalid={this.state.seniorityOptionError}
              />
              <FormText>{this.state.seniorityOptionError}</FormText>
            </FormGroup>
          </Col>
          <Col xs={4}>
            <FormGroup controlId="recipientOwnerCode">
              <FormInputControl
                type="text"
                defaultValue={this.state.recipientOwnerCode}
                updateState={this.updateState}
                isInvalid={this.state.recipientOwnerCodeError}
              />
              <FormText>{this.state.recipientOwnerCodeError}</FormText>
            </FormGroup>
          </Col>
        </Row>
      </div>
    );
  };

  handleEquipmentCheckedChange = (e, id) => {
    var checked = e.target.checked;

    var selectedEquipmentIds = [...this.state.selectedEquipmentIds];
    if (checked && !_.includes(selectedEquipmentIds, id)) {
      selectedEquipmentIds.push(id);
    } else {
      _.remove(selectedEquipmentIds, (x) => x === id);
    }

    this.setState({ selectedEquipmentIds: selectedEquipmentIds });
  };

  handleSelectAllEquipmentCheckedChange = (e) => {
    var checked = e.target.checked;

    var selectedEquipmentIds = checked ? _.map(this.props.equipment.data, (x) => x.id) : [];

    this.setState({
      selectedEquipmentIds: selectedEquipmentIds,
      selectAllEquipment: checked,
    });
  };

  validateSelectEquipment = () => {
    return this.state.selectedEquipmentIds.length > 0;
  };

  renderSelectEquipment = () => {
    if (this.props.equipment.loading) {
      return (
        <div className="spinner-container">
          <Spinner />
        </div>
      );
    }

    var equipmentList = this.props.equipment.data;

    var selectAllCheckbox = (
      <FormCheck
        id="selectAllEquipment"
        checked={this.state.selectAllEquipment}
        onChange={this.handleSelectAllEquipmentCheckedChange}
        label="Select All"
      />
    );

    var headers = [
      { field: '', node: selectAllCheckbox },
      { field: 'equipmentCode', title: 'Equip. ID' },
      { field: 'equipmentType', title: 'Equip. Type' },
      { field: 'details', title: 'Make/Model/Size/Year' },
      { field: 'attachmentCount', title: 'Attachments' },
    ];

    return (
      <TableControl id="select-equipment" headers={headers}>
        {_.map(equipmentList, (equipment) => {
          return (
            <tr key={equipment.id}>
              <td>
                <FormCheck
                  checked={_.includes(this.state.selectedEquipmentIds, equipment.id)}
                  onChange={(e) => this.handleEquipmentCheckedChange(e, equipment.id)}
                />
              </td>
              <td>{equipment.equipmentCode}</td>
              <td>{equipment.districtEquipmentType.districtEquipmentName}</td>
              <td>{equipment.details}</td>
              <td>{_.map(equipment.equipmentAttachments, (a) => a.description).join(', ')}</td>
            </tr>
          );
        })}
      </TableControl>
    );
  };

  renderConfirm = () => {
    return (
      <div id="confirm-transfer">
        <p>
          This action will transfer all selected equipment from the first owner/company to the second owner/company.
        </p>
        <ul>
          <li>The piece(s) of equipment from the first company will be archived.</li>
          {this.state.seniorityOption === OPTION_EQUIPMENT_ONLY.id && (
            <li>The piece(s) of equipment will be added to the “Transfer To” owner/company as new equipment.</li>
          )}
          {this.state.seniorityOption === OPTION_EQUIPMENT_ONLY.id && (
            <li>All previous seniority data will be lost.</li>
          )}
          {this.state.seniorityOption === OPTION_EQUIPMENT_AND_SENIORITY.id && (
            <li>
              The piece(s) of equipment will be added to the “Transfer To” owner/company while retaining their
              seniority.
            </li>
          )}
        </ul>
        {!this.state.waitingForResponse && <p>Do you want to continue?</p>}
        {this.state.waitingForResponse && (
          <p>Please wait while the equipment is transferred. This may take some time.</p>
        )}
      </div>
    );
  };

  renderComplete = () => {
    return (
      <div id="transfer-complete">
        {this.state.equipmentTransferError && (
          <div className="has-error">
            <FormText>Error: {this.state.equipmentTransferError}</FormText>
          </div>
        )}
        {this.state.equipmentTransferError && <p>The selected equipment has not been transferred.</p>}
        {!this.state.equipmentTransferError && <p>The selected equipment has been successfully transferred.</p>}
      </div>
    );
  };

  render() {
    var content = null;
    var transferText = 'Transfer';
    var transferEnabled = false;
    var transferFunc = function () {};

    if (!this.props.owners.loaded) {
      return (
        <div style={{ textAlign: 'center' }}>
          <Spinner />
        </div>
      );
    }

    switch (this.state.stage) {
      case STAGE_SELECT_OWNER:
        content = this.renderSelectOwner();
        transferEnabled = !isBlank(this.state.donorOwnerCode) && !isBlank(this.state.recipientOwnerCode);
        transferFunc = function () {
          if (this.validateSelectOwner()) {
            var ownerId = this.getOwner(this.state.donorOwnerCode).id;
            Api.getOwnerEquipment(ownerId);

            this.setState({ stage: STAGE_SELECT_EQUIPMENT });
          }
        };
        break;
      case STAGE_SELECT_EQUIPMENT:
        content = this.renderSelectEquipment();
        transferEnabled = this.state.selectedEquipmentIds.length > 0;
        transferFunc = function () {
          if (this.validateSelectEquipment()) {
            this.setState({ stage: STAGE_CONFIRM });
          }
        };
        break;
      case STAGE_CONFIRM:
        content = this.renderConfirm();
        transferText =
          this.state.seniorityOption === OPTION_EQUIPMENT_AND_SENIORITY.id
            ? 'Transfer Equipment and Seniority'
            : 'Transfer Equipment Only';
        transferEnabled = true;
        transferFunc = function () {
          this.setState({ waitingForResponse: true });

          var donorOwnerId = this.getOwner(this.state.donorOwnerCode).id;
          var recipientOwnerId = this.getOwner(this.state.recipientOwnerCode).id;
          var equipment = _.filter(this.props.equipment.data, (x) => _.includes(this.state.selectedEquipmentIds, x.id));
          var includeSeniority = this.state.seniorityOption === OPTION_EQUIPMENT_AND_SENIORITY.id;

          Api.transferEquipment(donorOwnerId, recipientOwnerId, equipment, includeSeniority)
            .catch((error) => {
              if (
                error.status === 400 &&
                (error.errorCode === 'HETS-31' ||
                  error.errorCode === 'HETS-32' ||
                  error.errorCode === 'HETS-33' ||
                  error.errorCode === 'HETS-34')
              ) {
                this.setState({
                  equipmentTransferError: error.errorDescription,
                });
              } else {
                throw error;
              }
            })
            .finally(() => {
              this.setState({
                waitingForResponse: false,
                stage: STAGE_COMPLETE,
              });
            });
        };
        break;
      case STAGE_COMPLETE:
        content = this.renderComplete();
        break;
      default:
        break;
    }

    var displayTransferButton = this.state.stage !== STAGE_COMPLETE && this.state.waitingForResponse !== true;

    return (
      <ModalDialog
        backdrop="static"
        className={'form-dialog'}
        id="equipment-transfer"
        title="Equipment Transfer"
        size="lg"
        onClose={this.props.onClose}
        show={this.props.show}
        footer={
          <span>
            <Button className="btn-custom mr-1" onClick={this.props.onClose}>
              Close
            </Button>
            {displayTransferButton && (
              <Button variant="primary" onClick={transferFunc.bind(this)} disabled={!transferEnabled}>
                {transferText}
              </Button>
            )}
          </span>
        }
      >
        {content}
      </ModalDialog>
    );
  }
}

function mapStateToProps(state) {
  return {
    equipment: state.models.ownerEquipment,
    owners: state.lookups.owners.lite,
  };
}

export default connect(mapStateToProps)(EquipmentTransferDialog);
