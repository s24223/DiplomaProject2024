import React, { useState, useEffect } from "react";
import { fetchOffers } from "../../services/OffersService/OffersService";
import FilterForm from "../../components/Filters/FiltersForm";
import Pagination from "../../components/Filters/Pagination";
import OffersList from "../../components/Offers/OffersList";
import { jwtRefresh } from "../../services/JwtRefreshService/JwtRefreshService";
import "./MainPage.css"; 

const MainPage = () => {
    jwtRefresh();

    const [offers, setOffers] = useState(null);
    const [filters, setFilters] = useState({
        searchText: '',
        orderBy: 'publishStart',
        ascending: true,
        maxItems: 5,
        page: 1,
    });

    const handleFilterChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFilters((prevFilters) => ({
            ...prevFilters,
            [name]: type === 'checkbox' ? checked : value,
        }));
    };

    const handlePageChange = (newPage) => {
        setFilters((prevFilters) => ({ ...prevFilters, page: newPage }));
    };

    useEffect(() => {
        const loadOffers = async () => {
            try {
                const data = await fetchOffers(filters);
                if(data.error){
                    throw new Error(data.error)
                }
                setOffers(data);
            } catch (error) {
                console.error('Error fetching offers:', error);
            }
        };
        loadOffers();
    }, [filters]);

    return (
        <div className="main-container">
    
            <div className="filters-sidebar">
                
                <FilterForm filters={filters} onFilterChange={handleFilterChange} />
            </div>

            {/* Sekcja ofert */}
            <div className="offers-container">
                <h1 className="title">Offers</h1>
                {offers && offers.items && (
                    <OffersList offers={offers.items} onOfferClick={(id) => console.log(id)} />
                )}
                <Pagination
                    currentPage={filters.page}
                    onPageChange={handlePageChange}
                    hasNext={offers && offers.items.length === filters.maxItems}
                />
            </div>
        </div>
    );
};

export default MainPage;
