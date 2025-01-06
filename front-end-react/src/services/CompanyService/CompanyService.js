import axios from 'axios';

export const createCompany = async (companyData) => {
    try {
        const response = await axios.post(
            "https://localhost:7166/api/User/company",
            companyData,
            {
                headers: {
                    Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
                    "Content-Type": "application/json",
                },
            }
        );
        return response.data;
    } catch (error) {
        console.error("Error creating company:", error.response?.data || error.message);
        throw error.response?.data?.Message || "Failed to create company. Please try again.";
    }
};

// Pobierz dane firmy
export const fetchCompanyData = async () => {
    try {
        const response = await axios.get("https://localhost:7166/api/User", {
            headers: {
                Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
            },
        });
        const { company } = response.data.item;
        return company;
    } catch (error) {
        console.error("Error fetching company data:", error.response?.data || error.message);
        throw "Failed to load company data. Please try again.";
    }
};

// Zaktualizuj dane firmy
export const updateCompany = async (companyData) => {
    try {
        const response = await axios.put(
            "https://localhost:7166/api/User/company",
            companyData,
            {
                headers: {
                    Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
                    "Content-Type": "application/json",
                },
            }
        );
        return response.data;
    } catch (error) {
        console.error("Error updating company:", error.response?.data || error.message);
        throw error.response?.data?.Message || "Failed to update company. Please try again.";
    }
};

