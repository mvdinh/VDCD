import axios from 'axios';

// Base API instance
const api = axios.create({
  baseURL: 'https://localhost:7231/api', // Adjust if needed based on launchSettings.json
  headers: {
    'Content-Type': 'application/json'
  }
});

export const productApi = {
  getAll: () => api.get('/Products'),
  getById: (id) => api.get(`/Products/${id}`),
  create: (data) => api.post('/Products', data),
  update: (id, data) => api.put(`/Products/${id}`, data),
  delete: (id) => api.delete(`/Products/${id}`)
};

export const categoryApi = {
  getAll: () => api.get('/Categories'),
  getById: (id) => api.get(`/Categories/${id}`)
};

export const unitApi = {
  getAll: () => api.get('/Units'),
  getById: (id) => api.get(`/Units/${id}`)
};

export const saleApi = {
  getAll: () => api.get('/Sales'),
  getById: (id) => api.get(`/Sales/${id}`),
  create: (data) => api.post('/Sales', data)
};

export const statisticsApi = {
  getCategorySales: (params) => api.get('/Statistics/category-sales', { params }),
  getProductRevenue: (productId, params) => api.get(`/Statistics/product-revenue/${productId}`, { params })
};

export default api;
