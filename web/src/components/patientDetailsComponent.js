import React, { Component } from 'react';
import PropTypes from 'prop-types';

import ApiClient from '@api/apiClient';

import {
  Wrapper,
  FieldWrapper,
  Label,
  ErrorLabel,
  Status,
  Input,
  Link,
  ErrorIcon
} from '@styles/controls';

import NextButton from './nextButtonComponent';

class PatientDetailsComponent extends Component {
  state = {};

  static propTypes = {
    onAction: PropTypes.func,
    condition: PropTypes.object,
    severity: PropTypes.object,
    help: PropTypes.object
  }

  static defaultProps = {
    onAction: (data, action) => {},
    condition: null,
    severity: null,
    help: null
  }

  constructor(props) {
    super(props);

    this.state = {
      patient: null,
      busy: false,
      error: false,
      errorMessage: '',
      name: ''
    };

    this.onConditionSelect = this.onConditionSelect.bind(this);
    this.onSeveritySelect = this.onSeveritySelect.bind(this);
    this.onHelpSelect = this.onHelpSelect.bind(this);
    this.onNextClick = this.onNextClick.bind(this);
    this.onPatientNameChange = this.onPatientNameChange.bind(this);
  }

  onConditionSelect = (event) => {
    this.props.onAction(null, 'condition');
  }

  onSeveritySelect = (event) => {
    this.props.onAction(null, 'severity');
  }

  onHelpSelect = (event) => {
    this.props.onAction(null, 'help');
  }

  onNextClick = (event) => {
    this.showBusy();

    const condition = this.props.condition;
    const severity = this.props.severity;
    const help = this.props.help;

    ApiClient
      .bookPatient(condition.id, condition.name, severity.level, help.id, help.name, this.state.name)
      .then(res => {
        this.showReady();

        const patient = {
          id: res.data.data.id,
          name: this.state.name
        };

        this.props.onAction(patient, 'next');
      })
      .catch(error => {
        this.showError(error.message);
      });
  }

  onPatientNameChange = (event) => {
    this.setState({ name: event.target.value });
  }

  showBusy() {
    this.setState({
      busy: true,
      error: false,
      errorMessage: ''
    });
  }

  showError(message) {
    this.setState({
      error: true,
      busy: false,
      errorMessage: message
    });
  }

  showReady() {
    this.setState({
      busy: false,
      error: false,
      errorMessage: ''
    });
  }

  checkDisabled() {
    return this.state.busy || !this.state.name || this.state.name.length == 0;
  }

  render() {
    return (
      <Wrapper>
        <FieldWrapper>
          <Status>
            You've been bitten, <Link onClick={this.onConditionSelect}>{this.props.condition.name}</Link> happened, and <Link onClick={this.onSeveritySelect}>{this.props.severity.text}</Link>.
          </Status>
        </FieldWrapper>

        <FieldWrapper>
          <Status>
            You're getting help from <Link onClick={this.onHelpSelect}>{this.props.help.name}</Link>.
          </Status>
        </FieldWrapper>

        <Label>What's your name?</Label>

        <FieldWrapper>
          <Input value={this.state.name} onChange={this.onPatientNameChange} />

          <NextButton
            title="Next"
            onClick={this.onNextClick}
            disabled={this.checkDisabled()} />
        </FieldWrapper>

        {this.state.error && (
          <FieldWrapper>
            <ErrorIcon />
            <ErrorLabel>{this.state.errorMessage}</ErrorLabel>
          </FieldWrapper>
        )}

      </Wrapper>
    );
  }
}

export default PatientDetailsComponent;