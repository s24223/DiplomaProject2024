import axios from "axios";


export const updateUrl = async (urlData) => {
    return await axios.put(`https://localhost:7166/api/User/urls/urls`, [urlData], {
        headers: {
            Authorization: `Bearer ${localStorage.getItem("jwt")}`,
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

export const deleteUrl = async (urlItem) => {
    return await axios.delete(`https://localhost:7166/api/User/urls/urls`, {
        headers: {
            Authorization: `Bearer ${localStorage.getItem("jwt")}`,
        },
        data: [{ urlTypeId: urlItem.urlTypeId, created: urlItem.created }],
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


// Pobierz typy URL-i
export const fetchUrlTypes = async () => {
    return await axios.get("https://localhost:7166/api/Dictionaries/user/urls/types", {
        headers: {
            Authorization: `Bearer ${localStorage.getItem("jwt")}`,
        },
    }).then(res => Object.values(res.data)).catch(error => { // Zamień obiekt na tablicę
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

// Dodaj nowy URL
export const addUrl = async (urlData) => {
    return await axios.post(
        "https://localhost:7166/api/User/urls/urls",
        [urlData],
        {
            headers: {
                Authorization: `Bearer ${localStorage.getItem("jwt")}`,
            },
        }
    ).then(res => res.data).catch(error => {
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

export const fetchUrls = async () => {
    return await axios.get(
        "https://localhost:7166/api/User/urls?orderBy=created&ascending=true&itemsCount=100&page=1",
        {
            headers: {
                Authorization: `Bearer ${localStorage.getItem("jwt")}`,
            },
        }
    ).then(res => res.data.item.urls).catch(error => {
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