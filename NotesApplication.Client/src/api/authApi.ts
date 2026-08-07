import { authClient } from './axios'
import type { AuthResponse, LoginRequest, RegisterRequest } from '@/types/auth'

// Maps to the ASP.NET Core Auth controller:
//   POST /api/auth/login
//   POST /api/auth/register
export const authApi = {
  async login(payload: LoginRequest): Promise<AuthResponse> {
    const { data } = await authClient.post<AuthResponse>('/auth/login', payload)
    return data
  },

  async register(payload: RegisterRequest): Promise<AuthResponse> {
    const { data } = await authClient.post<AuthResponse>('/auth/register', payload)
    return data
  },
}
