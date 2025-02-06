import axios from "axios"

export const fetchCharacteristics = async () => {
    return await axios.get(
        "https://localhost:7166/api/Dictionaries/characteristics?isOrderByType=false",
        {
            headers: {
                Authorization: `Bearer ${localStorage.getItem("jwt")}`,
            },
        }
    ).then(res => {
        if (!res.data.items){
            throw new Error("Failed to fetch characteristics.")
        }
        return res.data.items
    }).catch(error => {
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