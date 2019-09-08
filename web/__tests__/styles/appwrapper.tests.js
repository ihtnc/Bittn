import React from 'react';
import renderer from 'react-test-renderer';

import AppWrapper from "@styles/appwrapper";

const shallow = (comp) => renderer.create(comp).toJSON();

describe("AppWrapper styled component", () => {
  test("should match latest snapshot", () => {
    const wrapper = shallow(<AppWrapper />)
    expect(wrapper).toMatchSnapshot();
  });
});