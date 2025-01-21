import React from "react";
import EditProfile from "../../components/PersonProfile/EditProfile";
import { jwtRefresh } from "../../services/JwtRefreshService/JwtRefreshService";


const ProfileEditPage = () =>{
    jwtRefresh();
    
    return(
        <div>

            <EditProfile/>
        </div>
    )
}

export default ProfileEditPage;