import React, { Component } from 'react';

import icons from '@src/icons';

import ExpandableIconButton from './expandableIconButtonComponent';

class SelectIconButtonComponent extends Component {
  constructor(props) {
    super(props);
  }

  getButtonIcon = () => {
    return icons.SELECTED;
  };

  render() {
    return (
      <ExpandableIconButton
        icon={this.getButtonIcon()}
        {...this.props} />
    );
  }
}

export default SelectIconButtonComponent;