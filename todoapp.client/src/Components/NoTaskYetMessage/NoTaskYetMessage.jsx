const NoTaskYetMessage = () => {
    return (
        <div style={{ textAlign: 'center' }}>
            <img
                src="https://www.gstatic.com/tasks/empty-tasks-dark.svg"
                alt="No tasks"
                style={{ height: '200px', width:"100%", margin: '0 auto' }}
            />
            <h5 className="mt-3">No task yet</h5>
            <p className="text-muted text-break">
                Add your to-dos and keep track of them across this Workspace
            </p>

        </div>
    );
};

export default NoTaskYetMessage;
