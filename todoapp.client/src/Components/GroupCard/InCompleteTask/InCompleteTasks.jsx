import { useState } from 'react';
import CIcon from '@coreui/icons-react';
import { cilStar, cibMacys } from '@coreui/icons'
import { FormateDate } from '../../../global/Helper';
import { ToggleStarTask } from '../../../api/TaskApi';
import { useTaskEvents } from '../../../Hooks/TaskEvents';
import { AddOrUpdateTask, TaskActions } from '../../index';
import { CListGroup, CListGroupItem, CFormCheck } from '@coreui/react'

const InCompleteTasks = ({ groupId, task, onComplete }) => {
    const { RefreshTaskLists } = useTaskEvents();
    const [editTaskId, setEditTaskId] = useState(0);
    const [visibleModel, setVisibleModel] = useState(false);

    const handleEditTask = (taskId) => {
        setEditTaskId(taskId);
        setVisibleModel(true);
    }

    // mark a task to star or undo
    const handleToggleStar = async (taskId) => {
        let res = await ToggleStarTask(taskId);
        if (!res.isSuccess) {
            console.error(`error while calling hendleUpdateStar message '${res.message}'`);
        } else {
            await RefreshTaskLists();
        }
    };

    return (
        <>
            <CListGroupItem className={`d-flex justify-content-between align-items-start position-relative task-item `}>
                <CFormCheck type="radio" style={{ cursor: 'pointer' }} onClick={() => onComplete(task, true)} />
                <div className="flex-grow-1 text-wrap text-break" onClick={() => handleEditTask(task.taskId)} style={{ cursor: 'pointer' }}>

                    <div className="ms-4">{task.title}</div>
                    <div className="ms-4">{task.description}</div>
                    {
                        task.toDoDate?.trim() && <span className="badge bg-light text-dark mt-1 ms-4">{FormateDate(task.toDoDate)}</span>
                    }
                </div>

                <div className="task-actions d-flex align-items-start">
                    <TaskActions task={task} />
                    <div className={`btn-gorup star-div ${task.isStarred ? 'always-visible' : ''}`}>
                        <button className={`btn btn-undefined `} type="button" onClick={() => handleToggleStar(task.taskId)}>
                            <CIcon icon={task.isStarred ? cibMacys :cilStar} className="ms-2 action-icon" />
                        </button>
                    </div>
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
export default InCompleteTasks;