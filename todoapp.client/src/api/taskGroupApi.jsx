// src/api/taskGroupApi.jsx

const BASE_URL = '/api/TaskGroups';

export const GetGroups = async () => {
    const response = await fetch(`${BASE_URL}`);
    return await response.json();
};

export const AddGroup = async (item) => {
    const response = await fetch(`${BASE_URL}`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(item),
    });
    return await response.json();

};

export const UpdateGroup = async (id, item) => {
    const response = await fetch(`${BASE_URL}/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(item),
    });

    return await response.json();;

};

export const GetGroupById = async (id) => {
    const response = await fetch(`${BASE_URL}/${id}`);
    return await response.json();

};

export const DeleteGroup = async (id) => {
    const response = await fetch(`${BASE_URL}/${id}`, {
        method: 'DELETE',
    });
    return await response.json();

}

export const GetGroupsTaskList = async () => {
    const response = await fetch(`${BASE_URL}/tasks`);
    return await response.json();

}

export const GetStarredTask = async () => {
    const response = await fetch(`${BASE_URL}/tasks/star`);
    return await response.json();

};

export const DeleteCompletedTask = async (groupId) => {
    const response = await fetch(`${BASE_URL}/${groupId}/complete`,
        { method: 'DELETE' });
    return await response.json();

};

export const UpdateGroupVisibility = async (groupId, isVisible) => {
    const response = await fetch(`${BASE_URL}/${groupId}/visibility/${isVisible}`,
        { method: 'PATCH' });
        return await response.json();
};

export const UpdateSortBy = async (groupId, sortBy) => {
    const response = await fetch(`${BASE_URL}/${groupId}/sortBy/${sortBy}`,
        { method: 'PATCH' });
        return await response.json();
};
