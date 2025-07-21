import React, { useEffect, useState } from 'react'
import { GetGroupsTaskList } from '../../api/TaskGroupApi';
import { useTaskEvents } from '@/Hooks/TaskEvents';
import { GroupCard } from '../index';
import ScrollContainer from 'react-indiana-drag-scroll'
import { CSpinner } from '@coreui/react'

const Dashboard = () => {
    const { allGroupTaskList, setAllGroupTaskList, searchedTask, searchTerm } = useTaskEvents();
    const [responseError, setResponseError] = useState(null);
    const [isLoading, setIsLoading] = useState(false);

    useEffect(() => {
        (async () => {
            setIsLoading(true);
            const response = await GetGroupsTaskList();
            if (!response.isSuccess) {
                console.error("Failed or unexpected response:", response.message, response.data);
                setResponseError(`Error! ${response.message}`)
                setIsLoading(false);
                return;
            }
            setIsLoading(false);
            setAllGroupTaskList(response.data);
        })();
    }, []);

    return (
        <ScrollContainer className="scroll-container" vertical={false} hideScrollbars={false} style={{ height: "100vh", width: '100%' }}>
            <div className="row g-4 mb-4 flex-nowrap" style={{width:'100%'}}>
                {
                    isLoading ? <div className="text-center" style={{ paddingTop: '200px' }}> <CSpinner style={{ width: '3rem', height: '3rem' }} /></div> : 
                    responseError != null ? <div className="text-center text-danger"><h4>{responseError}</h4></div>
                        :
                        allGroupTaskList && allGroupTaskList.length > 0 && !searchTerm.trim() ?
                            allGroupTaskList.map(groupItem => (
                                <GroupCard
                                    key={groupItem.groupId}
                                    group={groupItem}
                                    isStarredList={false} />
                            ))
                            :
                            searchTerm.trim() && searchedTask && searchedTask.length > 0 ? searchedTask.map(groupItem => (
                                <GroupCard
                                    key={groupItem.groupId}
                                    group={groupItem}
                                    isStarredList={false} />
                            ))
                            :
                            <h2 className="text-center">No Record Found!!</h2>
                }
            </div>
        </ScrollContainer>
    )
}

export default Dashboard;
