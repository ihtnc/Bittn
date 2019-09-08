import React, { Component } from 'react';
import PropTypes from 'prop-types';

import ApiClient from '@api/apiClient';

import location from '@src/location';

import {
  Wrapper,
  FieldWrapper,
  PanelWrapper,
  Status,
  Link,
  ErrorIcon,
  ErrorLabel
} from '@styles/controls';

import NextButton from './nextButtonComponent';
import HelpList from './helpListComponent';
import HelpMap from './helpMapComponent';

class FindHelpComponent extends Component {
  state = {};

  static propTypes = {
    onAction: PropTypes.func,
    condition: PropTypes.object,
    severity: PropTypes.object
  }

  static defaultProps = {
    onAction: (data, action) => {},
    condition: null,
    severity: null
  }

  constructor(props) {
    super(props);

    this.state = {
      busy: false,
      error: false,
      errorMessage: '',
      helpItems: null,
      selectedItem: null,
      nextPage: null,
      currentPage: null,
      mapCenter: null
    };

    this.onConditionSelect = this.onConditionSelect.bind(this);
    this.onSeveritySelect = this.onSeveritySelect.bind(this);
    this.onNextClick = this.onNextClick.bind(this);
    this.onHelpAction = this.onHelpAction.bind(this);
  }

  onConditionSelect = (event) => {
    this.props.onAction(null, 'condition');
  }

  onSeveritySelect = (event) => {
    this.props.onAction(null, 'severity');
  }

  onNextClick = (event) => {
    this.props.onAction(this.state.selectedItem, 'next');
  }

  onHelpAction = (data, action) => {
    if(action == 'next') {
      const location = {
        lat: data.lat,
        lng: data.lng
      };

      this.setState({
        selectedItem: data,
        mapCenter: location
      });

    } else if(action == 'more') {
      this.findHelp(this.props.condition.id, this.props.severity.level, data, true);
    }
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
    return this.state.busy || !this.state.selectedItem;
  }

  findHelp(conditionId, severity, page, append) {
    this.showBusy();

    ApiClient
      .findHelp(conditionId, severity, page)
      .then(res => {
        let list = res.data.data;
        if(append) { list = this.state.helpItems.concat(list); }

        this.setState({
          helpItems: list,
          nextPage: res.data.navigation.nextPageIndex
        });

        this.showReady();
      })
      .catch(error => {
        this.setState({ helpItems: null });
        this.showError(error.message);
      });
  }

  componentDidMount() {
    if(!this.state.helpItems) {
      this.findHelp(this.props.condition.id, this.props.severity.level);

      location.getCurrentLocation(pos => {
        this.setState({
          mapCenter: pos,
          currentLocation: pos
        });
      });
    }
  }

  render() {
    return (
      <Wrapper>
        <FieldWrapper>
          <Status>
            You've been bitten, <Link onClick={this.onConditionSelect}>{this.props.condition.name}</Link> happened, and <Link onClick={this.onSeveritySelect}>{this.props.severity.text}</Link>.
          </Status>
        </FieldWrapper>

        {!this.state.selectedItem && (
          <FieldWrapper>
            <Status>
              Get help.
            </Status>

            <NextButton
              title="Next"
              disabled={this.checkDisabled()} />
          </FieldWrapper>
        )}

        {this.state.selectedItem && (
          <FieldWrapper>
            <Status>
              Get help from <b>{this.state.selectedItem.name}</b>.
            </Status>

            <NextButton
              title="Next"
              onClick={this.onNextClick}
              disabled={this.checkDisabled()} />
          </FieldWrapper>
        )}

        {!this.state.error && (
          <PanelWrapper>
            <HelpList
              items={this.state.helpItems}
              nextPage={this.state.nextPage}
              selectedItem={this.state.selectedItem}
              currentLocation={this.state.currentLocation}
              onAction={this.onHelpAction}
            />

            <HelpMap
              center={this.state.mapCenter}
              items={this.state.helpItems}
              onAction={this.onHelpAction}
            />
          </PanelWrapper>
        )}

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

export default FindHelpComponent;