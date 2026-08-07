import axios from 'axios'

// The two backend processes shown in the API docs run on different ports
// (Notes API on 7057, Auth API on 5096 in some setups). Both are configurable
// via .env so this points at whatever your ASP.NET Core services are actually
// listening on. See .env.example.
export const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? 'https://localhost:7057/api'
export const AUTH_BASE_URL = import.meta.env.VITE_AUTH_BASE_URL ?? 'https://localhost:7057/api'

export const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: { 'Content-Type': 'application/json' },
})

export const authClient = axios.create({
  baseURL: AUTH_BASE_URL,
  headers: { 'Content-Type': 'application/json' },
})

const TOKEN_KEY = 'notes_app_token'

export function getToken(): string | null {
  return localStorage.getItem(TOKEN_KEY)
}

export function setToken(token: string | null): void {
  if (token) localStorage.setItem(TOKEN_KEY, token)
  else localStorage.removeItem(TOKEN_KEY)
}

apiClient.interceptors.request.use((config) => {
  const token = getToken()
  if (token) config.headers.Authorization = `Bearer ${token}`
  return config
})

// If the API ever tells us the session is no longer valid, clear the stale
// token and send the user back to the login page.
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error?.response?.status === 401) {
      setToken(null)
      if (window.location.pathname !== '/login') {
        window.location.href = '/login'
      }
    }
    return Promise.reject(error)
  },
)
