import React, { useState, useEffect } from 'react';
import { fetchOffers } from '../../services/OffersService/OffersService';
import OffersList from '../../components/OffersList/OffersList';
import LoginButton from '../../components/LoginButton/LoginButton';
import NotificationButton from '../../components/NotificationButton/NotificationButton';

import './MainPage.css';

const MainPage = () => {
    const [offers, setOffers] = useState(null);
    const [filters, setFilters] = useState({
        companyId: '',
        divisionId: '',
        isPayed: '',
        publishStart: '',
        publishEnd: '',
        salaryMin: '',
        salaryMax: '',
        isNegotiatedSalary: '',
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
                setOffers(data);
            } catch (error) {
                console.error('Error fetching offers:', error);
            }
        };
        loadOffers();
    }, [filters]);

    return (
        <div className="main-container">
            
            {/* Left Sidebar: Filters */}
            <div className="filters-sidebar">
                <LoginButton/>
                <NotificationButton/>
                
                <h3>Filters</h3>
                <form>
                    <input
                        type="text"
                        name="companyId"
                        placeholder="Company ID"
                        value={filters.companyId}
                        onChange={handleFilterChange}
                    />
                    <input
                        type="number"
                        name="divisionId"
                        placeholder="Division ID"
                        value={filters.divisionId}
                        onChange={handleFilterChange}
                    />
                    <label>
                        Paid:
                        <input
                            type="checkbox"
                            name="isPayed"
                            checked={filters.isPayed}
                            onChange={handleFilterChange}
                        />
                    </label>
                    publishStart:
                    <input
                        type="date"
                        name="publishStart"
                        placeholder="Publish Start"
                        value={filters.publishStart}
                        onChange={handleFilterChange}
                    />
                    publishEnd: 
                    <input
                        type="date"
                        name="publishEnd"
                        placeholder="Publish End"
                        value={filters.publishEnd}
                        onChange={handleFilterChange}
                    />
                    <input
                        type="number"
                        name="salaryMin"
                        placeholder="Min Salary"
                        value={filters.salaryMin}
                        onChange={handleFilterChange}
                    />
                    <input
                        type="number"
                        name="salaryMax"
                        placeholder="Max Salary"
                        value={filters.salaryMax}
                        onChange={handleFilterChange}
                    />
                    <label>
                        Możliwość negocjacji?:
                        <input
                            type="checkbox"
                            name="isNegotiatedSalary"
                            checked={filters.isNegotiatedSalary}
                            onChange={handleFilterChange}
                        />
                    </label>
                    <select name="orderBy" value={filters.orderBy} onChange={handleFilterChange}>
                        <option value="publishStart">Publish Start</option>
                        <option value="salaryMin">Salary Min</option>
                        <option value="salaryMax">Salary Max</option>
                    </select>
                    <select name="ascending" value={filters.ascending} onChange={handleFilterChange}>
                        <option value={true}>Ascending</option>
                        <option value={false}>Descending</option>
                    </select>
                    <input
                        type="number"
                        name="maxItems"
                        placeholder="Items per Page"
                        value={filters.maxItems}
                        onChange={handleFilterChange}
                    />
                </form>
            </div>

            {/* Center Content: Offers */}
            <div className="offers-container">
                
                <h1>Offers</h1>
                {offers && offers.items && <OffersList offers={offers.items} />}
                {/* Pagination */}
                <div className="pagination">
                    <button
                        onClick={() => handlePageChange(filters.page - 1)}
                        disabled={filters.page === 1}
                    >
                        Previous
                    </button>
                    <span>Page: {filters.page}</span>
                    <button
                        onClick={() => handlePageChange(filters.page + 1)}
                        disabled={offers && offers.items.length < filters.maxItems}
                    >
                        Next
                    </button>
                </div>
            </div>
        </div>
    );
};

export default MainPage;
