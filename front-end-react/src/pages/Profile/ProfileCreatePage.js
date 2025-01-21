import React from "react";
import CreateProfile from "../../components/PersonProfile/CreateProfile";
import { jwtRefresh } from "../../services/JwtRefreshService/JwtRefreshService";


const ProfileCreatePage = () =>{
    jwtRefresh();
    
    return(
        <div className="centered">
            <h2>Ceate Profile</h2>
            <CreateProfile/>
        </div>
    )
}

export default ProfileCreatePage;