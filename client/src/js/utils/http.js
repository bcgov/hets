import _ from 'lodash';

import * as Action from '../actionTypes';
import { keycloak } from '../Keycloak';

import * as Constant from '../constants';
import { resetSessionTimeoutTimer } from '../App';

var numRequestsInFlight = 0;

const incrementRequests = () => (dispatch) => {
  numRequestsInFlight += 1;
  if (numRequestsInFlight === 1) {
    dispatch({ type: Action.REQUESTS_BEGIN });
  }
};

const decrementRequests = () => (dispatch) => {
  numRequestsInFlight -= 1;
  if (numRequestsInFlight <= 0) {
    numRequestsInFlight = 0; // sanity check;
    dispatch({ type: Action.REQUESTS_END });
  }
};

export const HttpError = function (msg, method, path, status, body) {
  this.message = msg || '';
  this.method = method;
  this.path = path;
  this.status = status || null;
  this.body = body;
};

HttpError.prototype = Object.create(Error.prototype, {
  constructor: { value: HttpError },
});

export const ApiError = function (msg, method, path, status, errorCode, errorDescription, json) {
  this.message = msg || '';
  this.method = method;
  this.path = path;
  this.status = status || null;
  this.errorCode = errorCode || null;
  this.errorDescription = errorDescription || null;
  this.json = json || null;
};

ApiError.prototype = Object.create(Error.prototype, {
  constructor: { value: ApiError },
});

export const Resource404 = function (name, id) {
  this.name = name;
  this.id = id;
};

Resource404.prototype = Object.create(Error.prototype, {
  constructor: { value: Resource404 },
  toString: {
    value() {
      return `Resouce ${this.name} #${this.id} Not Found`;
    },
  },
});

export const request = (path, options) => async (dispatch) => {
  try {
    if (await keycloak.updateToken(5)) {
      //if token expires within 70 seconds it gets updated will return true to trigger resetSessionTimeoutTimer refresh.
      resetSessionTimeoutTimer();
    }
  } catch {
    console.log('Failed to refresh the token, or the session has expired');
  }

  options = options || {};
  options.headers = Object.assign(
    {
      Authorization: `Bearer ${keycloak.token}`,
    },
    options.headers || {}
  );

  let xhr = new XMLHttpRequest();
  let method = (options.method || 'GET').toUpperCase();

  if (!options.files) {
    options.headers = Object.assign(
      {
        'Content-Type': 'application/x-www-form-urlencoded',
      },
      options.headers
    );
  }

  if (options.onUploadProgress) {
    xhr.upload.addEventListener('progress', function (e) {
      if (e.lengthComputable) {
        options.onUploadProgress((e.loaded / e.total) * 100);
      } else {
        options.onUploadProgress(null);
      }
    });

    xhr.upload.addEventListener('load', function (/*e*/) {
      options.onUploadProgress(100);
    });
  }

  if (options.responseType && window.navigator.appName !== 'Netscape') {
    xhr.responseType = options.responseType;
  } else if (options.responseType && window.navigator.appName === 'Netscape') {
    xhr.open(options.method, path);
    xhr.responseType = options.responseType;
  }

  return new Promise((resolve, reject) => {
    xhr.addEventListener('load', function () {
      if (xhr.status >= 400) {
        let responseText = '';
        try {
          responseText = xhr.responseText;
        } catch (e) {
          /* swallow */
        }

        var err = new HttpError(
          `API ${method} ${path} failed (${xhr.status}) "${responseText}"`,
          method,
          path,
          xhr.status,
          responseText
        );
        reject(err);
      } else {
        // console.log('Call complete! Path: ' + path);
        resolve(xhr);
      }
    });

    xhr.addEventListener('error', function () {
      reject(new HttpError(`Request ${method} ${path} failed to send`, method, path));
    });

    let qs = _.map(options.querystring, (value, key) => `${encodeURIComponent(key)}=${encodeURIComponent(value)}`).join(
      '&'
    );

    if (options.responseType) xhr.responseType = options.responseType;

    xhr.open(method, `${path}${qs ? '?' : ''}${qs}`, true);

    Object.keys(options.headers).forEach((key) => {
      xhr.setRequestHeader(key, options.headers[key]);
    });

    if (!options.silent) {
      dispatch(incrementRequests());
    }

    var payload = options.body || null;

    if (options.files) {
      payload = new FormData();
      if (typeof options.body === 'object') {
        Object.keys(options.body).forEach((key) => {
          payload.append(key, options.body[key]);
        });
      }
      options.files.forEach((file) => {
        payload.append('files', file, file.name);
      });
    }

    xhr.send(payload);
  }).finally(() => {
    if (!options.silent) {
      dispatch(decrementRequests());
    }
  });
}

