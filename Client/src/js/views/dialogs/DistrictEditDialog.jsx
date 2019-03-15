import React from 'react';

import _ from 'lodash';

import { FormGroup, HelpBlock } from 'react-bootstrap';

import EditDialog from '../../components/EditDialog.jsx';
import CheckboxControl from '../../components/CheckboxControl.jsx';
import FilterDropdown from '../../components/FilterDropdown.jsx';
import Form from '../../components/Form.jsx';

class DistrictEditDialog extends React.Component {
  static propTypes = {
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    districts: React.PropTypes.object,
    user: React.PropTypes.object,
    district: React.PropTypes.object,
    userDistricts: React.PropTypes.object,
  };

  constructor(props) {
    super(props);
    var isNew = props.district.id === 0;

    this.state = {
      isNew: isNew,
      districtId: isNew ? 0 : props.district.district.id,
      isPrimary: props.district.isPrimary || false,

      districtIdError: '',
    };
  }

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  didChange = () => {
    if (this.state.isNew && this.state.districtId !== '') { return true; }
    if (this.state.isNew && this.state.isPrimary !== '') { return true; }
    if (!this.state.isNew && this.state.districtId !== this.props.district.district.id) { return true; }
    if (!this.state.isNew && this.state.isPrimary !== this.props.district.isPrimary) { return true; }

    return false;
  };

  isValid = () => {
    this.setState({
      districtIdError: '',
    });

    var valid = true;

    if (this.state.districtId === 0) {
      this.setState({ districtIdError: 'District is required' });
      valid = false;
    }

    return valid;
  };

  onSave = () => {
    this.props.onSave({
      id: this.props.district.id,
      user: { id: this.props.user.id },
      district: { id: this.state.districtId },
      isPrimary: this.state.isPrimary,
    });
  };

  render() {
    var userDistrictsFiltered = _.map(this.props.userDistricts, district => {
      if (this.state.isNew || district.district.id !== this.props.district.district.id) {
        return district.district.id;
      }
      return;
    });

    var districts = _.chain(this.props.districts)
      .sortBy('name')
      .reject(district => {
        return _.includes(userDistrictsFiltered, district.id);
      } )
      .value();

    return <EditDialog id="equipment-add" show={ this.props.show } bsSize="small"
      onClose={ this.props.onClose } onSave={ this.onSave } didChange={ this.didChange } isValid={ this.isValid }
      title={<strong>{ this.state.isNew ? 'Add District' : 'Edit District' }</strong>}>
      <Form>
        <FormGroup controlId="districtId" validationState={ this.state.districtIdError ? 'error' : null }>
          <FilterDropdown id="districtId" placeholder="Choose District" className="full-width"
            items={ districts } selectedId={ this.state.districtId } updateState={ this.updateState } />
          <HelpBlock>{ this.state.districtIdError }</HelpBlock>
        </FormGroup>
        <CheckboxControl id="isPrimary" checked={ this.state.isPrimary } updateState={ this.updateState }>Primary District</CheckboxControl>
      </Form>
    </EditDialog>;
  }
}

export default DistrictEditDialog;
