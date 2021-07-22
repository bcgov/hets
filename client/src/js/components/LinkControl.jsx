import PropTypes from 'prop-types';
import React from 'react';
import { FormControl, InputGroup, FormLabel, Button } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import _ from 'lodash';

class LinkControl extends React.Component {
  static propTypes = {
    // url(value) returns a URL, default is value.
    url: PropTypes.func,
    value: PropTypes.string,
    id: PropTypes.string,
    label: PropTypes.string,
    title: PropTypes.string,
    className: PropTypes.string,
    updateState: PropTypes.func,
    onChange: PropTypes.func,
  };

  constructor(props) {
    super(props);

    this.state = {
      url: this.getUrl(props.value),
    };
  }

  getUrl = (value) => {
    return this.props.url ? this.props.url(value) : value;
  };

  changed = (e) => {
    // On change listener
    if (this.props.onChange) {
      this.props.onChange(e);
    }

    // Update state
    if (this.props.updateState && e.target.id) {
      // Use e.target.id insted of this.props.id because it comes from the controlId.
      this.props.updateState({ [e.target.id]: e.target.value });
    }

    // Update href
    this.setState({
      url: this.getUrl(e.target.value),
    });
  };

  render() {
    var props = _.omit(this.props, 'updateState', 'url', 'id');

    return (
      <div className={`link-control ${this.props.className || ''}`} id={this.props.id}>
        {(() => {
          // Inline label
          if (this.props.label) {
            return <FormLabel>{this.props.label}</FormLabel>;
          }
        })()}
        <InputGroup>
          <FormControl {...props} type="text" onChange={this.changed} />
          <InputGroup.Button>
            <Button target="_blank" href={this.state.url}>
              <FontAwesomeIcon icon="link" title={this.props.title} />
            </Button>
          </InputGroup.Button>
        </InputGroup>
      </div>
    );
  }
}

export default LinkControl;
