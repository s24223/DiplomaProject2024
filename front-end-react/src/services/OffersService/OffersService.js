// offersService.js

export const fetchOffers = async (query) => {
    const response = await fetch(`https://localhost:7166/api/Offers?query=${query}`, {
        method: 'GET',
        withCredentials: true,    
        crossorigin: true,  
        headers: {'Access-Control-Allow-Origin': '*'},
    });
    if (!response.ok) throw new Error("Error fetching offers");
    return await response.json();
};

export const fetchOfferDetails = async (offerId) => {
    const response = await fetch(`https://localhost:7166/api/Offers/${offerId}`, {
        method: 'GET',
        withCredentials: true,    
        crossorigin: true,  
        headers: {'Access-Control-Allow-Origin': '*'},
    });
    if (!response.ok) throw new Error("Error fetching offers");
    return await response.json();
};

