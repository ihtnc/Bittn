import React, { Component } from 'react';
import PropTypes from 'prop-types';

import {
  MarkerInfoWrapper,
  MarkerInfoHeader,
  MarkerInfoField
} from '@styles/controls';

import formatter from '@src/formatter';

import config from '@config';

import SelectIconButton from './selectIconButtonComponent';
import ThresholdLabel from './thresholdLabelComponent';

class HelpDetailsComponent extends Component {
  state = {};

  static propTypes = {
    data: PropTypes.object,
    onAction: PropTypes.func
  }

  static defaultProps = {
    data: { },
    onAction: (data, action) => {}
  }

  constructor(props) {
    super(props);

    this.onSelectClick = this.onSelectClick.bind(this);
  }

  onSelectClick = () => {
    this.props.onAction(this.props.data, 'next');
  }

  render() {
    return (
      <MarkerInfoWrapper>
        <MarkerInfoHeader>{this.props.data.name}</MarkerInfoHeader>

        <MarkerInfoField>
          <ThresholdLabel
            value={this.props.data.queueLength}
            minValue={config.VALUE_THRESHOLDS.QUEUE.MIN}
            maxValue={config.VALUE_THRESHOLDS.QUEUE.MAX}>
            Queue Length: <b>{this.props.data.queueLength}</b>
          </ThresholdLabel>
        </MarkerInfoField>

        <MarkerInfoField>
          <ThresholdLabel
            value={this.props.data.averageProcessTime}
            minValue={config.VALUE_THRESHOLDS.PROCESS.MIN}
            maxValue={config.VALUE_THRESHOLDS.PROCESS.MAX}>
            Avg Processing Time: <b>{formatter.toDuration(this.props.data.averageProcessTime)}</b>
          </ThresholdLabel>
        </MarkerInfoField>

        <MarkerInfoField>
          <ThresholdLabel
            value={this.props.data.waitingTime}
            minValue={config.VALUE_THRESHOLDS.WAIT.MIN}
            maxValue={config.VALUE_THRESHOLDS.WAIT.MAX}>
            Waiting Time: <b>{formatter.toDuration(this.props.data.waitingTime)}</b>
          </ThresholdLabel>
        </MarkerInfoField>

        <SelectIconButton title="Select" onClick={this.onSelectClick} />
      </MarkerInfoWrapper>
    );
  }
}

export default HelpDetailsComponent;