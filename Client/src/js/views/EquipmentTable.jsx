import React from 'react';

import { Link } from 'react-router';

import _ from 'lodash';

import { Button, Glyphicon } from 'react-bootstrap';
import { LinkContainer } from 'react-router-bootstrap';

import * as Constant from '../constants';

import SortTable from '../components/SortTable.jsx';

import { formatDateTime } from '../utils/date';

var EquipmentTable = React.createClass({
  propTypes: {
    ui: React.PropTypes.object,
    updateUIState: React.PropTypes.func,
    equipmentList: React.PropTypes.object,
  },

  shouldComponentUpdate(nextProps) {
    if (this.props.ui !== nextProps.ui || this.props.equipmentList !== nextProps.equipmentList) {
      return true;
    }
    return false;
  },

  render() {
    
    var equipmentList = _.sortBy(this.props.equipmentList, this.props.ui.sortField);
    if (this.props.ui.sortDesc) {
      _.reverse(equipmentList);
    } 

    return (
      <SortTable sortField={ this.props.ui.sortField } sortDesc={ this.props.ui.sortDesc } onSort={ this.props.updateUIState } headers={[
        { field: 'equipmentCode',        title: 'Equipment ID'  },
        { field: 'typeName',             title: 'Type'          },
        { field: 'organizationName',     title: 'Owner'         },
        { field: 'senioritySortOrder',   title: 'Seniority'     },
        { field: 'isWorking',            title: 'Hired'         },
        { field: 'make',                 title: 'Make'          },
        { field: 'model',                title: 'Model'         },
        { field: 'size',                 title: 'Size'          },
        { field: 'equipmentAttachments', title: 'Attachments'   },
        { field: 'lastVerifiedDate',     title: 'Last Verified' },
        { field: 'blank'                                        },
      ]}>
        {
          _.map(equipmentList, (equip) => {
            return <tr key={ equip.id }>
              <td>{ equip.equipmentCode }</td>
              <td>{ equip.equipmentType }</td>
              <td><Link to={`${Constant.OWNERS_PATHNAME}/${equip.ownerId}`}>{ equip.ownerName }</Link></td>
              <td>{ equip.seniorityString }</td>
              <td>{ equip.isHired ? 'Y' : 'N' }</td>
              <td>{ equip.make }</td>
              <td>{ equip.model }</td>
              <td>{ equip.size }</td>
              <td>{ equip.attachmentCount }</td>
              <td>{ formatDateTime(equip.lastVerifiedDate, 'YYYY-MMM-DD') }</td>
              <td style={{ textAlign: 'right' }}>
                <LinkContainer to={{ pathname: 'equipment/' + equip.id }}>
                  <Button title="View Equipment" bsSize="xsmall"><Glyphicon glyph="edit" /></Button>
                </LinkContainer>
              </td>
            </tr>;
          })
        }
      </SortTable>
    );
  },
});

export default EquipmentTable;
