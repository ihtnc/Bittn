import React from 'react';
import renderer from 'react-test-renderer';

import App from "@components/app"

const shallow = (comp) => renderer.create(comp).toJSON();

describe("App component", () => {
  test("should match latest snapshot", () => {
    const wrapper = shallow(<App />)
    expect(wrapper).toMatchSnapshot();
  });
});