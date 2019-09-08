import React, { Component } from 'react';
import PropTypes from 'prop-types';

import ApiClient from '@api/apiClient';

import {
  Wrapper,
  FieldWrapper,
  Header,
  Label,
  ErrorLabel,
  Table,
  Row,
  Column,
  ErrorIcon
} from '@styles/controls';

import formatter from '@src/formatter';

import SeverityButton from './severityButtonComponent';
import MoreButton from './moreButtonComponent';
import NewButton from './newButtonComponent';

class BookingsComponent extends Component {
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
      busy: true,
      error: false,
      errorMessage: '',
      bookings: null,
      nextPage: null,
      currentPage: null
    };

    this.onNewClick = this.onNewClick.bind(this);
    this.onMoreClick = this.onMoreClick.bind(this);
  }

  onNewClick = () => {
    this.props.onAction(null, 'next');
  }

  onMoreClick = () => {
    this.setState({currentPage: this.state.nextPage});
    this.getBookings(this.state.nextPage, true);
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
    return this.state.busy;
  }

  getBookings(page, append) {
    this.showBusy();

    ApiClient
      .getBookings(page)
      .then(res => {
        let list = res.data.data;
        if(append) { list = this.state.bookings.concat(list); }

        this.setState({
          bookings: list,
          nextPage: res.data.navigation.nextPageIndex
        });

        this.showReady();
      })
      .catch(error => {
        this.setState({ bookings: null });
        this.showError(error.message);
      });
  }

  componentDidMount() {
    if(!this.state.bookings) {
      this.getBookings();
    }
  }

  render() {
    return (
      <Wrapper>
        <Header>You've booked help!</Header>

        <Label>Here are the others who booked help as well...</Label>

        {!this.state.error && this.state.bookings && (
          <Table>
            <tbody>
            {this.state.bookings.map(b =>
              <Row key={b.id}>
                <Column title={formatter.toIsoDate(b.createDate)}>{formatter.toIsoDate(b.createDate)}</Column>
                <Column title={b.patientName}>{b.patientName}</Column>
                <Column title={b.conditionName}>{b.conditionName}</Column>
                <Column>
                  <SeverityButton severity={b.severityLevel} expandable={false} iconSize={true} />
                </Column>
                <Column title={b.helpName}>{b.helpName}</Column>
              </Row>)}
            </tbody>
          </Table>
        )}

        {!this.state.error && this.state.nextPage && (
          <MoreButton
            title="Load more"
            onClick={this.onMoreClick}
            disabled={this.state.busy}
          />
        )}

        {!this.state.error && <Header>Bitten again?</Header>}

        {!this.state.error && (
          <NewButton
            title="Help!"
            onClick={this.onNewClick}
            disabled={this.checkDisabled()} />
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

export default BookingsComponent;