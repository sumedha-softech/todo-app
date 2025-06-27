import { useState} from 'react';
import { CListGroupItem, CFormCheck } from '@coreui/react';
import { AddOrUpdateTask, TaskActions } from '../../index';
import { FormateDate } from '../../../global/Helper';
import { cilStar } from '@coreui/icons';
import CIcon from '@coreui/icons-react';
import { cibMacys } from '@coreui/icons';
import { ToggleStarSubTask } from '@/api/subTaskApi';
import { useTaskEvents } from '@/Hooks/TaskEvents';


const SubTaskItem = ({ subTask, groupId, onComplete }) => {
    const { RefreshTaskLists } = useTaskEvents();
    const [editSubTaskId, setEditSubTaskId] = useState(null);
    const [visibleModel, setVisibleModel] = useState(false);

    const handleEditSubTask = (subTaskId) => {
        setVisibleModel(true);
        setEditSubTaskId(subTaskId);
    }

    // mark a sub task to star or undo
    const handleToggleStar = async (taskId) => {
        let res = await ToggleStarSubTask(taskId);
        if (!res.isSuccess) {
            console.error(`error while calling hendleUpdateStar message '${res.message}'`);
        } else {
            await RefreshTaskLists();
        }
    };

    return (
        <>
            <CListGroupItem style={{ marginLeft: "20px", borderBottom: "none" }} className={`d-flex justify-content-between align-items-start position-relative task-item`}>
                <CFormCheck type="radio" style={{ cursor: 'pointer', border: "1px solid cornflowerblue" }} onClick={() => onComplete(subTask.subTaskId,true)} />
            <div className="flex-grow-1 text-wrap text-break" onClick={() => handleEditSubTask(subTask.subTaskId)} style={{ cursor: 'pointer' }}>

                    <div className="ms-4" style={{ fontWeight: '600' }}>{subTask.title}</div>
                    <div className="ms-4">{subTask.description}</div>
                {
                        subTask.toDoDate?.trim() && <span className="badge bg-light text-dark mt-1 ms-4">{FormateDate(subTask.toDoDate)}</span>
                }
            </div>

            <div className="task-actions d-flex align-items-start">
                    <TaskActions task={subTask} isSubTask={true} groupId={groupId} />
                    <div className={`btn-gorup star-div ${subTask.isStarred ? 'always-visible' : ''}`}>
                        <button className={`btn btn-undefined `} type="button" onClick={() => handleToggleStar(subTask.subTaskId)}>
                            <CIcon icon={subTask.isStarred ? cibMacys : cilStar} className="ms-2 action-icon" />
                    </button>
                </div>
            </div>
            </CListGroupItem>

            {visibleModel && editSubTaskId && (
                <AddOrUpdateTask
                    visible={visibleModel}
                    setVisibility={setVisibleModel}
                    taskId={editSubTaskId}
                    setTaskId={setEditSubTaskId}
                    isStarredTask={subTask.isStarred}
                    isSubTask={true}
                />
            )}
        </>

    );
}

export default SubTaskItem;