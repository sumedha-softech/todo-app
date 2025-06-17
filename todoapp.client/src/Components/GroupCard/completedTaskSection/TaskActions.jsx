import { useState } from "react";
import CIcon from "@coreui/icons-react";
import { cilOptions, cilTrash, cilCheck } from "@coreui/icons";
import { useTaskEvents } from "../../../Hooks/TaskEvents";
import { CDropdown, CDropdownToggle, CDropdownMenu, CDropdownItem, CDropdownDivider } from '@coreui/react'
import { DeleteTask, GetTaskById, UpdateTask } from "../../../api/TaskApi";
import { AddOrUpdateGroups } from "../../index";

const TaskActions = ({ task }) => {
    const { taskGroups, RefreshTaskLists } = useTaskEvents();
    const [taskIdForMove, setTaskIdForMove] = useState(0);
    const [visibleModelPopup, setVisibleModelPopup] = useState(false);

    // Delete task handler 
    const handleDeleteTask = async (taskId) => {
        const res = await DeleteTask(taskId);
        if (!res.isSuccess) {
            console.error("error while delete task", res);
            return;
        }

        await RefreshTaskLists();
    };

    // handle move task to anothe created group
    const handleMoveTask = async (taskId, groupId) => {
        const task = await GetTaskById(taskId);

        if (!task.isSuccess) {
            console.log("error while getting task for move to another group ", res);
            return;
        }

        const res = await UpdateTask(taskId, { ...task.data, taskGroupId: groupId });
        if (!res.isSuccess) {
            console.log("error while moving task to another group ", res);
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
                    <CDropdownItem> <CIcon icon={cilTrash} onClick={() => handleDeleteTask(task.taskId)} /> Delete</CDropdownItem>
                    <CDropdownDivider />

                    <CDropdownItem disabled><strong>My Task</strong></CDropdownItem>
                    {taskGroups &&
                        taskGroups.map(groupItem => (
                            <CDropdownItem key={groupItem.listId} onClick={() => handleMoveTask(task.taskId, groupItem.listId)}>
                                {task.taskGroupId === groupItem.listId ? <CIcon icon={cilCheck} /> : ""}
                                {groupItem.listName}
                            </CDropdownItem>
                        ))
                    }


                    <CDropdownItem onClick={() => {
                        setTaskIdForMove(task.taskId);
                        setVisibleModelPopup(true);
                    }}>
                        new group
                    </CDropdownItem>
                </CDropdownMenu>
            </CDropdown>
            {visibleModelPopup &&
                <AddOrUpdateGroups
                    visible={visibleModelPopup}
                    setVisibility={setVisibleModelPopup}
                    groupId={0}
                    taskIdToMove={taskIdForMove}
                />
            }
        </>
    )
}
export default TaskActions;