// offersService.js

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
        const response = await axios.get(
            `https://localhost:7166/api/BranchOffers/branches/${branchId}/branchOffers?${queryParams}`,
            {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem("jwt")}`,
                },
            }
        );
        console.log('Request URL:', `https://localhost:7166/api/BranchOffers/branches/${branchId}/branchOffers?${queryParams}`);


        if (response.status !== 200) {
            throw new Error(`Error fetching branch offers: ${response.statusText}`);
        }
        return response.data;
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
    const response = await fetch(
        `https://localhost:7166/api/BranchOffers?${queryParams.toString()}`,
        {
            method: 'GET',
            headers,
        }
    );

    if (!response.ok) throw new Error('Error fetching offers');

    return await response.json();
};


export const fetchOfferDetailsPrivate = async (offerId) => {
    try {
        const response = await axios.get(`https://localhost:7166/api/BranchOffers/offers/${offerId}`, {
            headers: {
                Authorization: `Bearer ${localStorage.getItem("jwt")}`,
                "Content-Type": "application/json",
            },
        });
        return response.data.item; // Zwróć szczegóły oferty
    } catch (error) {
        console.error("Error fetching offer details:", error.response?.data || error.message);
        throw new Error("Failed to fetch offer details.");
    }
};
export const fetchOfferDetailsPublic = async (offerId) => {
    try {
        const response = await axios.get(`https://localhost:7166/api/BranchOffers/offers/${offerId}`, {
            headers: { 'Access-Control-Allow-Origin': '*' },
            withCredentials: true,
        });
        return response.data.item;
    } catch (error) {
        console.error("Error fetching offer details:", error);
        throw error;
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
                    Authorization: `Bearer ${localStorage.getItem("jwt")}`,
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
                    Authorization: `Bearer ${localStorage.getItem("jwt")}`,
                },
            }
        );
        return response.data;
    } catch (error) {
        console.error("Error assigning offer to branch:", error.response?.data || error.message);
        throw error.response?.data?.Message || "Failed to assign offer to branch.";
    }
};

export const updateOffer = async (offerData) => {
    try {
        const response = await axios.put(
            `https://localhost:7166/api/User/company/offers`,
            [offerData], // API oczekuje tablicy
            {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem("jwt")}`,
                },
            }
        );
        if (response.status !== 200) {
            throw new Error("Failed to update the offer.");
        }
        return response.data;
    } catch (error) {
        console.error("Error updating offer:", error);
        throw error;
    }
};

