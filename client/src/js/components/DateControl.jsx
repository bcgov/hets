import PropTypes from 'prop-types';
import React, { useRef, useCallback } from 'react';
import { FormLabel, InputGroup, Button } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import Moment from 'moment';
import DateTime from 'react-datetime';
import classNames from 'classnames';

const DateControl = ({
  id,
  date = '',
  format = 'YYYY-MM-DD',
  className = '',
  label,
  onChange,
  updateState,
  placeholder = 'yyyy-mm-dd',
  title,
  disabled = false,
  isValidDate,
  isInvalid,
}) => {
  const inputRef = useRef(null);

  const clicked = useCallback(() => {
    if (!disabled && inputRef.current) {
      inputRef.current.focus();
    }
  }, [disabled]);

  const notifyValueChanged = useCallback(
    (dateString) => {
      onChange && onChange(dateString, id);
      updateState && updateState({ [id]: dateString });
    },
    [onChange, updateState, id]
  );

  const dateChanged = useCallback(
    (selectedDate) => {
      if (typeof selectedDate === 'string' || !selectedDate || !selectedDate.isValid()) {
        return;
      }
      const formattedDate = selectedDate.format(format);
      notifyValueChanged(formattedDate);
    },
    [format, notifyValueChanged]
  );

  const dateBlurred = useCallback(
    (blurredDate) => {
      if (typeof blurredDate === 'string' || !blurredDate || !blurredDate.isValid()) {
        notifyValueChanged('');
      }
    },
    [notifyValueChanged]
  );

  const momentDate = date === '' ? null : Moment.utc(date);

  return (
    <div className={`date-control ${className}`} id={id}>
      {label && <FormLabel>{label}</FormLabel>}
      <InputGroup>
        <DateTime
          value={momentDate}
          dateFormat={format}
          timeFormat={false}
          closeOnSelect
          onChange={dateChanged}
          onBlur={dateBlurred}
          inputProps={{
            placeholder,
            disabled,
            className: classNames('form-control', { 'is-invalid': isInvalid }),
            ref: inputRef,
          }}
          isValidDate={isValidDate}
        />
        <Button className="btn-custom" disabled={disabled} onClick={clicked}>
          <FontAwesomeIcon icon="calendar-alt" title={title} />
        </Button>
      </InputGroup>
    </div>
  );
};

DateControl.propTypes = {
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
  isInvalid: PropTypes.oneOfType([PropTypes.string, PropTypes.bool]),
};

export default DateControl;
