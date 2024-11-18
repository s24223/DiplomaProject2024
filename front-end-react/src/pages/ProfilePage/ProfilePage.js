import React, { useEffect } from "react";
import { fetchProfileGet } from "../../services/ProfileService/ProfilceService";
import Profile from "../../components/PersonProfile/Profile";

const ProfilePage = () => {

    return(
        <div>
            <Profile />
        </div>
    )
}

export default ProfilePage;