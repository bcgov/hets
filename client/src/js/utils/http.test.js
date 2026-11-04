import { setupStore } from '../store';
import { decrementRequests, incrementRequests } from './http';

test('keeps the working indicator active until overlapping requests finish', () => {
  const store = setupStore();
  store.dispatch(incrementRequests());
  store.dispatch(incrementRequests());
  store.dispatch(decrementRequests());
  expect(store.getState().ui.requests.waiting).toBe(true);
  store.dispatch(decrementRequests());
  expect(store.getState().ui.requests.waiting).toBe(false);
});
