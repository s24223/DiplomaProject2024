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
import CompanyCreatePage from './pages/CompanyCreatePage/CompanyCreatePage';
import CompanyEditPage from './pages/CompanyEditPage/CompanyEditPage';
import CreateBranchPage from './pages/BranchCreatePage/BranchCreatePage';
import BranchDetailPage from './pages/BranchDetailsPage/BranchDetailsPage';
import NotificationPage from './pages/NotificationPage/NotificationPage';
import NotificationDetailPgae from './pages/NotificationDetailsPage/NotificationDetailPage';
import NotificationCreate from './pages/NotificationCreate/NotificationCreate';
import UrlDetailsPage from './pages/UrlDetailsPage/UrlDetailsPage';
import EditUrlPage from './pages/UrlEditPage/EditUrlPage';

const job = new Cron("*/5 * * * *", () => {
    console.log(`Cron run... ${new Date().toLocaleTimeString()}`)
    if(sessionStorage.getItem("jwt")){
        const fetchDummy = async () => {
            let response = await fetch("https://localhost:7166/api/User/refresh", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${sessionStorage.getItem("jwt")}`,
                    "Access-Control-Allow-Origin": "*"
                },
                body: JSON.stringify({"refreshToken": sessionStorage.getItem("refreshToken")})
            })
            console.log(response)
            if (response.ok)
            {
                response = await response.json()
                sessionStorage.setItem("jwt", response.jwt)
                sessionStorage.setItem("jwtValidTo", response.jwtValidTo)
                sessionStorage.setItem("refreshToken", response.refereshToken)
                sessionStorage.setItem("refreshTokenValidTo", response.refereshTokenValidTo)
            } 
            else 
            {
                sessionStorage.removeItem("jwt")
                sessionStorage.removeItem("jwtValidTo")
                sessionStorage.removeItem("refreshToken")
                sessionStorage.removeItem("refreshTokenValidTo")
            }
        }
        fetchDummy()
    }
    console.log(`Cron run ended ${new Date().toLocaleTimeString()}`)
})

function App() {
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

                <Route path="/userCreateCompany" element={<CompanyCreatePage />} />
                <Route path="/userEditCompany" element={<CompanyEditPage />} />

                <Route path="/createBranch" element={<CreateBranchPage />} />
                <Route path="/branch/:id" element={<BranchDetailPage />} />

                <Route path="notification" element={<NotificationPage />} />
                <Route path="notification/:id" element={<NotificationDetailPgae />} />
                <Route path="/notification/create" element={<NotificationCreate />} />
                
                <Route path="/url/:id" element={<UrlDetailsPage />} />
                <Route path="/url/edit/:id" element={<EditUrlPage />} />
                


            </Routes>
        </Router>
    );
}

export default App;
