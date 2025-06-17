import { CCardHeader } from '@coreui/react'
import { GroupActions } from '../../index';

const GroupCardHeader = ({ group, isStarredList }) => {

    return (
        <CCardHeader className="d-flex justify-content-between align-items-center">
            <strong>{group.groupName}</strong>
            <GroupActions group={group} isStarredList={isStarredList} />
        </CCardHeader>
    )
}
export default GroupCardHeader;