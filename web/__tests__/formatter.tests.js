import formatter from "@src/formatter";

describe("Formatter module", () => {

  describe("toDuration", () => {
    test("should calculate months", () => {
      const months = 2.00;
      const minutes = months * 30 * 24 * 60;

      const value = formatter.toDuration(minutes);

      expect(value).toEqual(`${months.toFixed(2)} mo(s)`);
    });

    test("should calculate days", () => {
      const days = 1.25;
      const minutes = days * 24 * 60;

      const value = formatter.toDuration(minutes);

      expect(value).toEqual(`${days.toFixed(2)} day(s)`);
    });

    test("should calculate hours", () => {
      const hours = 1;
      const minutes = hours * 60;

      const value = formatter.toDuration(minutes);

      expect(value).toEqual(`${hours.toFixed(2)} hr(s)`);
    });

    test("should calculate minutes", () => {
      const minutes = 59;

      const value = formatter.toDuration(minutes);

      expect(value).toEqual(`${minutes.toFixed(2)} min(s)`);
    });
  });

  describe("toDistance", () => {
    test("should calculate kilometers", () => {
      const kilometers = 3.33;
      const meters = kilometers * 1000;

      const value = formatter.toDistance(meters);

      expect(value).toEqual(`${kilometers.toFixed(2)} km`);
    });

    test("should calculate meters", () => {
      const meters = 432.00;

      const value = formatter.toDistance(meters);

      expect(value).toEqual(`${meters.toFixed(2)} m`);
    });
  });

  describe("toIsoDate", () => {
    test("should return correctly", () => {
      const date = '22/July/2017 GMT+10';
      const expected = '2017-07-21'

      const value = formatter.toIsoDate(date);

      expect(value).toEqual(expected);
    });
  });
});