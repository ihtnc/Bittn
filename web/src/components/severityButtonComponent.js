import React, { Component } from 'react';
import PropTypes from 'prop-types';

import icons from '@src/icons';

import ExpandableButton from './expandableButtonComponent';
import ExpandableIconButton from './expandableIconButtonComponent';

class SeverityButtonComponent extends Component {
  static propTypes = {
    severity: PropTypes.number,
    iconSize: PropTypes.bool
  }

  static defaultProps = {
    severity: 0,
    iconSize: false
  }

  constructor(props) {
    super(props);
  }

  getButtonIcon = (severity) => {
    switch(severity) {
      case 0:
        return icons.SEV1;
      case 1:
        return icons.SEV2;
      case 2:
        return icons.SEV3;
      case 3:
        return icons.SEV4;
      case 4:
        return icons.SEV5;
    }
  }

  getTitle = (severity) => {
    switch(severity) {
      case 0:
        return 'Not bad';
      case 1:
        return 'Ok, I guess';
      case 2:
        return 'Manageable';
      case 3:
        return 'Really bad';
      case 4:
        return 'I\'m dying';
    }
  }

  getText = (severity) => {
    switch(severity) {
      case 0:
        return 'it\'s not bad';
      case 1:
        return 'it seems ok';
      case 2:
        return 'you can manage';
      case 3:
        return 'it\'s really bad';
      case 4:
        return 'you\'re dying';
    }
  }

  render() {
    if(this.props.iconSize) {
      return (
        <ExpandableIconButton
          icon={this.getButtonIcon(this.props.severity)}
          title={this.getTitle(this.props.severity)}
          data-severity={this.props.severity}
          data-text={this.getText(this.props.severity)}
          {...this.props} />
      );
    }

    return (
      <ExpandableButton
        icon={this.getButtonIcon(this.props.severity)}
        title={this.getTitle(this.props.severity)}
        data-severity={this.props.severity}
        data-text={this.getText(this.props.severity)}
        {...this.props} />
    );
  }
}

export default SeverityButtonComponent;