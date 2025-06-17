const TaskCompletedMessage = () => {
    return (
        <div style={{ textAlign: 'center' }}>
            <img className="m-auto" src="https://www.gstatic.com/tasks/all-tasks-completed-dark.svg" height="200px" width="100%" />
            <h3 className="mt-3">All task is completed</h3>
            <p className="text-muted text-break">Nice work!</p>
        </div>
    )
}
export default TaskCompletedMessage;