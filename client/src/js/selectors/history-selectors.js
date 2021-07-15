import { createSelector } from 'reselect';

import * as Constant from '../constants';

import { modelsSelector } from '../reducers/models';

import { activeEquipmentIdSelector, activeOwnerIdSelector, activeProjectIdSelector, activeRentalRequestIdSelector } from './ui-selectors';

const getHistoryType = (state, props) => {
  return props.historyEntity.type;
};

export const makeGetHistorySelector = () => {
  return createSelector(
    [ modelsSelector,
      getHistoryType,
      activeEquipmentIdSelector,
      activeOwnerIdSelector,
      activeProjectIdSelector,
      activeRentalRequestIdSelector,
    ],
    (models, historyType, activeEquipmentId, activeOwnerId, activeProjectId, activeRentalRequestId) => {
      switch (historyType) {
        case Constant.HISTORY_EQUIPMENT:
          return models.history.equipment[activeEquipmentId];
        case Constant.HISTORY_OWNER:
          return models.history.owner[activeOwnerId];
        case Constant.HISTORY_PROJECT:
          return models.history.project[activeProjectId];
        case Constant.HISTORY_REQUEST:
          return models.history.rentalRequest[activeRentalRequestId];
        default:
          return null;
      }
    }
  );
};
