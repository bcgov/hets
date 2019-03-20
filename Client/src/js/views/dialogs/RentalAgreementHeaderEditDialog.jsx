import PropTypes from 'prop-types';
import React from 'react';

import { connect } from 'react-redux';

import { Grid, Row, Col } from 'react-bootstrap';
import { FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import _ from 'lodash';

import * as Api from '../../api';

import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';
import DropdownControl from '../../components/DropdownControl.jsx';
import Spinner from '../../components/Spinner.jsx';
import Form from '../../components/Form.jsx';

import { isBlank } from '../../utils/string';

class RentalAgreementHeaderEditDialog extends React.Component {
  static propTypes = {
    rentalAgreement: PropTypes.object.isRequired,
    projects: PropTypes.object,
    equipment: PropTypes.object,
    onSave: PropTypes.func.isRequired,
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool,
  };

  constructor(props) {
    super(props);

    this.state = {
      loaded: true,
      projectId: props.rentalAgreement.projectId || '',
      equipmentCode: props.rentalAgreement.equipment.equipmentCode || '',

      equipmentCodeError: '',
    };
  }

  componentDidMount() {
    Api.getProjectsCurrentFiscal();

    const equipmentPromise = Api.getEquipmentLite();
    if (!this.props.equipment.loaded) {
      this.setState({ loaded: false });
      equipmentPromise.then(() => {
        this.setState({ loaded: true });
      });
    }
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  didChange = () => {
    if (this.state.projectId !== this.props.rentalAgreement.projectId) { return true; }
    if (this.state.equipmentCode.toLowerCase().trim() !== this.props.rentalAgreement.equipment.equipmentCode.toLowerCase().trim()) { return true; }

    return false;
  };

  isValid = () => {
    return this.validateEquipmentCode(this.state.equipmentCode);
  };

  onSave = () => {
    var equipment = this.getEquipment(this.state.equipmentCode);
    var equipmentId = equipment ? equipment.id : null;

    this.props.onSave({ ...this.props.rentalAgreement, ...{
      projectId: this.state.projectId || null,
      equipmentId: equipmentId,
    }});
  };

  getEquipment = (equipmentCode) => {
    var code = equipmentCode.toLowerCase().trim();
    var equipment = _.find(this.props.equipment.data, (e) => {
      return e.equipmentCode.toLowerCase().trim() === code;
    });
    return equipment;
  };

  validateEquipmentCode = (equipmentCode) => {
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
  };

  validateEquipmentCodeInput = (e) => {
    this.validateEquipmentCode(e.target.value);
  };

  render() {
    return <EditDialog id="rental-agreements-edit" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title={<strong>Rental Agreement</strong>}>
      {(() => {
        if (!this.state.loaded) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

        var projects = _.sortBy(this.props.projects.data, 'name');

        return <Form>
          <Grid fluid>
            <Row>
              <Col md={6}>
                <FormGroup controlId="projectId">
                  <ControlLabel>Project <sup>*</sup></ControlLabel>
                  <DropdownControl
                    id="projectId"
                    fieldName="label"
                    items={projects}
                    disabled={!this.props.projects.loaded}
                    updateState={this.updateState}
                    selectedId={this.state.projectId}
                    blankLine="(None)"
                    placeholder="(None)"/>
                </FormGroup>
              </Col>
              <Col md={6}>
                <FormGroup controlId="equipmentCode" validationState={ this.state.equipmentCodeError ? 'error' : null}>
                  <ControlLabel>Equipment ID <sup>*</sup></ControlLabel>
                  <FormInputControl type="text" value={ this.state.equipmentCode } updateState={ this.updateState } onChange={ this.validateEquipmentCodeInput } autoFocus/>
                  <HelpBlock>{ this.state.equipmentCodeError }</HelpBlock>
                </FormGroup>
              </Col>
            </Row>
          </Grid>
        </Form>;
      })()}
    </EditDialog>;
  }
}

function mapStateToProps(state) {
  return {
    projects: state.lookups.projectsCurrentFiscal,
    equipment: state.lookups.equipment.lite,
  };
}

export default connect(mapStateToProps)(RentalAgreementHeaderEditDialog);
