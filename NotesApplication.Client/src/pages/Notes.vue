<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useNotes } from '@/composables/useNotes'
import NoteCard from '@/components/NoteCard.vue'
import SearchBar from '@/components/SearchBar.vue'
import SortDropdown from '@/components/SortDropdown.vue'
import DeleteModal from '@/components/DeleteModal.vue'
import LoadingSpinner from '@/components/LoadingSpinner.vue'

const router = useRouter()
const {
  filteredNotes,
  loading,
  error,
  search,
  sortField,
  sortDirection,
  fetchAll,
  deleteNote,
  setSearch,
  setSort,
  getNoteById,
} = useNotes()

const pendingDeleteId = ref<number | null>(null)
const deleting = ref(false)

onMounted(fetchAll)

function askDelete(id: number) {
  pendingDeleteId.value = id
}

async function confirmDelete() {
  if (pendingDeleteId.value == null) return
  deleting.value = true
  try {
    await deleteNote(pendingDeleteId.value)
    pendingDeleteId.value = null
  } finally {
    deleting.value = false
  }
}
</script>

<template>
  <section>
    <div class="flex flex-col justify-between gap-4 sm:flex-row sm:items-end">
      <div>
        <p class="font-mono text-[11px] uppercase tracking-widest text-accent">Your pages</p>
        <h1 class="font-display text-3xl text-ink sm:text-4xl">All notes</h1>
      </div>
      <button
        type="button"
        class="rounded-sm bg-ink px-4 py-2 font-mono text-xs uppercase tracking-widest text-paper transition hover:bg-stamp"
        @click="router.push('/notes/new')"
      >
        + New note
      </button>
    </div>

    <div class="mt-6 flex flex-col gap-3 sm:flex-row sm:items-center">
      <SearchBar :model-value="search" @update:model-value="setSearch" />
      <SortDropdown :sort-field="sortField" :sort-direction="sortDirection" @change="setSort" />
    </div>

    <p v-if="error" class="mt-6 border border-stamp-soft bg-card p-3 text-sm text-stamp">
      {{ error }}
    </p>

    <LoadingSpinner v-if="loading" label="Fetching your notes…" />

    <template v-else>
      <div v-if="filteredNotes.length" class="mt-8 grid grid-cols-1 gap-6 sm:grid-cols-2 lg:grid-cols-3">
        <NoteCard
          v-for="note in filteredNotes"
          :key="note.id"
          :note="note"
          @delete="askDelete"
        />
      </div>
      <div v-else class="mt-16 flex flex-col items-center gap-2 text-center">
        <p class="font-display text-2xl text-ink">
          {{ search ? 'Nothing matches that search.' : 'This notebook is empty.' }}
        </p>
        <p class="max-w-sm text-sm text-ink-soft">
          {{
            search
              ? 'Try a different word, or clear the search to see every note.'
              : 'Start your first page — write down whatever is on your mind.'
          }}
        </p>
        <button
          v-if="!search"
          type="button"
          class="mt-2 rounded-sm bg-ink px-4 py-2 font-mono text-xs uppercase tracking-widest text-paper transition hover:bg-stamp"
          @click="router.push('/notes/new')"
        >
          + New note
        </button>
      </div>
    </template>

    <DeleteModal
      :open="pendingDeleteId != null"
      :title="pendingDeleteId != null ? getNoteById(pendingDeleteId)?.title : undefined"
      :loading="deleting"
      @cancel="pendingDeleteId = null"
      @confirm="confirmDelete"
    />
  </section>
</template>
