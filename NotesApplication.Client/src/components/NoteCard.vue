<script setup lang="ts">
import { RouterLink } from 'vue-router'
import type { Note } from '@/types/note'
import { formatDate } from '@/utils/date'

defineProps<{ note: Note }>()
defineEmits<{ delete: [id: number] }>()
</script>

<template>
  <article class="note-card group flex flex-col justify-between border border-line p-5 pr-7 shadow-sm transition hover:-translate-y-0.5 hover:shadow-md">
    <RouterLink :to="`/notes/${note.id}`" class="flex-1">
      <p class="font-mono text-[10px] uppercase tracking-widest text-accent">
        {{ formatDate(note.createdAt) }}
      </p>
      <h3 class="mt-2 font-display text-xl leading-snug text-ink line-clamp-2">
        {{ note.title }}
      </h3>
      <p v-if="note.content" class="mt-2 text-sm leading-relaxed text-ink-soft line-clamp-3">
        {{ note.content }}
      </p>
      <p v-else class="mt-2 text-sm italic text-ink-faint">No content yet.</p>
    </RouterLink>

    <div class="mt-4 flex items-center justify-between rule-divider pt-3">
      <RouterLink
        :to="`/notes/${note.id}`"
        class="font-mono text-[11px] uppercase tracking-widest text-ink-soft hover:text-stamp"
      >
        Open →
      </RouterLink>
      <button
        type="button"
        class="rounded-sm px-2 py-1 font-mono text-[11px] uppercase tracking-widest text-ink-faint opacity-0 transition hover:text-stamp group-hover:opacity-100 focus-visible:opacity-100"
        aria-label="Delete note"
        @click.stop.prevent="$emit('delete', note.id)"
      >
        Delete
      </button>
    </div>
  </article>
</template>
