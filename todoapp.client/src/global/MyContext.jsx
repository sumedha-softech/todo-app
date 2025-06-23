import React, { createContext, useEffect, useState } from 'react';

export const Context = createContext();

export const MyContextProvider = ({ children }) => {
    const [taskGroups, setTaskGroups] = useState([]);
    const [allGroupTaskList, setAllGroupTaskList] = useState([]);
    const [allStarredTasks, setallStarredTasks] = useState({});
    const [sidebarShow, setSidebarShow] = useState(true);
    const [unfoldable, setUnfoldable] = useState(false);
    const [theme, setTheme] = useState('light');
    const [searchTerm, setSearchTerm] = useState("")
    const [searchedTask, setSearchedTask] = useState([])

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
                setSearchTerm
            }}>
            {children}
        </Context.Provider>
    );

}  