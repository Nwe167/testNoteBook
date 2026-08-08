import { defineStore } from 'pinia'
import { authApi } from '@/api/authApi'
import { getToken, setToken } from '@/api/axios'
import type { AuthUser, LoginRequest, RegisterRequest } from '@/types/auth'

import { authClient } from '@/api/axios'
const USER_KEY = 'notes_app_user'

interface AuthState {
  user: AuthUser | null
  token: string | null
  loading: boolean
  error: string | null
}

function loadStoredUser(): AuthUser | null {
  const raw = localStorage.getItem(USER_KEY)
  if (!raw) return null
  try {
    return JSON.parse(raw) as AuthUser
  } catch {
    return null
  }
}

export const useAuthStore = defineStore('auth', {
  state: (): AuthState => ({
    user: loadStoredUser(),
    token: getToken(),
    loading: false,
    error: null,
  }),

  getters: {
    isAuthenticated: (state) => Boolean(state.token),
  },

  actions: {
    async login(payload: LoginRequest) {
      this.loading = true
      this.error = null
      try {
        const res = await authApi.login(payload)
        this.token = res.token
        this.user = res.user
        setToken(res.token)
        localStorage.setItem(USER_KEY, JSON.stringify(res.user))
      } catch (err: any) {
        this.error = err?.response?.data?.message ?? err?.response?.data ?? 'Could not sign in. Check your details and try again.'
        throw err
      } finally {
        this.loading = false
      }
    },

    async register(payload: RegisterRequest) {
      this.loading = true
      this.error = null
      try {
        const res = await authApi.register(payload)
        this.token = res.token
        this.user = res.user
        setToken(res.token)
        localStorage.setItem(USER_KEY, JSON.stringify(res.user))
      } catch (err: any) {
        this.error = err?.response?.data?.message ?? err?.response?.data ?? 'Could not create your account. Try again.'
        throw err
      } finally {
        this.loading = false
      }
    },

    async googleLogin(credential: string) {
      this.loading = true
      this.error = null
      try {
        const { data } = await authClient.post('/auth/google', { credential })
        this.token = data.token
        this.user = data.user
        setToken(data.token)
        localStorage.setItem(USER_KEY, JSON.stringify(data.user))
      } catch (err: any) {
        this.error = err?.response?.data?.message ?? 'Google sign-in failed. Try again.'
        throw err
      } finally {
        this.loading = false
      }
    },

    logout() {
      this.user = null
      this.token = null
      setToken(null)
      localStorage.removeItem(USER_KEY)
    },
  },
})
