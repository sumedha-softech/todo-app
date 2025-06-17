import { useState } from "react";
import { cilCaretRight, cilCaretBottom } from '@coreui/icons';
import { CompletedTask } from "../../index";
import { CListGroup } from '@coreui/react'
import CIcon from '@coreui/icons-react';

const CompletedTaskList = ({ groupId, completedTaskList, onComplete }) => {
    const [showCompleted, setShowCompleted] = useState(false)
    return (
        <>
            <div className="mt-3">
                <div
                    className="d-flex align-items-center cursor-pointer"
                    onClick={() => setShowCompleted(!showCompleted)}
                    style={{cursor: 'pointer' }}>
                    <CIcon icon={showCompleted ? cilCaretBottom : cilCaretRight} className="me-2" />
                    <strong> Completed ({completedTaskList.length})</strong>
                </div>

                {showCompleted && (
                    <CListGroup flush className="mt-2">
                        {completedTaskList.map(completeTask => (
                                <CompletedTask key={completeTask.taskId} groupId={groupId} task={completeTask} onComplete={onComplete} />
                            ))}
                    </CListGroup>
                )}
            </div>
        </>
    )
}
export default CompletedTaskList;