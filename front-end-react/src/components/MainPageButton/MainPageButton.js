import React from 'react';

const MainPageButton = () => {
    const handleLogin = () => {
       
        window.location.href = "/"; 
    };

    return (
        <button onClick={handleLogin} style={{ position: 'absolute', top: 10, left: 10 }}>
            Main Page
        </button>
    );
};

export default MainPageButton;