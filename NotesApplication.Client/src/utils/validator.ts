export interface ValidationResult {
  valid: boolean
  errors: Record<string, string>
}

const EMAIL_RE = /^[^\s@]+@[^\s@]+\.[^\s@]+$/

export function isValidEmail(email: string): boolean {
  return EMAIL_RE.test(email.trim())
}

export function validateNoteForm(title: string): ValidationResult {
  const errors: Record<string, string> = {}

  if (!title || !title.trim()) {
    errors.title = 'Give this note a title before saving.'
  } else if (title.trim().length > 200) {
    errors.title = 'Title must be 200 characters or fewer.'
  }

  return { valid: Object.keys(errors).length === 0, errors }
}

export function validateLoginForm(email: string, password: string): ValidationResult {
  const errors: Record<string, string> = {}

  if (!email.trim()) {
    errors.email = 'Enter your email address.'
  } else if (!isValidEmail(email)) {
    errors.email = 'Enter a valid email address.'
  }

  if (!password) {
    errors.password = 'Enter your password.'
  }

  return { valid: Object.keys(errors).length === 0, errors }
}

export function validateRegisterForm(
  fullName: string,
  email: string,
  password: string,
  confirmPassword: string,
): ValidationResult {
  const errors: Record<string, string> = {}

  if (!fullName.trim()) {
    errors.fullName = 'Enter your full name.'
  }

  if (!email.trim()) {
    errors.email = 'Enter your email address.'
  } else if (!isValidEmail(email)) {
    errors.email = 'Enter a valid email address.'
  }

  if (!password) {
    errors.password = 'Choose a password.'
  } else if (password.length < 6) {
    errors.password = 'Password must be at least 6 characters.'
  }

  if (confirmPassword !== password) {
    errors.confirmPassword = 'Passwords do not match.'
  }

  return { valid: Object.keys(errors).length === 0, errors }
}
