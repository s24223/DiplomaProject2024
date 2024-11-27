import React from 'react';

const ProfileCancelButton = () => {
    const handleLogin = () => {
       
        window.location.href = "/userProfile"; 
    };

    return (
        <button onClick={handleLogin}>
            Cancel
        </button>
    );
};

export default ProfileCancelButton;