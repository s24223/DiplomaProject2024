// offersService.js

export const fetchOffers = async (query) => {
    const response = await fetch(`/api/offers?query=${query}`);
    if (!response.ok) throw new Error("Error fetching offers");
    return await response.json();
};

export const fetchOfferDetails = async (offerId) => {
    const response = await fetch(`/api/offers/${offerId}`);
    if (!response.ok) throw new Error("Error fetching offer details");
    return await response.json();
};
