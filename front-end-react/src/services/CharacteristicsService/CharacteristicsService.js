import axios from "axios"

export const fetchCharacteristics = async () => {
    
        const response = await axios.get(
            "https://localhost:7166/api/Dictionaries/characteristics?isOrderByType=false",
            {
                headers: {
                    Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
                },
            }
        );
        if (!response.data.items) {
            throw new Error("Failed to fetch characteristics.");
        }
    
        return response.data.items;
};