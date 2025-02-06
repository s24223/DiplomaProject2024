import axios from 'axios';

export const createCompany = async (companyData) => {
    try {
        return await axios.post(
            "https://localhost:7166/api/User/company",
            companyData,
            {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem("jwt")}`,
                    "Content-Type": "application/json",
                },
            }
        ).then(res => res.data).catch(error => {
            switch (error.response.status) {
                case 500:
                    const idAppProblem = error.response.data.ProblemId
                    window.location.href = `/notification/create/${idAppProblem}`
                    break;
                default:
                    return { error: error.response.data.Message }
            }
        });
    } catch (error) {
        console.error("Error creating company:", error.response?.data || error.message);
        throw error.response?.data?.Message || "Failed to create company. Please try again.";
    }
};

// Pobierz dane firmy
export const fetchCompanyData = async () => {
    try {
        return await axios.get("https://localhost:7166/api/User", {
            headers: {
                Authorization: `Bearer ${localStorage.getItem("jwt")}`,
            },
        }).then(res => res.data.item).catch(error => {
            switch (error.response.status) {
                case 500:
                    const idAppProblem = error.response.data.ProblemId
                    window.location.href = `/notification/create/${idAppProblem}`
                    break;
                default:
                    return { error: error.response.data.Message }
            }
        });
    } catch (error) {
        console.error("Error fetching company data:", error.response?.data || error.message);
        throw new Error("Failed to load company data. Please try again.");
    }
};

// Zaktualizuj dane firmy
export const updateCompany = async (companyData) => {
    try {
        return await axios.put(
            "https://localhost:7166/api/User/company",
            companyData,
            {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem("jwt")}`,
                    "Content-Type": "application/json",
                },
            }
        ).then(res => res.data).catch(error => {
            switch (error.response.status) {
                case 500:
                    const idAppProblem = error.response.data.ProblemId
                    window.location.href = `/notification/create/${idAppProblem}`
                    break;
                default:
                    return { error: error.response.data.Message }
            }
        });
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

    return await axios.get(`https://localhost:7166/api/Companies/${companyId}/branch?${queryParams}`).then(res => res.data).catch(error => {
        switch (error.response.status) {
            case 500:
                const idAppProblem = error.response.data.ProblemId
                window.location.href = `/notification/create/${idAppProblem}`
                break;
            default:
                return { error: error.response.data.Message }
        }
    });
};

export const fetchCompanyBranches = async (companyId, queryParams) => {
    try {
        return await axios.get(`https://localhost:7166/api/Companies/${companyId}/branch?${queryParams}`).then(res => res.data).catch(error => {
            switch (error.response.status) {
                case 500:
                    const idAppProblem = error.response.data.ProblemId
                    window.location.href = `/notification/create/${idAppProblem}`
                    break;
                default:
                    return { error: error.response.data.Message }
            }
        });
    } catch (error) {
        console.error("Error fetching company branches:", error);
        throw new Error("Failed to fetch company branches.");
    }
};