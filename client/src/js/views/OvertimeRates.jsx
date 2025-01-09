import PropTypes from 'prop-types';
import React, { useEffect, useState } from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { Button } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import _ from 'lodash';

import * as Api from '../api';
import * as Constant from '../constants';

import PageHeader from '../components/ui/PageHeader.jsx';
import TableControl from '../components/TableControl.jsx';
import Spinner from '../components/Spinner.jsx';
import OvertimeRateEditDialog from './dialogs/OvertimeRateEditDialog.jsx';

const OvertimeRates = () => {
  const dispatch = useDispatch();
  const currentUser = useSelector((state) => state.user);
  const overtimeRateTypes = useSelector((state) => state.lookups.overtimeRateTypes);

  const [showOvertimeRateEditDialog, setShowOvertimeRateEditDialog] = useState(false);
  const [overtimeRateType, setOvertimeRateType] = useState({});

  useEffect(() => {
    fetchOvertimeRateTypes();
  }, []);

  const fetchOvertimeRateTypes = () => {
    dispatch(Api.getOvertimeRateTypes());
  };

  const editRate = (overtimeRateType) => {
    setOvertimeRateType(overtimeRateType);
    setShowOvertimeRateEditDialog(true);
  };

  const closeOvertimeRateEditDialog = () => {
    setShowOvertimeRateEditDialog(false);
  };

  const overtimeRateSaved = () => {
    fetchOvertimeRateTypes();
  };

  if (!currentUser.hasPermission(Constant.PERMISSION_ADMIN)) {
    return <div>You do not have permission to view this page.</div>;
  }

  return (
    <div id="overtime-rates">
      <PageHeader>Manage Rental Agreement Overtime Rates</PageHeader>

      <div className="well">
        {overtimeRateTypes.length === 0 ? (
          <div style={{ textAlign: 'center' }}>
            <Spinner />
          </div>
        ) : (
          <TableControl
            headers={[
              { field: 'rateType', title: 'Rate Code' },
              { field: 'description', title: 'Description' },
              { field: 'value', title: 'Value' },
              { field: 'blank' },
            ]}
          >
            {_.map(overtimeRateTypes, (overtimeRateType) => (
              <tr key={overtimeRateType.id}>
                <td>{overtimeRateType.rateType}</td>
                <td>{overtimeRateType.description}</td>
                <td>{`$${overtimeRateType.rate.toFixed(2)}/Hr`}</td>
                <td style={{ textAlign: 'right' }}>
                  <Button
                    className="btn-custom"
                    title="Edit Rate"
                    size="sm"
                    onClick={() => editRate(overtimeRateType)}
                  >
                    <FontAwesomeIcon icon="edit" />
                  </Button>
                </td>
              </tr>
            ))}
          </TableControl>
        )}
      </div>

      {showOvertimeRateEditDialog && (
        <OvertimeRateEditDialog
          show={showOvertimeRateEditDialog}
          onClose={closeOvertimeRateEditDialog}
          onSave={overtimeRateSaved}
          overtimeRateType={overtimeRateType}
        />
      )}
    </div>
  );
};

OvertimeRates.propTypes = {
  currentUser: PropTypes.object,
  overtimeRateTypes: PropTypes.array,
};

export default OvertimeRates;
