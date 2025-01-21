import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import UrlDetailsPage from './pages/Url/UrlDetailsPage';
import TestPage from './pages/TestPage/TestPage';
import MainPage from './pages/MainPage/MainPage';
import OfferDetailsPage from './pages/Offer/OfferDetailsPage';
import EditUrlPage from './pages/Url/EditUrlPage';
import LoginPage from './pages/LoginPage/LoginPage';
import RegisterPage from './pages/RegisterPage/RegisterPage';
import ProfilePage from './pages/Profile/ProfilePage';
import ProfileCreatePage from './pages/Profile/ProfileCreatePage';
import ProfileEditPage from './pages/Profile/ProfileEditPage';
import CompanyCreatePage from './pages/Company/CompanyCreatePage';
import CompanyEditPage from './pages/Company/CompanyEditPage';
import CompanyRecruitmentPage from './pages/Company/CompanyRecruitmentPage';
import BranchCreatePage from './pages/Branch/BranchCreatePage';
import BranchDetailPagePrivate from './pages/Branch/BranchDetailsPagePrivate';
import NotificationPage from './pages/Notification/NotificationPage/NotificationPage';
import NotificationDetailPage from './pages/Notification/NotificationDetailsPage/NotificationDetailPage';
import NotificationCreate from './pages/Notification/NotificationCreate/NotificationCreate';
import CompanyDetailsPublic from './components/Company/CompanyDetailsPublic';
import PersonRecruitmentPage from './pages/PersonRecruitmentPage/PersonRecruitmentPage';
import LoginButton from './components/Buttons/LoginButton/LoginButton';
import ReturnButton from './components/Buttons/CancelButton/ReturnButton';
import MainPageButton from './components/Buttons/MainPageButton/MainPageButton';
import NotificationButton from './components/Buttons/NotificationButton/NotificationButton';
import BranchDetailsPagePublic from './pages/Branch/BranchDetailsPagePublic';
import PrivateOfferDetailsPage from './pages/Offer/PrivateOfferDetailsPage';
import ProfileChnagePassword from './pages/Profile/ProfilePasswordChange';

function App() {
    return (
        <Router>
            <div>
            <div className="header">
                    <div className="buttons-group-left">
                        <MainPageButton />
                        <ReturnButton />
                    </div>
                    <div className="buttons-group-right">
                        <NotificationButton />
                        <LoginButton />
                    </div>
                    
                </div>

                <div className="new-container">
                <Routes>
                    <Route path="/" element={<MainPage />} />
                    <Route path="/offers" element={<MainPage />} />
                    {/* <Route path="/offers/:offerId" element={<OfferDetailsPage />} />
                    */}
                    <Route path="/offers/:offerId" element={<OfferDetailsPage/>} />
                    <Route path="/offers/:offerId/edit" element={<PrivateOfferDetailsPage />} />




                    <Route path="/login" element={<LoginPage />} />
                    <Route path="/register" element={<RegisterPage />} />
                    <Route path="/userProfile" element={<ProfilePage />} />
                    <Route path="/userCreateProfile" element={<ProfileCreatePage />} />
                    
                    <Route path="/userEditProfile" element={<ProfileEditPage />} />
                    {/* <Route path="/changePassword" element={<ChangePasswordPage />} /> */}

                    <Route path="/userCreateCompany" element={<CompanyCreatePage />} />
                    <Route path="/userEditCompany" element={<CompanyEditPage />} />

                    <Route path="/createBranch" element={<BranchCreatePage />} />
                    <Route path="/branch/:id" element={<BranchDetailPagePrivate />} />

                    <Route path="notification" element={<NotificationPage />} />
                    <Route path="notification/:id" element={<NotificationDetailPage />} />
                    <Route path="/notification/create" element={<NotificationCreate />} />
                    
                    <Route path="/url/:id" element={<UrlDetailsPage />} />
                    <Route path="/url/edit/:id" element={<EditUrlPage />} />
                    
                    <Route path="test" element={<TestPage />} />


                    <Route path="/companyRecruitment" element={<CompanyRecruitmentPage />} />
                    <Route path="/personRecruitment" element={<PersonRecruitmentPage />} />


                    <Route path="/company/:companyId" element={<CompanyDetailsPublic />} />
                    <Route path="public/branch/:branchId" element={<BranchDetailsPagePublic />} />

                    <Route path='/changePassword' element={<ProfileChnagePassword />} />
                </Routes>
                </div>
            </div>
        </Router>
    );
}

export default App;
