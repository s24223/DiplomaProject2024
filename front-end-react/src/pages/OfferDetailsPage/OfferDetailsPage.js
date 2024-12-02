import React from "react";
import MainPageButton from "../../components/MainPageButton/MainPageButton";
import OfferDetails from "../../components/OfferDetails/OfferDetails";
import ProfileButton from "../../components/ProfileButton/ProfileButton";
import ReturnButton from "../../components/CancelButton/ReturnButton";


const ProfileCreatePage = () =>{
    return(
        <div>
            <MainPageButton/><br/>
            <ReturnButton/>
            <OfferDetails/>
        </div>
    )
}

export default ProfileCreatePage;