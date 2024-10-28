import React from 'react';
import { useNavigate } from 'react-router-dom';

const OffersList = ({ offers }) => {
    const navigate = useNavigate();

    const handleOfferClick = (offerId) => {
        navigate(`/offers/${offerId}`);
    };

    return (
        <div>
            <ul>
                {offers.map((offer) => (
                    <li key={offer.id} onClick={() => handleOfferClick(offer.id)}>
                        {offer.title}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default OffersList;
