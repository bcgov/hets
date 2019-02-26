import { createSelector } from 'reselect';

import { uiSelector } from '../reducers/ui';
import { modelsSelector } from '../reducers/models';


export const activeRentalAgreementIdSelector = createSelector(
  uiSelector,
  (ui) => parseInt(ui.activeRentalAgreementId, 10)
);

export const activeRentalAgreementSelector = createSelector(
  uiSelector,
  modelsSelector,
  (ui, models) => models.rentalAgreement[ui.activeRentalAgreementId] || null
);
