// export default App;
import React from 'react';
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
