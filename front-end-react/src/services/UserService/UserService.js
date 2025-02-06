import axios from "axios";

export const fetchUserProfile = async () => {
    return await axios.get("https://localhost:7166/api/User", {
        headers: {
            Authorization: `Bearer ${localStorage.getItem("jwt")}`,
        },
    }).then(res => res.data.item).catch(error => {
        switch (error.response.status) {
            case 500:
                const idAppProblem = error.response.data.ProblemId
                window.location.href = `/notification/create/${idAppProblem}`
                break;
            default:
                return { error: error.response.data.Message }
        }
    });
};
