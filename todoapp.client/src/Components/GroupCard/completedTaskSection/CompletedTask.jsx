import { useState } from "react";
import { DeleteTask } from "../../../api/TaskApi";
import { FormateDate } from "../../../global/Helper";
import { useTaskEvents } from "../../../Hooks/TaskEvents";
import CIcon from '@coreui/icons-react';
import { cilTrash, cibVerizon } from '@coreui/icons';
import { CListGroupItem } from '@coreui/react';
import { AddOrUpdateTask } from "../../index";

const CompletedTask = ({ groupId, task, onComplete }) => {
    const { RefreshTaskLists } = useTaskEvents();
    const [editTaskId, setEditTaskId] = useState(0);
    const [visibleModel, setVisibleModel] = useState(false);

    const handleEditTask = (taskId) => {
        setEditTaskId(taskId);
        setVisibleModel(true);
    }

    // Delete task handler 
    const handleDeleteTask = async (taskId) => {
        const response = await DeleteTask(taskId);
        if (!response.isSuccess) {
            console.error("error while delete task", response);
            alert(`Error! ${response.message}`);
            return;
        }

        await RefreshTaskLists();
    };

    return (
        <>
            <CListGroupItem className={`d-flex justify-content-between align-items-start position-relative task-item `}>
                <div className="me-2 d-flex align-items-start">
                    <CIcon
                        icon={cibVerizon}
                        className="mt-1"
                        onClick={() => onComplete(task, false)}
                        style={{ cursor: 'pointer' }}
                    />
                </div>
                <div className="flex-grow-1 text-wrap text-break" onClick={() => handleEditTask(task.taskId)} style={{ cursor: 'pointer' }}>
                    <div className="ms-4"><del>{task.title}</del></div>
                    <div className="ms-4">{task.description}</div>
                    {
                        task.completeDate?.trim() && <div className="small mt-1"> Completed: {FormateDate(task.completeDate)}</div>
                    }
                </div>

                <div className="btn-gorup trash-icon-complete-section">
                    <button className="btn btn-undefined" type="button" onClick={() => handleDeleteTask(task.taskId)}>
                        <CIcon icon={cilTrash} className="ms-2 action-icon" />
                    </button>
                </div>
            </CListGroupItem>

            {visibleModel && editTaskId > 0 && (
                <AddOrUpdateTask
                    visible={visibleModel}
                    setVisibility={setVisibleModel}
                    taskId={editTaskId}
                    setTaskId={setEditTaskId}
                    groupId={groupId}
                    isStarredTask={task.isStarred}
                />
            )}
        </>
    )
}
export default CompletedTask;