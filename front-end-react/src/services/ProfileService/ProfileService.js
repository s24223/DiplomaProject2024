import axios from "axios";

export const fetchProfilePost = async (body) => {
    await fetch("https://localhost:7166/api/User/person",
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Access-Contorl-Allow-Origin": "*",
                "Authorization": `Bearer ${sessionStorage.getItem("jwt")}`
            },
            body: JSON.stringify(body)
        }
    ).then((response) => {
        if (!response.ok)
            throw new Error(response.status)
        else console.log(response.json())
    });
}



export const fetchUserProfile = async () => {
    const token = sessionStorage.getItem("jwt") || localStorage.getItem("jwt");
    if (!token) throw new Error("No authorization token found.");
    
    const response = await axios.get(`https://localhost:7166/api/User`, {
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });
    return response.data.item.person;
};

export const updateUserProfile = async (profileData) => {
    const token = sessionStorage.getItem("jwt") || localStorage.getItem("jwt");
    if (!token) throw new Error("No authorization token found.");

    const response = await axios.put(`https://localhost:7166/api/User/person`, profileData, {
        headers: {
            Authorization: `Bearer ${token}`,
            "Content-Type": "application/json",
        },
    });
    return response.data;
};
