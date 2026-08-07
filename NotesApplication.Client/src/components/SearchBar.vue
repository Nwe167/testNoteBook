<script setup lang="ts">
const model = defineModel<string>({ default: '' })
</script>

<template>
  <div class="relative flex-1">
    <svg
      class="pointer-events-none absolute left-3 top-1/2 h-4 w-4 -translate-y-1/2 text-ink-faint"
      viewBox="0 0 20 20"
      fill="none"
      aria-hidden="true"
    >
      <circle cx="9" cy="9" r="6" stroke="currentColor" stroke-width="1.5" />
      <path d="m17 17-3.5-3.5" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" />
    </svg>
    <input
      v-model="model"
      type="search"
      placeholder="Search notes by title or content…"
      class="w-full rounded-sm border border-line bg-card py-2 pl-9 pr-3 text-sm text-ink placeholder:text-ink-faint focus:border-stamp"
      aria-label="Search notes"
    />
  </div>
</template>
