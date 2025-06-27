
const BASE_URL = '/api/SubTasks';

export const GetSubTaskById = async (id) => {
    const response = await fetch(`${BASE_URL}/${id}`);
    return await response.json();
};

export const AddSubTask = async (item) => {
    const response = await fetch(`${BASE_URL}`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(item),
    });
    return await response.json();
};

export const UpdateSubTask = async (id, item) => {
    const response = await fetch(`${BASE_URL}/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(item),
    });
    return await response.json();
};

export const ToggleStarSubTask = async (subTaskId) => {
    const response = await fetch(`${BASE_URL}/${subTaskId}/star`, {
        method: 'PATCH'
    });
    return await response.json();
};

export const UpdateSubTaskCompletionStatus = async (subTaskId) => {
    const response = await fetch(`${BASE_URL}/${subTaskId}/complete`, {
        method: 'PATCH'
    });
    return await response.json();
};

export const DeleteSubTask = async (subTaskId) => {
    const response = await fetch(`${BASE_URL}/${subTaskId}`, {
        method: 'DELETE'
    });
    return await response.json();
};

export const MoveSubTaskToExistingGroup = async (subTaskId, groupId) => {
    const response = await fetch(`${BASE_URL}/${subTaskId}/move/${groupId}`, {
        method: 'PATCH'
    });
    return await response.json();
};

export const MoveSubTaskToNewGroup = async (subTaskId, groupItem) => {
    const response = await fetch(`${BASE_URL}/${subTaskId}/move`, {
        method: 'PATCH',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(groupItem),
    });
    return await response.json();
};


