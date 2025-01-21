import axios from 'axios';

export const createCompany = async (companyData) => {
    try {
        const response = await axios.post(
            "https://localhost:7166/api/User/company",
            companyData,
            {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem("jwt")}`,
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
                Authorization: `Bearer ${localStorage.getItem("jwt")}`,
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
                    Authorization: `Bearer ${localStorage.getItem("jwt")}`,
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

export const fetchCompanyDetails = async (companyId, page, maxItems, selectedWojewodztwo) => {
    const queryParams = new URLSearchParams({
        orderBy: 'hierarchy',
        ascending: true,
        maxItems,
        page,
        wojewodztwo: selectedWojewodztwo || '',
    }).toString();

    const response = await fetch(
        `https://localhost:7166/api/Companies/${companyId}/branch?${queryParams}`
    );
    if (!response.ok) {
        throw new Error('Failed to fetch company details');
    }
    return await response.json();
};

export const fetchCompanyBranches = async (companyId, queryParams) => {
    try {
        const response = await axios.get(`https://localhost:7166/api/Companies/${companyId}/branch?${queryParams}`);
        return response.data;
    } catch (error) {
        console.error("Error fetching company branches:", error);
        throw new Error("Failed to fetch company branches.");
    }
};