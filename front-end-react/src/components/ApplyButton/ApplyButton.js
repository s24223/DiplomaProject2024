import React from 'react';

const LoginButton = () => {
    const handleLogin = () => {
        window.location.href = "/apply"; 
    };

    return (
        <button onClick={handleLogin} style={{ position: 'absolute', bottom: 0, right: 0 }}>
            Apply
        </button>
    );
};

export default LoginButton;