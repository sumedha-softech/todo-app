import { useState } from "react";
import CIcon from "@coreui/icons-react";
import { cilOptions, cilTrash, cilListLowPriority } from "@coreui/icons";
import { useTaskEvents } from "../../../Hooks/TaskEvents";
import { CDropdown, CDropdownToggle, CDropdownMenu, CDropdownItem, CDropdownDivider } from '@coreui/react'
import { DeleteTask, MoveTaskToExistingGroup } from "../../../api/TaskApi";
import { DeleteSubTask, MoveSubTaskToExistingGroup } from "../../../api/subTaskAPi";
import { AddOrUpdateGroups, AddOrUpdateTask } from "../../index";

const TaskActions = ({ task, isSubTask, groupId }) => {
    const { taskGroups, RefreshTaskLists } = useTaskEvents();
    const [taskIdForMove, setTaskIdForMove] = useState(0);
    const [visibleTaskModelPopup, setVisibleTaskModelPopup] = useState(false);
    const [visibleGroupModelPopup, setVisibleGroupModelPopup] = useState(false);

    // Delete task handler 
    const handleDeleteTask = async (taskId) => {
        let res = {};
        if (isSubTask) {
            res = await DeleteSubTask(taskId);
        } else {
            res = await DeleteTask(taskId);
        }

        if (!res.isSuccess) {
            console.error("error while delete task", res);
            return;
        }

        await RefreshTaskLists();
    };

    // handle move task to anothe created group
    const handleMoveTask = async (taskId, groupId) => {

        let res = {};
        if (isSubTask) {
            res = await MoveSubTaskToExistingGroup(taskId, groupId);
        } else {
            res = await MoveTaskToExistingGroup(taskId, groupId);
        }
        if (!res.isSuccess) {
            console.log("error while moving task to another group ", res);
            alert(res.message);
            return;
        }

        await RefreshTaskLists();
    }

    return (
        <>
            <CDropdown className="task-menu" alignment="end">
                <CDropdownToggle caret={false}>
                    <CIcon icon={cilOptions} />
                </CDropdownToggle>
                <CDropdownMenu>
                    {!isSubTask &&
                        <CDropdownItem onClick={() => setVisibleTaskModelPopup(true)}>
                            <CIcon icon={cilListLowPriority} /> Add sub Task</CDropdownItem>
                    }
                    <CDropdownItem onClick={() => handleDeleteTask(!isSubTask ? task.taskId : task.subTaskId)}> <CIcon icon={cilTrash} /> Delete</CDropdownItem>
                    <CDropdownDivider />

                    <CDropdownItem disabled><strong>My Task</strong></CDropdownItem>
                    {taskGroups &&
                        taskGroups.map(groupItem => (
                            <CDropdownItem key={groupItem.groupId} onClick={() => handleMoveTask(!isSubTask ? task.taskId : task.subTaskId, groupItem.groupId)}>
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