import { useContext } from 'react';
import { Context } from '../global/MyContext';
import { GetGroupsTaskList, GetStarredTask, } from '../api/TaskGroupApi';

export function useTaskEvents() {

    const {
        allGroupTaskList,
        setAllGroupTaskList,
        allStarredTasks,
        setallStarredTasks,
        taskGroups,
        setTaskGroups,
        searchedTask,
        setSearchedTask,
        searchTerm,
        setSearchTerm
    } = useContext(Context);

    // Refresh groups task list and starred task list
    const RefreshTaskLists = async () => {
        try {
            const [groupRes, starredRes] = await Promise.all([
                GetGroupsTaskList(),
                GetStarredTask()
            ]);

            if (!groupRes?.isSuccess) {
                console.error("Group tasks fetch failed:", groupRes?.message || "Unknown error", groupRes?.data);
                alert(`Error! ${groupRes.message}`);
            } else {
                setAllGroupTaskList(groupRes.data);
            }

            if (!starredRes?.isSuccess) {
                console.error("Starred tasks fetch failed:", starredRes?.message || "Unknown error", starredRes?.data);
                alert(`Error! ${starredRes.message}`);
            } else {
                setallStarredTasks(starredRes.data);
            }

        } catch (error) {
            console.error("Unexpected error during RefreshTaskLists:", error);
            alert(`Error! ${error}`);
        }
    };

    return {
        //handlers
        RefreshTaskLists,
        //context values
        allGroupTaskList,
        setAllGroupTaskList,
        allStarredTasks,
        setallStarredTasks,
        taskGroups,
        setTaskGroups,
        searchedTask,
        setSearchedTask,
        searchTerm,
        setSearchTerm
    };
}