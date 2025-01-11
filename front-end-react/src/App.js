// export default App;
import React from 'react';
import { Cron } from 'croner';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import UrlDetailsPage from './pages/UrlDetailsPage/UrlDetailsPage';
import TestPage from './pages/TestPage/TestPage';
import MainPage from './pages/MainPage/MainPage';
import OfferDetailsPage from './pages/Offer/OfferDetailsPage';
import EditUrlPage from './pages/UrlEditPage/EditUrlPage';
import LoginPage from './pages/LoginPage/LoginPage';
import RegisterPage from './pages/RegisterPage/RegisterPage';
import ProfilePage from './pages/Profile/ProfilePage';
import ProfileCreatePage from './pages/Profile/ProfileCreatePage';
import ProfileEditPage from './pages/Profile/ProfileEditPage';
// import ChangePasswordPage from './pages/ChangePasswordPage/ChangePasswordPage';
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
            <div>
                <div className="header">
                    <NotificationButton />
                    <LoginButton />
                </div>
                <div className="header2">
                    <MainPageButton/>
                    <ReturnButton/>
                </div>
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
        </Router>
    );
}

export default App;
