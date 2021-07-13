import PropTypes from 'prop-types';
import React from 'react';
import { ControlLabel, InputGroup, Button, Glyphicon } from 'react-bootstrap';
import _ from 'lodash';
import Moment from 'moment';
import DateTime from 'react-datetime';


class DateControl extends React.Component {
  static propTypes = {
    id: PropTypes.string.isRequired,
    date: PropTypes.string,
    format: PropTypes.string,
    className: PropTypes.string,
    label: PropTypes.string,
    onChange: PropTypes.func,
    updateState: PropTypes.func,
    placeholder: PropTypes.string,
    title: PropTypes.string,
    disabled: PropTypes.bool,
    isValidDate: PropTypes.func,
  };

  clicked = () => {
    if (!this.props.disabled) {
      this.input.focus();
    }
  };

  dateChanged = (date) => {
    // ignore invalid dates
    if (_.isString(date) || !date || !date.isValid()) {
      return;
    }

    var dateString = date.format(this.props.format || 'YYYY-MM-DD');
    this.notifyValueChanged(dateString);
  };

  dateBlurred = (date) => {
    // when focus leaves input, if date is invalid, reset value to empty string
    if (_.isString(date) || !date || !date.isValid()) {
      this.notifyValueChanged('');
    }
  };

  notifyValueChanged = (dateString) => {
    // On change listener
    if (this.props.onChange) {
      this.props.onChange(dateString, this.props.id);
    }

    // Update state
    if (this.props.updateState) {
      this.props.updateState({
        [this.props.id]: dateString,
      });
    }
  };

  render() {
    var date = Moment.utc(this.props.date);
    var format = this.props.format || 'YYYY-MM-DD';

    var placeholder = this.props.placeholder || 'yyyy-mm-dd';
    var disabled = this.props.disabled;

    return <div className={ `date-control ${this.props.className || ''}` } id={ this.props.id }>
      {(() => {
        // Inline label
        if (this.props.label) { return <ControlLabel>{ this.props.label }</ControlLabel>; }
      })()}
      <InputGroup>
        <DateTime value={ date } dateFormat={ format } timeFormat={ false } closeOnSelect={ true } onChange={ this.dateChanged } onBlur={ this.dateBlurred }
          inputProps={{ placeholder: placeholder, disabled: disabled, ref: input => { this.input = input; } }} isValidDate={ this.props.isValidDate }
        />
        <InputGroup.Button>
          <Button disabled={disabled} onClick={ this.clicked }><Glyphicon glyph="calendar" title={ this.props.title }/></Button>
        </InputGroup.Button>
      </InputGroup>
    </div>;
  }
}


export default DateControl;
