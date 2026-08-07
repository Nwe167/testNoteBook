<script setup lang="ts">
import { RouterLink } from 'vue-router'
import { useAuth } from '@/composables/useAuth'

const { isAuthenticated, user, logout } = useAuth()
</script>

<template>
  <header class="sticky top-0 z-20 border-b border-line bg-paper/95 backdrop-blur">
    <div class="mx-auto flex max-w-5xl items-center justify-between px-4 py-4 sm:px-6">
      <RouterLink to="/" class="flex items-baseline gap-2">
        <span class="font-display text-2xl italic text-ink">Notebook</span>
        <span class="font-mono text-[10px] uppercase tracking-widest text-accent">/ notes</span>
      </RouterLink>

      <nav class="flex items-center gap-4">
        <template v-if="isAuthenticated">
          <span v-if="user?.fullName || user?.email" class="hidden font-mono text-xs text-ink-soft sm:inline">
            {{ user?.fullName || user?.email }}
          </span>
          <button
            type="button"
            class="rounded-sm border border-line bg-card px-3 py-1.5 font-mono text-xs uppercase tracking-wide text-ink-soft transition hover:border-stamp hover:text-stamp"
            @click="logout"
          >
            Sign out
          </button>
        </template>
        <template v-else>
          <RouterLink
            to="/login"
            class="font-mono text-xs uppercase tracking-wide text-ink-soft hover:text-ink"
          >
            Sign in
          </RouterLink>
          <RouterLink
            to="/register"
            class="rounded-sm bg-ink px-3 py-1.5 font-mono text-xs uppercase tracking-wide text-paper transition hover:bg-stamp"
          >
            Register
          </RouterLink>
        </template>
      </nav>
    </div>
  </header>
</template>
