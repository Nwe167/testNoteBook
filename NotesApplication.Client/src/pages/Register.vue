<script setup lang="ts">
import { reactive } from 'vue'
import { useRouter } from 'vue-router'
import { useAuth } from '@/composables/useAuth'
import { validateRegisterForm } from '@/utils/validator'

const router = useRouter()
const { register, loading, error } = useAuth()

const form = reactive({ fullName: '', email: '', password: '', confirmPassword: '' })
const errors = reactive<{ fullName?: string; email?: string; password?: string; confirmPassword?: string }>({})

async function onSubmit() {
  const result = validateRegisterForm(form.fullName, form.email, form.password, form.confirmPassword)
  Object.assign(errors, { fullName: undefined, email: undefined, password: undefined, confirmPassword: undefined })
  Object.assign(errors, result.errors)
  if (!result.valid) return

  try {
    await register({
      fullName: form.fullName.trim(),
      email: form.email.trim(),
      password: form.password,
      confirmPassword: form.confirmPassword,
    })
    router.push('/')
  } catch {
    // error state is surfaced from the store
  }
}
</script>

<template>
  <section class="mx-auto max-w-sm">
    <p class="font-mono text-[11px] uppercase tracking-widest text-accent">Get started</p>
    <h1 class="mt-1 font-display text-3xl text-ink">Create an account</h1>

    <form class="mt-8 flex flex-col gap-5" @submit.prevent="onSubmit">
      <div>
        <label for="fullName" class="font-mono text-[11px] uppercase tracking-widest text-ink-faint">Full name</label>
        <input
          id="fullName"
          v-model="form.fullName"
          type="text"
          autocomplete="name"
          class="mt-1 w-full border-b border-line bg-transparent pb-2 text-ink focus:border-stamp"
        />
        <p v-if="errors.fullName" class="mt-1 text-xs text-stamp">{{ errors.fullName }}</p>
      </div>

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
          autocomplete="new-password"
          class="mt-1 w-full border-b border-line bg-transparent pb-2 text-ink focus:border-stamp"
        />
        <p v-if="errors.password" class="mt-1 text-xs text-stamp">{{ errors.password }}</p>
      </div>

      <div>
        <label for="confirmPassword" class="font-mono text-[11px] uppercase tracking-widest text-ink-faint">
          Confirm password
        </label>
        <input
          id="confirmPassword"
          v-model="form.confirmPassword"
          type="password"
          autocomplete="new-password"
          class="mt-1 w-full border-b border-line bg-transparent pb-2 text-ink focus:border-stamp"
        />
        <p v-if="errors.confirmPassword" class="mt-1 text-xs text-stamp">{{ errors.confirmPassword }}</p>
      </div>

      <p v-if="error" class="border border-stamp-soft bg-card p-3 text-sm text-stamp">{{ error }}</p>

      <button
        type="submit"
        :disabled="loading"
        class="mt-2 rounded-sm bg-ink px-4 py-2.5 font-mono text-xs uppercase tracking-widest text-paper transition hover:bg-stamp disabled:cursor-not-allowed disabled:opacity-60"
      >
        {{ loading ? 'Creating account…' : 'Create account' }}
      </button>
    </form>

    <p class="mt-6 text-sm text-ink-soft">
      Already have an account?
      <RouterLink to="/login" class="text-stamp hover:underline">Sign in</RouterLink>
    </p>
  </section>
</template>
