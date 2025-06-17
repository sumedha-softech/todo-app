import '../src/App.css';
import './scss/style.scss'
import './scss/examples.scss'
import { BrowserRouter, Routes, Route } from "react-router-dom";
import DefaultLayout from './Layout/DefaultLayout';

function App() {
    return (

        <BrowserRouter>
            <Routes>
                <Route path="*" element={<DefaultLayout />} />
            </Routes>
        </BrowserRouter>
    )
}

export default App;