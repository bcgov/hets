import { createSelector } from 'reselect';

import { uiSelector } from '../reducers/ui';
import { modelsSelector } from '../reducers/models';


export const activeRentalAgreementIdSelector = createSelector(
  uiSelector,
  (ui) => parseInt(ui.activeRentalAgreementId, 10)
);

export const activeRentalAgreementSelector = createSelector(
  activeRentalAgreementIdSelector,
  modelsSelector,
  (activeRentalAgreementId, models) => models.rentalAgreement[activeRentalAgreementId] || null
);


export const activeProjectIdSelector = createSelector(
  uiSelector,
  (ui) => parseInt(ui.activeProjectId, 10)
);

export const activeProjectSelector = createSelector(
  activeProjectIdSelector,
  modelsSelector,
  (activeProjectId, models) => models.project[activeProjectId] || null
);


export const activeRentalRequestIdSelector = createSelector(
  uiSelector,
  (ui) => parseInt(ui.activeRentalRequestId, 10)
);

export const activeRentalRequestSelector = createSelector(
  activeRentalRequestIdSelector,
  modelsSelector,
  (activeRentalRequestId, models) => models.rentalRequest[activeRentalRequestId] || null
);
