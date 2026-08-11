import { BrowserRouter, Navigate, Route, Routes } from 'react-router-dom'
import { Layout } from './components/Layout'
import { HomePage } from './pages/HomePage'
import { TracksPage } from './pages/TracksPage'
import { LessonPage } from './pages/LessonPage'
import { PracticePage } from './pages/PracticePage'
import { MockPage } from './pages/MockPage'
import { ProgressPage } from './pages/ProgressPage'

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<Layout />}>
          <Route index element={<HomePage />} />
          <Route path="tracks" element={<TracksPage />} />
          <Route path="tracks/:slug/lessons/:id" element={<LessonPage />} />
          <Route path="practice" element={<PracticePage />} />
          <Route path="practice/:slug" element={<PracticePage />} />
          <Route path="mock" element={<MockPage />} />
          <Route path="progress" element={<ProgressPage />} />
          <Route path="*" element={<Navigate to="/" replace />} />
        </Route>
      </Routes>
    </BrowserRouter>
  )
}
