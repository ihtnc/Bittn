import React, { Component } from 'react';
import PropTypes from 'prop-types';

import {
  IconButton,
  ExpandableButtonWrapper
} from '@styles/controls';

class ExpandableIconButtonComponent extends Component {
  static propTypes = {
    expandable: PropTypes.bool
  }

  static defaultProps = {
    expandable: true
  }

  constructor(props) {
    super(props);
  }

  render() {
    return (
      <ExpandableButtonWrapper disabled={!this.props.expandable}>
        <IconButton
          {...this.props} />
      </ExpandableButtonWrapper>
    );
  }
}

export default ExpandableIconButtonComponent;