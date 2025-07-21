import { CContainer } from '@coreui/react'
import Dashboard from '../Dashboard/Dashboard'
import Starred from '../StarTask/Starred'
import { Routes, Route } from "react-router-dom";
import { useTaskEvents } from '../../Hooks/TaskEvents';
import { CToast, CToastBody, CButton, CToastClose, CToaster } from '@coreui/react';

const AppContent = () => {
    const { recentActionItem, handleUndo } = useTaskEvents();
    return (
        <CContainer fluid className="px-5" style={{
            overflowX: "auto", height: "100%", flex: "1", display: "flex", flexDirection: "column"
        }}>
            <Routes>
                <Route path="/" element={<Dashboard />} />
                <Route path="/starred" element={<Starred />} />
            </Routes>
            {recentActionItem && (                <>                    <CToaster className="custom-toaster">
                        <CToast autohide={false} visible={true} animation={true} color="primary" className="align-items-center">
                            <div className="d-flex">
                                <CToastBody>
                                    <p style={{ fontWeight: '700', fontSize: '18px', marginBottom: '0px' }}>
                                        Click for <CButton color="light" onClick={() => handleUndo()}>Undo</CButton>
                                    </p>
                                </CToastBody>
                                <CToastClose className="me-2 m-auto" />
                            </div>
                        </CToast>
                    </CToaster>
                </>
            )}
        </CContainer>
    )
}
export default AppContent;
