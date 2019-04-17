import PropTypes from 'prop-types';
import React from 'react';

import { Grid, Row } from 'react-bootstrap';
import { FormGroup, ControlLabel } from 'react-bootstrap';
import Moment from 'moment';

import DateControl from '../../components/DateControl.jsx';
import EditDialog from '../../components/EditDialog.jsx';
import Form from '../../components/Form.jsx';


class ReleaseExtendHireDialog extends React.Component {
  static propTypes = {
    onSave: PropTypes.func.isRequired,
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool.isRequired,
    title: PropTypes.string,
    releaseHire: PropTypes.bool,
  };

  state = {
    endHireDate: Moment(new Date()).format('YYYY-MM-DD'),
  };

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  didChange = () => {
    // todo
    return true;
  };

  isValid = () => {
    // todo
    return true;
  };

  onSave = () => {
    this.props.onSave({
      endHireDate: this.state.endHireDate,
    });
  };

  render() {
    return (
      <EditDialog
        id="hire-offer-edit"
        show={ this.props.show }
        onClose={ this.props.onClose }
        onSave={ this.onSave }
        didChange={ this.didChange }
        isValid={ this.isValid }
        title={<strong>{ this.props.title }</strong>}
        bsSize="small">
        <Form>
          <Grid fluid>
            <Row>
              <FormGroup>Hours Entered:</FormGroup>
            </Row>
            <Row>
              <FormGroup>Last Hours Date:</FormGroup>
            </Row>
            <Row>
              <FormGroup>
                <ControlLabel>{ this.props.releaseHire ? 'End Hire Date' : 'Extend Hire Until' }</ControlLabel>
                <DateControl
                  id="endHireDate"
                  date={ this.state.endHireDate }
                  updateState={ this.updateState }
                  title={ this.props.releaseHire ? 'End Hire Date' : 'Extend Hire Until' }
                />
              </FormGroup>
            </Row>
          </Grid>
        </Form>
      </EditDialog>
    );
  }
}

export default ReleaseExtendHireDialog;
