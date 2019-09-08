import React, { Component } from 'react';
import PropTypes from 'prop-types';

import {
  ListPanelWrapper,
  FieldWrapper,
  List,
  ListItem,
  SelectedIcon,
  Label,
  SubLabel
} from '@styles/controls';

import config from '@config';

import formatter from '@src/formatter';
import distance from '@src/distanceCalculator';
import location from '@src/location';

import MoreButton from './moreButtonComponent';
import ThresholdLabel from './thresholdLabelComponent';

class HelpListComponent extends Component {
  state = {};

  static propTypes = {
    onAction: PropTypes.func,
    items: PropTypes.array,
    nextPage: PropTypes.number,
    selectedItem: PropTypes.object,
    currentLocation: PropTypes.object
  }

  static defaultProps = {
    onAction: (data, action) => {},
    items: null,
    nextPage: null
  }

  constructor(props) {
    super(props);

    this.state = {
      selectedItem: null
    };

    this.onItemSelect = this.onItemSelect.bind(this);
    this.onMoreClick = this.onMoreClick.bind(this);
  }

  onItemSelect = (event) => {
    const item = {
      id: event.currentTarget.dataset.id,
      name: event.currentTarget.dataset.name,
      lat: event.currentTarget.dataset.lat,
      lng: event.currentTarget.dataset.lng,
    };

    this.setState({selectedItem: item});
    this.props.onAction(item, 'next');
  }

  onMoreClick = () => {
    this.props.onAction(this.props.nextPage, 'more');
  }

  componentDidUpdate(prevProps) {
    if(prevProps.selectedItem && this.props.selectedItem && prevProps.selectedItem.id != this.props.selectedItem.id) {
      this.setState({ selectedItem: this.props.selectedItem });
    }
  }

  render() {
    return (
      <ListPanelWrapper>
        {this.props.items && (
          <List>
            {this.props.items.map(i => {
              const itemLocation = {
                lat: i.location.latitude,
                lng: i.location.longitude
              };
              const dist = distance.calculate(this.props.currentLocation || location.SYDNEY_OPERA_HOUSE, itemLocation);

              return (
                <ListItem
                  key={i.id}
                  data-id={i.id}
                  data-name={i.name}
                  data-lat={i.location.latitude}
                  data-lng={i.location.longitude}
                  onClick={this.onItemSelect}
                >
                  <FieldWrapper>
                    <SelectedIcon selected={this.state.selectedItem && i.id == this.state.selectedItem.id} />
                    <Label title={i.name}>{i.name}</Label>
                  </FieldWrapper>

                  <FieldWrapper>
                    <SubLabel>
                      <ThresholdLabel
                        value={i.waitingTime}
                        minValue={config.VALUE_THRESHOLDS.WAIT.MIN}
                        maxValue={config.VALUE_THRESHOLDS.WAIT.MAX}>
                        Waiting Time: <b>{formatter.toDuration(i.waitingTime)}</b>
                      </ThresholdLabel>

                      <ThresholdLabel
                        value={dist}
                        minValue={config.VALUE_THRESHOLDS.DISTANCE.MIN}
                        maxValue={config.VALUE_THRESHOLDS.DISTANCE.MAX}>
                        Distance: <b>{formatter.toDistance(dist)}</b>
                      </ThresholdLabel>
                    </SubLabel>
                  </FieldWrapper>

                </ListItem>)}
              )}
            </List>)}

        {this.props.nextPage && (
          <MoreButton title="Load more" onClick={this.onMoreClick} />
        )}

      </ListPanelWrapper>
    );
  }
}

export default HelpListComponent;