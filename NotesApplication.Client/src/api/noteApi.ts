import { apiClient } from './axios'
import { getToken } from './axios'
import type { CreateNoteDto, Note, UpdateNoteDto } from '@/types/note'

function getUserId(): number {
  const raw = localStorage.getItem('notes_app_user')
  if (!raw) return 0
  try { return Number(JSON.parse(raw).id ?? 0) } catch { return 0 }
}

export const noteApi = {
  async getAll(): Promise<Note[]> {
    const { data } = await apiClient.get<Note[]>('/Notes', { params: { userId: getUserId() } })
    return data
  },

  async getById(id: number): Promise<Note> {
    const { data } = await apiClient.get<Note>(`/Notes/${id}`, { params: { userId: getUserId() } })
    return data
  },

  async create(payload: CreateNoteDto): Promise<Note> {
    const { data } = await apiClient.post<Note>('/Notes', payload)
    return data
  },

  async update(id: number, payload: UpdateNoteDto): Promise<Note> {
    const { data } = await apiClient.put<Note>(`/Notes/${id}`, payload)
    return data
  },

  async remove(id: number): Promise<void> {
    await apiClient.delete(`/Notes/${id}`, { params: { userId: getUserId() } })
  },
}
