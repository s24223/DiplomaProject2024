import React, { useState, useEffect } from "react";
import { useParams, Link } from "react-router-dom";
import OfferDetails from "../../components/Offers/OfferDetails";
import OfferEditForm from "../../components/Forms/OfferEditForm";
import { fetchOfferDetailsPublic } from "../../services/OffersService/OffersService";
import { jwtRefresh } from "../../services/JwtRefreshService/JwtRefreshService";

const PrivateOfferDetailsPage = () => {
    jwtRefresh();
    
    const { offerId } = useParams();
    const [offerDetails, setOfferDetails] = useState(null);
    const [editMode, setEditMode] = useState(false);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const isLoggedIn = !!localStorage.getItem("jwt");

    useEffect(() => {
        const loadOfferDetails = async () => {
            try {
                const data = await fetchOfferDetailsPublic(offerId);
                if(data.error){
                    throw new Error(data.error)
                }
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
            <div className="edit-container" >
                {isLoggedIn && (
                    <>
                    <button onClick={() => setEditMode(!editMode)} >
                        {editMode ? "Cancel Edit" : "Edit Mode"}
                    </button>
                    <button onClick={() => (window.location.href = `/offers/${offerId}/applications`)}>
                    Manage Applications
                    </button>
                  </>
                )}
            </div>
            {editMode && isLoggedIn ? (
                <OfferEditForm offerDetails={offerDetails} onCancelEdit={() => setEditMode(false)} />
            ) : (
                <OfferDetails offerDetails={offerDetails} />
            )}
            
        </div>
    );
};

export default PrivateOfferDetailsPage;
