import React, { Component } from 'react';
import PropTypes from 'prop-types';

import {
  Button,
  ExpandableButtonWrapper
} from '@styles/controls';

class ExpandableButtonComponent extends Component {
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
        <Button
          {...this.props} />
      </ExpandableButtonWrapper>
    );
  }
}

export default ExpandableButtonComponent;