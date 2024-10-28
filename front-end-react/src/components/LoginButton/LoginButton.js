import React from 'react';

const LoginButton = () => {
    const handleLogin = () => {
        // Przekierowanie do strony logowania
        window.location.href = "/login"; // lub jakikolwiek inny routing do logowania
    };

    return (
        <button onClick={handleLogin} style={{ position: 'absolute', top: 10, right: 10 }}>
            Zaloguj
        </button>
    );
};

export default LoginButton;
