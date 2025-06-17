import { createRoot } from 'react-dom/client';
import App from './App.jsx';
import { MyContextProvider } from './global/MyContext';

createRoot(document.getElementById('root')).render( 
    <MyContextProvider>
        <App />
    </MyContextProvider>
  ,
)
