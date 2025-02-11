import axios from "axios";

export const fetchBranchPost = async (body) => {
    return await axios.post('https://localhost:7166/api/User/company/branches', body, {
        headers: {
            "Authorization": `Bearer ${localStorage.getItem("jwt")}`,
            "Access-Contorl-Allow-Origin": "*"
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
    })
}

export const fetchBranchGet = async () => {
    return await axios.get('https://localhost:7166/api/User/company/branches/core', {
        headers: {
            "Authorization": `Bearer ${localStorage.getItem("jwt")}`,
            "Access-Contorl-Allow-Origin": "*"
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

export const fetchBranchPut = async (body) => {
    return await axios.put('https://localhost:7166/api/User/company/branches', body, {
        headers: {
            "Authorization": `Bearer ${localStorage.getItem("jwt")}`,
            "Access-Contorl-Allow-Origin": "*"
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
    })
}

export const fetchBranchDetails = async (branchId) => {
    return await axios.get(`https://localhost:7166/api/BranchOffers/branches/${branchId}`, {
        headers: {
            "Access-Contorl-Allow-Origin": "*"
        }
    })
        .then(res => res.data).catch(error => {
            switch (error.response.status) {
                case 500:
                    const idAppProblem = error.response.data.ProblemId
                    window.location.href = `/notification/create/${idAppProblem}`
                    break;
                default:
                    return { error: error.response.data.Message }
            }
        })
};