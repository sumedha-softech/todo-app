import { useEffect } from 'react';
import { useTaskEvents } from '../../Hooks/TaskEvents';
import { GetStarredTask } from '../../api/TaskGroupApi';
import { GroupCard } from '../index';
import { useState } from 'react';

const Starred = () => {
    const { allStarredTasks, setallStarredTasks } = useTaskEvents();
    const [responseError, setResponseError] = useState(null);
    useEffect(() => {
        (async () => {
            const response = await GetStarredTask();
            if (!response.isSuccess) {
                console.error("Failed or unexpected response:", response);
                setResponseError(`Error! ${response.message}`);
                return;
            }
            setallStarredTasks(response.data);
        })();
    }, []);

    return (
        <div className="row g-4 mb-4">
            {
                responseError != null ? <div className="text-center text-danger"><h4>{responseError}</h4></div>
                    :
                    allStarredTasks &&
                    <GroupCard
                        key={allStarredTasks.groupId}
                        group={allStarredTasks}
                        isStarredList={true}
                    />}
        </div>
    )
}

export default Starred;