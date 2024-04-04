import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';

import _ from 'lodash';

import { Row, Col, FormCheck, Alert, FormGroup, FormLabel } from 'react-bootstrap';

import * as Api from '../../api';
import * as Constant from '../../constants';

import FormDialog from '../../components/FormDialog.jsx';
import TableControl from '../../components/TableControl.jsx';
import DropdownControl from '../../components/DropdownControl.jsx';
import Spinner from '../../components/Spinner.jsx';

import { formatDateTime } from '../../utils/date';

class CloneDialog extends React.Component {
  static propTypes = {
    onSave: PropTypes.func.isRequired,
    onClose: PropTypes.func.isRequired,
    show: PropTypes.bool,
    rentalAgreement: PropTypes.object,
    projectRentalAgreements: PropTypes.object,
    equipmentRentalAgreements: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      isSaving: false,
      cloneRentalAgreementError: '',
      loading: false,
      rentalAgreementId: '',
      type: Constant.BY_PROJECT,
    };
  }

  componentDidMount() {
    const getProjectRentalAgreementsPromise = this.props.dispatch(Api.getProjectRentalAgreements(this.props.rentalAgreement.project.id));
    const getEquipmentRentalAgreementsPromise = this.props.dispatch(Api.getEquipmentRentalAgreements(this.props.rentalAgreement.equipment.id));
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
    if (this.state.rentalAgreementId !== '') {
      return true;
    }

    return false;
  };

  isValid = () => {
    return true;
  };

  formSubmitted = () => {
    if (this.isValid()) {
      if (this.didChange()) {
        this.setState({ isSaving: true });

        const data = {
          projectId: this.props.rentalAgreement.project.id,
          agreementToCloneId: parseInt(this.state.rentalAgreementId, 10),
          rentalAgreementId: this.props.rentalAgreement.id,
          equipmentId: this.props.rentalAgreement.equipmentId,
        };

        const promise =
          this.state.type === Constant.BY_EQUIPMENT
            ? this.props.dispatch(Api.cloneEquipmentRentalAgreement(data))
            : this.props.dispatch(Api.cloneProjectRentalAgreement(data));

        promise
          .then(() => {
            this.setState({ isSaving: false });
            if (this.props.onSave) {
              this.props.onSave();
            }
            this.onClose();
          })
          .catch((error) => {
            if (
              error.status === 400 &&
              (error.errorCode === 'HETS-11' || error.errorCode === 'HETS-12' || error.errorCode === 'HETS-13')
            ) {
              this.setState({
                isSaving: false,
                cloneRentalAgreementError: 'There was an error cloning the rental agreement.',
              });
            } else {
              throw error;
            }
          });
      } else {
        this.onClose();
      }
    }
  };

  onClose = () => {
    this.setState({ cloneRentalAgreementError: '' });
    this.props.onClose();
  };

  render() {
    var headers = [
      { field: 'blank' },
      { field: 'equipmentId', title: 'Equipment ID' },
      { field: 'equipmentType', title: 'Equipment Type' },
      { field: 'equipmentMakeModelSize', title: 'Year Make/Model/Size' },
      { field: 'projectName', title: 'Project Name' },
      { field: 'rentalAgreementNumber', title: 'Rental Agreement #' },
      { field: 'datedOn', title: 'Dated On' },
    ];

    var rentalAgreements = _.filter(
      this.state.type === Constant.BY_PROJECT
        ? this.props.projectRentalAgreements.data
        : this.props.equipmentRentalAgreements.data,
      (item) => {
        return item.id !== this.props.rentalAgreement.id;
      }
    );

    rentalAgreements = _.reverse(
      _.sortBy(rentalAgreements, function (rentalAgreement) {
        return rentalAgreement.datedOn;
      })
    );

    return (
      <FormDialog
        id="clone-dialog"
        show={this.props.show}
        title="Clone Rental Agreement"
        saveButtonLabel="Clone"
        size="lg"
        isSaving={this.state.isSaving}
        onClose={this.onClose}
        onSubmit={this.formSubmitted}
      >
        <Row>
          <Col md={12}>
            <FormGroup controlId="type">
              <FormLabel>Rental Agreements</FormLabel>
              <DropdownControl
                id="type"
                updateState={this.updateDropdownState}
                selectedId={this.state.type}
                title={this.state.type}
                items={[Constant.BY_PROJECT, Constant.BY_EQUIPMENT]}
              />
            </FormGroup>
            <p>Select a rental agreement to clone...</p>
            {(() => {
              if (this.state.loading) {
                return (
                  <div style={{ textAlign: 'center' }}>
                    <Spinner />
                  </div>
                );
              }

              if (!rentalAgreements || Object.keys(rentalAgreements).length === 0) {
                return (
                  <Alert variant="success" style={{ marginTop: 10 }}>
                    No rental agreements.
                  </Alert>
                );
              }

              return (
                <TableControl id="notes-list" headers={headers}>
                  {_.map(rentalAgreements, (rentalAgreement) => {
                    return (
                      <tr key={rentalAgreement.id}>
                        <td>
                          <FormCheck
                            type="radio"
                            name="rentalAgreementId"
                            value={rentalAgreement.id}
                            onChange={this.updateState}
                          />
                        </td>
                        <td>{rentalAgreement.equipment.equipmentCode}</td>
                        <td>{rentalAgreement.equipment.districtEquipmentType.districtEquipmentName}</td>
                        <td>{`${rentalAgreement.equipment.year} ${rentalAgreement.equipment.make}/${rentalAgreement.equipment.model}/${rentalAgreement.equipment.size}`}</td>
                        <td>{rentalAgreement.project && rentalAgreement.project.name}</td>
                        <td>{rentalAgreement.number}</td>
                        <td>{formatDateTime(rentalAgreement.datedOn, 'YYYY-MMM-DD')}</td>
                      </tr>
                    );
                  })}
                </TableControl>
              );
            })()}
            {this.state.cloneRentalAgreementError && (
              <Alert variant="danger">{this.state.cloneRentalAgreementError}</Alert>
            )}
          </Col>
        </Row>
      </FormDialog>
    );
  }
}

const mapStateToProps = (state) => ({
  projectRentalAgreements: state.models.projectRentalAgreements,
  equipmentRentalAgreements: state.models.equipmentRentalAgreements,
});

const mapDispatchToProps = (dispatch) => ({ dispatch });

export default connect(mapStateToProps, mapDispatchToProps)(CloneDialog);
