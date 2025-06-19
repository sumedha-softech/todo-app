import { useState } from 'react';
import { AddOrUpdateTask } from '../index';
import { cibSymantec } from '@coreui/icons'
import CIcon from "@coreui/icons-react";

const AddTaskButton = ({ groupId, isStarredTask }) => {
    const [visibleModel, setVisibleModel] = useState(false);

    return (
        <>
            <div className="mb-3 d-flex align-items-center gap-2 add-task-button"
                style={{ cursor: 'pointer', color: 'cornflowerblue' }}
                onClick={() => setVisibleModel(true)}
            >
                <CIcon icon={cibSymantec} size="xl" />
                <span>{isStarredTask ? "Add a star task" : "Add a task"} </span>
            </div>

            {visibleModel &&
                <AddOrUpdateTask visible={visibleModel} setVisibility={setVisibleModel} taskId={0} groupId={groupId ?? 0} isStarredTask={isStarredTask} />
            }
        </>
    )
}
export default AddTaskButton;