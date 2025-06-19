import { useState } from "react";
import CIcon from "@coreui/icons-react";
import { cilOptions, cilTrash, cilCheck } from "@coreui/icons";
import { useTaskEvents } from "../../../Hooks/TaskEvents";
import { CDropdown, CDropdownToggle, CDropdownMenu, CDropdownItem, CDropdownDivider } from '@coreui/react'
import { DeleteTask, MoveTaskToExistingGroup } from "../../../api/TaskApi";
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

        const res = await MoveTaskToExistingGroup(taskId, groupId);
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
                    <CDropdownItem onClick={() => handleDeleteTask(task.taskId)}> <CIcon icon={cilTrash} /> Delete</CDropdownItem>
                    <CDropdownDivider />

                    <CDropdownItem disabled><strong>My Task</strong></CDropdownItem>
                    {taskGroups &&
                        taskGroups.map(groupItem => (
                            <CDropdownItem key={groupItem.groupId} onClick={() => handleMoveTask(task.taskId, groupItem.groupId)}>
                                {task.taskGroupId === groupItem.groupId ? <CIcon icon={cilCheck} /> : ""}
                                {groupItem.groupName}
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