import { useState } from 'react';
import CIcon from "@coreui/icons-react";
import { UpdateSortBy, DeleteGroup, DeleteCompletedTask } from '../../../api/TaskGroupApi';
import { CDropdown, CDropdownToggle, CDropdownMenu, CDropdownItem, CDropdownDivider } from '@coreui/react'
import { useTaskEvents } from '../../../Hooks/TaskEvents';
import { cilOptions } from '@coreui/icons';
import { AddOrUpdateGroups } from '../../index';

const GroupActions = ({ group, isStarredList }) => {
    const orderList = ["My order", "Date", "Title", "Description"];
    const { RefreshTaskLists, setAllGroupTaskList, allGroupTaskList, setTaskGroups, taskGroups } = useTaskEvents();
    const [visibleModel, setVisibleModel] = useState(false);
    const [groupId, setGroupId] = useState(0);

    const handleSort = async (option, groupId) => {

        const response = await UpdateSortBy(groupId, option);
        if (!response.isSuccess) {
            console.log("error while calling api for save group", response);
            return;
        }

        await RefreshTaskLists();

    };

    const handleRenameGroup = (groupId) => {
        setVisibleModel(true);
        setGroupId(groupId);
    }

    // Delete a goroup 
    const handleDeleteGroup = async (groupId) => {
        const res = await DeleteGroup(groupId);
        if (!res.isSuccess) {
            console.error("error while delete group", res);
            return;
        }

        const updatedGroupTaskList = allGroupTaskList.filter(group => group.groupId !== groupId);
        setAllGroupTaskList(updatedGroupTaskList);
        const updatedTaskGroups = taskGroups.filter(group => group.groupId !== groupId);
        setTaskGroups(updatedTaskGroups);
    };

    const handleDeleteCompletedTask = async (groupId) => {
        const confirmed = confirm('Are you sure you want to delete all completed task ?');
        if (confirmed) {

            const res = await DeleteCompletedTask(groupId);
            if (!res.isSuccess) {
                console.error("error while delete group", res);
                return;
            }

            const updatedGroupTaskList = allGroupTaskList.map(group =>
                group.groupId === groupId ? { ...group, completedTaskList: [] } :
                    { ...group }
            );
            setAllGroupTaskList(updatedGroupTaskList);
        }
        return;
    }

    return (
        <>
            <CDropdown alignment="end">
                <CDropdownToggle caret={false}>
                    <CIcon icon={cilOptions} />
                </CDropdownToggle>
                <CDropdownMenu>

                    <CDropdownItem disabled><strong>Sort by</strong></CDropdownItem>
                    {
                        orderList.map(order => (
                            <CDropdownItem
                                key={order}
                                onClick={() => handleSort(order, group.groupId)}
                                style={{ fontWeight: group.sortBy === order ? "bold" : "normal" }} >
                                {order}
                            </CDropdownItem>
                        ))
                    }

                    {!isStarredList &&
                        <>
                            <CDropdownDivider />
                            <CDropdownItem onClick={() => handleRenameGroup(group.groupId)}>Rename Group</CDropdownItem>
                            <CDropdownItem disabled={group.groupId === 1} onClick={() => handleDeleteGroup(group.groupId)}>Delete Group</CDropdownItem>
                            {
                                group.completedTaskList && group.completedTaskList?.length > 0 &&
                                <CDropdownItem color="danger" onClick={() => handleDeleteCompletedTask(group.groupId)}>Delete all completed tasks</CDropdownItem>
                            }

                        </>
                    }

                </CDropdownMenu>
            </CDropdown>

            {visibleModel &&
                <AddOrUpdateGroups
                    visible={visibleModel}
                    setVisibility={setVisibleModel}
                    groupId={groupId}
                    taskIdToMove={0}
                />
            }
        </>
    )
}
export default GroupActions; 