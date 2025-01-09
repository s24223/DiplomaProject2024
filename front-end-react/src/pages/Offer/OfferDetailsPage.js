import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import OfferDetails from "../../components/OfferDetails/OfferDetails";
import { fetchOfferDetailsPublic } from "../../services/OffersService/OffersService";

const OfferDetailsPage = () => {
    const { offerId } = useParams();
    const [offerDetails, setOfferDetails] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const loadOfferDetails = async () => {
            try {
                const data = await fetchOfferDetailsPublic(offerId);
                setOfferDetails(data);
            } catch (err) {
                setError("Failed to load offer details.");
            } finally {
                setLoading(false);
            }
        };
        loadOfferDetails();
    }, [offerId]);

    if (loading) return <p>Loading offer details...</p>;
    if (error) return <p>{error}</p>;

    return (
        <div>
            <OfferDetails offerDetails={offerDetails} />
        </div>
    );
};

export default OfferDetailsPage;
