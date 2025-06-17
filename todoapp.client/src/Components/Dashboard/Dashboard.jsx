import React, { useEffect, useState } from 'react'
import { GetGroupsTaskList } from '../../api/TaskGroupApi';
import { useTaskEvents } from '@/Hooks/TaskEvents';
import { GroupCard } from '../index';

const Dashboard = () => {
    const { allGroupTaskList, setAllGroupTaskList } = useTaskEvents();
    const [responseError, setResponseError] = useState(null);

    useEffect(() => {
        (async () => {
            const response = await GetGroupsTaskList();
            if (!response.isSuccess) {
                console.error("Failed or unexpected response:", response.message, response.data);
                setResponseError(`Error! ${response.message}`)
                return;
            }
            setAllGroupTaskList(response.data);
        })();
    }, []);


    return (
        <div className="row g-4">
            {
                responseError != null ? <div className="text-center text-danger"><h4>{responseError}</h4></div>
                    :
                    allGroupTaskList && allGroupTaskList.length > 0 &&
                    allGroupTaskList.map(groupItem => (
                        <GroupCard
                            key={groupItem.groupId}
                            group={groupItem}
                            isStarredList={false} />
                    ))
            }
        </div>
    )
}

export default Dashboard;
