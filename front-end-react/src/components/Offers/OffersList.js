import React from 'react';
import { Link } from 'react-router-dom';

const OffersList = ({ offers }) => {
    return (
        <div className='offers-list'>
            <ul className='bulletless-list'>
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
                        <Link key={offerId} className='decoractionless-link' to={{pathname:`/offers/${offerId}`}}>
                            <li key={offerId} className='offer-item'>
                                <h2>{offerName}</h2>
                                <p>Company: {companyName}</p>
                                <p>Voivodeship: {voivodeship}, city: {city}</p>
                                <p>Description: {offerDescription}</p>
                            </li>
                        </Link>
                    );
                })}
            </ul>
        </div>
    );
};

export default OffersList;
