import Profile from "../../components/PersonProfile/Profile";
import { jwtRefresh } from "../../services/JwtRefreshService/JwtRefreshService";

const ProfilePage = () => {
    jwtRefresh();

    return(
        <div>
            <Profile />
        </div>
    )
}

export default ProfilePage;
