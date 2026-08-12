import type {
  Lesson,
  LessonDraft,
  MockSession,
  Problem,
  Progress,
  SendChatResponse,
  ChatThread,
  Track,
} from './types'

// Empty = same origin (single-app deploy). Local Vite uses proxy or VITE_API_BASE.
const BASE = import.meta.env.VITE_API_BASE ?? ''

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  const res = await fetch(`${BASE}${path}`, {
    headers: { 'Content-Type': 'application/json', ...(init?.headers ?? {}) },
    ...init,
  })
  if (!res.ok) {
    const text = await res.text()
    throw new Error(text || res.statusText)
  }
  if (res.status === 204) return undefined as T
  return res.json() as Promise<T>
}

export const api = {
  tracks: () => request<Track[]>('/api/tracks'),
  lesson: (track: string, id: string) =>
    request<Lesson>(`/api/tracks/${encodeURIComponent(track)}/lessons/${encodeURIComponent(id)}`),
  progress: () => request<Progress>('/api/progress'),
  complete: (lessonId: string, completed = true) =>
    request<Progress>(`/api/progress/lessons/${encodeURIComponent(lessonId)}/complete?completed=${completed}`, {
      method: 'POST',
    }),
  suggested: () => request<Lesson[]>('/api/progress/suggested'),
  getChat: (lessonId: string) => request<ChatThread | null>(`/api/lessons/${encodeURIComponent(lessonId)}/chat`),
  sendChat: (
    lessonId: string,
    body: { message: string; mode?: string | null; preferredLanguage?: string; threadId?: string | null },
  ) =>
    request<SendChatResponse>(`/api/lessons/${encodeURIComponent(lessonId)}/chat`, {
      method: 'POST',
      body: JSON.stringify(body),
    }),
  confirmDraft: (id: string, body?: Partial<LessonDraft>) =>
    request<Lesson>(`/api/drafts/${id}/confirm`, {
      method: 'POST',
      body: JSON.stringify({
        editedTitle: body?.title,
        editedMarkdown: body?.markdownBody,
        trackSlug: body?.trackSlug,
        module: body?.module,
      }),
    }),
  cancelDraft: (id: string) => request<void>(`/api/drafts/${id}/cancel`, { method: 'POST' }),
  problems: () => request<Problem[]>('/api/problems'),
  problem: (slug: string) => request<Problem>(`/api/problems/${encodeURIComponent(slug)}`),
  startMock: (mode: string, durationMinutes = 30) =>
    request<MockSession>('/api/mock/start', {
      method: 'POST',
      body: JSON.stringify({ mode, durationMinutes }),
    }),
  mockMessage: (id: string, message: string) =>
    request<MockSession>(`/api/mock/${id}/message`, {
      method: 'POST',
      body: JSON.stringify({ message }),
    }),
  endMock: (id: string) => request<MockSession>(`/api/mock/${id}/end`, { method: 'POST' }),
}
