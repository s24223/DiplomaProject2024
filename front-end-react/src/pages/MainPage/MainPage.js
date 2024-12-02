// MainPage.js
import React, { useState, useEffect } from 'react';
import SearchBar from '../../components/SearchBar/SearchBar';
import LoginButton from '../../components/LoginButton/LoginButton';
import { fetchOffers } from '../../services/OffersService/OffersService';
import OffersList from '../../components/OffersList/OffersList';
import NotificationButton from '../../components/NotificationButton/NotificationButton';
import './MainPage.css';

const MainPage = () => {
    const [offers, setOffers] = useState();
    const [searchQuery, setSearchQuery] = useState('');

    useEffect(() => {
        const loadOffers = async () => {
            try {
                const data = await fetchOffers(searchQuery);
                //setOffers(data);
                // filtrowanie unikalnych offer, jesli cos nie zadziała:
                const uniqueOffers = data.items.filter(
                    (item, index, self) => 
                        index === self.findIndex((o) => o.offer.id === item.offer.id)
                );
                setOffers({ ...data, items: uniqueOffers });
            } catch (error) {
                console.error("Error fetching offers:", error);
            }
        };
        loadOffers();
    }, [searchQuery]);

    return (
        <div className="main-page">
            <h1 className='title-text'>Strona główna</h1>
            <LoginButton /><br />
            <NotificationButton />
            <SearchBar onSearch={setSearchQuery} />
            {offers && offers.items && <OffersList offers={offers.items} />}
        </div>
    );
};

export default MainPage;
