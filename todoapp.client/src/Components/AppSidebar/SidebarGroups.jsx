import React, { useState } from 'react'
import { cilTask, cilSquare, cilStorage } from '@coreui/icons'
import CIcon from '@coreui/icons-react'
import { CNavGroup, CNavItem, CBadge, CNavLink, } from '@coreui/react'
import { useTaskEvents } from '../../Hooks/TaskEvents'
import { useEffect } from 'react'
import { GetGroups, UpdateGroupVisibility } from '../../api/TaskGroupApi'

const SidebarGroups = () => {
    const { taskGroups, setTaskGroups, allGroupTaskList, setAllGroupTaskList } = useTaskEvents();
    const [responseError, setResponseError] = useState(null);


    useEffect(() => {
        (async () => {
            const response = await GetGroups();
            if (!response.isSuccess) {
                console.error("Failed to fetch groups:", response.message);
                setResponseError(response.message);
                return;
            }
            setTaskGroups(response.data);
        })();
    }, []);

    const HandleVisibilityCheck = async (groupId, isVisible) => {
        const response = await UpdateGroupVisibility(groupId, isVisible);
        if (!response.isSuccess) {
            console.error("Failed to update group visibility:", response.message);
            alert(`Error! ${response.message}`);
            return;
        }

        const updatedTaskGroups = taskGroups.map(item => {
            if (item.groupId === groupId) {
                return { ...item, isEnableShow: isVisible }
            } else {
                return item;
            }
        });
        setTaskGroups(updatedTaskGroups);

        if (allGroupTaskList && allGroupTaskList?.length > 0) {

            const updatedGroupsTaskList = allGroupTaskList.map(group => {
                const isGroupVisible = updatedTaskGroups.find(item => item.groupId === group.groupId)?.isEnableShow;
                if (group.groupId === groupId) {
                    return { ...group, isEnableShow: isGroupVisible };
                } else {
                    return { ...group }
                }
            })
            setAllGroupTaskList(updatedGroupsTaskList);
        }
    }

    const countTask = (groupId) => {
        if (!allGroupTaskList || allGroupTaskList.length === 0) return 0;
        const group = allGroupTaskList.find(item => item.groupId === groupId);
        return group && group.taskList.length;
    }

    return (
        <CNavGroup compact as="div"
            toggler={<><CIcon icon={cilStorage}
                customClassName="nav-icon"
            /> <span>Groups</span></>}>
            {
                responseError != null ? <li><p className="pl-1 mt-2 mb-0 text-danger">{responseError}</p></li>
                    :
                    taskGroups.length > 0 && taskGroups.map((item) => (
                        <CNavItem as="div" key={item.groupId}>
                            <CNavLink href="#" style={{ cursor: 'pointer' }} onClick={() => HandleVisibilityCheck(item.groupId, !item.isEnableShow)}>
                                <CIcon
                                    icon={item.isEnableShow ? cilTask : cilSquare}
                                    customClassName="nav-icon" />
                                {item.groupName}
                                <CBadge color="secondary" className="ms-auto" size="sm">
                                    {countTask(item.groupId)}
                                </CBadge>
                            </CNavLink>
                        </CNavItem>
                    ))
            }
        </CNavGroup>
    )
}

export default SidebarGroups