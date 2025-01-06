import React from "react";
import CreateProfile from "../../components/PersonProfileCreate/CreateProfile";
import './ProfileCreatePage.css'


const ProfileCreatePage = () =>{
    return(
        <div className="centered">
            <h2>Ceate Profile</h2>
            <CreateProfile/>
        </div>
    )
}

export default ProfileCreatePage;