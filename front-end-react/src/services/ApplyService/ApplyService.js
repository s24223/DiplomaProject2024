import axios from "axios"

export const fetchApply = async ({ url, formData }) => {
    return await axios.post(url, formData, {
        headers: {
            'Authorization': `Bearer ${localStorage.getItem("jwt")}`,
            'Access-Control-Allow-Origin': '*',
        },
    }).then(res => {
        alert("Application successful!")
        return res.data
    }).catch(error => {
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