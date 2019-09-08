import React, { Component } from 'react';

import icons from '@src/icons';

import ExpandableButton from './expandableButtonComponent';

class NewButtonComponent extends Component {
  constructor(props) {
    super(props);
  }

  getButtonIcon = () => {
    return icons.NEW;
  };

  render() {
    return (
      <ExpandableButton
        icon={this.getButtonIcon()}
        {...this.props} />
    );
  }
}

export default NewButtonComponent;