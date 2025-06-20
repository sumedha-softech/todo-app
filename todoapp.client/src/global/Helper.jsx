import { useLocation } from "react-router-dom";

export const FormateDate = (date) => {
    const formattedDate = new Date(date).toLocaleDateString("en-US", {
        weekday: "short",  // "Thu"
        month: "short",    // "May"
        day: "2-digit"     // "22"
    });
    return formattedDate;
}

export const GetPath = () => {
    const location = useLocation();
    return location.pathname;
}


 