import axios from "axios";

export const fetchUserProfile = async () => {
    const response = await axios.get("https://localhost:7166/api/User", {
        headers: {
            Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
        },
    });
    return response.data.item;
};
