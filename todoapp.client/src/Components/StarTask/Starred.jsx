import { useEffect } from 'react';
import { useTaskEvents } from '../../Hooks/TaskEvents';
import { GetStarredTask } from '../../api/TaskGroupApi';
import { GroupCard } from '../index';
import { useState } from 'react';
import { CSpinner } from '@coreui/react'

const Starred = () => {
    const { allStarredTasks, setallStarredTasks } = useTaskEvents();
    const [responseError, setResponseError] = useState(null);
    const [isLoading, setIsLoading] = useState(false);

    useEffect(() => {
        (async () => {
            setIsLoading(true);
            const response = await GetStarredTask();
            if (!response.isSuccess) {
                console.error("Failed or unexpected response:", response);
                setResponseError(`Error! ${response.message}`);
                setIsLoading(false);
                return;
            }
            setIsLoading(false);
            setallStarredTasks(response.data);
        })();
    }, []);

    return (
        <div className="row g-4 mb-4">
            {
                isLoading
                    ? <div className="text-center" style={{ paddingTop: '200px' }}> <CSpinner style={{ width: '3rem', height: '3rem' }} /></div>
                    : responseError != null
                        ? <div className="text-center text-danger"><h4>{responseError}</h4></div>
                        : allStarredTasks
                            ? <GroupCard key={allStarredTasks.groupId} group={allStarredTasks} isStarredList={true} />
                            :
                            <h2 className="text-center">No Record Found!!</h2>
            }
        </div>
    )
}

export default Starred;