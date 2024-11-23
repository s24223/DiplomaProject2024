import React from "react";
import MainPageButton from "../../components/MainPageButton/MainPageButton";
import EditProfile from "../../components/PersonProfileEdit/EditProfile";
import ProfileButton from "../../components/ProfileButton/ProfileButton";


const ProfileCreatePage = () =>{
    return(
        <div>
            <MainPageButton/><br/>
            <ProfileButton/><br/>
            <EditProfile/>
        </div>
    )
}

export default ProfileCreatePage;