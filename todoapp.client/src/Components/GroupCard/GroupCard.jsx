import { useTaskEvents } from '../../Hooks/TaskEvents';
import { UpdateTaskCompletionStatus } from '../../api/TaskApi';
import { UpdateSubTaskCompletionStatus } from '../../api/subTaskAPi';
import {
    AddTaskButton,
    CompletedTaskList,
    GroupCardHeader,
    InCompleteTasks,
    NoTaskYetMessage,
    TaskCompletedMessage
} from '../index';
import { CCard, CCardBody, CListGroup } from '@coreui/react'

const GroupCard = ({ group, isStarredList }) => {
    const { RefreshTaskLists } = useTaskEvents();

    // mark complete or uncomplete a task
    const handleCompleteTask = async (taskId, isSubTask) => {

        let response = {};
        if (isSubTask) {
            response = await UpdateSubTaskCompletionStatus(taskId);
        } else {
            response = await UpdateTaskCompletionStatus(taskId);
        }

        if (!response.isSuccess) {
            console.error("error while updating task for complete or uncomplete", response);
            alert(`Error! ${response.message}`);
            return;
        }

        await RefreshTaskLists();
    };

    return (
        group.isEnableShow &&
        <div className={`col-md-4 ${isStarredList ? 'm-auto mt-4' : ''}`}>
            <CCard className="shadow-sm rounded-3">
                    <GroupCardHeader group={group} isStarredList={isStarredList} />
                    {/*add this css on CCardBody for add scroll*/}
                    {/*style={{ minHeight: "100px", maxHeight: "720px", overflowX: "auto" }}*/}
                    <CCardBody>
                    <AddTaskButton groupId={group.groupId} isStarredTask={isStarredList} />
                    {
                        group.taskList && group.taskList.length > 0
                            ? <CListGroup flush>
                                {
                                    group.taskList.map(task => (
                                        <InCompleteTasks key={task.taskId} groupId={group.groupId} task={task} onComplete={handleCompleteTask} />
                                    ))
                                }</CListGroup>

                            : group.completedTaskList && group.completedTaskList.length > 0 ? <TaskCompletedMessage /> : <NoTaskYetMessage />
                    }
                    {
                        group.completedTaskList && group.completedTaskList.length > 0 &&
                        <>
                            <CompletedTaskList groupId={group.groupId} completedTaskList={group.completedTaskList} onComplete={handleCompleteTask} />
                        </>
                    }
                </CCardBody>
            </CCard>
        </div >
    )
}

export default GroupCard;