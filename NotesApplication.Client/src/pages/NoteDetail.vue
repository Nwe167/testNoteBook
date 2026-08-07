<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useNotes } from '@/composables/useNotes'
import NoteForm from '@/components/NoteForm.vue'
import DeleteModal from '@/components/DeleteModal.vue'
import LoadingSpinner from '@/components/LoadingSpinner.vue'
import { formatDateTime } from '@/utils/date'
import type { Note } from '@/types/note'

const route = useRoute()
const router = useRouter()
const { fetchOne, createNote, updateNote, deleteNote, getNoteById, error } = useNotes()

const isNew = computed(() => route.params.id === 'new')
const noteId = computed(() => (isNew.value ? null : Number(route.params.id)))

const note = ref<Note | null>(null)
const loading = ref(false)
const submitting = ref(false)
const isEditing = ref(false)
const showDelete = ref(false)
const deleting = ref(false)

async function load() {
  if (isNew.value) {
    isEditing.value = true
    return
  }
  if (noteId.value == null) return
  const cached = getNoteById(noteId.value)
  if (cached) note.value = cached
  loading.value = !cached
  try {
    note.value = await fetchOne(noteId.value)
  } finally {
    loading.value = false
  }
}

onMounted(load)

async function handleSubmit(payload: { title: string; content: string }) {
  submitting.value = true
  try {
    if (isNew.value) {
      const created = await createNote(payload)
      router.replace(`/notes/${created.id}`)
    } else if (noteId.value != null) {
      note.value = await updateNote(noteId.value, payload)
      isEditing.value = false
    }
  } finally {
    submitting.value = false
  }
}

function handleCancel() {
  if (isNew.value) router.push('/')
  else isEditing.value = false
}

async function confirmDelete() {
  if (noteId.value == null) return
  deleting.value = true
  try {
    await deleteNote(noteId.value)
    router.push('/')
  } finally {
    deleting.value = false
  }
}
</script>

<template>
  <section class="mx-auto max-w-2xl">
    <RouterLink to="/" class="font-mono text-[11px] uppercase tracking-widest text-ink-soft hover:text-stamp">
      ← All notes
    </RouterLink>

    <LoadingSpinner v-if="loading" label="Opening note…" />

    <p v-else-if="error && !note && !isNew" class="mt-6 border border-stamp-soft bg-card p-3 text-sm text-stamp">
      {{ error }}
    </p>

    <div v-else class="note-card mt-6 border border-line p-6 pr-8 shadow-sm sm:p-8 sm:pr-10">
      <template v-if="isEditing">
        <p class="font-mono text-[11px] uppercase tracking-widest text-accent">
          {{ isNew ? 'New note' : 'Editing' }}
        </p>
        <div class="mt-3">
          <NoteForm
            :initial="note ?? undefined"
            :submitting="submitting"
            :submit-label="isNew ? 'Create note' : 'Save changes'"
            @submit="handleSubmit"
            @cancel="handleCancel"
          />
        </div>
      </template>

      <template v-else-if="note">
        <div class="flex items-start justify-between gap-4">
          <div>
            <p class="font-mono text-[11px] uppercase tracking-widest text-accent">
              Created {{ formatDateTime(note.createdAt) }}
            </p>
            <p v-if="note.updatedAt" class="font-mono text-[11px] uppercase tracking-widest text-ink-faint">
              Updated {{ formatDateTime(note.updatedAt) }}
            </p>
          </div>
          <div class="flex shrink-0 gap-2">
            <button
              type="button"
              class="rounded-sm border border-line px-3 py-1.5 font-mono text-[11px] uppercase tracking-widest text-ink-soft hover:border-stamp hover:text-stamp"
              @click="isEditing = true"
            >
              Edit
            </button>
            <button
              type="button"
              class="rounded-sm border border-line px-3 py-1.5 font-mono text-[11px] uppercase tracking-widest text-ink-soft hover:border-stamp hover:text-stamp"
              @click="showDelete = true"
            >
              Delete
            </button>
          </div>
        </div>

        <h1 class="mt-4 font-display text-3xl leading-snug text-ink sm:text-4xl">
          {{ note.title }}
        </h1>

        <p v-if="note.content" class="mt-5 whitespace-pre-wrap text-base leading-relaxed text-ink-soft">
          {{ note.content }}
        </p>
        <p v-else class="mt-5 italic text-ink-faint">No content yet — click Edit to add some.</p>
      </template>
    </div>

    <DeleteModal
      :open="showDelete"
      :title="note?.title"
      :loading="deleting"
      @cancel="showDelete = false"
      @confirm="confirmDelete"
    />
  </section>
</template>
