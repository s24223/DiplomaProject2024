import React, { useState, useEffect } from "react";
import CompanyEditForm from "../../components/Forms/CompanyEditForm";
import { fetchCompanyData, updateCompany } from "../../services/CompanyService/CompanyService";
import CancelButton from "../../components/Buttons/CancelButton/CancelButton";
import { jwtRefresh } from "../../services/JwtRefreshService/JwtRefreshService";

const EditCompanyPage = () => {
    jwtRefresh();
    
    const [companyData, setCompanyData] = useState({
        urlSegment: null,
        contactEmail: "",
        name: "",
        description: "",
    });

    const [loading, setLoading] = useState(true);
    const [message, setMessage] = useState("");

    useEffect(() => {
        const loadCompanyData = async () => {
            try {
                const company = await fetchCompanyData();
                if(company.error){
                    throw new Error(company.error)
                }
                if (company) {
                    setCompanyData({
                        urlSegment: company.urlSegment || "",
                        contactEmail: company.contactEmail || "",
                        name: company.name || "",
                        description: company.description || "",
                    });
                }
            } catch (error) {
                console.error("Error fetching company data:", error);
                setMessage("Failed to load company data. Please try again.");
            } finally {
                setLoading(false);
            }
        };

        loadCompanyData();
    }, []);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setCompanyData((prevData) => ({
            ...prevData,
            [name]: value,
        }));
    };

    const handleEditCompany = async (e) => {
        e.preventDefault();
        try {
            let res = await updateCompany(companyData);
            if(res.error){
                throw new Error(res.error)
            }
            setMessage("Company updated successfully.");
            setTimeout(() => (window.location.href = "/userProfile"), 2000);
        } catch (error) {
            setMessage(error || "Failed to update company.");
        }
    };

    if (loading) {
        return <p>Loading company data...</p>;
    }

    return (
        <div className="centered">

            <h1>Edit Company</h1>
            <CompanyEditForm
                companyData={companyData}
                onInputChange={handleInputChange}
                onSubmit={handleEditCompany}
            />
            <CancelButton />
            {message && <p>{message}</p>}
        </div>
    );
};

export default EditCompanyPage;
