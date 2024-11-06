import React from 'react';

const LoginButton = () => {
    const handleLogin = () => {
        // Przekierowanie do strony logowania
        window.location.href = "/login"; // lub jakikolwiek inny routing do logowania
    };

    const handleRegistration = () => {
        window.location.href = '/register';
    };

    return (
        <div>
            <button onClick={handleLogin} style={{ position: 'absolute', top: 10, right: 10 }}>
                Zaloguj
            </button>
            <button onClick={handleRegistration}>
                Zarejestruj
            </button>
        </div>
    );
};

export default LoginButton;
