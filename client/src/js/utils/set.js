export function arrayToSet(arr) {
  const set = new Set();
  arr.forEach(item => set.add(item)); // faster than new Set(arr)
  return set;
}

export function setToArray(set) {
  const arr = [];
  set.forEach(item => arr.push(item));
  return arr;
}

export function setIntersection(set1, set2) {
  const res = new Set();
  const smallerSet = set1.size <= set2.size ? set1 : set2;
  const largerSet = set1.size <= set2.size ? set2 : set1;

  smallerSet.forEach(smallerSetItem => {
    if (largerSet.has(smallerSetItem)) {
      res.add(smallerSetItem);
    }
  });

  return res;
}

export function arrIntersection(arr1, arr2) {
  const set1 = arrayToSet(arr1);
  const set2 = arrayToSet(arr2);

  return setToArray(setIntersection(set1, set2));
}

export function hasSetsIntersect(set1, set2) {
  return setIntersection(set1, set2).size > 0;
}

export function hasArrsIntersect(arr1, arr2) {
  const set1 = arrayToSet(arr1);
  const set2 = arrayToSet(arr2);

  return hasSetsIntersect(set1, set2);
}
