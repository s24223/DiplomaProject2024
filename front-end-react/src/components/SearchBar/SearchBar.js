import React, { useState } from 'react';

const SearchBar = ({ onSearch }) => {
    const [query, setQuery] = useState('');

    const handleSearch = () => {
        onSearch(query);
    };

    return (
        <div className='searchBar'>
            <input
                type="text" 
                placeholder="Szukaj ofert..." 
                value={query} 
                onChange={(e) => setQuery(e.target.value)} 
            />
            <button onClick={handleSearch}>Szukaj</button>
        </div>
    );
};

export default SearchBar;
