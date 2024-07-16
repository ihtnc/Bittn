import config from "@config";

describe("Config module", () => {
  test("should define GOOGLE_MAP_API_KEY", () => {
    expect(config.GOOGLE_MAP_API_KEY).toEqual("CHANGE_TO_YOUR_GOOGLE_MAP_API_KEY");
  });

  test("should define API_URL", () => {
    expect(config.API_URL).toEqual("https://localhost:8179/api/");
  });

  test("should define API_URL", () => {
    const actual = {
      QUEUE: { MIN: 8, MAX: 12 },
      PROCESS: { MIN: 40, MAX: 80 },
      WAIT: { MIN: 400, MAX: 750 },
      DISTANCE: { MIN: 5000, MAX: 10000 }
    };

    expect(config.VALUE_THRESHOLDS).toEqual(actual);
  });
});