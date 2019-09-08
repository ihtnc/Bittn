import React, { PureComponent } from 'react';
import PropTypes from 'prop-types';
import { Map, Marker, GoogleApiWrapper } from 'google-maps-react';

import config from '@config';
import icons from '@src/icons';
import location from '@src/location';

import HelpDetails from './helpDetailsComponent';
import InterActiveInfoWindow from './interactiveInfoWindow';

class HelpMapComponent extends PureComponent {
  state = {
    activeMarker: {},
    currentCenter: {}
  };

  static propTypes = {
    onAction: PropTypes.func,
    center: PropTypes.object,
    zoom: PropTypes.number,
    items: PropTypes.array
  }

  static defaultProps = {
    onAction: (data, action) => {},
    zoom: 17,
    items: null,
    center: null
  }

  constructor(props) {
    super(props);

    this.onMarkerClick = this.onMarkerClick.bind(this);
    this.onMapClick = this.onMapClick.bind(this);
    this.onMapRightClick = this.onMapRightClick.bind(this);
    this.onInfoWindowClose = this.onInfoWindowClose.bind(this);
    this.onHelpDetailsAction = this.onHelpDetailsAction.bind(this);

    this.state.currentCenter = this.props.center;
  }

  componentDidUpdate(prevProps) {
    // navigating to another marker, so clear the markers
    if(prevProps.center != this.props.center) {
      this.setState({ activeMarker: null });

      if(this.props.center) { this.setState({ currentCenter: this.props.center }); }

      return;
    }
  }

  onMarkerClick = (props, marker, e) => {
    // reset previous active marker
    if(this.state.activeMarker != null) {
      this.setState({ activeMarker: null });
    }

    // set marker as active
    this.setState({ activeMarker: marker });
  }

  onMapClick = (ref, map, ev) => {
    // reset the previous active marker
    if (this.state.activeMarker != null) {
      this.setState({ activeMarker: null });
    }
  };

  onMapRightClick = (ref, map, ev) => {
    location.getCurrentLocation(pos => {
      this.setState({
        currentCenter: pos,
        currentLocation: pos
      });
    });
  };

  onInfoWindowClose = () => {
    if (this.state.activeMarker != null) {
      this.setState({ activeMarker: null });
    }
  };

  onHelpDetailsAction = (data, action) => {
    if(action=='next') {
      const item = {
        id: data.id,
        name: data.name,
        lat: data.location.latitude,
        lng: data.location.longitude
      };

      this.props.onAction(item, 'next');

      this.setState({ activeMarker: null });
    }
  };

  renderMarkers(list) {
    return list && list.map(i => (
      <Marker
        key={i.id}
        name={i.id}
        title={i.name}
        onClick={this.onMarkerClick}
        position={{ lat: i.location.latitude, lng: i.location.longitude }}
        data={i}
        icon={icons.HOSPITAL}
      />
    ))
  }

  renderCurrentLocation() {
    return (
      <Marker
        name={'You'}
        title={'You'}
        position={this.state.currentLocation}
        icon={icons.SELF}
      />
    );
  }

  render() {
    const list = this.props.items;

    return (
      <Map
        google={this.props.google}
        zoom={this.props.zoom}
        style={{ height: '75vh', minHeight: '500px', position: 'relative', width: '33vw', minWidth: '500px' }}
        onClick={this.onMapClick}
        onRightclick={this.onMapRightClick}
        center={this.state.currentCenter}
      >

        {this.renderMarkers(list)}
        {this.renderCurrentLocation()}

        <InterActiveInfoWindow
          marker={this.state.activeMarker}
          visible={this.state.activeMarker != null}
          onClose={this.onInfoWindowClose}
        >
          {this.state.activeMarker == null || this.state.activeMarker.data == null
            ? (<div>Please select a marker.</div>)
            : (<HelpDetails
                data={this.state.activeMarker.data}
                onAction={this.onHelpDetailsAction}
              />)}
        </InterActiveInfoWindow>

      </Map>
    );
  }
}

export default GoogleApiWrapper({
  apiKey: (config.GOOGLE_MAP_API_KEY)
})(HelpMapComponent);