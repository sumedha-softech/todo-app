
const BASE_URL = '/Tasks';

export const GetTaskList = async () => {
    try {
        const response = await fetch(`${BASE_URL}`);
        return await response.json();
    } catch (e) {
        return {
            isSuccess: false,
            message: e.message,
        };
    }
};

export const GetTaskById = async (id) => {
    try {
        const response = await fetch(`${BASE_URL}/${id}`);
        return await response.json();
    } catch (e) {
        return {
            isSuccess: false,
            message: e.message,
        };
    }
};

export const AddTask = async (item) => {
    try {
        const response = await fetch(`${BASE_URL}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(item),
        });
        return await response.json();
    } catch (e) {
        return {
            isSuccess: false,
            message: e.message,
        };
    }
};

export const UpdateTask = async (id, item) => {
    try {
        const response = await fetch(`${BASE_URL}/${id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(item),
        });
        return await response.json();
    } catch (e) {
        return {
            isSuccess: false,
            message: e.message,
        };
    }
};

export const ToggleStarTask = async (taskId) => {
    try {
        const response = await fetch(`${BASE_URL}/${taskId}/star`, {
            method: 'PATCH'
        });
        return await response.json();

    } catch (e) {
        return {
            isSuccess: false,
            message: e.message,
        };
    }
};

export const DeleteTask = async (taskId) => {
    try {
        const response = await fetch(`${BASE_URL}/${taskId}`, {
            method: 'DELETE'
        });
        return await response.json();
    } catch (e) {
        return {
            isSuccess: false,
            message: e.message,
        };
    }
};

export const MoveTaskToNewGroup = async (taskId, groupItem) => {
    try {
        const response = await fetch(`${BASE_URL}/${taskId}/move`, {
            method: 'PATCH',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(groupItem),
        });
        return await response.json();
    } catch (e) {
        return {
            isSuccess: false,
            message: e.message,
        };
    }
};


