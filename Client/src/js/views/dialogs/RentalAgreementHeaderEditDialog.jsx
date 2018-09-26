import React from 'react';

import { connect } from 'react-redux';

import { Grid, Row, Col } from 'react-bootstrap';
import { Form, FormGroup, HelpBlock, ControlLabel } from 'react-bootstrap';

import * as Api from '../../api';

import EditDialog from '../../components/EditDialog.jsx';
import FormInputControl from '../../components/FormInputControl.jsx';
import DropdownControl from '../../components/DropdownControl.jsx';
import Spinner from '../../components/Spinner.jsx';

var RentalAgreementHeaderEditDialog = React.createClass({
  propTypes: {
    rentalAgreement: React.PropTypes.object.isRequired,
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    owner: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loading: true,
    };
  },

  componentDidMount() {
    this.fetch();
  },

  fetch() {
    this.setState({ loading: true });
    Api.getOwner(this.props.rentalAgreement.ownerId).finally(() => {
      this.setState({ loading: false });
    });
  },

  updateState(state, callback) {
    this.setState(state, callback);
  },

  didChange() {
    return false;
  },

  isValid() {
    var valid = true;
    return valid;
  },

  onSave() {
  },

  render() {
    return <EditDialog id="rental-agreements-edit" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title={
        <strong>Rental Agreement</strong>
      }>
      {(() => {
        if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

        return <Form>
          <Grid fluid>
            <Row>
              <Col md={6}>
                <FormGroup controlId="projectStatus" validationState={ this.state.projectStatusCodeError ? 'error' : null }>
                  <ControlLabel>Project <sup>*</sup></ControlLabel>
                  <DropdownControl id="projectStatus" title={ this.state.projectStatus } updateState={ this.updateState }
                    value={ 'Burns Lake Brushing' }
                    items={[ 'Burns Lake Brushing', 'Burns Lake Side Roads', 'Donaldson Rd.Intersection Construction', 'Good Hope Lake Landfill' ]}
                  />
                  <HelpBlock>{ this.state.projectStatusCodeError }</HelpBlock>
                </FormGroup>
              </Col>
              <Col md={6}>
                <FormGroup controlId="projectName" validationState={ this.state.projectNameError ? 'error' : null}>
                  <ControlLabel>Equipment ID <sup>*</sup></ControlLabel>
                  <FormInputControl type="text" value={ this.state.projectName } updateState={ this.updateState} inputRef={ ref => { this.input = ref; }}/>
                  <HelpBlock>{ this.state.projectNameError }</HelpBlock>
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
    owner: state.models.owner,
  };
}

export default connect(mapStateToProps)(RentalAgreementHeaderEditDialog);
