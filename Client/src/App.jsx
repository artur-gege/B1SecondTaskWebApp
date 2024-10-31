import { useState } from 'react'
import './App.css'
import SInput from './SInput'
import SFiles from './SFiles'
import SFile from './SFileAlone'

function App() {
    const [active, setActive] = useState(0)
    const [key, setKey] = useState()

    const changeSection = (acive) => {
        switch (acive) {
            case 0: return <SInput />
            case 1: return <SFiles setActive={setActive} setKey={setKey} />
            case 2: return <SFile cardKey={key} setActive={setActive}/>
        }
    }

  return (
      <main>
          <div>
              <button onClick={() => setActive(0)}>File upload page</button>
              <button onClick={() => setActive(1)}>Check files Page</button>
          </div>
          {changeSection(active)}
      </main>
  )
}

export default App
