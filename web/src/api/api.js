import axios from 'axios';

import config from '@config';

const init = () => {
  const headers = { 'X-Requested-With': 'XMLHttpRequest' };

  return axios.create({
    baseURL: config.API_URL,
    headers
  });
};

export default {
  init
};