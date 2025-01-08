import axios from "axios";


export const updateUrl = async (urlData) => {
    const response = await axios.put(`https://localhost:7166/api/User/urls/urls`, [urlData], {
        headers: {
            Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
        },
    });
    return response.data;
};

export const deleteUrl = async (urlItem) => {
    const response = await axios.delete(`https://localhost:7166/api/User/urls`, {
        headers: {
            Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
        },
        data: [{ urlTypeId: urlItem.urlTypeId, created: urlItem.created }],
    });
    return response.data;
};
