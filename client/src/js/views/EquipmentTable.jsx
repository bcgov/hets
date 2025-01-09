import PropTypes from 'prop-types';
import React, { memo, useMemo } from 'react';
import { Link } from 'react-router-dom';
import _ from 'lodash';
import { Button } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

import * as Constant from '../constants';
import SortTable from '../components/SortTable.jsx';
import { formatDateTime } from '../utils/date';

const EquipmentTable = ({ ui, updateUIState, equipmentList }) => {
  // Memoize sorted equipment list to prevent unnecessary re-sorting
  const sortedEquipmentList = useMemo(() => {
    const list = _.sortBy(equipmentList, (equipment) => {
      const sortValue = equipment[ui.sortField];
      return typeof sortValue === 'string' ? sortValue.toLowerCase() : sortValue;
    });
    if (ui.sortDesc) {
      _.reverse(list);
    }
    return list;
  }, [equipmentList, ui.sortField, ui.sortDesc]);

  return (
    <SortTable
      sortField={ui.sortField}
      sortDesc={ui.sortDesc}
      onSort={updateUIState}
      headers={[
        { field: 'sortableEquipmentCode', title: 'Equipment ID' },
        { field: 'localArea', title: 'Local Area' },
        { field: 'ownerName', title: 'Company Name' },
        { field: 'equipmentType', title: 'Equipment Type' },
        { field: 'details', title: 'Make/Model/Size/Year' },
        { field: 'attachmentCount', title: 'Attachments' },
        { field: 'projectName', title: 'Project' },
        { field: 'status', title: 'Status' },
        { field: 'lastVerifiedDate', title: 'Last Verified' },
        { field: 'blank' },
      ]}
    >
      {_.map(sortedEquipmentList, (equip) => (
        <tr
          key={equip.id}
          className={equip.status === Constant.EQUIPMENT_STATUS_CODE_APPROVED ? null : 'bg-info'}
        >
          <td>{equip.equipmentCode}</td>
          <td className="equipment-list-local-area-column">{equip.localArea}</td>
          <td>
            <Link to={`${Constant.OWNERS_PATHNAME}/${equip.ownerId}`}>{equip.ownerName}</Link>
          </td>
          <td>{equip.equipmentType}</td>
          <td className="equipment-list-details-column">{equip.details}</td>
          <td className="equipment-list-attachment-column">{equip.attachmentCount}</td>
          <td className="equipment-list-project-column">
            <Link to={`${Constant.PROJECTS_PATHNAME}/${equip.projectId}`}>{equip.projectName}</Link>
          </td>
          <td>{equip.status}</td>
          <td>{formatDateTime(equip.lastVerifiedDate, 'YYYY-MMM-DD')}</td>
          <td style={{ textAlign: 'right' }}>
            <Link to={`${Constant.EQUIPMENT_PATHNAME}/${equip.id}`}>
              <Button className="btn-custom" title="View Equipment" size="sm">
                <FontAwesomeIcon icon="edit" />
              </Button>
            </Link>
          </td>
        </tr>
      ))}
    </SortTable>
  );
};

EquipmentTable.propTypes = {
  ui: PropTypes.object.isRequired,
  updateUIState: PropTypes.func.isRequired,
  equipmentList: PropTypes.arrayOf(PropTypes.object).isRequired,
};

export default memo(EquipmentTable);
