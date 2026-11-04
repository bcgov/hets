import { combineReducers } from 'redux';

import uiReducer from './ui';
import userReducer from './user';
import searchReducer from './search';
import modelsReducer from './models';
import lookupsReducer from './lookups';
import versionReducer from './version';
import { serverApi } from '../services/serverApi';


export default combineReducers({
  ui      : uiReducer,
  user    : userReducer,
  search  : searchReducer,
  models  : modelsReducer,
  lookups : lookupsReducer,
  version : versionReducer,
  [serverApi.reducerPath]: serverApi.reducer,
});
