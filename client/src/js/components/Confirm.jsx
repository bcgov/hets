import PropTypes from 'prop-types';
import React, { forwardRef } from 'react';
import { Popover, ButtonGroup, Button } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

const Confirm = forwardRef(({ onConfirm, onCancel, hide, children, title, ...rest }, ref) => {
  const confirmed = () => {
    onConfirm();
  };

  const canceled = () => {
    if (onCancel) {
      onCancel();
    }
    //used to close popover on Cancel. Overlaytrigger must be set with trigger="click" and rootClose
    //(rootClose closes modal when you click outside of the box)
    document.body.click();
  };

  return (
    <Popover id="confirm" ref={ref} {...rest}>
      <Popover.Title>{title}</Popover.Title>
      <Popover.Content>
        {children}
        <div style={{ textAlign: 'center', marginTop: '6px' }}>
          <ButtonGroup>
            <Button onClick={confirmed}>
              <FontAwesomeIcon icon={['far', 'check-circle']} /> Yes
            </Button>
            <Button className="btn-custom" onClick={canceled}>
              <FontAwesomeIcon icon={['far', 'times-circle']} /> No
            </Button>
          </ButtonGroup>
        </div>
      </Popover.Content>
    </Popover>
  );
});

Confirm.propTypes = {
  onConfirm: PropTypes.func.isRequired,
  onCancel: PropTypes.func,

  children: PropTypes.node,
  title: PropTypes.string,
};

Confirm.defaultProps = {
  title: 'Are you sure?',
};

export default Confirm;
