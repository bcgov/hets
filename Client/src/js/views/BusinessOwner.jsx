import React from 'react';
import { connect } from 'react-redux';
import { Link } from 'react-router';
import { PageHeader, Well, Row, Col } from 'react-bootstrap';
import _ from 'lodash';

import Main from './Main.jsx';
import Spinner from '../components/Spinner.jsx';
import ColDisplay from '../components/ColDisplay.jsx';
import SortTable from '../components/SortTable.jsx';

import * as Constant from '../constants';

var BusinessOwner = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
    districtOwners: React.PropTypes.array,
  },

  getInitialState() {
    return {
      loading: true,
      owner: null,
      equipment: [],
      ui : {
        sortField: 'equipmentCode',
        sortDesc: true,
      },
    };
  },

  componentDidMount() {
    this.setState({ loading: true });

    // TODO: Make a real API call. Faked via a 2s timeout for now.
    new Promise((resolve) => setTimeout(resolve, 2 * 1000))
    .then(() => {
      // TODO: call setState() with actual response from API.
      this.setState({
        owner: {
          organizationName: 'ABC Trucking',
          fullAddress: '232 New test Ave Victoria city BC V8J 9H7',
          ownerName: 'ABC Trucking',
          ownerCode: 'ABC',
          primaryContactName: 'CHRYSTAL JONES',
          doingBusinessAs: null,
          registeredCompanyNumber: '9087',
          districtName: 'Bulkley-Stikine',
          localAreaName: 'Atlin',
          meetsResidency: true,
        },
        equipment: [
          {
            id: 123,
            equipmentCode: 'ABC-001',
            equipmentType: 'Large-Dump Truck',
            make: 'John Deere',
            model: '450J',
            size: '',
            year: null,
            serialNumber: null,
            attachments: [],
          },
        ],
      });
    }).finally(() => {
      this.setState({ loading: false });
    });
  },

  updateUIState(state) {
    this.setState({ ui: { ...this.state.ui, ...state } });
  },

  render() {
    const companyName = this.state.owner ? this.state.owner.organizationName : 'Loading...';

    return (
      <Main showNav={false}>
        <div id="business-owner-screen">
          <PageHeader>
            <Row>
              <Col md={6}>
                Company: {companyName}
              </Col>
              <Col md={6} style={{textAlign: 'right'}}>
                <Link to={ Constant.BUSINESS_LOGIN_PATHNAME } className="btn btn-default">← Return to List </Link>
              </Col>
            </Row>
          </PageHeader>
          {this.state.loading && <Spinner />}
          {!this.state.loading && this.renderPage()}
        </div>
      </Main>
    );
  },

  renderPage() {
    const { owner, equipment } = this.state;

    const equipmentList = _.sortBy(equipment, (equip) => {
      if (this.state.ui.sortField === 'makeModelSize') {
        return `${equip.make.toLowerCase()}:${equip.model.toLowerCase()}:${equip.size.toLowerCase()}`;
      }

      var sortValue = equip[this.state.ui.sortField];
      if (typeof sortValue === 'string') {
        return sortValue.toLowerCase();
      }
      return sortValue;
    });

    if (this.state.ui.sortDesc) {
      equipmentList.reverse();
    }

    const sortTableHeaders = [
      { field: 'equipmentCode',        title: 'ID'              },
      { field: 'equipmentType',        title: 'Type'            },
      { field: 'makeModelSize',        title: 'Make/Model/Size' },
      { field: 'year',                 title: 'Year'            },
      { field: 'serialNumber',         title: 'S/N'             },
      { field: 'attachmentCount',      title: 'Attachments'     },
    ];

    return (
      <div>
        <Well>
          <div id="owners-information">
            <h2>Owner Information</h2>
            <Row>
              <ColDisplay md={6} labelProps={{ md: 6 }} label="Company">{ owner.organizationName }</ColDisplay>
              <ColDisplay md={6} labelProps={{ md: 4 }} label="Company Address">{ owner.fullAddress }</ColDisplay>
            </Row>
            <Row>
              <ColDisplay md={6} labelProps={{ md: 6 }} label="Owner Name">{ owner.ownerName }</ColDisplay>
              <ColDisplay md={6} labelProps={{ md: 4 }} label="Owner Code">{ owner.ownerCode }</ColDisplay>
            </Row>
            <Row>
              <ColDisplay md={6} labelProps={{ md: 6 }} label="Primary Contact">{ owner.primaryContactName }</ColDisplay>
              <ColDisplay md={6} labelProps={{ md: 4 }} label="Doing Business As">{ owner.doingBusinessAs }</ColDisplay>
            </Row>
            <Row>
              <ColDisplay md={6} labelProps={{ md: 6 }} label="Registered BC Company Number">{ owner.registeredCompanyNumber }</ColDisplay>
              <ColDisplay md={6} labelProps={{ md: 4 }} label="District Office">{ owner.districtName }</ColDisplay>
            </Row>
            <Row>
              <ColDisplay md={6} labelProps={{ md: 6 }} label="Service/Local Area">{ owner.localAreaName }</ColDisplay>
              <ColDisplay md={6} labelProps={{ md: 4 }} label="Meets Residency?">{ owner.meetsResidency ? 'Yes' : 'No' }</ColDisplay>
            </Row>
          </div>
        </Well>
        <Well>
          <SortTable sortField={ this.state.ui.sortField } sortDesc={ this.state.ui.sortDesc } headers={sortTableHeaders} onSort={this.updateUIState}>
            {
              equipmentList.map((equip) => {
                return (
                  <tr key={ equip.id }>
                    <td>{ equip.equipmentCode }</td>
                    <td>{ equip.equipmentType }</td>
                    <td>{ equip.make }/{ equip.model }/{ equip.size }</td>
                    <td>{ equip.year }</td>
                    <td>{ equip.serialNumber }</td>
                    <td>{ equip.attachmentCount }</td>
                  </tr>
                );
              })
            }
          </SortTable>
        </Well>
      </div>
    );
  },
});


function mapStateToProps(state) {
  return {
    currentUser: state.user,
    districtOwners: [],
  };
}

export default connect(mapStateToProps)(BusinessOwner);
