import React, { Component } from 'react';
import PropTypes from 'prop-types';

import {
  Wrapper,
  FieldWrapper,
  Label,
  Status,
  Link
} from '@styles/controls';

import SeverityButton from './severityButtonComponent';

class SeverityComponent extends Component {
  state = {};

  static propTypes = {
    onAction: PropTypes.func,
    condition: PropTypes.object
  }

  static defaultProps = {
    onAction: (data, action) => {},
    condition: null
  }

  constructor(props) {
    super(props);

    this.state = {
      selectedSeverity: null
    };

    this.onConditionSelect = this.onConditionSelect.bind(this);
    this.onSeveritySelect = this.onSeveritySelect.bind(this);
  }

  onConditionSelect = (event) => {
    this.props.onAction(null, 'condition');
  }

  onSeveritySelect = (event) => {
    const severity = {
      level: event.target.dataset.severity,
      text: event.target.dataset.text
    };

    this.setState({selectedSeverity: severity});
    this.props.onAction(severity, 'next');
  }

  render() {
    return (
      <Wrapper>
        <FieldWrapper>
          <Status>
            You've been bitten, and <Link onClick={this.onConditionSelect}>{this.props.condition.name}</Link> happened?
          </Status>
        </FieldWrapper>

        <Label>How bad is it?</Label>

        <FieldWrapper>
          <SeverityButton
            severity={0}
            onClick={this.onSeveritySelect} />
          <SeverityButton
            severity={1}
            onClick={this.onSeveritySelect} />
          <SeverityButton
            severity={2}
            onClick={this.onSeveritySelect} />
          <SeverityButton
            severity={3}
            onClick={this.onSeveritySelect} />
          <SeverityButton
            severity={4}
            onClick={this.onSeveritySelect} />
        </FieldWrapper>

      </Wrapper>
    );
  }
}

export default SeverityComponent;