export const getFavourites = () => {
  return {
    customFav: {
      1: {
        id: 1,
        type: "customFav",
        name: "Custom Fav 1",
        value: JSON.stringify({
          date: "1900-01-01",
          items: [1],
        }),
        isDefault: false,
        userId: 123,
        districtId: 1,
        concurrencyVersionNumber: 1,
      },
      2: {
        id: 2,
        type: "customFav",
        name: "Custom Fav 2",
        value: JSON.stringify({
          date: "1900-01-02",
          items: [1, 2],
        }),
        isDefault: false,
        userId: 123,
        districtId: 1,
        concurrencyVersionNumber: 1,
      },
      3: {
        id: 3,
        type: "customFav",
        name: "Custom Fav 3",
        value: JSON.stringify({
          date: "1900-01-03",
          items: [1, 2, 3],
        }),
        isDefault: true,
        userId: 123,
        districtId: 1,
        concurrencyVersionNumber: 1,
      },
    }
  };
};
