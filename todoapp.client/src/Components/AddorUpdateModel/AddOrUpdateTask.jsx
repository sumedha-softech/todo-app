import { useState, useEffect, useRef } from 'react';
import Button from 'react-bootstrap/Button';
import Form from 'react-bootstrap/Form';
import Modal from 'react-bootstrap/Modal';
import { GetTaskById, AddTask, UpdateTask } from '@/api/taskApi';
import { GetSubTaskById, AddSubTask, UpdateSubTask } from '@/api/subTaskApi';
import { useTaskEvents } from '../../Hooks/TaskEvents';

const AddOrUpdateTask = ({ visible, setVisibility, taskId, setTaskId, groupId, isStarredTask, isSubTask, isRequestForSubTaskAdd }) => {

    const { RefreshTaskLists, taskGroups } = useTaskEvents();
    const dateRef = useRef(null);
    const [disable, setDisable] = useState(false);
    const [title, setTitle] = useState('');
    const [description, setDescription] = useState('');
    const [date, setDate] = useState('');
    const [isShowError, setIsShowError] = useState(false);
    const [selectedGroupId, setSelectedGroupId] = useState(1);
    const [responseError, setResponseError] = useState(null);
    const [validationErrors, setValidationErrors] = useState({});

    useEffect(() => {
        (async () => {
            if (taskId > 0 && !isRequestForSubTaskAdd) {
                let res = {};
                if (isSubTask && isSubTask === true) {
                    res = await GetSubTaskById(taskId);
                } else {
                    res = await GetTaskById(taskId);
                }
                if (res.isSuccess) {
                    let itemToEdit = res.data;
                    setTitle(itemToEdit.title ?? '');
                    setDescription(itemToEdit.description ?? '');
                    setDate(itemToEdit.toDoDate ?? '');
                } else {
                    alert(`Error! ${res.message}`);
                    setVisibility(false);
                }
            }
        })();
    }, []);

    const handleClose = () => {
        setTaskId && setTaskId(0);
        setVisibility(false);
    }

    const handleSubmit = async () => {
        setDisable(true);
        let response = {};
        const toDoDate = date?.trim() ? date : null;

        if (taskId > 0 && !isRequestForSubTaskAdd) {
            if (isSubTask && isSubTask === true) {
                response = await UpdateSubTask(taskId, { title: title, description: description, toDoDate: toDoDate });
            } else if (groupId > 0) {
                response = await UpdateTask(taskId, { title: title, description: description, toDoDate: toDoDate });
            }
        } else {
            const isStarred = isStarredTask === null || isStarredTask === undefined || !isStarredTask ? false : isStarredTask;
            const taskGroupId = groupId > 0 ? groupId : selectedGroupId;

            if (isSubTask && isSubTask === true) {
                response = await AddSubTask({ title: title, description: description, toDoDate: toDoDate, taskId: taskId, isStarred: isStarred });
            } else {
                response = await AddTask({ title: title, description: description, toDoDate: toDoDate, taskGroupId: taskGroupId, isStarred: isStarred });
            }
        }

        setDisable(false);

        if (!response.isSuccess) {
            if (response?.status === 400 && response?.errors) {
                setValidationErrors(response?.errors)
            } else {
                setResponseError(response.message);
            }
            console.log("Error while adding or updating task", response);
            return;
        }

        setSelectedGroupId(1);
        EmptyAllFields();
        response.isSuccess && setVisibility(false);
        await RefreshTaskLists();
    }

    const EmptyAllFields = () => {
        setTitle('');
        setDescription('');
        setDate('');
    }

    const HendleErrorAndSetTitleValue = (e) => {
        setTitle(e.target.value);
        if (e.target.value.trim()) {
            setIsShowError(false);
        } else {
            setIsShowError(true);
        }

    }

    return (

        <Modal show={visible} onHide={() => handleClose()} centered>
            <Modal.Header closeButton>
                <Modal.Title>{taskId > 0 ? "Edit task" : "Add task"}</Modal.Title>
                {validationErrors.Title && (<span className="text-danger">{validationErrors.Title[0]}</span>)}
            </Modal.Header>
            {responseError && <p className=" text-center mt-2 mb-0 text-danger">{responseError}</p>}
            <Modal.Body>
                <Form>
                    <Form.Group className="mb-3" controlId="exampleForm.ControlInput1">
                        <Form.Label>Title</Form.Label>
                        <Form.Control
                            onChange={(e) => HendleErrorAndSetTitleValue(e)}
                            type="text"
                            placeholder="enter task title"
                            autoFocus
                            value={title}
                        />
                        <Form.Text className={`text-danger ${!isShowError ? "d-none" : ""}`}>
                            please enter a text for title.
                        </Form.Text>
                    </Form.Group>

                    <Form.Group
                        className="mb-3"
                        controlId="exampleForm.ControlTextarea1"
                    >
                        <Form.Label>Description</Form.Label>
                        <Form.Control
                            onChange={(e) => setDescription(e.target.value)} as="textarea"
                            placeholder="enter desceription"
                            rows={3}
                            value={description}
                        />
                    </Form.Group>

                    <Form.Group className="mb-3" controlId="exampleForm.ControlInput2">
                        <Form.Label>Date</Form.Label>
                        <Form.Control
                            onChange={(e) => setDate(e.target.value)}
                            value={date}
                            ref={dateRef}
                            onClick={() => dateRef.current.showPicker?.()}
                            type="Date"
                        />
                    </Form.Group>

                    {
                        groupId === 0 && taskId == 0 && !isSubTask ?

                            <Form.Group className="mb-3" controlId="exampleForm.ControlInput3">
                                <Form.Label>Select Task Group</Form.Label>
                                <Form.Select
                                    onChange={(e) => setSelectedGroupId(e.target.value)} aria-label="Default select example">
                                    {taskGroups.map((item) => (
                                        <option key={item.groupId} value={item.groupId}>{item.groupName}</option>
                                    ))}
                                </Form.Select>

                                {validationErrors.TaskGroupId && (
                                    <span className="text-danger">{validationErrors.TaskGroupId[0]}</span>
                                )}
                            </Form.Group>
                            : ''
                    }

                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button variant="secondary" onClick={() => handleClose()}>
                    Close
                </Button>
                <Button variant="primary" onClick={handleSubmit} disabled={!title.trim() || disable}>
                    Save
                </Button>
            </Modal.Footer>
        </Modal>
    )
}

export default AddOrUpdateTask;