const DEFAULT_MODELS = {
  users: {},
  user: {},
};

export default function modelsReducer(state = DEFAULT_MODELS, action) {
  switch(action.type) {
    // Users
    case 'UPDATE_USERS':
      return { ...state, users: action.users };

    case 'UPDATE_USER':
      return { ...state, user: action.user };

  }

  return state;
}
