import PropTypes from 'prop-types';
import React from 'react';
import { connect } from 'react-redux';
import { Alert, Button } from 'react-bootstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import _ from 'lodash';

import * as Action from '../actionTypes';
import * as Constant from '../constants';
import * as History from '../history';
import store from '../store';

import SortTable from './SortTable.jsx';
import Spinner from './Spinner.jsx';

import { makeGetHistorySelector } from '../selectors/history-selectors.js';

import { formatDateTimeUTCToLocal } from '../utils/date';
import { sortDir } from '../utils/array';

// API limit: how many to fetch first time
const API_LIMIT = 10;

class HistoryComponent extends React.Component {
  static propTypes = {
    historyEntity: PropTypes.object.isRequired,
    refresh: PropTypes.bool.isRequired,

    // Used when displayed in a dialog
    onClose: PropTypes.func,

    history: PropTypes.object,
    users: PropTypes.object,
    ui: PropTypes.object,
  };

  constructor(props) {
    super(props);

    this.state = {
      loading: !this.props.history,
      fetchingMore: false,

      canShowMore: false,

      ui: {
        sortField: props.ui.sortField || 'timestampSort',
        sortDesc: props.ui.sortDesc !== false,
      },
    };
  }

  componentDidUpdate(prevProps) {
    if (this.props.refresh && !prevProps.refresh) {
      this.fetch(true);
    }
  }

  updateUIState = (state, callback) => {
    this.setState({ ui: { ...this.state.ui, ...state } }, () => {
      store.dispatch({ type: Action.UPDATE_HISTORY_UI, history: this.state.ui });
      if (callback) {
        callback();
      }
    });
  };

  fetch = (first) => {
    // Easy mode: show 10 the first time and let the user load all of them with the
    // "Show More" button. Can adapt for paginated / offset&limit calls if necessary.

    if (first) {
      if (this.props.history) {
        // if old data exists, update it in the background without displaying a loading spinner
        this.setState({ loading: false });
      } else {
        // if no data exists, display spinner while it loads
        this.setState({ loading: true });
      }
    }

    return History.get(this.props.historyEntity, 0, first ? API_LIMIT : null).finally(() => {
      this.setState({
        loading: false,
        canShowMore: first && Object.keys(this.props.history).length >= API_LIMIT,
      });
    });
  };

  showMore = () => {
    this.setState({ fetchingMore: true });
    this.fetch().finally(() => {
      this.setState({ fetchingMore: false });
    });
  };

  render() {
    const { loading, fetchingMore } = this.state;

    return (
      <div>
        {(() => {
          if (loading) {
            return (
              <div style={{ textAlign: 'center' }}>
                <Spinner />
              </div>
            );
          }

          if (!this.props.history || Object.keys(this.props.history).length === 0) {
            return <Alert variant="success">No history</Alert>;
          }

          var history = _.orderBy(this.props.history, [this.state.ui.sortField], sortDir(this.state.ui.sortDesc));

          var headers = [
            { field: 'timestampSort', title: 'Timestamp' },
            { field: 'userName', title: 'User' },
            { field: 'event', noSort: true, title: 'Event' },
            {
              field: 'showMore',
              title: 'Show More',
              style: { textAlign: 'right' },
              node: fetchingMore ? (
                <Spinner />
              ) : (
                <Button size="sm" onClick={this.showMore} className={this.state.canShowMore ? 'btn-custom' : 'd-none'}>
                  <FontAwesomeIcon icon="sync-alt" title="Show More" />
                </Button>
              ),
            },
          ];
          return (
            <SortTable
              id="history-list"
              sortField={this.state.ui.sortField}
              sortDesc={this.state.ui.sortDesc}
              onSort={this.updateUIState}
              headers={headers}
            >
              {history
                .map((history) => {
                  const event = History.renderEvent(history.historyText, this.props.onClose);
                  const formattedTimestamp = formatDateTimeUTCToLocal(
                    history.lastUpdateTimestamp,
                    Constant.DATE_TIME_LOG
                  );

                  return (
                    <tr key={history.id}>
                      <td>{formattedTimestamp}</td>
                      <td>{history.lastUpdateUserid}</td>
                      <td className="history-event" colSpan="2">
                        {event}
                      </td>
                    </tr>
                  );
                })
                .concat(
                  fetchingMore
                    ? [
                        <tr key="loading-more">
                          <td colSpan="4" style={{ textAlign: 'center' }}>
                            <Spinner />
                          </td>
                        </tr>,
                      ]
                    : []
                )}
            </SortTable>
          );
        })()}
      </div>
    );
  }
}

const makeMapStateToProps = () => {
  const getHistorySelector = makeGetHistorySelector();
  const mapStateToProps = (state, props) => {
    return {
      history: getHistorySelector(state, props),
      users: state.lookups.users,
      ui: state.ui.history,
    };
  };
  return mapStateToProps;
};

export default connect(makeMapStateToProps)(HistoryComponent);
