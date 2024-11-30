import React from 'react';
import { fetchLogout } from '../../services/LogOutService/LogOutService';

const LoginButton = () => {
    const handleLogin = () => {
        const currentPath = window.location.pathname;
        sessionStorage.setItem("redirectAfterLogin", currentPath);
        // Przekierowanie do strony logowania
        window.location.href = "/login"; // lub jakikolwiek inny routing do logowania
    };

    const handleRegistration = () => {
        window.location.href = '/register';
    };

    const handleLogOut = () => {
        fetchLogout()
        window.location.reload()
        window.location.href = '/'
    };

    const handleProfile = () => {
        window.location.href = "/userProfile"; 
    };
    return (
        <div className='login-div'>
            {sessionStorage.getItem("jwt") === null ?
            <div className='two-button'>
                <button className='button-one' onClick={handleLogin} >
                    Zaloguj
                </button>
                <button className='button-two' onClick={handleRegistration} >
                    Zarejestruj
                </button>
            </div> : 
            <div className='two-button'>
                <button className='button-one' onClick={handleProfile} >
                Profile
                </button>
                <button className='button-two' onClick={handleLogOut} >
                    Wyloguj    
                </button>    
            </div>}
        </div>
    );
};

export default LoginButton;
