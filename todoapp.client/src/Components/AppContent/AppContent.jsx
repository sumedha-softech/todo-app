import { CContainer } from '@coreui/react'
import Dashboard from '../Dashboard/Dashboard'
import Starred from '../StarTask/Starred'
import { Routes, Route } from "react-router-dom";


const AppContent = () => {
    return (
        <CContainer fluid className="px-5" style={{
            overflowX: "auto", height: "100%", flex: "1", display: "flex", flexDirection: "column"
        }}>
            <Routes>
                <Route path="/" element={<Dashboard />} />
                <Route path="/starred" element={<Starred />} />
            </Routes>
        </CContainer>
    )
}
export default AppContent;
