<script setup lang="ts">
defineProps<{ open: boolean; title?: string; loading?: boolean }>()
const emit = defineEmits<{ confirm: []; cancel: [] }>()
</script>

<template>
  <Teleport to="body">
    <div
      v-if="open"
      class="fixed inset-0 z-50 flex items-center justify-center bg-ink/40 px-4"
      role="dialog"
      aria-modal="true"
      @click.self="emit('cancel')"
    >
      <div class="w-full max-w-sm border border-line bg-card p-6 shadow-lg">
        <p class="font-mono text-[10px] uppercase tracking-widest text-stamp">Delete note</p>
        <h2 class="mt-2 font-display text-xl text-ink">
          Delete "<span class="italic">{{ title ?? 'this note' }}</span>"?
        </h2>
        <p class="mt-2 text-sm text-ink-soft">
          This can't be undone. The note will be removed from your list for good.
        </p>

        <div class="mt-6 flex items-center justify-end gap-3">
          <button
            type="button"
            class="font-mono text-xs uppercase tracking-widest text-ink-soft hover:text-ink"
            @click="emit('cancel')"
          >
            Keep it
          </button>
          <button
            type="button"
            :disabled="loading"
            class="rounded-sm bg-stamp px-4 py-2 font-mono text-xs uppercase tracking-widest text-paper transition hover:bg-ink disabled:cursor-not-allowed disabled:opacity-60"
            @click="emit('confirm')"
          >
            {{ loading ? 'Deleting…' : 'Delete' }}
          </button>
        </div>
      </div>
    </div>
  </Teleport>
</template>
