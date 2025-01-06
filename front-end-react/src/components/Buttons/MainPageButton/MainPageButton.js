import React from 'react';

const MainPageButton = () => {
    const handleLogin = () => {
       
        window.location.href = "/"; 
    };

    return (
        <button onClick={handleLogin}>
            Main Page
        </button>
    );
};

export default MainPageButton;