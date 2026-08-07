<script setup lang="ts">
withDefaults(defineProps<{ label?: string }>(), { label: 'Loading…' })
</script>

<template>
  <div class="flex flex-col items-center justify-center gap-3 py-16 text-ink-soft">
    <span
      class="h-8 w-8 animate-spin rounded-full border-2 border-line border-t-stamp"
      aria-hidden="true"
    />
    <p class="font-mono text-xs uppercase tracking-widest">{{ label }}</p>
  </div>
</template>
