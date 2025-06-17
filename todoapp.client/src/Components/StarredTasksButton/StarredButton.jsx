import { Star } from 'lucide-react'
import { Link } from "react-router-dom";

const StarredButton = () => {
    return (
        <>
            <Star />
            <Link className={`btn nav-link`} to="/starred">
                Starred
            </Link>
        </>
    )
}

export default StarredButton;