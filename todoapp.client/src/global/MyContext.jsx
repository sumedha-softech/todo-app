import React, { createContext, useEffect, useState } from 'react';
import { GetGroups, GetGroupsTaskList, GetStarredTask, UndoDeleteItem } from '../api/TaskGroupApi';
import { UndoRecentMovedTask } from '../api/taskApi';
import { UndoRecentMovedSubTask } from '../api/subTaskAPi';

export const Context = createContext();

export const MyContextProvider = ({ children }) => {
    const [taskGroups, setTaskGroups] = useState([]);
    const [allGroupTaskList, setAllGroupTaskList] = useState([]);
    const [allStarredTasks, setallStarredTasks] = useState({});
    const [sidebarShow, setSidebarShow] = useState(true);
    const [unfoldable, setUnfoldable] = useState(false);
    const [theme, setTheme] = useState('light');
    const [searchTerm, setSearchTerm] = useState("");
    const [searchedTask, setSearchedTask] = useState([]);
    const [recentActionItem, setRecentActionItem] = useState(null);

    // global task search
    useEffect(() => {
        if (searchTerm?.trim()) {
            let filteredGroupTasks = allGroupTaskList
                .map(groupWithTask => {
                    const filteredTask = groupWithTask.taskList?.filter(x =>
                        (x.title && x.title.toLowerCase().includes(searchTerm.toLowerCase())) ||
                        (x.description && x.description.toLowerCase().includes(searchTerm.toLowerCase())) ||
                        (x.subTasks && x.subTasks.some(subTask =>
                            (subTask.title && subTask.title.toLowerCase().includes(searchTerm.toLowerCase())) ||
                            (subTask.description && subTask.description.toLowerCase().includes(searchTerm.toLowerCase()))
                        ))
                    );

                    const filteredCompletedTask = groupWithTask.completedTaskList?.filter(x =>
                        (x.title && x.title.toLowerCase().includes(searchTerm.toLowerCase())) ||
                        (x.description && x.description.toLowerCase().includes(searchTerm.toLowerCase())) ||
                        (x.subTasks && x.subTasks.some(subTask =>
                            (subTask.title && subTask.title.toLowerCase().includes(searchTerm.toLowerCase())) ||
                            (subTask.description && subTask.description.toLowerCase().includes(searchTerm.toLowerCase()))
                        ))
                    );

                    // Check if the group name matches the search term  
                    const isGroupNameMatch = groupWithTask.groupName?.toLowerCase().includes(searchTerm.toLowerCase());

                    // Include the group if either the group name matches or it has filtered tasks  
                    if (isGroupNameMatch || (filteredTask && filteredTask.length > 0) || (filteredCompletedTask && filteredCompletedTask.length > 0)) {
                        return { ...groupWithTask, taskList: filteredTask, completedTaskList: filteredCompletedTask };
                    }

                    return null;
                })
                .filter(groupWithTask => groupWithTask !== null); // Exclude null groups  

            console.log(filteredGroupTasks);
            setSearchedTask(filteredGroupTasks);
        }

    }, [allGroupTaskList, searchTerm])

    const handleUndo = async () => {
        setRecentActionItem(null);

        if (!recentActionItem) {
            alert("item expired.");
            return;
        }

        let action = recentActionItem.action;
        switch (action) {
            case 'delete':
                await UndoDeleteItem();
                break;

            case 'move':
                if (recentActionItem.item === 'task')
                    await UndoRecentMovedTask();
                else
                    await UndoRecentMovedSubTask();
                break;

            default:
                console.warn(`Unknown action type: ${action}`);
                return;
        }
        await refreshTaskLists();
    }

    // Refresh groups task list and starred task list
    const refreshTaskLists = async () => {
        try {
            const [taskRes, starredRes, groupRes] = await Promise.all([
                GetGroupsTaskList(),
                GetStarredTask(),
                GetGroups(),
            ]);

            if (!taskRes?.isSuccess) {
                console.error("Group tasks fetch failed:", taskRes?.message || "Unknown error", taskRes?.data);
                alert(`Error! ${taskRes.message}`);
            } else {
                setAllGroupTaskList(taskRes.data);
            }

            if (!starredRes?.isSuccess) {
                console.error("Starred tasks fetch failed:", starredRes?.message || "Unknown error", starredRes?.data);
                alert(`Error! ${starredRes.message}`);
            } else {
                setallStarredTasks(starredRes.data);
            }

            if (!groupRes?.isSuccess) {
                console.error("Group fetch failed:", groupRes?.message || "Unknown error", groupRes?.data);
                alert(`Error! ${groupRes.message}`);
            } else {
                setTaskGroups(groupRes.data);
            }

        } catch (error) {
            console.error("Unexpected error during refreshTaskLists:", error);
            alert(`Error! ${error}`);
        }
    };

    return (
        <Context.Provider
            value={{
                theme,
                setTheme,
                sidebarShow,
                setSidebarShow,
                unfoldable,
                setUnfoldable,
                taskGroups,
                setTaskGroups,
                allGroupTaskList,
                setAllGroupTaskList,
                allStarredTasks,
                setallStarredTasks,
                searchedTask,
                setSearchedTask,
                searchTerm,
                setSearchTerm,
                recentActionItem, setRecentActionItem,
                handleUndo,
                refreshTaskLists
            }}>
            {children}
        </Context.Provider>
    );

}  