import PropTypes from "prop-types";
import React from "react";
//temporary fix
//import { Link } from "react-router";
import { Link } from "react-router-dom";
import _ from "lodash";
import { Button, Glyphicon } from "react-bootstrap";
//temporary fix remove when no longer needed
import { LinkContainer } from "react-router-bootstrap";

import * as Constant from "../constants";

import SortTable from "../components/SortTable.jsx";

import { formatDateTime } from "../utils/date";

class EquipmentTable extends React.Component {
  static propTypes = {
    ui: PropTypes.object,
    updateUIState: PropTypes.func,
    equipmentList: PropTypes.object,
  };

  shouldComponentUpdate(nextProps) {
    if (
      this.props.ui !== nextProps.ui ||
      this.props.equipmentList !== nextProps.equipmentList
    ) {
      return true;
    }
    return false;
  }

  render() {
    var equipmentList = _.sortBy(this.props.equipmentList, (equipment) => {
      var sortValue = equipment[this.props.ui.sortField];
      if (typeof sortValue === "string") {
        return sortValue.toLowerCase();
      }
      return sortValue;
    });

    if (this.props.ui.sortDesc) {
      _.reverse(equipmentList);
    }

    return (
      <SortTable
        sortField={this.props.ui.sortField}
        sortDesc={this.props.ui.sortDesc}
        onSort={this.props.updateUIState}
        headers={[
          { field: "sortableEquipmentCode", title: "Equipment ID" },
          { field: "localArea", title: "Local Area" },
          { field: "ownerName", title: "Company Name" },
          { field: "equipmentType", title: "Equipment Type" },
          { field: "details", title: "Make/Model/Size/Year" },
          { field: "attachmentCount", title: "Attachments" },
          { field: "projectName", title: "Project" },
          { field: "status", title: "Status" },
          { field: "lastVerifiedDate", title: "Last Verified" },
          { field: "blank" },
        ]}
      >
        {_.map(equipmentList, (equip) => {
          return (
            <tr
              key={equip.id}
              className={
                equip.status === Constant.EQUIPMENT_STATUS_CODE_APPROVED
                  ? null
                  : "info"
              }
            >
              <td>{equip.equipmentCode}</td>
              <td>{equip.localArea}</td>
              <td>
                <Link to={`${Constant.OWNERS_PATHNAME}/${equip.ownerId}`}>
                  {equip.ownerName}
                </Link>
              </td>
              <td>{equip.equipmentType}</td>
              <td>{equip.details}</td>
              <td>{equip.attachmentCount}</td>
              <td>
                <Link to={`${Constant.PROJECTS_PATHNAME}/${equip.projectId}`}>
                  {equip.projectName}
                </Link>
              </td>
              <td>{equip.status}</td>
              <td>{formatDateTime(equip.lastVerifiedDate, "YYYY-MMM-DD")}</td>
              <td style={{ textAlign: "right" }}>
                {/* temporary fix  */}
                {/* <LinkContainer to={{ pathname: 'equipment/' + equip.id }}>
                    <Button title="View Equipment" bsSize="xsmall"><Glyphicon glyph="edit" /></Button>
                  </LinkContainer> */}
                <Link to={`${Constant.EQUIPMENT_PATHNAME}/${equip.id}`}>
                  <Button title="View Equipment" bsSize="xsmall">
                    <Glyphicon glyph="edit" />
                  </Button>
                </Link>
              </td>
            </tr>
          );
        })}
      </SortTable>
    );
  }
}

export default EquipmentTable;
