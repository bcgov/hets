import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Button, OverlayTrigger } from 'react-bootstrap';

import * as Api from '../api';
import * as Constant from '../constants';

import PageHeader from '../components/ui/PageHeader.jsx';
import SubHeader from '../components/ui/SubHeader.jsx';
import CheckboxControl from '../components/CheckboxControl.jsx';
import Confirm from '../components/Confirm.jsx';
import Spinner from '../components/Spinner.jsx';

import { formatDateTimeUTCToLocal } from '../utils/date';

class Rollover extends React.Component {
  static propTypes = {
    currentUser: PropTypes.object,
    rolloverStatus: PropTypes.object,
    history: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      loading: true,
      checkListStep1: false,
      checkListStep2: false,
      checkListStep3: false,
      checkListStep4: false,
      refreshStatusTimerId: null,
    };
  }

  componentDidMount() {
    var user = this.props.currentUser;
    var status = this.props.rolloverStatus;

    Api.getRolloverStatus(this.props.currentUser.district.id).then(() => {
      this.setState({ loading: false });

      if (!user.hasPermission(Constant.PERMISSION_DISTRICT_ROLLOVER) && !status.rolloverActive) {
        // redirect to home page
        this.props.history.push(Constant.HOME_PATHNAME);
      }

      if (status.rolloverActive && this.state.refreshStatusTimerId === null) {
        this.startRefreshStatusTimer();
      }
    });
  }

  componentDidUpdate() {
    if (this.props.rolloverStatus.rolloverActive && this.state.refreshStatusTimerId === null) {
      this.startRefreshStatusTimer();
    }
  }

  startRefreshStatusTimer = () => {
    var refreshStatusTimerId = setInterval(this.refreshStatus, 2000); // 2 seconds
    this.setState({ refreshStatusTimerId: refreshStatusTimerId });
  };

  refreshStatus = () => {
    const districtId = this.props.currentUser.district.id;

    Api.getRolloverStatus(districtId).then(() => {
      const status = this.props.rolloverStatus;

      if (!status.rolloverActive && this.state.refreshStatusTimerId !== null) {
        clearInterval(this.state.refreshStatusTimerId);
        this.setState({ refreshStatusTimerId: null });
      }

      if (status.rolloverComplete) {
        // refresh fiscal years
        Api.getFiscalYears(districtId);
      }
    });
  };

  initiateRollover = () => {
    Api.initiateRollover(this.props.currentUser.district.id);
  };

  dismissRolloverNotice = () => {
    Api.dismissRolloverMessage(this.props.currentUser.district.id);
  };

  updateState = (state, callback) => {
    this.setState(state, callback);
  };

  renderContentRolloverActive = () => {
    var status = this.props.rolloverStatus;

    return (
      <div>
        <div>A roll over is currently in progress.</div>
        <div id="roll-over-progress" className="progress">
          <div
            className="progress-bar progress-bar-info progress-bar-striped active"
            role="progressbar"
            aria-valuenow={status.progressPercentage}
            aria-valuemin="0"
            aria-valuemax="100"
            style={{ width: `${status.progressPercentage}%` }}
          >
            <span className="sr-only">{status.progressPercentage}% Complete</span>
          </div>
        </div>
      </div>
    );
  };

  renderContentRolloverComplete = () => {
    return (
      <div className="text-center">
        <p>
          The hired equipment roll over has been completed on{' '}
          {formatDateTimeUTCToLocal(this.props.rolloverStatus.rolloverEndDate, Constant.DATE_TIME_READABLE)}.
        </p>
        <p>
          <strong>Note: </strong>Please save/print out the new seniority lists for all equipments corresponding to each
          local area.
        </p>
        <Button onClick={this.dismissRolloverNotice} variant="primary">
          Dismiss
        </Button>
      </div>
    );
  };

  renderContent = () => {
    var rolloverButtonDisabled =
      !this.state.checkListStep1 ||
      !this.state.checkListStep2 ||
      !this.state.checkListStep3 ||
      !this.state.checkListStep4;

    return (
      <div className="well">
        <SubHeader title="Pre-Roll Over Checklist" />
        <div id="checklist">
          <CheckboxControl
            id="checkListStep1"
            checked={this.state.checkListStep1}
            updateState={this.updateState}
            label="Verify all equipment hours have been entered in the system"
          />

          <CheckboxControl
            id="checkListStep2"
            checked={this.state.checkListStep2}
            updateState={this.updateState}
            label="Save the seniority list (pre-roll over)"
          />

          <CheckboxControl
            id="checkListStep3"
            checked={this.state.checkListStep3}
            updateState={this.updateState}
            label="Take note of any equipment currently hired"
          />

          <CheckboxControl
            id="checkListStep4"
            checked={this.state.checkListStep4}
            updateState={this.updateState}
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
              <Confirm onConfirm={this.initiateRollover}>
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

  render() {
    var status = this.props.rolloverStatus;
    var user = this.props.currentUser;

    return (
      <div id="roll-over">
        <PageHeader>{user.districtName} Roll Over</PageHeader>

        <div className="col-xs-10 offset-xs-1 col-sm-8 offset-sm-2 col-md-6 offset-md-3 col-lg-6 offset-lg-3">
          {(() => {
            if (this.state.loading) {
              return (
                <div style={{ textAlign: 'center' }}>
                  <Spinner />
                </div>
              );
            } else if (status.rolloverActive) {
              return this.renderContentRolloverActive();
            } else if (status.rolloverComplete) {
              return this.renderContentRolloverComplete();
            } else {
              return this.renderContent();
            }
          })()}
        </div>
      </div>
    );
  }
}

function mapStateToProps(state) {
  return {
    currentUser: state.user,
    rolloverStatus: state.lookups.rolloverStatus,
  };
}

export default connect(mapStateToProps)(Rollover);
