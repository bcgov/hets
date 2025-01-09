import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';
import { connect, useDispatch } from 'react-redux';
import { Button, OverlayTrigger } from 'react-bootstrap';

import * as Api from '../api';
import * as Constant from '../constants';

import PageHeader from '../components/ui/PageHeader.jsx';
import SubHeader from '../components/ui/SubHeader.jsx';
import CheckboxControl from '../components/CheckboxControl.jsx';
import Confirm from '../components/Confirm.jsx';
import Spinner from '../components/Spinner.jsx';

import { formatDateTimeUTCToLocal } from '../utils/date';

const Rollover = ({ currentUser, rolloverStatus, history }) => {
  const [loading, setLoading] = useState(true);
  const [checkListStep1, setCheckListStep1] = useState(false);
  const [checkListStep2, setCheckListStep2] = useState(false);
  const [checkListStep3, setCheckListStep3] = useState(false);
  const [checkListStep4, setCheckListStep4] = useState(false);
  const [refreshStatusTimerId, setRefreshStatusTimerId] = useState(null);

  const dispatch = useDispatch();

  useEffect(() => {
    dispatch(Api.getRolloverStatus(currentUser.district.id)).then(() => {
      setLoading(false);

      if (!currentUser.hasPermission(Constant.PERMISSION_DISTRICT_ROLLOVER) && !rolloverStatus.rolloverActive) {
        // redirect to home page
        history.push(Constant.HOME_PATHNAME);
      }

      if (rolloverStatus.rolloverActive && refreshStatusTimerId === null) {
        startRefreshStatusTimer();
      }
    });
  }, [dispatch, currentUser, history, rolloverStatus.rolloverActive, refreshStatusTimerId]);

  useEffect(() => {
    if (rolloverStatus.rolloverActive && refreshStatusTimerId === null) {
      startRefreshStatusTimer();
    }
  }, [rolloverStatus.rolloverActive, refreshStatusTimerId]);

  const startRefreshStatusTimer = () => {
    const timerId = setInterval(refreshStatus, 2000); // 2 seconds
    setRefreshStatusTimerId(timerId);
  };

  const refreshStatus = () => {
    const districtId = currentUser.district.id;
    dispatch(Api.getRolloverStatus(districtId)).then(() => {
      const status = rolloverStatus;

      if (!status.rolloverActive && refreshStatusTimerId !== null) {
        clearInterval(refreshStatusTimerId);
        setRefreshStatusTimerId(null);
      }

      if (status.rolloverComplete) {
        // refresh fiscal years
        dispatch(Api.getFiscalYears(districtId));
      }
    });
  };

  const initiateRollover = () => {
    dispatch(Api.initiateRollover(currentUser.district.id));
  };

  const dismissRolloverNotice = () => {
    dispatch(Api.dismissRolloverMessage(currentUser.district.id));
  };

  const renderContentRolloverActive = () => {
    return (
      <div>
        <div>A roll over is currently in progress.</div>
        <div id="roll-over-progress" className="progress">
          <div
            className="progress-bar progress-bar-info progress-bar-striped active"
            role="progressbar"
            aria-valuenow={rolloverStatus.progressPercentage}
            aria-valuemin="0"
            aria-valuemax="100"
            style={{ width: `${rolloverStatus.progressPercentage}%` }}
          >
            <span className="sr-only">{rolloverStatus.progressPercentage}% Complete</span>
          </div>
        </div>
      </div>
    );
  };

  const renderContentRolloverComplete = () => {
    return (
      <div className="text-center">
        <p>
          The hired equipment roll over has been completed on{' '}
          {formatDateTimeUTCToLocal(rolloverStatus.rolloverEndDate, Constant.DATE_TIME_READABLE)}.
        </p>
        <p>
          <strong>Note: </strong>Please save/print out the new seniority lists for all equipments corresponding to each
          local area.
        </p>
        <Button onClick={dismissRolloverNotice} variant="primary">
          Dismiss
        </Button>
      </div>
    );
  };

  const renderContent = () => {
    const rolloverButtonDisabled =
      !checkListStep1 || !checkListStep2 || !checkListStep3 || !checkListStep4;

    return (
      <div className="well">
        <SubHeader title="Pre-Roll Over Checklist" />
        <div id="checklist">
          <CheckboxControl
            id="checkListStep1"
            checked={checkListStep1}
            updateState={setCheckListStep1}
            label="Verify all equipment hours have been entered in the system"
          />

          <CheckboxControl
            id="checkListStep2"
            checked={checkListStep2}
            updateState={setCheckListStep2}
            label="Save the seniority list (pre-roll over)"
          />

          <CheckboxControl
            id="checkListStep3"
            checked={checkListStep3}
            updateState={setCheckListStep3}
            label="Take note of any equipment currently hired"
          />

          <CheckboxControl
            id="checkListStep4"
            checked={checkListStep4}
            updateState={setCheckListStep4}
            label="Release all blocked rotation lists, as the hiring order may change after the roll over"
          />
        </div>

        <div id="description">
          <strong>Note: </strong>
          <span>
            The roll over is an important annual process that recalculates the seniority for each piece of equipment
            across the district at the start of a fiscal year. Once triggered, this process is not reversible. In case
            you have any questions, please contact the primary coordinator for the process before proceeding with the
            hired equipment roll over.
          </span>
        </div>

        <div className="clearfix">
          <OverlayTrigger
            trigger="click"
            placement="top"
            rootClose
            overlay={
              <Confirm onConfirm={initiateRollover}>
                <p>Please ensure all processes corresponding to the checklist are complete before rolling over.</p>
                <p>
                  If you are certain all tasks have been completed, click <strong>Yes</strong> to proceed with roll
                  over. Otherwise, click <strong>No</strong>.
                </p>
              </Confirm>
            }
          >
            <Button className="float-right" disabled={rolloverButtonDisabled} title="Roll Over">
              Roll Over
            </Button>
          </OverlayTrigger>
        </div>
      </div>
    );
  };

  if (loading) {
    return (
      <div style={{ textAlign: 'center' }}>
        <Spinner />
      </div>
    );
  }

  if (rolloverStatus.rolloverActive) {
    return renderContentRolloverActive();
  } else if (rolloverStatus.rolloverComplete) {
    return renderContentRolloverComplete();
  } else {
    return renderContent();
  }
};

Rollover.propTypes = {
  currentUser: PropTypes.object,
  rolloverStatus: PropTypes.object,
  history: PropTypes.object,
};

const mapStateToProps = (state) => ({
  currentUser: state.user,
  rolloverStatus: state.lookups.rolloverStatus,
});

export default connect(mapStateToProps)(Rollover);
