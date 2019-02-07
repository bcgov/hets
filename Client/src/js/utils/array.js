import _ from 'lodash';

export function caseInsensitiveSort(arr, keys, dirs) {
  var comparators = keys.map((key) => {
    return (item) => {
      var val = _.get(item, key);
      if (typeof val === 'string') {
        return val.toLowerCase();
      }
      return val;
    };
  });
  return _.orderBy(arr, comparators, dirs);
}

export function sortDir(isDescending) {
  return isDescending ? 'desc' : 'asc';
}
