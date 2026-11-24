import { useState } from 'react'

import './App.css'
import Login from './pages/login/Login'
import Rotas from './routes/Routes'

function App() {
    const [count, setCount] = useState(0)

    return (
        <>
            <Rotas />
        </>
    )
}

export default App