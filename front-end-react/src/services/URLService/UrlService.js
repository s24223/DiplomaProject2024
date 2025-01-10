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
    const response = await axios.delete(`https://localhost:7166/api/User/urls/urls`, {
        headers: {
            Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
        },
        data: [{ urlTypeId: urlItem.urlTypeId, created: urlItem.created }],
    });
    return response.data;
};


// Pobierz typy URL-i
export const fetchUrlTypes = async () => {
    const response = await axios.get("https://localhost:7166/api/Dictionaries/user/urls/types", {
        headers: {
            Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
        },
    });
    return Object.values(response.data); // Zamień obiekt na tablicę
};

// Dodaj nowy URL
export const addUrl = async (urlData) => {
    const response = await axios.post(
        "https://localhost:7166/api/User/urls/urls",
        [urlData],
        {
            headers: {
                Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
            },
        }
    );

    if (response.status !== 201) {
        throw new Error("Failed to add URL.");
    }

    return response.data;
};
export const fetchUrls = async () => {
    const response = await axios.get(
        "https://localhost:7166/api/User/urls?orderBy=created&ascending=true&itemsCount=100&page=1",
        {
            headers: {
                Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
            },
        }
    );

    if (response.status !== 200) {
        throw new Error("Failed to fetch URLs.");
    }

    return response.data.item.urls;
};