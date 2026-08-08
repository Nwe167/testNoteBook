

export interface LoginRequest {
  email: string
  password: string
}

export interface RegisterRequest {
  fullName: string
  email: string
  password: string
  confirmPassword: string
}

export interface AuthUser {
  id: string
  email: string
  fullName?: string
}

export interface AuthResponse {
  token: string
  expiresAt: string
  user: AuthUser
}
