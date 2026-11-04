let resetHandler = () => {};

export const registerSessionTimeoutReset = (handler) => {
  resetHandler = handler;
};

export const resetSessionTimeout = () => {
  resetHandler();
};

