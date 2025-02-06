import axios from "axios"

export const fetchLogin = async (body) => {
    let data = await axios.post('https://localhost:7166/api/User/login', body, {
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*',
        }
    }).then(res => res.data).catch(error => {
        switch (error.response.status) {
            case 500:
                const idAppProblem = error.response.data.ProblemId
                window.location.href = `/notification/create/${idAppProblem}`
                break;
            case 401:
                return { error: "Email or password is inccorect" }
            default:
                return { error: error.response.data.Message }
        }
    })
    return data
}