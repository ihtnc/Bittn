import React, { Component } from 'react';

import AppWrapper from '@styles/appwrapper';

import Condition from './conditionComponent';
import Severity from './severityComponent';
import FindHelp from './findHelpComponent';
import PatientDetails from './patientDetailsComponent';
import Bookings from './bookingsComponent';

class AppComponent extends Component {
  state = {};

  constructor(props) {
    super(props);

    this.state = {
      selectedCondition: null,
      selectedSeverity: null,
      selectedHelp: null,
      selectedName: null
    };

    this.onConditionAction = this.onConditionAction.bind(this);
    this.onSeverityAction = this.onSeverityAction.bind(this);
    this.onFindHelpAction = this.onFindHelpAction.bind(this);
    this.onPatientAction = this.onPatientAction.bind(this);
  }

  onConditionAction = (data, action) => {
    if(action == 'next') {
      this.setState({
        selectedCondition: data,
        selectedSeverity: null,
        selectedHelp: null,
        selectedPatient: null
      });
    }
  }

  onSeverityAction = (data, action) => {
    if(action == 'next') {
      this.setState({
        selectedSeverity: data,
        selectedHelp: null,
        selectedPatient: null
    });
    } else if(action == 'condition') {
      this.setState({
        selectedCondition: null,
        selectedSeverity: null,
        selectedHelp: null,
        selectedPatient: null
      });
    }
  }

  onFindHelpAction = (data, action) => {
    if(action == 'next') {
      this.setState({
        selectedHelp: data,
        selectedPatient: null
      });
    } else if(action == 'condition') {
      this.setState({
        selectedCondition: null,
        selectedSeverity: null,
        selectedHelp: null,
        selectedPatient: null
      });
    } else if(action == 'severity') {
      this.setState({
        selectedSeverity: null,
        selectedHelp: null,
        selectedPatient: null
      });
    }
  }

  onPatientAction = (data, action) => {
    if(action == 'next') {
      this.setState({
        selectedPatient: data
      });
    } else if(action == 'condition') {
      this.setState({
        selectedCondition: null,
        selectedSeverity: null,
        selectedHelp: null,
        selectedPatient: null
      });
    } else if(action == 'severity') {
      this.setState({
        selectedSeverity: null,
        selectedHelp: null,
        selectedPatient: null
      });
    } else if(action == 'help') {
      this.setState({
        selectedHelp: null,
        selectedPatient: null
      });
    }
  }

  onBookingAction = (data, action) => {
    if(action == 'next') {
      this.setState({
        selectedCondition: null,
        selectedSeverity: null,
        selectedHelp: null,
        selectedPatient: null
      });
    }
  }

  showCondition() {
    return !this.state.selectedCondition;
  }

  showSeverity() {
    return this.state.selectedCondition && !this.state.selectedSeverity;
  }

  showFindHelp() {
    return this.state.selectedCondition && this.state.selectedSeverity && !this.state.selectedHelp;
  }

  showPatientDetails() {
    return this.state.selectedCondition && this.state.selectedSeverity && this.state.selectedHelp && !this.state.selectedPatient;
  }

  showBooked() {
    return this.state.selectedCondition && this.state.selectedSeverity && this.state.selectedHelp && this.state.selectedPatient;
  }

  render() {
    return (
      <AppWrapper>
        {this.showCondition() && (
          <Condition
            onAction={this.onConditionAction}
          />
        )}

        {this.showSeverity() && (
          <Severity
            condition={this.state.selectedCondition}
            onAction={this.onSeverityAction}
          />
        )}

        {this.showFindHelp() && (
          <FindHelp
            condition={this.state.selectedCondition}
            severity={this.state.selectedSeverity}
            onAction={this.onFindHelpAction}/>
        )}

        {this.showPatientDetails() && (
          <PatientDetails
            condition={this.state.selectedCondition}
            severity={this.state.selectedSeverity}
            help={this.state.selectedHelp}
            onAction={this.onPatientAction}/>
        )}

        {this.showBooked() && (
          <Bookings
            bookingId={1 || this.state.selectedPatient.id}
            onAction={this.onBookingAction}/>
        )}
      </AppWrapper>
    );
  }
}

export default AppComponent;