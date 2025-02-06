import axios from "axios"

export const fetchNotificationGetAuthorized = async () => {
    return await axios.get('https://localhost:7166/api/User/notifications/authorized', {
        headers: {
            "Access-Contorl-Allow-Origin": "*",
            "Authorization": `Bearer ${localStorage.getItem("jwt")}`
        }
    }).then(res => res.data).catch(error => {
        switch (error.response.status) {
            case 500:
                const idAppProblem = error.response.data.ProblemId
                window.location.href = `/notification/create/${idAppProblem}`
                break;
            default:
                return { error: error.response.data.Message }
        }
    })
}

export const fetchNotificationPostAuthorized = async (body) => {
    return await axios.post('https://localhost:7166/api/User/notifications/authorized', body, {
        headers: {
            "Access-Contorl-Allow-Origin": "*",
            "Authorization": `Bearer ${localStorage.getItem("jwt")}`
        },
    }).then(res => res.data).catch(error => {
        alert("We are currently facing problems handling your request. Please try again later.")
        throw new Error(error)
    })
}

export const fetchNotificationPostUnAuthorized = async (body) => {
    return await axios.post('https://localhost:7166/api/User/notifications/unauthorized', body, {
        headers: {
            "Access-Contorl-Allow-Origin": "*"
        },
    }).then(res => res.data).catch(error => {
        alert("We are currently facing problems handling your request. Please try again later.")
        throw new Error(error)
    })
}