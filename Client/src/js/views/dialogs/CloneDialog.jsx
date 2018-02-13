import React from 'react';
import { connect } from 'react-redux';

import _ from 'lodash';

import { Row, Col, Radio, Alert } from 'react-bootstrap';

import * as Api from '../../api';

import EditDialog from '../../components/EditDialog.jsx';
import TableControl from '../../components/TableControl.jsx';

import { formatDateTime } from '../../utils/date';

var CloneDialog = React.createClass({
  propTypes: {
    onSave: React.PropTypes.func.isRequired,
    onUpdate: React.PropTypes.func.isRequired,
    onClose: React.PropTypes.func.isRequired,
    show: React.PropTypes.bool,
    rentalAgreement: React.PropTypes.object,
    projectRentalAgreements: React.PropTypes.object,
    cloneRentalAgreementError: React.PropTypes.string,
  },

  getInitialState() {
    return {
      rentalAgreementId: '',
    };
  },

  componentDidMount() {
    Api.getProjectRentalAgreements(this.props.rentalAgreement.project.id);
  },

  updateState(e) {
    this.setState({ [e.target.name]: e.target.value });
  },

  didChange() {
    if (this.state.rentalAgreementId !== '') { return true; }

    return false;
  },

  isValid() {
    return true; 
  },

  onSave() {
    this.props.onSave(this.state.rentalAgreementId);
  },

  render() {
    var headers = [
      { field: 'blank'                                              },
      { field: 'equipmentType',            title: 'Equipment Type'  },
      { field: 'equipmentMakeModelSize',   title: 'Make/Model/Size' },
      { field: 'projectName',              title: 'Project Name'    },
      { field: 'datedOn',                  title: 'Dated On'        },
    ];
    
    return <EditDialog id="notes" show={ this.props.show }
      onClose={ this.props.onClose } onSave={ this.onSave } isValid={ this.isValid } didChange={ this.didChange }
      title= {
        <strong>Clone Rental Agreement</strong>
      }>
      <Row>
        <Col md={12}>
          <p>Select a rental agreement to clone...</p>
          <TableControl id="notes-list" headers={ headers }>
            {
              _.map(_.filter(this.props.projectRentalAgreements.data, item => { return item.id !== this.props.rentalAgreement.id; }), (rentalAgreement) => {
                return <tr key={ rentalAgreement.id }>
                  <td><Radio name="rentalAgreementId" value={ rentalAgreement.id } onChange={ this.updateState } /></td>
                  <td>{ rentalAgreement.equipment.districtEquipmentType.districtEquipmentName }</td>
                  <td>{`${rentalAgreement.equipment.make}/${rentalAgreement.equipment.model}/${rentalAgreement.equipment.size}`}</td>
                  <td>{ rentalAgreement.project.name }</td>
                  <td>{ formatDateTime(rentalAgreement.datedOn, 'YYYY-MMM-DD') }</td>
                </tr>;
              })
            }
          </TableControl>
          { this.props.cloneRentalAgreementError &&
            <Alert bsStyle="danger">{ this.props.cloneRentalAgreementError }</Alert>
          }
        </Col> 
      </Row>
    </EditDialog>;
  },
});

function mapStateToProps(state) {
  return {
    projectRentalAgreements: state.models.projectRentalAgreements,
  };
}

export default connect(mapStateToProps)(CloneDialog);