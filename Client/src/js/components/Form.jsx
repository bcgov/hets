import React from 'react';
import { Form as BootstrapForm } from 'react-bootstrap';


const Form = (props) => {
  const { children, onSubmit, ...rest } = props;

  function formSubmited(e) {
    e.preventDefault();

    if (onSubmit) { onSubmit(e); }
  }

  return (
    <BootstrapForm { ...rest } data-react-form="true" onSubmit={formSubmited}>
      { children }
    </BootstrapForm>
  );
};

Form.propTypes = {
  children: React.PropTypes.node,
  onSubmit: React.PropTypes.func,
};

export default Form;
