import React from 'react';

const ProfileCencelButton = () => {
    const handleLogin = () => {
       
        window.location.href = "/userProfile"; 
    };

    return (
        <button onClick={handleLogin}>
            Cencel
        </button>
    );
};

export default ProfileCencelButton;