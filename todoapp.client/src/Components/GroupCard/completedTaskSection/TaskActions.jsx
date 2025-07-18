import { useState } from "react";
import CIcon from "@coreui/icons-react";
import { cilOptions, cilTrash, cilListLowPriority } from "@coreui/icons";
import { useTaskEvents } from "../../../Hooks/TaskEvents";
import { CDropdown, CDropdownToggle, CDropdownMenu, CDropdownItem, CDropdownDivider } from '@coreui/react'
import { DeleteTask, MoveTaskToExistingGroup } from "../../../api/TaskApi";
import { DeleteSubTask, MoveSubTaskToExistingGroup } from "../../../api/subTaskAPi";
import { AddOrUpdateGroups, AddOrUpdateTask } from "../../index";

const TaskActions = ({ task, isSubTask, groupId, isStarredDashboard }) => {
    const { taskGroups, refreshTaskLists, setRecentActionItem } = useTaskEvents();
    const [taskIdForMove, setTaskIdForMove] = useState(0);
    const [visibleTaskModelPopup, setVisibleTaskModelPopup] = useState(false);
    const [visibleGroupModelPopup, setVisibleGroupModelPopup] = useState(false);

    // Delete task handler 
    const handleDeleteTask = async () => {
        let res = {};
        if (isSubTask || (task.subTaskId && task.subTaskId > 0)) {
            res = await DeleteSubTask(task.subTaskId);
        } else {
            res = await DeleteTask(task.taskId);
        }

        if (!res.isSuccess) {
            console.error("error while delete task", res);
            return;
        }
        await refreshTaskLists();

        setRecentActionItem({ action: 'delete' });

        // Auto clear after 3 sec
        setTimeout(() => {
            setRecentActionItem(false);
        }, 5000);
    };

    // handle move task to anothe created group
    const handleMoveTask = async (groupId) => {

        let res = {};
        let itemToSet = {};
        if (isSubTask) {
            res = await MoveSubTaskToExistingGroup(task.subTaskId, groupId);
            itemToSet = { action: 'move', item: 'sub-task' };
        } else {
            res = await MoveTaskToExistingGroup(task.taskId, groupId);
            itemToSet = { action: 'move', item: 'task' };
        }
        if (!res.isSuccess) {
            console.log("error while moving task to another group ", res);
            alert(res.message);
            return;
        }

        await refreshTaskLists();
        setRecentActionItem(itemToSet);
        // Auto clear after 3 sec
        setTimeout(() => {
            setRecentActionItem(false);
        }, 3000);
    }

    return (
        <>
            <CDropdown className="task-menu" alignment="end">
                <CDropdownToggle caret={false}>
                    <CIcon icon={cilOptions} />
                </CDropdownToggle>
                <CDropdownMenu>
                    {!isSubTask && !isStarredDashboard &&
                        <CDropdownItem onClick={() => setVisibleTaskModelPopup(true)}>
                            <CIcon icon={cilListLowPriority} /> Add sub Task</CDropdownItem>
                    }
                    <CDropdownItem onClick={() => handleDeleteTask()}> <CIcon icon={cilTrash} /> Delete</CDropdownItem>
                    <CDropdownDivider />

                    <CDropdownItem disabled><strong>My Task</strong></CDropdownItem>
                    {taskGroups &&
                        taskGroups.map(groupItem => (
                            <CDropdownItem key={groupItem.groupId} onClick={() => handleMoveTask(groupItem.groupId)}>
                                {groupId === groupItem.groupId ? <strong>{groupItem.groupName} </strong> : groupItem.groupName}
                            </CDropdownItem>
                        ))
                    }

                    <CDropdownItem onClick={() => {
                        setTaskIdForMove(!isSubTask ? task.taskId : task.subTaskId);
                        setVisibleGroupModelPopup(true);
                    }}>
                        New group
                    </CDropdownItem>
                </CDropdownMenu>
            </CDropdown>

            {visibleTaskModelPopup && !isSubTask && (
                <AddOrUpdateTask
                    visible={visibleTaskModelPopup}
                    setVisibility={setVisibleTaskModelPopup}
                    taskId={task.taskId}
                    isStarredTask={false}
                    isSubTask={true}
                    isRequestForSubTaskAdd={true}
                />
            )}
            {visibleGroupModelPopup && taskIdForMove && taskIdForMove > 0 &&
                <AddOrUpdateGroups
                    visible={visibleGroupModelPopup}
                    setVisibility={setVisibleGroupModelPopup}
                    groupId={0}
                    taskIdToMove={taskIdForMove}
                    isSubTask={isSubTask}
                />
            }
        </>
    )
}
export default TaskActions;