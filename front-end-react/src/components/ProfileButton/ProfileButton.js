import React from 'react';

const ProfileButton = () => {
    const handleLogin = () => {
       
        window.location.href = "/userProfile"; 
    };

    return (
        <button onClick={handleLogin}>
            Profile
        </button>
    );
};

export default ProfileButton;