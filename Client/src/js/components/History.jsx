import React from 'react';

import { connect } from 'react-redux';

import { Alert, Button, Glyphicon } from 'react-bootstrap';

import _ from 'lodash';

import * as Action from '../actionTypes';
import * as Api from '../api';
import * as Constant from '../constants';
import * as History from '../history';
import store from '../store';

import SortTable from './SortTable.jsx';
import Spinner from './Spinner.jsx';

import { formatDateTime } from '../utils/date';


// API limit: how many to fetch first time
const API_LIMIT = 10;

var HistoryComponent = React.createClass({
  propTypes: {
    historyEntity: React.PropTypes.object.isRequired,
    refresh: React.PropTypes.bool.isRequired,

    // Used when displayed in a dialog
    onClose: React.PropTypes.func,

    history: React.PropTypes.object,
    users: React.PropTypes.object,
    ui: React.PropTypes.object,
  },

  getInitialState() {
    return {
      loading: false,

      canShowMore: false,

      ui : {
        sortField: this.props.ui.sortField || 'timestampSort',
        sortDesc: this.props.ui.sortDesc !== false,
      },
    };
  },

  componentDidMount() {
    this.setState({ loading: true });
    Api.getUsers().then(() => {
      return this.fetch(true);
    }).finally(() => {
      this.setState({ loading: false });
    });
  },

  componentWillReceiveProps(nextProps) {
    if (nextProps.refresh && !this.props.refresh) {
      this.fetch(true);
    }
  },

  getUserName(smUserId) {
    var user = _.find(this.props.users, user => { return user.smUserId === smUserId; });
    return user ? user.name : smUserId;
  },

  updateUIState(state, callback) {
    this.setState({ ui: { ...this.state.ui, ...state }}, () =>{
      store.dispatch({ type: Action.UPDATE_HISTORY_UI, history: this.state.ui });
      if (callback) { callback(); }
    });
  },

  fetch(first) {
    // Easy mode: show 10 the first time and let the user load all of them with the
    // "Show More" button. Can adapt for paginated / offset&limit calls if necessary.
    this.setState({ loading: true });
    return History.get(this.props.historyEntity, 0, first ? API_LIMIT : null).finally(() => {
      this.setState({
        loading: false,
        canShowMore: first && Object.keys(this.props.history).length >= API_LIMIT,
      });
    });
  },

  showMore() {
    this.fetch();
  },

  render() {
    return <div>
      {(() => {
        if (this.state.loading) { return <div style={{ textAlign: 'center' }}><Spinner/></div>; }

        if (Object.keys(this.props.history).length === 0) { return <Alert bsStyle="success">No history</Alert>; }

        var history = _.sortBy(this.props.history, this.state.ui.sortField);
        if (this.state.ui.sortDesc) {
          _.reverse(history);
        }

        var headers = [
          { field: 'timestampSort',       title: 'Timestamp' },
          { field: 'lastUpdateUserid',    title: 'User'      },
          { field: 'event', noSort: true, title: 'Event'     },
          { field: 'showMore',            title: 'Show More', style: { textAlign: 'right'  },
            node: <Button bsSize="xsmall" onClick={ this.showMore } className={ this.state.canShowMore ? '' : 'hidden' }>
                    <Glyphicon glyph="refresh" title="Show More" />
                  </Button>,
          },
        ];

        return <SortTable id="history-list" sortField={ this.state.ui.sortField } sortDesc={ this.state.ui.sortDesc } onSort={ this.updateUIState } headers={ headers }>
          {
            _.map(history, (history) => {
              return <tr key={ history.id }>
                <td>{ formatDateTime(history.lastUpdateTimestamp, Constant.DATE_TIME_LOG) }</td>
                <td>{ this.getUserName(history.lastUpdateUserid) }</td>
                <td className="history-event" colSpan="2">{ History.renderEvent(history.historyText, this.props.onClose) }</td>
              </tr>;
            })
          }
        </SortTable>;
      })()}
    </div>;
  },
});

function mapStateToProps(state) {
  return {
    history: state.models.history,
    users: state.models.users,
    ui: state.ui.history,
  };
}

export default connect(mapStateToProps)(HistoryComponent);
