import _ from 'lodash';


export function sort(arr, keys, dir, sortCompareFn) {
  const flatKeys = Array.isArray(keys) ? _.flatten(keys) : [keys];

  const comparators = flatKeys.map((key) => {
    return (item) => {
      const val = _.get(item, key);
      return sortCompareFn ? sortCompareFn(val, item, key) : val;
    };
  });
  const sortDirection = typeof dir === 'boolean' ? sortDir(dir) : dir;
  return _.orderBy(arr, comparators, _.fill(Array(comparators.length), sortDirection || 'asc'));
}

export function caseInsensitiveSort(val/* , item, key */) {
  if (typeof val === 'string') {
    return val.toLowerCase();
  }
  return val;
}

export function sortDir(isDescending) {
  return isDescending ? 'desc' : 'asc';
}

export function findAndUpdate(arr, obj, key = 'id') {
  const val = obj[key];
  const pos = _.findIndex(arr, (item) => item[key] === val);
  if (pos !== -1) {
    arr[pos] = { ...arr[pos], ...obj };
  }
  return arr;
}
