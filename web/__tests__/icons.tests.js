import icons from "@src/icons";

describe("Icons module", () => {
  test("should define SELF", () => {
    expect(icons.SELF).toEqual("https://img.icons8.com/dusk/32/000000/street-view.png");
  });

  test("should define HOSPITAL", () => {
    expect(icons.HOSPITAL).toEqual("https://img.icons8.com/dusk/32/000000/hospital-2.png");
  });

  test("should define NEXT", () => {
    expect(icons.NEXT).toEqual("https://img.icons8.com/dusk/64/000000/circled-right-2.png");
  });

  test("should define DISABLED_NEXT", () => {
    expect(icons.DISABLED_NEXT).toEqual("data:image/png;base64,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");
  });

  test("should define MORE", () => {
    expect(icons.MORE).toEqual("https://img.icons8.com/dusk/64/000000/circled-chevron-down.png");
  });

  test("should define DISABLED_MORE", () => {
    expect(icons.DISABLED_MORE).toEqual("data:image/png;base64,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");
  });


  test("should define NEW", () => {
    expect(icons.NEW).toEqual("https://img.icons8.com/dusk/50/000000/hypertension.png");
  });

  test("should define SELECTED", () => {
    expect(icons.SELECTED).toEqual("https://img.icons8.com/dusk/64/000000/ok.png");
  });

  test("should define ERROR", () => {
    expect(icons.ERROR).toEqual("https://img.icons8.com/dusk/64/000000/delete-sign.png");
  });

  test("should define SEV1", () => {
    expect(icons.SEV1).toEqual("https://img.icons8.com/dusk/64/000000/happy.png");
  });

  test("should define SEV2", () => {
    expect(icons.SEV2).toEqual("https://img.icons8.com/dusk/64/000000/neutral-emoticon.png");
  });

  test("should define SEV3", () => {
    expect(icons.SEV3).toEqual("https://img.icons8.com/dusk/64/000000/question.png");
  });

  test("should define SEV4", () => {
    expect(icons.SEV4).toEqual("https://img.icons8.com/dusk/64/000000/sad.png");
  });

  test("should define SEV5", () => {
    expect(icons.SEV5).toEqual("https://img.icons8.com/dusk/64/000000/skull.png");
  });
});