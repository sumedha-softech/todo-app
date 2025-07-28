import React from 'react'
import { CFooter } from '@coreui/react'

const AppFooter = () => {
    return (
        <CFooter className="px-4">
            <div>
                <span className="ms-1">
                    &copy; {new Date().getFullYear()} ToDoApp. All rights reserved.
                </span>
            </div>
        </CFooter>
    )
}

export default React.memo(AppFooter)
