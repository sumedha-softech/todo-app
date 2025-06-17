import { useTaskEvents } from '../../Hooks/TaskEvents';
import { UpdateTask } from '../../api/TaskApi';
import {
    AddTaskButton,
    CompletedTaskList,
    GroupCardHeader,
    InCompleteTasks,
    NoTaskYetMessage,
    TaskCompletedMessage
} from '../index';
import { CCard, CCardBody,CListGroup } from '@coreui/react'

const GroupCard = ({ group, isStarredList }) => {
    const { RefreshTaskLists } = useTaskEvents();

    // mark complete or uncomplete a task
    const handleCompleteTask = async (task, isComplete = true) => {

        let response = await UpdateTask(task.taskId, { ...task, isCompleted: isComplete });
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
                            <hr /><CompletedTaskList groupId={group.groupId} completedTaskList={group.completedTaskList} onComplete={handleCompleteTask} />
                        </>
                    }
                </CCardBody>
            </CCard>
        </div >
    )
}

export default GroupCard;