export const jsonRequest = (path, options) => async (dispatch) => {
  let jsonHeaders = {
    Accept: 'application/json',
  };

  if (options.body) {
    options.body = JSON.stringify(options.body);
    jsonHeaders['Content-Type'] = 'application/json';
  }

  options.headers = Object.assign(options.headers || {}, jsonHeaders);

  try {
    const xhr = await dispatch(request(path, options));
    if (xhr.status === 204) {
      return;
    } 
    if (xhr.responseType === Constant.RESPONSE_TYPE_BLOB) {
      return xhr.response;
    } 
    if (options.ignoreResponse) {
      return null;
    }
    return xhr.responseText ? JSON.parse(xhr.responseText) : null;
  } catch (err) {
    if (err instanceof HttpError) {
      const errMsg = `API ${err.method} ${err.path} failed (${err.status})`;
      let json = null;
      let errorCode = null;
      let errorDescription = null;
      try {
        // Example error payload from server:
        // {
        //   "responseStatus": "ERROR",
        //   "data": null,
        //   "error": {
        //     "error": "HETS-01",
        //     "description": "Record not found"
        //   }
        // }

        json = JSON.parse(err.body);
        errorCode = json.error.error;
        errorDescription = json.error.description;
      } catch (err) {
        /* not json */
      }

      throw new ApiError(errMsg, err.method, err.path, err.status, errorCode, errorDescription, json);
    } else {
      throw err;
    }
  }
};

export function buildApiPath(path) {
  return `/api${path}`.replace('//', '/'); // remove double slashes
}

export function ApiRequest(path, options) {
  this.path = `/api${path}`;
  this.options = options;
}

ApiRequest.prototype.get = function apiGet(params, options) {
  const path = this.path;
  const mainOptions = this.options;
  return async function(dispatch) {
    return await dispatch(jsonRequest(path, {
      method: 'GET',
      querystring: params,
      ...mainOptions,
      ...options,
    }));
  };
};

ApiRequest.prototype.getBlob = function apiGet() {
  const path = this.path;
  return async function(dispatch) {
    return await dispatch(request(path, { method: 'GET', responseType: 'blob' }));
  };
};

ApiRequest.prototype.post = function apiPost(data, options) {
  const path = this.path;
  const mainOptions = this.options;
  return async function(dispatch) {
    return await dispatch(jsonRequest(path, {
      method: 'POST',
      body: data,
      ...mainOptions,
      ...options,
    }));
  };
};

ApiRequest.prototype.put = function apiPut(data, options) {
  const path = this.path;
  const mainOptions = this.options;
  return async function(dispatch) {
    return await dispatch(jsonRequest(path, {
      method: 'PUT',
      body: data,
      ...mainOptions,
      ...options,
    }));
  };
};

ApiRequest.prototype.delete = function apiDelete(data, options) {
  const path = this.path;
  const mainOptions = this.options;
  return async function(dispatch) {
    return await dispatch(jsonRequest(path, {
      method: 'DELETE',
      body: data,
      ...mainOptions,
      ...options,
    }));
  };
};
