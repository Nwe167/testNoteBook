import { storeToRefs } from 'pinia'
import { useNotesStore } from '@/store/notes'
import type { CreateNoteDto, SortDirection, SortField, UpdateNoteDto } from '@/types/note'

export function useNotes() {
  const store = useNotesStore()
  const { notes, filteredNotes, loading, error, search, sortField, sortDirection } =
    storeToRefs(store)

  async function fetchAll() {
    await store.fetchAll()
  }

  async function fetchOne(id: number) {
    return store.fetchOne(id)
  }

  async function createNote(payload: CreateNoteDto) {
    return store.create(payload)
  }

  async function updateNote(id: number, payload: UpdateNoteDto) {
    return store.update(id, payload)
  }

  async function deleteNote(id: number) {
    await store.remove(id)
  }

  function setSearch(term: string) {
    store.setSearch(term)
  }

  function setSort(field: SortField, direction: SortDirection) {
    store.setSort(field, direction)
  }

  return {
    notes,
    filteredNotes,
    loading,
    error,
    search,
    sortField,
    sortDirection,
    fetchAll,
    fetchOne,
    createNote,
    updateNote,
    deleteNote,
    setSearch,
    setSort,
    getNoteById: store.getNoteById,
  }
}
