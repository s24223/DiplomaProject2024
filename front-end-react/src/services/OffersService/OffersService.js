import axios from 'axios';

export const fetchBranchOffers = async (branchId, params = {}) => {
    const queryParams = new URLSearchParams({
        orderBy: 'publishStart',
        ascending: 'true',
        maxItems: 100,
        page: 1,
        ...params,
    }).toString();

    try {
        return await axios.get(
            `https://localhost:7166/api/BranchOffers/branches/${branchId}/branchOffers?${queryParams}`,
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
        // console.log('Request URL:', `https://localhost:7166/api/BranchOffers/branches/${branchId}/branchOffers?${queryParams}`);


        // if (response.status !== 200) {
        //     throw new Error(`Error fetching branch offers: ${response.statusText}`);
        // }
        // return response.data;
    } catch (error) {
        console.error("Error fetching branch offers:", error);
        throw error; // Rzucamy błąd do wyższej warstwy
    }
};



export const fetchOffers = async (filters) => {
    const queryParams = new URLSearchParams();

    // Dodajemy tylko niepuste wartości do query params
    for (const key in filters) {
        if (filters[key] !== '' && filters[key] !== null) {
            queryParams.append(key, filters[key]);
        }
    }

    const headers = {
        'Access-Control-Allow-Origin': '*',
    };

    // Sprawdzamy, czy użytkownik jest zalogowany i dodajemy token do nagłówka
    const token = localStorage.getItem("jwt");
    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }

    // Wysyłamy zapytanie do API
    return await axios.get(
        `https://localhost:7166/api/BranchOffers?${queryParams.toString()}`,
        {
            headers,
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


export const fetchOfferDetailsPrivate = async (offerId) => {
    try {
        return await axios.get(`https://localhost:7166/api/BranchOffers/offers/${offerId}`, {
            headers: {
                Authorization: `Bearer ${localStorage.getItem("jwt")}`,
                "Content-Type": "application/json",
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
    } catch (error) {
        console.error("Error fetching offer details:", error.response?.data || error.message);
        throw new Error("Failed to fetch offer details.");
    }
};
export const fetchOfferDetailsPublic = async (offerId) => {
    try {
        return await axios.get(`https://localhost:7166/api/BranchOffers/offers/${offerId}`, {
            headers: { 'Access-Control-Allow-Origin': '*' },
            withCredentials: true,
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
    } catch (error) {
        console.error("Error fetching offer details:", error);
        throw error;
    }
};
// Tworzenie nowej oferty
export const createOffer = async (offerData) => {
    try {
        return await axios.post(
            `https://localhost:7166/api/User/company/offers`,
            offerData,
            {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem("jwt")}`,
                },
            }
        ).then(res => res.data.items[0]).catch(error => {
            switch (error.response.status) {
                case 500:
                    const idAppProblem = error.response.data.ProblemId
                    window.location.href = `/notification/create/${idAppProblem}`
                    break;
                default:
                    return { error: error.response.data.Message }
            }
        });
    } catch (error) {
        console.error("Error creating offer:", error.response?.data || error.message);
        throw error.response?.data?.Message || "Failed to create offer.";
    }
};

// Przypisanie oferty do oddziału
export const assignOfferToBranch = async (publishData) => {
    try {
        return await axios.post(
            `https://localhost:7166/api/User/company/branches&offers`,
            publishData,
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
    } catch (error) {
        console.error("Error assigning offer to branch:", error.response?.data || error.message);
        throw error.response?.data?.Message || "Failed to assign offer to branch.";
    }
};

export const updateOffer = async (offerData) => {
    try {
        return await axios.put(
            `https://localhost:7166/api/User/company/offers`,
            [offerData], // API oczekuje tablicy
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
    } catch (error) {
        console.error("Error updating offer:", error);
        throw error;
    }
};

