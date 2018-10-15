import React from 'react';

import { connect } from 'react-redux';

import { Grid, Row, Col } from 'react-bootstrap';
import { Form, FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import _ from 'lodash';

import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';
import DropdownControl from '../../components/DropdownControl.jsx';
import Spinner from '../../components/Spinner.jsx';

import { isBlank } from '../../utils/string';

var RentalAgreementHeaderEditDialog = React.createClass({
  propTypes: {
    rentalAgreement: React.PropTypes.object.isRequired,
    projects: React.PropTypes.object,
    equipmentList: React.PropTypes.object,
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
  },

  getInitialState() {
    return {
      loading: false,
      projectId: this.props.rentalAgreement.projectId || '',
      equipmentCode: this.props.rentalAgreement.equipment.equipmentCode || '',

      equipmentCodeError: '',
    };
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    if (this.state.projectId !== this.props.rentalAgreement.projectId) { return true; }
    if (this.state.equipmentCode.toLowerCase().trim() !== this.props.rentalAgreement.equipment.equipmentCode.toLowerCase().trim()) { return true; }

    return false;
  },

  isValid() {
    return this.validateEquipmentCode(this.state.equipmentCode);
  },

  onSave() {
    var equipment = this.getEquipment(this.state.equipmentCode);
    var equipmentId = equipment ? equipment.id : null;

    this.props.onSave({ ...this.props.rentalAgreement, ...{
      projectId: this.state.projectId || null,
      equipmentId: equipmentId,
    }});
  },

  getEquipment(equipmentCode) {
    var code = equipmentCode.toLowerCase().trim();
    var equipment = _.find(this.props.equipmentList.data, (e) => {
      return e.equipmentCode.toLowerCase().trim() === code;
    });
    return equipment;
  },

  validateEquipmentCode(equipmentCode) {
    var valid = true;

    this.setState({
      equipmentCodeError: '',
    });

    if (!isBlank(equipmentCode)) {
      // does the equipment exist?
      var equipment = this.getEquipment(equipmentCode);
      
      if (!equipment) {
        this.setState({ equipmentCodeError: 'This equipment ID does not exist in the system' });
        valid = false;
      }
    }

    return valid;
  },

  validateEquipmentCodeInput(e) {
    this.validateEquipmentCode(e.target.value);
  },

  render() {
    return <EditDialog id="rental-agreements-edit" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title={
        <strong>Rental Agreement</strong>
      }>
      {(() => {
        if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

        var projects = _.sortBy(this.props.projects, 'name');

        return <Form>
          <Grid fluid>
            <Row>
              <Col md={6}>
                <FormGroup controlId="projectId">
                  <ControlLabel>Project <sup>*</sup></ControlLabel>
                  <DropdownControl id="projectId" updateState={ this.updateState } items={ projects } selectedId={ this.state.projectId } blankLine="(None)" placeholder="(None)" />
                </FormGroup>
              </Col>
              <Col md={6}>
                <FormGroup controlId="equipmentCode" validationState={ this.state.equipmentCodeError ? 'error' : null}>
                  <ControlLabel>Equipment ID <sup>*</sup></ControlLabel>
                  <FormInputControl type="text" value={ this.state.equipmentCode } updateState={ this.updateState } onChange={ this.validateEquipmentCodeInput } inputRef={ ref => { this.input = ref; }}/>
                  <HelpBlock>{ this.state.equipmentCodeError }</HelpBlock>
                </FormGroup>
              </Col>
            </Row>
          </Grid>
        </Form>;
      })()}
    </EditDialog>;
  },
});

function mapStateToProps(state) {
  return {
    rentalAgreement: state.models.rentalAgreement,
    projects: state.lookups.projects,
    equipmentList: state.models.equipmentList,
  };
}

export default connect(mapStateToProps)(RentalAgreementHeaderEditDialog);
