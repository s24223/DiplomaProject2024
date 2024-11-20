// export default App;
import React from 'react';
import { Cron } from 'croner';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import MainPage from './pages/MainPage/MainPage';
import OfferDetailsPage from './pages/OfferDetailsPage/OfferDetailsPage';
import LoginPage from './pages/LoginPage/LoginPage';
import RegisterPage from './pages/RegisterPage/RegisterPage';
import ProfilePage from './pages/ProfilePage/ProfilePage';
import ProfileCreatePage from './pages/ProfileCreatePage/ProfileCreatePage';
import EditProfilePage from './pages/EditProfilePage/EditProfilePage';
import ChangePasswordPage from './pages/ChangePasswordPage/ChangePasswordPage';


function App() {
    const job = new Cron("*/10 * * * *", () => {
        console.log(`Cron run... ${new Date().toLocaleTimeString()}`)
        if(sessionStorage.getItem("jwt")){
            fetch("https://localhost:7166/api/User/refresh", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${sessionStorage.getItem("jwt")}`,
                    "Access-Control-Allow-Origin": "*"
                },
                body: JSON.stringify({"refreshToken": sessionStorage.getItem("refreshToken")})
            }).then((response) => {
                console.log(response.status)
                sessionStorage.setItem("jwt", response.jwt)
                sessionStorage.setItem("jwtValidTo", response.jwtValidTo)
                sessionStorage.setItem("refreshToken", response.refereshToken)
                sessionStorage.setItem("refreshTokenValidTo", response.refereshTokenValidTo)
            })
        }
        console.log(`Cron run ended ${new Date().toLocaleTimeString()}`)
    })

    return (
        <Router>
            <Routes>
                <Route path="/" element={<MainPage />} />
                <Route path="/offers" element={<MainPage />} />
                <Route path="/offers/:offerId" element={<OfferDetailsPage />} />
                <Route path="/login" element={<LoginPage />} />
                <Route path="/register" element={<RegisterPage />} />
                <Route path="/userProfile" element={<ProfilePage />} />
                <Route path="/userCreateProfile" element={<ProfileCreatePage />} />
                
                <Route path="/userEditProfile" element={<EditProfilePage />} />
                <Route path="/changePassword" element={<ChangePasswordPage />} />
            </Routes>
        </Router>
    );
}

export default App;
