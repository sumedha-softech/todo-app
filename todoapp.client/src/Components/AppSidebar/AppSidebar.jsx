import React, { useContext } from 'react';
import {
    CCloseButton,
    CSidebar,
    CSidebarBrand,
    CSidebarFooter,
    CSidebarHeader,
    CSidebarToggler,
} from '@coreui/react';
import {cibSymantec } from '@coreui/icons';
import CIcon from "@coreui/icons-react";
import { AppSidebarNav } from './AppSidebarNav';
import navigation from '../../_nav';
import { Context } from '../../global/MyContext';

const AppSidebar = () => {
    const { sidebarShow, setSidebarShow, unfoldable, setUnfoldable } = useContext(Context);

    return (
        <CSidebar
            className="border-end"
            colorScheme="dark"
            position="fixed"
            unfoldable={unfoldable}
            visible={sidebarShow}
            onVisibleChange={(visible) => setSidebarShow(visible)}>

            <CSidebarHeader className="border-bottom">
                <CSidebarBrand to="/" style={{ textDecoration: "none" }}>
                    <div className="sidebar-brand-full no-display-fix">
                        <CIcon icon={cibSymantec} height={32} style={{ verticalAlign: 'middle' }} />
                        <span className="brand-text">Tasks</span>
                    </div>
                    <span className="sidebar-brand-narrow" style={{ color: 'white' }}>
                        <CIcon icon={cibSymantec} height={32} />
                    </span>
                </CSidebarBrand>

                <CCloseButton
                    className="d-lg-none"
                    dark
                    onClick={() => setSidebarShow(false)}
                />
            </CSidebarHeader>
            <AppSidebarNav items={navigation} />
            <CSidebarFooter className="border-top d-none d-lg-flex">
                <CSidebarToggler onClick={() => setUnfoldable(!unfoldable)} />
            </CSidebarFooter>
        </CSidebar>
    )
}

export default React.memo(AppSidebar)
