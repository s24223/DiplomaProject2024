import axios from "axios";

export const fetchRegistration = async (body) => {
    return await axios.post('https://localhost:7166/api/User', body, {
        headers:{
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
    }).then(res => res.data).catch(error => {
        switch(error.response.status){
            case 500:
                const idAppProblem = error.response.data.ProblemId
                window.location.href = `/notification/create/${idAppProblem}`
                break;
            case 400:
                return { error: "There is already an account with this e-mail"}
            default:
                return { error: error.response.data.Message}
        }
    })
}