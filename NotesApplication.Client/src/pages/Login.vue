<script setup lang="ts">
import { reactive, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { useAuth } from '@/composables/useAuth'

declare global {
  interface Window {
    google?: any
  }
}

const router = useRouter()
const { login, googleLogin, loading, error } = useAuth()

const form = reactive({ email: '', password: '' })
const errors = reactive<{ email?: string; password?: string }>({})

async function onSubmit() {
  errors.email = undefined
  errors.password = undefined
  if (!form.email) {
    errors.email = 'Email is required'
    return
  }
  if (!form.password) {
    errors.password = 'Password is required'
    return
  }
  try {
    await login({ email: form.email.trim(), password: form.password })
    router.push('/')
  } catch {}
}

async function handleGoogleLogin(response: { credential: string }) {
  try {
    await googleLogin(response.credential)
    router.push('/')
  } catch {}
}

function initializeGoogleSignIn() {
  if (window.google?.accounts?.id) {
    window.google.accounts.id.initialize({
      client_id: '235774630129-ag4gauihces85baitbhm851sc1ntit1p.apps.googleusercontent.com',
      callback: handleGoogleLogin,
      auto_select: false,
      itp_support: true,
    })

    window.google.accounts.id.renderButton(
      document.getElementById('google-login-button'),
      {
        theme: 'outline',
        size: 'large',
        width: 380,
        text: 'continue_with',
        shape: 'rectangular',
      }
    )
  }
}

onMounted(() => {
  // If the Google library is already loaded, initialize directly
  if (window.google?.accounts?.id) {
    initializeGoogleSignIn()
  } else {
    // Dynamically load script if missing from index.html
    const scriptExists = document.querySelector('script[src="https://accounts.google.com/gsi/client"]')
    if (!scriptExists) {
      const script = document.createElement('script')
      script.src = 'https://accounts.google.com/gsi/client'
      script.async = true
      script.defer = true
      script.onload = initializeGoogleSignIn
      document.head.appendChild(script)
    } else {
      // Poll briefly if script element exists but hasn't parsed yet
      const checkInterval = setInterval(() => {
        if (window.google?.accounts?.id) {
          clearInterval(checkInterval)
          initializeGoogleSignIn()
        }
      }, 50)
      setTimeout(() => clearInterval(checkInterval), 5000)
    }
  }
})
</script>

<template>
  <section class="mx-auto max-w-sm">
    <p class="font-mono text-[11px] uppercase tracking-widest text-accent">Welcome back</p>
    <h1 class="mt-1 font-display text-3xl text-ink">Sign in</h1>

    <form class="mt-8 flex flex-col gap-5" @submit.prevent="onSubmit">
      <div>
        <label for="email" class="font-mono text-[11px] uppercase tracking-widest text-ink-faint">Email</label>
        <input
          id="email"
          v-model="form.email"
          type="email"
          autocomplete="email"
          class="mt-1 w-full border-b border-line bg-transparent pb-2 text-ink focus:border-stamp"
        />
        <p v-if="errors.email" class="mt-1 text-xs text-stamp">{{ errors.email }}</p>
      </div>

      <div>
        <label for="password" class="font-mono text-[11px] uppercase tracking-widest text-ink-faint">Password</label>
        <input
          id="password"
          v-model="form.password"
          type="password"
          autocomplete="current-password"
          class="mt-1 w-full border-b border-line bg-transparent pb-2 text-ink focus:border-stamp"
        />
        <p v-if="errors.password" class="mt-1 text-xs text-stamp">{{ errors.password }}</p>
      </div>

      <p v-if="error" class="border border-stamp-soft bg-card p-3 text-sm text-stamp">{{ error }}</p>

      <button
        type="submit"
        :disabled="loading"
        class="mt-2 rounded-sm bg-ink px-4 py-2.5 font-mono text-xs uppercase tracking-widest text-paper transition hover:bg-stamp disabled:cursor-not-allowed disabled:opacity-60"
      >
        {{ loading ? 'Signing in…' : 'Sign in' }}
      </button>
    </form>

    <div class="my-6 flex items-center gap-3">
      <div class="h-px flex-1 bg-line"></div>
      <span class="font-mono text-[10px] uppercase tracking-widest text-ink-faint">OR</span>
      <div class="h-px flex-1 bg-line"></div>
    </div>

    <div id="google-login-button" class="flex justify-center min-h-[44px]"></div>

    <p class="mt-6 text-sm text-ink-soft">
      New here?
      <RouterLink to="/register" class="text-stamp hover:underline">Create an account</RouterLink>
    </p>
  </section>
</template>