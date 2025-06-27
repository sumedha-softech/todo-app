import { useState } from "react";
import { DeleteTask } from "../../../api/TaskApi";
import { DeleteSubTask } from "../../../api/subTaskAPi";
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
        setEditTaskId(task.taskGroupId > 0 ? taskId : task.subTaskId);
        setVisibleModel(true);
    }

    // Delete task handler 
    const handleDeleteTask = async (taskId) => {
        let response = {};
        if (task.taskGroupId > 0) {
            response = await DeleteTask(taskId);
        }
        else {
            response = await DeleteSubTask(taskId);
        }
        if (!response.isSuccess) {
            console.error("error while delete task", response);
            alert(`Error! ${response.message}`);
            return;
        }

        await RefreshTaskLists();
    };

    return (
        <>
            <CListGroupItem className={`d-flex justify-content-between align-items-start position-relative task-item`} style={{border:"none"}}>
                <div className="me-2 d-flex align-items-start">
                    <CIcon
                        icon={cibVerizon}
                        className="mt-1"
                        onClick={() => onComplete(task.taskGroupId > 0 ? task.taskId : task.subTaskId, task.taskGroupId === 0)}
                        style={{ cursor: 'pointer' }}
                    />
                </div>
                <div className="flex-grow-1 text-wrap text-break" onClick={() => handleEditTask(task.taskGroupId > 0 ? task.taskId : task.subTaskId)} style={{ cursor: 'pointer' }}>
                    <div className="ms-4" style={{ fontWeight: '600' }}><del>{task.title}</del></div>
                    <div className="ms-4">{task.description}</div>
                    {
                        task.completeDate?.trim() && <div className="small mt-1"> Completed: {FormateDate(task.completeDate)}</div>
                    }
                </div>

                <div className="btn-gorup trash-icon-complete-section">
                    <button className="btn btn-undefined" type="button" onClick={() => handleDeleteTask(task.taskGroupId > 0 ? task.taskId : task.subTaskId)}>
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
                    isSubTask={task.subTaskId === 0}
                />
            )}
        </>
    )
}
export default CompletedTask;