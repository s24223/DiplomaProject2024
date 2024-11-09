import React from 'react';
import { fetchLogout } from '../../services/LogOutService/LogOutService';

const LoginButton = () => {
    const handleLogin = () => {
        // Przekierowanie do strony logowania
        window.location.href = "/login"; // lub jakikolwiek inny routing do logowania
    };

    const handleRegistration = () => {
        window.location.href = '/register';
    };

    const handleLogOut = () => {
        fetchLogout()
        window.location.reload()
    };

    return (
        <div>
            {localStorage.getItem("jwt") === null ?
            <div>
                <button onClick={handleLogin} style={{ position: 'absolute', top: 10, right: 10 }}>
                    Zaloguj
                </button>
                <button onClick={handleRegistration} style={{ position: 'absolute', top: 10, right: 70 }}>
                    Zarejestruj
                </button>
            </div> : 
            <div>
                <button onClick={handleLogOut} style={{ position: 'absolute', top: 10, right: 10 }}>
                    Wyloguj    
                </button>    
            </div>}
        </div>
    );
};

export default LoginButton;
