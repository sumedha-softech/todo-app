// src/api/taskGroupApi.jsx

const BASE_URL = '/TaskGroups';

export const GetGroups = async () => {
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

export const AddGroup = async (item) => {
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

export const UpdateGroup = async (id, item) => {
    try {
        const response = await fetch(`${BASE_URL}/${id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(item),
        });

        return await response.json();;
    } catch (e) {
        return {
            isSuccess: false,
            message: e.message,
        };
    }
};

export const GetGroupById = async (id) => {
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

export const DeleteGroup = async (id) => {
    try {
        const response = await fetch(`${BASE_URL}/${id}`, {
            method: 'DELETE',
        });
        return await response.json();
    } catch (e) {
        return {
            isSuccess: false,
            message: e.message,
        };
    }
}

export const GetGroupsTaskList = async () => {
    try {
        const response = await fetch(`${BASE_URL}/tasks`);
        return await response.json();
    } catch (e) {
        return {
            isSuccess: false,
            message: e.message,
        };
    }
}

export const GetStarredTask = async () => {
    try {
        const response = await fetch(`${BASE_URL}/tasks/star`);
        return await response.json();
    } catch (e) {
        return {
            isSuccess: false,
            message: e.message,
        };
    }

};

export const DeleteCompletedTask = async (groupId) => {
    try {
        const response = await fetch(`${BASE_URL}/${groupId}/complete`, { method: 'DELETE' });
        return await response.json();
    } catch (e) {
        return {
            isSuccess: false,
            message: e.message,
        };
    }
};
