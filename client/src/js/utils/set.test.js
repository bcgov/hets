import {
    arrayToSet,
    setToArray,
    setIntersection,
    arrIntersection,
    hasSetsIntersect,
    hasArrsIntersect,
} from "./set";

test("distinct array items equal set items", () => {
    const arr = [1, 2, 3];
    const set = arrayToSet(arr);
    expect(set.size).toBe(3);
    const inSet = arr.filter(item => set.has(item));
    expect(inSet.length).toBe(3);
});

test("duplicate array items removed by set", () => {
    const arr = [1, 1, 1, 2, 3, 2];
    const set = arrayToSet(arr);
    expect(set.size).toBe(3);
    const inSet = [1, 2, 3].filter(item => set.has(item));
    expect(inSet.length).toBe(3);
});

test("set has same items in array form", () => {
    const arr = [1, 2, 3];
    const set = arrayToSet(arr);
    const setInArrForm = setToArray(set);
    expect(setInArrForm.length).toBe(arr.length);
    const inSet = arr.filter(item => setInArrForm.includes(item));
    expect(inSet.length).toBe(setInArrForm.length);
});

test("subset for set intersections", () => {
    const arr1 = [1, 2, 3];
    const arr2 = [2, 3, 4, 2];
    const set1 = arrayToSet(arr1);
    const set2 = arrayToSet(arr2);
    const expectedIntersectionResult = [2, 3];
    const intersectionResult = setToArray(setIntersection(set1, set2));
    expect(intersectionResult.length).toBe(expectedIntersectionResult.length);
    expectedIntersectionResult.forEach(expectedItem => expect(intersectionResult).toContainEqual(expectedItem));
});

test("empty set for distinct set intersections", () => {
    const arr1 = [1, 2, 3];
    const arr2 = [4, 5, 6];
    const set1 = arrayToSet(arr1);
    const set2 = arrayToSet(arr2);
    expect(setIntersection(set1, set2).size).toBe(0);
});

test("subarray for array intersections", () => {
    const arr1 = [1, 2, 3];
    const arr2 = [2, 3, 4, 2];
    const expectedIntersectionResult = [2, 3];
    const intersectionResult = arrIntersection(arr1, arr2);
    expect(intersectionResult.length).toBe(expectedIntersectionResult.length);
    expectedIntersectionResult.forEach(expectedItem => expect(intersectionResult).toContainEqual(expectedItem));
});

test("empty array for distinct array intersections", () => {
    const arr1 = [1, 2, 3];
    const arr2 = [4, 5, 6];
    expect(arrIntersection(arr1, arr2).length).toBe(0);
});

test("set intersection for overlapping sets", () => {
    const set1 = arrayToSet([1, 2, 3]);
    const set2 = arrayToSet([2, 3, 4, 2]);
    expect(hasSetsIntersect(set1, set2)).toBe(true);
});

test("no set intersection for distinct sets", () => {
    const set1 = arrayToSet([1, 2, 3]);
    const set2 = arrayToSet([4, 5, 6]);
    expect(hasSetsIntersect(set1, set2)).toBe(false);
});

test("array intersection for overlapping arrays", () => {
    const arr1 = [1, 2, 3];
    const arr2 = [2, 3, 4, 2];
    expect(hasArrsIntersect(arr1, arr2)).toBe(true);
});

test("no array intersection for distinct arrays", () => {
    const arr1 = [1, 2, 3];
    const arr2 = [4, 5, 6];
    expect(hasArrsIntersect(arr1, arr2)).toBe(false);
});
