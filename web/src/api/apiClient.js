import API from './api';

const getConditions = function (name, page) {
  return API.init().get(`conditions`, {
    params: { name, page }
  });
};

const findHelp = function (conditionId, severity, page) {
  const request = {
    conditionId,
    severity,
    page,
    sortField: 'waitingTime'
  };

  return API.init().post(`findHelp`, request);
};

const bookPatient = function (conditionId, conditionName, severity, helpId, helpName, patientName) {
  const request = {
    conditionId,
    conditionName,
    severity,
    helpId,
    helpName,
    patientName
  };

  return API.init().post(`bookings`, request);
};

const getBookings = function (page) {
  return API.init().get(`bookings`, {
    params: { page, sortField: 'createDate', sortDescending: true }
  });
};

export default {
  getConditions,
  findHelp,
  bookPatient,
  getBookings
};