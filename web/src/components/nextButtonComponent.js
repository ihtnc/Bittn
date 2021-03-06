import React, { Component } from 'react';

import icons from '@src/icons';

import ExpandableButton from './expandableButtonComponent';

class NextButtonComponent extends Component {
  constructor(props) {
    super(props);
  }

  getButtonIcon = (disabled) => {
    return disabled ? icons.DISABLED_NEXT : icons.NEXT;
  };

  render() {
    return (
      <ExpandableButton
        icon={this.getButtonIcon(this.props.disabled)}
        {...this.props} />
    );
  }
}

export default NextButtonComponent;