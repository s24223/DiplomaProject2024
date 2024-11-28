import React from "react";
import CreateProfile from "../../components/PersonProfileCreate/CreateProfile";
import MainPageButton from "../../components/MainPageButton/MainPageButton";
import ProfileButton from "../../components/ProfileButton/ProfileButton";
import ReturnButton from "../../components/CancelButton/ReturnButton";


const ProfileCreatePage = () =>{
    return(
        <div>
            <MainPageButton/><br/>
            <ReturnButton/><br/>

            Ceate Profile
            <CreateProfile/>
        </div>
    )
}

export default ProfileCreatePage;