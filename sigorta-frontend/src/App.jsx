import { useState } from 'react'
import LoginPage from './pages/LoginPage'


function App() {
  const [count, setCount] = useState(0)

  return (
    <>
      <div>
        <h1>sigorta uygulamasi</h1>
        <LoginPage />
      </div>

    </>
  )
}

export default App
