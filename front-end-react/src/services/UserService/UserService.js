import axios from "axios";

export const fetchUserProfile = async () => {
    const response = await axios.get("https://localhost:7166/api/User", {
        headers: {
            Authorization: `Bearer ${localStorage.getItem("jwt")}`,
        },
    });
    return response.data.item;
};
