import PropTypes from "prop-types";
import React from "react";
import { connect } from "react-redux";
import { Button, ButtonGroup, Glyphicon, Well } from "react-bootstrap";
import _ from "lodash";

import * as Api from "../api";
import * as Constant from "../constants";

import PageHeader from "../components/ui/PageHeader.jsx";
import TableControl from "../components/TableControl.jsx";
import Spinner from "../components/Spinner.jsx";
import OvertimeRateEditDialog from "./dialogs/OvertimeRateEditDialog.jsx";

class OvertimeRates extends React.Component {
  static propTypes = {
    currentUser: PropTypes.object,
    overtimeRateTypes: PropTypes.array,
    router: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      showOvertimeRateEditDialog: false,
      overtimeRateType: {},
    };
  }

  componentDidMount() {
    this.fetch();
  }

  fetch = () => {
    Api.getOvertimeRateTypes();
  };

  editRate = (overtimeRateType) => {
    this.setState(
      { overtimeRateType: overtimeRateType },
      this.showOvertimeRateEditDialog
    );
  };

  showOvertimeRateEditDialog = () => {
    this.setState({ showOvertimeRateEditDialog: true });
  };

  closeOvertimeRateEditDialog = () => {
    this.setState({ showOvertimeRateEditDialog: false });
  };

  overtimeRateSaved = () => {
    this.fetch();
  };

  render() {
    if (!this.props.currentUser.hasPermission(Constant.PERMISSION_ADMIN)) {
      return <div>You do not have permission to view this page.</div>;
    }

    return (
      <div id="overtime-rates">
        <PageHeader>Manage Rental Agreement Overtime Rates</PageHeader>

        <Well>
          {(() => {
            if (this.props.overtimeRateTypes.length === 0) {
              return (
                <div style={{ textAlign: "center" }}>
                  <Spinner />
                </div>
              );
            }

            return (
              <TableControl
                headers={[
                  { field: "rateType", title: "Rate Code" },
                  { field: "description", title: "Description" },
                  { field: "value", title: "Value" },
                  { field: "blank" },
                ]}
              >
                {_.map(this.props.overtimeRateTypes, (overtimeRateType) => {
                  return (
                    <tr key={overtimeRateType.id}>
                      <td>{overtimeRateType.rateType}</td>
                      <td>{overtimeRateType.description}</td>
                      <td>{`$${overtimeRateType.rate.toFixed(2)}/Hr`}</td>
                      <td style={{ textAlign: "right" }}>
                        <ButtonGroup>
                          <Button
                            title="Edit Rate"
                            bsSize="xsmall"
                            onClick={this.editRate.bind(this, overtimeRateType)}
                          >
                            <Glyphicon glyph="edit" />
                          </Button>
                        </ButtonGroup>
                      </td>
                    </tr>
                  );
                })}
              </TableControl>
            );
          })()}
        </Well>

        {this.state.showOvertimeRateEditDialog && (
          <OvertimeRateEditDialog
            show={this.state.showOvertimeRateEditDialog}
            onClose={this.closeOvertimeRateEditDialog}
            onSave={this.overtimeRateSaved}
            overtimeRateType={this.state.overtimeRateType}
          />
        )}
      </div>
    );
  }
}

function mapStateToProps(state) {
  return {
    currentUser: state.user,
    overtimeRateTypes: state.lookups.overtimeRateTypes,
  };
}

export default connect(mapStateToProps)(OvertimeRates);
