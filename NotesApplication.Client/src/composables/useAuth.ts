import { storeToRefs } from 'pinia'
import { useAuthStore } from '@/store/auth'
import type { LoginRequest, RegisterRequest } from '@/types/auth'

export function useAuth() {
  const store = useAuthStore()

  const { user, token, loading, error, isAuthenticated } = storeToRefs(store)

  async function login(payload: LoginRequest) {
    await store.login(payload)
  }

  async function register(payload: RegisterRequest) {
    await store.register(payload)
  }

  async function googleLogin(credential: string) {
    await store.googleLogin(credential)
  }

  function logout() {
    store.logout()
  }

  return { user, token, loading, error, isAuthenticated, login, register, googleLogin, logout }
}
