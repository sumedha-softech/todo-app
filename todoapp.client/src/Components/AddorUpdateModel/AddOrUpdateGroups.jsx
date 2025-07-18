import { useState, useEffect } from 'react';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Modal from 'react-bootstrap/Modal';
import { AddGroup, UpdateGroup, GetGroupById, GetGroups, GetGroupsTaskList, GetStarredTask } from '@/api/TaskGroupApi';
import { MoveTaskToNewGroup } from '../../api/TaskApi';
import { MoveSubTaskToNewGroup } from '../../api/subTaskAPi';
import { useTaskEvents } from '../../Hooks/TaskEvents';

const AddOrUpdateGroups = ({ visible, setVisibility, groupId, taskIdToMove, isSubTask }) => {

    const { refreshTaskLists, setRecentActionItem } = useTaskEvents();
    const [disable, setDisable] = useState(false);
    const [isShowError, setIsShowError] = useState(false);
    const [groupName, setGroupName] = useState('');
    const [responseError, setResponseError] = useState(null);
    const [validationErrors, setValidationErrors] = useState({});

    useEffect(() => {
        (async () => {
            if (groupId > 0) {
                let res = await GetGroupById(groupId);
                if (res.isSuccess) {

                    let itemToEdit = res.data;
                    setGroupName(itemToEdit.groupName ?? '');
                }
            }

        })();
    }, []);

    const handleSubmit = async () => {
        setDisable(true);
        let response = {}
        let isTaskMoved = true;
        let itemToSet = {};
        if (taskIdToMove && taskIdToMove != null && taskIdToMove != undefined && taskIdToMove > 0) {
            if (isSubTask && isSubTask === true) {
                response = await MoveSubTaskToNewGroup(taskIdToMove, { groupName: groupName });
                itemToSet = { action: 'move', item: 'sub-task' };
            } else {
                response = await MoveTaskToNewGroup(taskIdToMove, { groupName: groupName });
                itemToSet = { action: 'move', item: 'task' };
            }
            if (!response.isSuccess) {
                console.error("error while moving task to new group ", response);
                setResponseError(response.message);
                setDisable(false);
                return;
            }

            isTaskMoved = response.isSuccess;

        } else {

            isTaskMoved = false;
            if (groupId > 0) {
                response = await UpdateGroup(groupId, { groupName: groupName });
            } else {
                response = await AddGroup({ groupName: groupName });
            }
            if (!response.isSuccess) {
                if (response?.status === 400 && response?.errors) {
                    setValidationErrors(response?.errors)
                } else {
                    setResponseError(response.message);
                }
                console.log("Error while adding or updating group", response);
                setResponseError(response.message);
                setDisable(false);
                return;
            }
        }

        setGroupName('');
        response.isSuccess && setVisibility(false);
        setDisable(false);
        await refreshTaskLists();

        if (isTaskMoved) {
            setRecentActionItem(itemToSet);
            // Auto clear after 3 sec
            setTimeout(() => {
                setRecentActionItem(null);
            }, 3000);
        }
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

    return (

        < Modal show={visible} onHide={() => handleClose()} centered size="sm">
            {/* <Modal.Header closeButton>
                <Modal.Title>{groupId > 0 ? "Rename Group" : "Add Group"}</Modal.Title>
            </Modal.Header>*/}
            {responseError && <p className=" text-center mt-2 mb-0 text-danger">{responseError}</p>}
            <Modal.Body>
                <Form.Group className="mb-3" controlId="exampleForm.ControlInput1">
                    <Form.Label>Enter group name</Form.Label>
                    <Form.Control
                        onChange={(e) => handleErrorAddGroup(e.target.value)}
                        type="text"
                        placeholder="Enter name"
                        autoFocus
                        value={groupName}
                    />
                    <Form.Text className={`text-danger ${!isShowError ? "d-none" : ""}`}>
                        please enter group name.
                    </Form.Text>
                    <Form.Text className={`text-danger ${!validationErrors.GroupName ? "d-none" : ""}`}>
                        {validationErrors.GroupName && validationErrors.GroupName[0]}
                    </Form.Text>
                </Form.Group>
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