import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { fetchOfferDetails } from '../../services/OffersService/OffersService';
import './OfferDetailsPage.css';

const OfferDetailsPage = () => {
    const { offerId } = useParams();
    const [offer, setOffer] = useState(null);

    useEffect(() => {
        const loadOffer = async () => {
            try {
                const data = await fetchOfferDetails(offerId);
                setOffer(data);
            } catch (error) {
                console.error("Error fetching offer details:", error);
            }
        };
        loadOffer();
    }, [offerId]);

    if (!offer) return <div>Loading...</div>;

    return (
        <div className="offer-details">
            <h1>{offer.title}</h1>
            <p>{offer.description}</p>
            {/* Wyświetl inne szczegóły oferty */}
        </div>
    );
};

export default OfferDetailsPage;
