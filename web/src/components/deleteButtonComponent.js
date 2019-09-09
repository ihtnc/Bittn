import React, { Component } from 'react';
import PropTypes from 'prop-types';

import icons from '@src/icons';

import ExpandableButton from './expandableButtonComponent';
import ExpandableIconButton from './expandableIconButtonComponent';

class DeleteButtonComponent extends Component {
  static propTypes = {
    iconSize: PropTypes.bool
  }

  static defaultProps = {
    iconSize: false
  }

  constructor(props) {
    super(props);
  }

  getButtonIcon = (disabled) => {
    return disabled ? icons.DISABLED_DELETE : icons.DELETE;
  };

  render() {

    if(this.props.iconSize) {
      return (
        <ExpandableIconButton
          icon={this.getButtonIcon(this.props.disabled)}
          {...this.props} />
      );
    }

    return (
      <ExpandableButton
        icon={this.getButtonIcon(this.props.disabled)}
        {...this.props} />
    );
  }
}

export default DeleteButtonComponent;