import React from 'react';
import renderer from 'react-test-renderer';

import {
  Wrapper,
  FieldWrapper,
  PanelWrapper,
  ListPanelWrapper,
  MarkerInfoWrapper,
  Header,
  MarkerInfoHeader,
  MarkerInfoField,
  ExpandableButtonWrapper,
  Status,
  Label,
  ErrorLabel,
  SubLabel,
  LevelLabel,
  Input,
  List,
  ListItem,
  Link,
  Button,
  SelectedIcon,
  ErrorIcon,
  IconButton,
  Table,
  Row,
  Column
} from "@styles/controls";

const shallow = (comp) => renderer.create(comp).toJSON();

describe("Controls styled components", () => {
  test("Wrapper should match latest snapshot", () => {
    const wrapper = shallow(<Wrapper />)
    expect(wrapper).toMatchSnapshot();
  });

  test("FieldWrapper should match latest snapshot", () => {
    const wrapper = shallow(<FieldWrapper />)
    expect(wrapper).toMatchSnapshot();
  });

  test("MarkerInfoWrapper should match latest snapshot", () => {
    const wrapper = shallow(<MarkerInfoWrapper />)
    expect(wrapper).toMatchSnapshot();
  });

  describe("ExpandableButtonWrapper", () => {
    test("should render correctly when disabled", () => {
      const wrapper = shallow(<ExpandableButtonWrapper disabled={true} />);
      expect(wrapper).toMatchSnapshot();
    });

    test("should render correctly when not disabled", () => {
      const wrapper = shallow(<ExpandableButtonWrapper />);
      expect(wrapper).toMatchSnapshot();
    });
  });

  test("Header should match latest snapshot", () => {
    const wrapper = shallow(<Header />)
    expect(wrapper).toMatchSnapshot();
  });

  test("MarkerInfoHeader should match latest snapshot", () => {
    const wrapper = shallow(<MarkerInfoHeader />)
    expect(wrapper).toMatchSnapshot();
  });

  test("MarkerInfoField should match latest snapshot", () => {
    const wrapper = shallow(<MarkerInfoField />)
    expect(wrapper).toMatchSnapshot();
  });

  test("Status should match latest snapshot", () => {
    const wrapper = shallow(<Status />)
    expect(wrapper).toMatchSnapshot();
  });

  test("Label should match latest snapshot", () => {
    const wrapper = shallow(<Label />)
    expect(wrapper).toMatchSnapshot();
  });

  test("ErrorLabel should match latest snapshot", () => {
    const wrapper = shallow(<ErrorLabel />)
    expect(wrapper).toMatchSnapshot();
  });

  test("SubLabel should match latest snapshot", () => {
    const wrapper = shallow(<SubLabel />)
    expect(wrapper).toMatchSnapshot();
  });

  test("Input should match latest snapshot", () => {
    const wrapper = shallow(<Input />)
    expect(wrapper).toMatchSnapshot();
  });

  test("Link should match latest snapshot", () => {
    const size = '18px';
    const wrapper = shallow(<Link />)
    expect(wrapper).toMatchSnapshot();
  });

  describe("Button", () => {
    test("should render correctly when disabled", () => {
      const icon = "background-image";
      const wrapper = shallow(<Button disabled={true} icon={icon} />);
      expect(wrapper).toMatchSnapshot();
    });

    test("should render correctly when not disabled", () => {
      const icon = "background-image"
      const wrapper = shallow(<Button icon={icon} />);
      expect(wrapper).toMatchSnapshot();
    });
  });

  describe("SelectedIcon", () => {
    test("should render correctly when selected", () => {
      const wrapper = shallow(<SelectedIcon selected={true} />);
      expect(wrapper).toMatchSnapshot();
    });

    test("should render correctly when not selected", () => {
      const wrapper = shallow(<SelectedIcon />);
      expect(wrapper).toMatchSnapshot();
    });
  });

  test("ErrorIcon should match latest snapshot", () => {
    const wrapper = shallow(<ErrorIcon />)
    expect(wrapper).toMatchSnapshot();
  });


  test("IconButton should match latest snapshot", () => {
    const icon = 'background-image';
    const wrapper = shallow(<IconButton icon={icon} />)
    expect(wrapper).toMatchSnapshot();
  });

  test("List should match latest snapshot", () => {
    const wrapper = shallow(<List />)
    expect(wrapper).toMatchSnapshot();
  });

  test("ListItem should match latest snapshot", () => {
    const wrapper = shallow(<ListItem />)
    expect(wrapper).toMatchSnapshot();
  });

  test("ListPanelWrapper should match latest snapshot", () => {
    const wrapper = shallow(<ListPanelWrapper />)
    expect(wrapper).toMatchSnapshot();
  });

  describe("LevelLabel", () => {
    test("should render correctly for above level", () => {
      const wrapper = shallow(<LevelLabel level={1} />);
      expect(wrapper).toMatchSnapshot();
    });

    test("should render correctly for same level", () => {
      const wrapper = shallow(<LevelLabel level={0} />);
      expect(wrapper).toMatchSnapshot();
    });

    test("should render correctly for below level", () => {
      const wrapper = shallow(<LevelLabel level={-1} />);
      expect(wrapper).toMatchSnapshot();
    });

    test("should render correctly for default level", () => {
      const wrapper = shallow(<LevelLabel />);
      expect(wrapper).toMatchSnapshot();
    });
  });

  test("PanelWrapper should match latest snapshot", () => {
    const wrapper = shallow(<PanelWrapper />)
    expect(wrapper).toMatchSnapshot();
  });

  test("Table should match latest snapshot", () => {
    const wrapper = shallow(<Table />)
    expect(wrapper).toMatchSnapshot();
  });

  test("Row should match latest snapshot", () => {
    const wrapper = shallow(<Row />)
    expect(wrapper).toMatchSnapshot();
  });

  test("Column should match latest snapshot", () => {
    const wrapper = shallow(<Column />)
    expect(wrapper).toMatchSnapshot();
  });
});