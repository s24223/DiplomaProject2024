import React from "react";
import CreateProfile from "../../components/PersonProfileCreate/CreateProfile";
import MainPageButton from "../../components/MainPageButton/MainPageButton";
import './ProfileCreatePage.css'


const ProfileCreatePage = () =>{
    return(
        <div className="centered">
            <MainPageButton/><br/>
            <h2>Ceate Profile</h2>
            <CreateProfile/>
        </div>
    )
}

export default ProfileCreatePage;