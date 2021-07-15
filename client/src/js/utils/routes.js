import { hashHistory } from 'react-router';

export function currentPathStartsWith(path) {
  if (path.charAt(0) !== '/') {
    path = '/' + path;
  }

  var currentPath = hashHistory.getCurrentLocation().pathname;

  return currentPath.indexOf(path) === 0;
}
