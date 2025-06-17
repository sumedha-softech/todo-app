import React from 'react'
import CIcon from '@coreui/icons-react'
import { cilStar, cilPlus, cibSymantec } from '@coreui/icons'
import {CNavItem } from '@coreui/react'

const _nav = [
    {
        component: CNavItem,
        name: 'Create',
        modelName:'AddOrUpdateTask',
        icon: <CIcon icon={cilPlus} customClassName="nav-icon" />
    },
    {
        component: CNavItem,
        name: 'All task',
        to: '/',
        icon: <CIcon icon={cibSymantec} customClassName="nav-icon" />
    },
    {
        component: CNavItem,
        name: 'Starred',
        to: '/starred',
        icon: <CIcon icon={cilStar} customClassName="nav-icon" />
    },
    {
        component: 'SidebarGroups',
        name:'ComponentName'
    },
    {
        component: CNavItem,
        name: 'Create Group',
        modelName: 'AddOrUpdateGroups',
        icon: <CIcon icon={cilPlus} customClassName="nav-icon" />
    }
]

export default _nav
