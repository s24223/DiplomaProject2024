import React from "react";
import CreateProfile from "../../components/PersonProfileCreate/CreateProfile";
import MainPageButton from "../../components/MainPageButton/MainPageButton";
import ProfileButton from "../../components/ProfileButton/ProfileButton";


const ProfileCreatePage = () =>{
    return(
        <div>
            <MainPageButton/><br/>
            <ProfileButton/><br/>
            Ceate Profile
            <CreateProfile/>
        </div>
    )
}

export default ProfileCreatePage;