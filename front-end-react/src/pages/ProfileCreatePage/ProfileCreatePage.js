import React from "react";
import CreateProfile from "../../components/PersonProfileCreate/CreateProfile";
import MainPageButton from "../../components/MainPageButton/MainPageButton";
import ProfileButton from "../../components/ProfileButton/ProfileButton";
import ReturnButton from "../../components/CancelButton/ReturnButton";
import './ProfileCreatePage.css'


const ProfileCreatePage = () =>{
    return(
        <div className="centered">
            <MainPageButton/><br/>
            <ReturnButton/><br/>

            <h2>Ceate Profile</h2>
            <CreateProfile/>
        </div>
    )
}

export default ProfileCreatePage;