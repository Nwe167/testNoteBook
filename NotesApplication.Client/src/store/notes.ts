import { defineStore } from 'pinia'
import { noteApi } from '@/api/noteApi'
import { useAuthStore } from '@/store/auth'
import type { CreateNoteDto, Note, SortDirection, SortField, UpdateNoteDto } from '@/types/note'

interface NotesState {
  notes: Note[]
  loading: boolean
  error: string | null
  search: string
  sortField: SortField
  sortDirection: SortDirection
}

export const useNotesStore = defineStore('notes', {
  state: (): NotesState => ({
    notes: [],
    loading: false,
    error: null,
    search: '',
    sortField: 'updatedAt',
    sortDirection: 'desc',
  }),

  getters: {
    filteredNotes(state): Note[] {
      const term = state.search.trim().toLowerCase()
      let result = state.notes

      if (term) {
        result = result.filter(
          (n) =>
            n.title.toLowerCase().includes(term) ||
            (n.content ?? '').toLowerCase().includes(term),
        )
      }

      const dir = state.sortDirection === 'asc' ? 1 : -1
      result = [...result].sort((a, b) => {
        const field = state.sortField
        if (field === 'title') {
          return a.title.localeCompare(b.title) * dir
        }
        const aVal = new Date(a[field] ?? a.createdAt).getTime()
        const bVal = new Date(b[field] ?? b.createdAt).getTime()
        return (aVal - bVal) * dir
      })

      return result
    },

    getNoteById: (state) => (id: number) => state.notes.find((n) => n.id === id) ?? null,
  },

  actions: {
    async fetchAll() {
      this.loading = true
      this.error = null
      try {
        this.notes = await noteApi.getAll()
      } catch (err: any) {
        this.error = err?.response?.data?.message ?? 'Could not load your notes. Please try again.'
        throw err
      } finally {
        this.loading = false
      }
    },

    async fetchOne(id: number): Promise<Note> {
      this.error = null
      try {
        const note = await noteApi.getById(id)
        const idx = this.notes.findIndex((n) => n.id === id)
        if (idx >= 0) this.notes[idx] = note
        else this.notes.push(note)
        return note
      } catch (err: any) {
        this.error = err?.response?.data?.message ?? 'Could not load that note.'
        throw err
      }
    },

    async create(payload: { title: string; content?: string | null }) {
      this.error = null
      const authStore = useAuthStore()
      const dto: CreateNoteDto = { userId: Number(authStore.user?.id ?? 0), ...payload }
      try {
        const note = await noteApi.create(dto)
        this.notes.unshift(note)
        return note
      } catch (err: any) {
        this.error = err?.response?.data?.message ?? 'Could not create the note. Please try again.'
        throw err
      }
    },

    async update(id: number, payload: { title: string; content?: string | null }) {
      this.error = null
      const authStore = useAuthStore()
      const dto: UpdateNoteDto = { userId: Number(authStore.user?.id ?? 0), ...payload }
      try {
        const note = await noteApi.update(id, dto)
        const idx = this.notes.findIndex((n) => n.id === id)
        if (idx >= 0) this.notes[idx] = note
        return note
      } catch (err: any) {
        this.error = err?.response?.data?.message ?? 'Could not save your changes. Please try again.'
        throw err
      }
    },

    async remove(id: number) {
      this.error = null
      try {
        await noteApi.remove(id)
        this.notes = this.notes.filter((n) => n.id !== id)
      } catch (err: any) {
        this.error = err?.response?.data?.message ?? 'Could not delete the note. Please try again.'
        throw err
      }
    },

    setSearch(term: string) {
      this.search = term
    },

    setSort(field: SortField, direction: SortDirection) {
      this.sortField = field
      this.sortDirection = direction
    },
  },
})
