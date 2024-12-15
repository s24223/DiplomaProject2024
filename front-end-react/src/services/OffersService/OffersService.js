// offersService.js

import axios from 'axios';

export const fetchBranchOffers = async (companyId, branchId, params = {}) => {
    const queryParams = new URLSearchParams({
        orderBy: 'publishStart',
        ascending: 'true',
        maxItems: 100,
        page: 1,
        ...params,
    }).toString();

    const response = await axios.get(
        `https://localhost:7166/api/Offers/companies/${companyId}/branches/${branchId}?${queryParams}`,
        {
            headers: {
                Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
            },
        }
    );

    if (!response.status === 200) throw new Error("Error fetching branch offers");
    return response.data.item;
};


// export const fetchOffers = async (query) => {
//     const response = await fetch(`https://localhost:7166/api/Offers?query=${query}`, {
//         method: 'GET',
//         withCredentials: true,    
//         crossorigin: true,  
//         headers: {'Access-Control-Allow-Origin': '*'},
//     });
//     if (!response.ok) throw new Error("Error fetching offers");
//     return await response.json();
// };

export const fetchOffers = async (filters) => {
    const queryParams = new URLSearchParams();

    for (const key in filters) {
        if (filters[key] !== '' && filters[key] !== null) {
            queryParams.append(key, filters[key]);
        }
    }

    const response = await fetch(
        `https://localhost:7166/api/Offers?${queryParams.toString()}`,
        {
            method: 'GET',
            headers: {
                Authorization: `Bearer ${sessionStorage.getItem('jwt')}`,
                'Access-Control-Allow-Origin': '*',
            },
        }
    );

    if (!response.ok) throw new Error('Error fetching offers');
    return await response.json();
};

export const fetchOfferDetails = async (offerId) => {
    try {
        const response = await axios.get(`https://localhost:7166/api/Offers/${offerId}`, {
            headers: {
                Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
                "Content-Type": "application/json",
            },
        });
        return response.data.item; // Zwróć szczegóły oferty
    } catch (error) {
        console.error("Error fetching offer details:", error.response?.data || error.message);
        throw new Error("Failed to fetch offer details.");
    }
};
// Tworzenie nowej oferty
export const createOffer = async (offerData) => {
    try {
        const response = await axios.post(
            `https://localhost:7166/api/User/company/offers`,
            offerData,
            {
                headers: {
                    Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
                },
            }
        );
        return response.data.items[0]; // Zwracamy utworzoną ofertę
    } catch (error) {
        console.error("Error creating offer:", error.response?.data || error.message);
        throw error.response?.data?.Message || "Failed to create offer.";
    }
};

// Przypisanie oferty do oddziału
export const assignOfferToBranch = async (publishData) => {
    try {
        const response = await axios.post(
            `https://localhost:7166/api/User/company/branches&offers`,
            publishData,
            {
                headers: {
                    Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
                },
            }
        );
        return response.data;
    } catch (error) {
        console.error("Error assigning offer to branch:", error.response?.data || error.message);
        throw error.response?.data?.Message || "Failed to assign offer to branch.";
    }
};
