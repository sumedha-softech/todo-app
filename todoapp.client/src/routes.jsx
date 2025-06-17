import React from 'react'

const Dashboard = React.lazy(() => import('./Components/Dashboard/Dashboard'))
const Starred = React.lazy(() => import('./Components/StarTask/Starred'))


const routes = [
    { path: '/', name: 'Dashboard', element: Dashboard },
    { path: '/starred', name: 'Starred', element: Starred }
]

export default routes
