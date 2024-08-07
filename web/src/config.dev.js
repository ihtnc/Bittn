export default {
  GOOGLE_MAP_API_KEY: 'CHANGE_TO_YOUR_GOOGLE_MAP_API_KEY',
  API_URL: 'https://localhost:8179/api/',
  VALUE_THRESHOLDS: {
    QUEUE: {
      MIN: 8,
      MAX: 12
    },
    PROCESS: {
      MIN: 40,
      MAX: 80
    },
    WAIT: {
      MIN: 400,
      MAX: 750
    },
    DISTANCE: {
      MIN: 5000,
      MAX: 10000
    }
  }
};