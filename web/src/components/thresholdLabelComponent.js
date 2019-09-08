import React, { Component } from 'react';

import { LevelLabel } from '@styles/controls';

class ThresholdLabelComponent extends Component {
  constructor(props) {
    super(props);
  }

  getLevelValue = (value, minThresholdValue, maxThresholdValue) => {
    if(value > maxThresholdValue) {
      return 1;
    } else if(value >= minThresholdValue && value <= maxThresholdValue) {
      return 0;
    } else {
      return -1;
    }
  };

  render() {
    return (
      <LevelLabel
        level={this.getLevelValue(this.props.value, this.props.minValue, this.props.maxValue)}
        {...this.props}>
        {this.props.children}
      </LevelLabel>
    );
  }
}

export default ThresholdLabelComponent;