import { AppContent, AppSidebar, AppFooter, AppHeader } from '../Components/index'

const DefaultLayout = () => {


    return (
        <div>
            <AppSidebar />
            <div className="wrapper d-flex flex-column" style={{ height:"100vh" }}>
                <AppHeader />
                <div className="contentwrapper flex-grow-1"
                    >
                    <AppContent />
                </div>
                <AppFooter />
            </div>
        </div>
    )
}

export default DefaultLayout