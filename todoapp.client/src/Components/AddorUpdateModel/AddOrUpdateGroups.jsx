import { useState, useEffect } from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import { AddGroup, UpdateGroup, GetGroupById, GetGroups, GetGroupsTaskList, GetStarredTask } from '@/api/TaskGroupApi';
import { MoveTaskToNewGroup } from '../../api/TaskApi';
import { useTaskEvents } from '../../Hooks/TaskEvents';

const AddOrUpdateGroups = ({ visible, setVisibility, groupId, taskIdToMove }) => {

    const { setTaskGroups, setAllGroupTaskList, setallStarredTasks } = useTaskEvents();
    const [disable, setDisable] = useState(false);
    const [isShowError, setIsShowError] = useState(false);
    const [groupName, setGroupName] = useState('');
    const [groupData, setGroupData] = useState({});
    const [responseError, setResponseError] = useState(null);

    useEffect(() => {
        (async () => {
            if (groupId > 0) {
                let res = await GetGroupById(groupId);
                if (res.isSuccess) {

                    let itemToEdit = res.data;
                    setGroupName(itemToEdit.listName ?? '');
                    setGroupData(itemToEdit);
                }
            }

        })();
    }, []);


    const handleSubmit = async () => {
        setDisable(true);
        let response = {}

        if (taskIdToMove && taskIdToMove != null && taskIdToMove != undefined && taskIdToMove > 0) {
            response = await MoveTaskToNewGroup(taskIdToMove, { listId: 0, listName: groupName, isEnableShow: true, sortBy: 'My order' });
            if (!response.isSuccess) {
                console.error("error while moving task to new group ", response);
                setResponseError(response.message);
                setDisable(false);
                return;
            }

        } else {

            if (groupId > 0) {
                response = await UpdateGroup(groupId, { ...groupData, listName: groupName });
            } else {
                response = await AddGroup({ listId: 0, listName: groupName, isEnableShow: true, sortBy: 'My order' });
            }

            if (!response.isSuccess) {
                console.log("Error while adding or updating group", response);
                setResponseError(response.message);
                setDisable(false);
                return;
            }
        }

        setGroupName('');
        response.isSuccess && setVisibility(false);
        setDisable(false);
        await refreshGroupData();
    }

    /*handle add list*/
    const handleErrorAddGroup = (value) => {
        setGroupName(value);
        if (!value) {
            setIsShowError(true);
        } else {
            setIsShowError(false);
        }
    }

    const handleClose = () => {
        setVisibility(false);
        setGroupName('');
        setResponseError(null);
    }

    const refreshGroupData = async () => {
        const [groupRes, taskRes, starredRes] = await Promise.all([
            GetGroups(),
            GetGroupsTaskList(),
            GetStarredTask()
        ]);

        if (!groupRes?.isSuccess) {
            console.error("Group fetch failed:", groupRes?.message || "Unknown error", groupRes?.data);
            alert(`Error! ${groupRes.message}`);
        } else {
            setTaskGroups(groupRes.data);
        }

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
    }

    return (

        < Modal show={visible} onHide={() => handleClose()}>
            <Modal.Header closeButton>
                <Modal.Title>{groupId > 0 ? "Rename Group" : "Add Group"}</Modal.Title>
            </Modal.Header>
            {responseError && <p className=" text-center mt-2 mb-0 text-danger">{responseError}</p>}
            <Modal.Body>
                Name: <input type="text" autoFocus className="form-control" value={groupName} onChange={(e) => handleErrorAddGroup(e.target.value)} />
                <br />
                <span className={`${isShowError == false ? "d-none" : ""} text-danger`}>please enter value</span>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={() => handleClose()}>
                    Close
                </Button>
                <Button variant="primary" onClick={() => handleSubmit()} disabled={!groupName?.trim() || disable}>
                    Submit
                </Button>
            </Modal.Footer>
        </Modal >
    )
}

export default AddOrUpdateGroups;