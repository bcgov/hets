import React from 'react';

import { connect } from 'react-redux';

import { PageHeader, Row, Col, Button } from 'react-bootstrap';

import _ from 'lodash';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import store from '../store';

import SortTable from '../components/SortTable.jsx';
import Spinner from '../components/Spinner.jsx';

var DistrictAdmin = React.createClass({
  propTypes: {
    currentUser: React.PropTypes.object,
    users: React.PropTypes.object,
    router: React.PropTypes.object,
  },

  render() {
    return <div id="home">

      {(() => {
        if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

        return (
          <SortTable sortField={ this.state.ui.sortField } sortDesc={ this.state.ui.sortDesc } onSort={ this.updateUIState } headers={[
            { field: 'surname',      title: 'Surname'    },
            { field: 'givenName',    title: 'First Name' },
            { field: 'smUserId',     title: 'User ID'    },
            { field: 'districtName', title: 'District'   },
          ]}>
            {
              _.map(this.props.users, (user) => {
                return <tr key={ user.id }>
                  <td>example</td>
                </tr>;
              })
            }
          </SortTable>
        );
      })()}
    </div>;
  },
});


function mapStateToProps(state) {
  return {
    currentUser: state.user,
    users: state.models.users,
  };
}

export default connect(mapStateToProps)(DistrictAdmin);
