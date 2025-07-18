import { useContext } from 'react';
import { Context } from '../global/MyContext';

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
        setSearchTerm,
        recentActionItem, setRecentActionItem,
        handleUndo,
        refreshTaskLists
    } = useContext(Context);

    return {
        //handlers
        refreshTaskLists,
        handleUndo,
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
        setSearchTerm,
        recentActionItem, setRecentActionItem
    };
}