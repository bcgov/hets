import React from 'react';
import { connect } from 'react-redux';

import _ from 'lodash';
import Promise from 'bluebird';

import { Row, Col, Radio, Alert, FormGroup, ControlLabel } from 'react-bootstrap';

import * as Api from '../../api';
import { BY_EQUIPMENT, BY_PROJECT } from '../../constants';

import EditDialog from '../../components/EditDialog.jsx';
import TableControl from '../../components/TableControl.jsx';
import DropdownControl from '../../components/DropdownControl.jsx';
import Spinner from '../../components/Spinner.jsx';

import { formatDateTime } from '../../utils/date';

class CloneDialog extends React.Component {
  static propTypes = {
    onSave: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    rentalAgreement: React.PropTypes.object,
    projectRentalAgreements: React.PropTypes.object,
    equipmentRentalAgreements: React.PropTypes.object,
    cloneRentalAgreementError: React.PropTypes.string,
  };

  state = {
    loading: false,
    rentalAgreementId: '',
    type: BY_PROJECT,
  };

  componentDidMount() {
    var getProjectRentalAgreementsPromise = Api.getProjectRentalAgreements(this.props.rentalAgreement.project.id);
    var getEquipmentRentalAgreementsPromise = Api.getEquipmentRentalAgreements(this.props.rentalAgreement.equipment.id);
    this.setState({ loading: true });
    return Promise.all([getProjectRentalAgreementsPromise, getEquipmentRentalAgreementsPromise]).finally(() => {
      this.setState({ loading: false });
    });
  }

  updateState = (e) => {
    this.setState({ [e.target.name]: e.target.value });
  };

  updateDropdownState = (state) => {
    this.setState(state);
  };

  didChange = () => {
    if (this.state.rentalAgreementId !== '') { return true; }

    return false;
  };

  isValid = () => {
    return true;
  };

  onSave = () => {
    this.props.onSave(parseInt(this.state.rentalAgreementId, 10), this.state.type);
  };

  render() {
    var headers = [
      { field: 'blank'                                                       },
      { field: 'equipmentId',              title: 'Equipment ID'             },
      { field: 'equipmentType',            title: 'Equipment Type'           },
      { field: 'equipmentMakeModelSize',   title: 'Year Make/Model/Size'     },
      { field: 'projectName',              title: 'Project Name'             },
      { field: 'rentalAgreementNumber',    title: 'Rental Agreement #'       },
      { field: 'datedOn',                  title: 'Dated On'                 },
    ];

    var rentalAgreements = _.filter(this.state.type === BY_PROJECT ?
      this.props.projectRentalAgreements.data : this.props.equipmentRentalAgreements.data, item => {
      return item.id !== this.props.rentalAgreement.id;
    });

    rentalAgreements = _.reverse(_.sortBy(rentalAgreements, function(rentalAgreement) {
      return rentalAgreement.datedOn;
    }));

    return <EditDialog id="clone-dialog" show={ this.props.show } bsSize="large"
      onClose={ this.props.onClose } onSave={ this.onSave } isValid={ this.isValid } didChange={ this.didChange } saveText="Clone"
      title={<strong>Clone Rental Agreement</strong>}>
      <Row>
        <Col md={12}>
          <FormGroup controlId="type">
            <ControlLabel>Rental Agreements</ControlLabel>
            <DropdownControl id="type" updateState={ this.updateDropdownState }
              selectedId={ this.state.type } title={ this.state.type } items={[BY_PROJECT, BY_EQUIPMENT]}
            />
          </FormGroup>
          <p>Select a rental agreement to clone...</p>
          {(() => {
            if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

            if (!rentalAgreements || Object.keys(rentalAgreements).length === 0) { return <Alert bsStyle="success" style={{ marginTop: 10 }}>No rental agreements.</Alert>; }

            return (
              <TableControl id="notes-list" headers={ headers }>
                {
                  _.map(rentalAgreements, (rentalAgreement) => {
                    return <tr key={ rentalAgreement.id }>
                      <td><Radio name="rentalAgreementId" value={ rentalAgreement.id } onChange={ this.updateState } /></td>
                      <td>{ rentalAgreement.equipment.equipmentCode }</td>
                      <td>{ rentalAgreement.equipment.districtEquipmentType.districtEquipmentName }</td>
                      <td>{`${rentalAgreement.equipment.year} ${rentalAgreement.equipment.make}/${rentalAgreement.equipment.model}/${rentalAgreement.equipment.size}`}</td>
                      <td>{ rentalAgreement.project && rentalAgreement.project.name }</td>
                      <td>{ rentalAgreement.number }</td>
                      <td>{ formatDateTime(rentalAgreement.datedOn, 'YYYY-MMM-DD') }</td>
                    </tr>;
                  })
                }
              </TableControl>
            );
          })()}
          { this.props.cloneRentalAgreementError &&
            <Alert bsStyle="danger">{ this.props.cloneRentalAgreementError }</Alert>
          }
        </Col>
      </Row>
    </EditDialog>;
  }
}

function mapStateToProps(state) {
  return {
    projectRentalAgreements: state.models.projectRentalAgreements,
    equipmentRentalAgreements: state.models.equipmentRentalAgreements,
  };
}

export default connect(mapStateToProps)(CloneDialog);
