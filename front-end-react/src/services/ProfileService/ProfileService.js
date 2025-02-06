import axios from "axios";

export const fetchProfilePost = async (body) => {
    return await axios.post("https://localhost:7166/api/User/person", body,
        {
            headers: {
                "Access-Contorl-Allow-Origin": "*",
                "Authorization": `Bearer ${localStorage.getItem("jwt")}`
            },
        }
    ).then(res => res.data).catch(error => {
        switch (error.response.status) {
            case 500:
                const idAppProblem = error.response.data.ProblemId
                window.location.href = `/notification/create/${idAppProblem}`
                break;
            default:
                throw new Error( error.response.data.Message)
        }
    });
}



export const fetchUserProfile = async () => {
    const token = localStorage.getItem("jwt") || localStorage.getItem("jwt");
    if (!token) throw new Error("No authorization token found.");
    
    return await axios.get(`https://localhost:7166/api/User`, {
        headers: {
            Authorization: `Bearer ${token}`,
        },
    }).then(res => res.data.item.person).catch(error => {
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

export const updateUserProfile = async (profileData) => {
    const token = localStorage.getItem("jwt") || localStorage.getItem("jwt");
    if (!token) throw new Error("No authorization token found.");

    return await axios.put(`https://localhost:7166/api/User/person`, profileData, {
        headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
        },
    }).then(res => res.data).catch(error => {
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

export const deleteUserProfile = async (body) => {
    console.log(body)
    return await fetch(`https://localhost:7166/api/User`, {
        method: 'DELETE',
        headers:{
            Authorization: `Bearer ${localStorage.getItem("jwt")}`,
            "Access-Contorl-Allow-Origin": "*",
            "Content-Type": "application/json",
        },
        body: JSON.stringify(body),
    }).then(res => res.data).catch(error => {
        switch (error.response.status) {
            case 500:
                const idAppProblem = error.response.data.ProblemId
                window.location.href = `/notification/create/${idAppProblem}`
                break;
            default:
                throw new Error(error.response.data.Message)
        }
    })
}
