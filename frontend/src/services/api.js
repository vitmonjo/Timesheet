import axios from 'axios';

const API_URL = 'https://localhost:7237/api';

// Create axios instance with your API base URL
const api = axios.create({
    baseURL: API_URL,
    headers: {
        'Content-Type': 'application/json'
    }
});

// Add request interceptor to attach token to all requests
api.interceptors.request.use(config => {
    const token = localStorage.getItem('authToken');
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

// Add response interceptor to handle errors
api.interceptors.response.use(
    response => response,
    error => {
        // Handle 401 Unauthorized errors (expired token)
        if (error.response && error.response.status === 401) {
            localStorage.removeItem('authToken');
            // Redirect to login
            window.location.href = '/login';
        }
        return Promise.reject(error);
    }
);

export default api;