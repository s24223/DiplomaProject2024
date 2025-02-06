import axios from "axios"

export const fetchLogout = async () => {
    return axios.post('https://localhost:7166/api/User/logOut', null, {
        headers: {
            'Access-Control-Allow-Origin': '*',
            'Authorization': `Bearer ${localStorage.getItem("jwt")}`
        }
    }).then(res => {
        localStorage.removeItem("jwt")
        localStorage.removeItem("jwtValidTo")
        localStorage.removeItem("refreshToken")
        localStorage.removeItem("refreshTokenValidTo")
        return res.data
    }).catch(error => {
        localStorage.removeItem("jwt")
        localStorage.removeItem("jwtValidTo")
        localStorage.removeItem("refreshToken")
        localStorage.removeItem("refreshTokenValidTo")
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