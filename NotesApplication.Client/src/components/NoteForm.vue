<script setup lang="ts">
import { reactive, watch } from 'vue'
import type { Note } from '@/types/note'
import { validateNoteForm } from '@/utils/validator'

const props = defineProps<{
  initial?: Partial<Note>
  submitting?: boolean
  submitLabel?: string
}>()

const emit = defineEmits<{
  submit: [{ title: string; content: string }]
  cancel: []
}>()

const form = reactive({
  title: props.initial?.title ?? '',
  content: props.initial?.content ?? '',
})

const errors = reactive<{ title?: string }>({})

watch(
  () => props.initial,
  (val) => {
    form.title = val?.title ?? ''
    form.content = val?.content ?? ''
  },
)

function handleSubmit() {
  const result = validateNoteForm(form.title)
  errors.title = result.errors.title
  if (!result.valid) return
  emit('submit', { title: form.title.trim(), content: form.content.trim() })
}
</script>

<template>
  <form class="flex flex-col gap-5" @submit.prevent="handleSubmit">
    <div>
      <label for="note-title" class="font-mono text-[11px] uppercase tracking-widest text-ink-faint">
        Title
      </label>
      <input
        id="note-title"
        v-model="form.title"
        type="text"
        maxlength="200"
        placeholder="What's this note about?"
        class="mt-1 w-full border-b border-line bg-transparent pb-2 font-display text-2xl text-ink placeholder:text-ink-faint focus:border-stamp"
        :aria-invalid="Boolean(errors.title)"
      />
      <p v-if="errors.title" class="mt-1 text-xs text-stamp">{{ errors.title }}</p>
    </div>

    <div>
      <label for="note-content" class="font-mono text-[11px] uppercase tracking-widest text-ink-faint">
        Content
      </label>
      <textarea
        id="note-content"
        v-model="form.content"
        rows="10"
        placeholder="Write it down…"
        class="mt-1 w-full resize-y rounded-sm border border-line bg-card p-3 text-sm leading-relaxed text-ink placeholder:text-ink-faint focus:border-stamp"
      />
    </div>

    <div class="flex items-center gap-3">
      <button
        type="submit"
        :disabled="submitting"
        class="rounded-sm bg-ink px-4 py-2 font-mono text-xs uppercase tracking-widest text-paper transition hover:bg-stamp disabled:cursor-not-allowed disabled:opacity-60"
      >
        {{ submitting ? 'Saving…' : (submitLabel ?? 'Save note') }}
      </button>
      <button 
        type="button"
        class="font-mono text-xs uppercase tracking-widest text-ink-soft hover:text-ink"
        @click="$emit('cancel')"
      >
        Cancel
      </button>
    </div>
  </form>
</template>
