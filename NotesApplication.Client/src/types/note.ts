export interface Note {
  id: number
  title: string
  content: string | null
  createdAt: string
  updatedAt: string | null
  userId?: string
}

export interface CreateNoteDto {
  userId: number
  title: string
  content?: string | null
}

export interface UpdateNoteDto {
  userId: number
  title: string
  content?: string | null
}

export type SortField = 'title' | 'createdAt' | 'updatedAt'
export type SortDirection = 'asc' | 'desc'

export interface SortOption {
  field: SortField
  direction: SortDirection
  label: string
}

export interface NotesQueryState {
  search: string
  sortField: SortField
  sortDirection: SortDirection
}
