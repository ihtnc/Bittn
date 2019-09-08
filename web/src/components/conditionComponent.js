import React, { Component } from 'react';
import PropTypes from 'prop-types';

import ApiClient from '@api/apiClient';

import {
  Wrapper,
  FieldWrapper,
  Header,
  Label,
  ErrorLabel,
  Input,
  List,
  ListItem,
  SelectedIcon,
  ErrorIcon
} from '@styles/controls';

import NextButton from './nextButtonComponent';
import MoreButton from './moreButtonComponent';

class ConditionComponent extends Component {
  state = {};

  static propTypes = {
    onAction: PropTypes.func
  }

  static defaultProps = {
    onAction: (data, action) => {}
  }

  constructor(props) {
    super(props);

    this.state = {
      busy: false,
      error: false,
      errorMessage: '',
      search: '',
      selectedCondition: null,
      conditions: null,
      nextPage: null,
      currentPage: null
    };

    this.onSearchChange = this.onSearchChange.bind(this);
    this.onConditionSelect = this.onConditionSelect.bind(this);
    this.onConditionHover = this.onConditionHover.bind(this);
    this.onNextClick = this.onNextClick.bind(this);
    this.onMoreClick = this.onMoreClick.bind(this);
  }

  onSearchChange = (event) => {
    this.setState({
      search: event.target.value,
      currentPage: null
    });
    this.searchConditions(event.target.value);
  }

  onConditionSelect = (event) => {
    const condition = {
      id: event.target.dataset.id,
      name: event.target.dataset.name
    };

    this.setState({selectedCondition: condition});
    this.props.onAction(condition, 'next');
  }

  onConditionHover = (event) => {
    const condition = {
      id: event.target.dataset.id,
      name: event.target.dataset.name
    };

    this.setState({selectedCondition: condition});
  }

  onNextClick = () => {
    this.props.onAction(this.state.selectedCondition, 'next');
  }

  onMoreClick = () => {
    this.setState({currentPage: this.state.nextPage});
    this.searchConditions(this.state.search, this.state.nextPage, true);
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
      errorMessage: message,
      conditions: null,
      nextPage: null
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
    return this.state.busy || !this.state.selectedCondition;
  }

  searchConditions(search, page, append) {
    this.showBusy();

    ApiClient
      .getConditions(search, page)
      .then(res => {
        let list = res.data.data;
        if(append) { list = this.state.conditions.concat(list); }

        this.setState({
          conditions: list,
          selectedCondition: res.data.data.length > 0 ? res.data.data[0] : null,
          nextPage: res.data.navigation.nextPageIndex
        });

        this.showReady();
      })
      .catch(error => {
        this.setState({ conditions: null });
        this.showError(error.message);
      });
  }

  componentDidMount() {
    if(!this.state.conditions) {
      this.searchConditions();
    }
  }

  render() {
    return (
      <Wrapper>
        <Header>You think you've been bitten?</Header>

        <Label>What happened?</Label>

        <FieldWrapper>
          <Input value={this.state.search} onChange={this.onSearchChange} />

          <NextButton
            title="Next"
            onClick={this.onNextClick}
            disabled={this.checkDisabled()} />
        </FieldWrapper>

        {this.state.conditions && (
          <List>
            {this.state.conditions.map(c =>
              <ListItem
                key={c.id}
                data-id={c.id}
                data-name={c.name}
                onClick={this.onConditionSelect}
                onMouseOver={this.onConditionHover}>
                <SelectedIcon selected={c.id == this.state.selectedCondition.id} />
                {c.name}
              </ListItem>)}
          </List>)}

        {this.state.nextPage && (
          <MoreButton
            title="Load more"
            onClick={this.onMoreClick}
            disabled={this.state.busy}
          />
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

export default ConditionComponent;