
const BASE_URL = '/api/Tasks';

export const GetTaskList = async () => {
    const response = await fetch(`${BASE_URL}`);
    return await response.json();
};

export const GetTaskById = async (id) => {
    const response = await fetch(`${BASE_URL}/${id}`);
    return await response.json();
};

export const AddTask = async (item) => {
    const response = await fetch(`${BASE_URL}`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(item),
    });
    return await response.json();
};

export const UpdateTask = async (id, item) => {
    const response = await fetch(`${BASE_URL}/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(item),
    });
    return await response.json();
};

export const ToggleStarTask = async (taskId) => {
    const response = await fetch(`${BASE_URL}/${taskId}/star`, {
        method: 'PATCH'
    });
    return await response.json();
};

export const UpdateTaskCompletionStatus = async (taskId) => {
    const response = await fetch(`${BASE_URL}/${taskId}/complete`, {
        method: 'PATCH'
    });
    return await response.json();
};

export const DeleteTask = async (taskId) => {
    const response = await fetch(`${BASE_URL}/${taskId}`, {
        method: 'DELETE'
    });
    return await response.json();
};

export const MoveTaskToExistingGroup = async (taskId, groupId) => {
    const response = await fetch(`${BASE_URL}/${taskId}/move/${groupId}`, {
        method: 'PATCH'
    });
    return await response.json();
};

export const MoveTaskToNewGroup = async (taskId, groupItem) => {
    const response = await fetch(`${BASE_URL}/${taskId}/move`, {
        method: 'PATCH',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(groupItem),
    });
    return await response.json();
};


