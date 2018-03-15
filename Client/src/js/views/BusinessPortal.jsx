import React from 'react';
import { connect } from 'react-redux';
import { PageHeader, Well, Form, FormGroup, FormControl, Button } from 'react-bootstrap';

import Main from './Main.jsx';
import Spinner from '../components/Spinner.jsx';

var BusinessPortal = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
    districtOwners: React.PropTypes.array,
  },

  getInitialState() {
    return {
      waiting: false,
      errors: {},
    };
  },

  validateSecretKey(e) {
    e.preventDefault();

    this.setState({ waiting: true, errors: {} });

    // TODO: XHR request
    new Promise((resolve) => setTimeout(resolve, 5 * 1000))
    .then(() => {
      // TODO goto owner details screen on success
    }).catch((err) => {
      console.error(err);
      this.setState({ errors: { secretKey: 'Error validating secret key' } });
    }).finally(() => {
      this.setState({ waiting: false });
    });
  },

  render() {
    const hasErrors = Object.keys(this.state.errors).length > 0;
    return (
      <Main showNav={false}>
        <div id="business-portal">
          <PageHeader>Business Portal Home Page</PageHeader>

          <div id="overview">
            <div id="bceid-box">
              <h3>Business BCeID</h3>
              <div id="bceid-logo" />
              <div id="company-name">{this.props.currentUser.districtName}</div>
            </div>

            <p>
              Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla vulputate efficitur
              diam, sit amet suscipit urna sodales sit amet. Phasellus aliquam sagittis
              vehicula. Curabitur sit amet nunc vel dui efficitur dignissim eleifend eget nulla.
              Sed faucibus, augue a blandit consequat, ipsum leo imperdiet ex, et laoreet justo
              nunc id odio. Pellentesque velit velit, malesuada quis convallis nec, efficitur et
              justo. Donec eget porta nunc. Nullam tortor metus, gravida id elit vel, faucibus
              mattis magna. In sed metus in neque laoreet molestie. Maecenas sed metus dapibus,
              consequat ante et, interdum arcu. Mauris eget iaculis ipsum. Integer non molestie
              velit. Phasellus sodales viverra ipsum sed egestas. Integer viverra nulla at
              efficitur cursus. Nunc fermentum lectus at porttitor ultrices.
            </p>
          </div>

          <div id="existing-district-owners">
            <h4>HETS District Owners already associated with your BCeID:</h4>
            {this.props.districtOwners.map(districtOwner => {
              return <Well>
                  <p>{districtOwner.companyName}</p>
                  <p>{districtOwner.ownerName}</p>
                </Well>;
            })}
          </div>

          <div id="enter-secret-key">
            <Form inline onSubmit={this.validateSecretKey}>
              <FormGroup controlId="secret-key" validationState={this.state.errors.secretKey ? 'error' : null}>
                <FormControl type="text" placeholder="Please enter your secret key here" disabled={this.state.waiting} />
              </FormGroup>
              <FormGroup controlId="postal-code" validationState={this.state.errors.postalCode ? 'error' : null}>
                <FormControl type="text" placeholder="Postal code" disabled={this.state.waiting} />
              </FormGroup>
              <Button type="submit" disabled={this.state.waiting}>
                Validate {this.state.waiting && <Spinner />}
              </Button>
              {hasErrors && <div>Error with secret key</div>}
            </Form>
          </div>
        </div>
      </Main>
    );
  },
});


function mapStateToProps(state) {
  return {
    currentUser: state.user,
    districtOwners: [],
  };
}

export default connect(mapStateToProps)(BusinessPortal);
