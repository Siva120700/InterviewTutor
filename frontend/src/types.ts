export type Lesson = {
  id: string
  title: string
  trackSlug: string
  module: string
  order: number
  summary: string
  languages: string[]
  source: string
  markdownBody?: string | null
  completed: boolean
}

export type Module = { name: string; lessons: Lesson[] }

export type Track = {
  slug: string
  title: string
  group: string
  description: string
  modules: Module[]
}

export type Progress = {
  completedLessonIds: string[]
  totalLessons: number
  completedCount: number
}

export type ChatMessage = {
  id: string
  role: string
  content: string
  createdAt: string
}

export type ChatThread = {
  id: string
  lessonId: string
  problemId?: string | null
  title: string
  messages: ChatMessage[]
}

export type LessonDraft = {
  id: string
  title: string
  trackSlug: string
  module: string
  summary: string
  markdownBody: string
  status: string
}

export type SendChatResponse = {
  thread: ChatThread
  assistantMessage: ChatMessage
  draft?: LessonDraft | null
  suggestedExisting?: Lesson | null
}

export type Problem = {
  id: string
  slug: string
  title: string
  difficulty: string
  trackSlug: string
  promptMarkdown: string
  javaSolution?: string | null
  csharpSolution?: string | null
  complexityNotes: string
}

export type MockSession = {
  id: string
  mode: string
  durationMinutes: number
  startedAt: string
  endedAt?: string | null
  transcript: ChatMessage[]
  rubric?: string | null
}
