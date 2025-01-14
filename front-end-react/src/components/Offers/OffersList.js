import React from 'react';
import { useNavigate } from 'react-router-dom';

const OffersList = ({ offers }) => {
    const navigate = useNavigate();

    const handleOfferClick = (offerId) => {
        navigate(`/offers/${offerId}`);
    };

    return (
        <div className='offers-list'>
            <ul>
                {offers.map((offerItem) => {
                    const {
                        company: { name: companyName },
                        branch: {
                            address: {
                                hierarchy,
                            },
                        },
                        offer: { id: offerId, name: offerName, description: offerDescription },
                    } = offerItem;

                    const voivodeship = hierarchy.find((item) => item.administrativeType.name === 'województwo')?.name;
                    const city = hierarchy.find((item) => item.administrativeType.name.includes('miasto'))?.name;

                    return (
                        <li key={offerId} className='offer-item' onClick={() => handleOfferClick(offerId)}>
                            
                            <h2>{offerName}</h2>
                            <p>Company: {companyName}</p>
                            <p>Województwo: {voivodeship}, miasto: {city}</p>
                            <p>Description: {offerDescription}</p>
                        </li>
                    );
                })}
            </ul>
        </div>
    );
};

export default OffersList;
