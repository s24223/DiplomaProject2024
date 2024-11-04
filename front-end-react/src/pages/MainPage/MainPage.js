import React, { useState, useEffect } from 'react';
import SearchBar from '../../components/SearchBar/SearchBar';
import LoginButton from '../../components/LoginButton/LoginButton';
import { fetchOffers } from '../../services/OffersService/OffersService';
import OffersList from '../../components/OffersList/OffersList';
import './MainPage.css';

const MainPage = () => {
    const [offers, setOffers] = useState([]);
    const [searchQuery, setSearchQuery] = useState('');

    useEffect(() => {
        const loadOffers = async () => {
            try {
                const data = await fetchOffers(searchQuery);
                console.log(data);
                setOffers(data);
            } catch (error) {
                console.error("Error fetching offers:", error);
            }
        };
        loadOffers();
    }, [searchQuery]);

    return (
        <div className="main-page">
            <h1>Strona główna</h1>
            <LoginButton />
            <SearchBar onSearch={setSearchQuery} />
            <OffersList offers={offers} />
        </div>
    );
};

export default MainPage;
