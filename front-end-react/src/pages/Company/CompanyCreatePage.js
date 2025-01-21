import React, { useState } from "react";
import CompanyCreateForm from "../../components/Forms/CompanyCreateForm";
import { isValidRegon } from "../../utils/validators";
import CancelButton from "../../components/Buttons/CancelButton/CancelButton";
import { createCompany } from "../../services/CompanyService/CompanyService";
import { jwtRefresh } from "../../services/JwtRefreshService/JwtRefreshService";

const CreateCompanyPage = () => {
    jwtRefresh();
    
    const [companyData, setCompanyData] = useState({
        urlSegment: null,
        contactEmail: "",
        name: "",
        regon: "",
        description: "",
        branches: [],
    });

    const [message, setMessage] = useState("");

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setCompanyData((prev) => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!isValidRegon(companyData.regon)) {
            setMessage("REGON must have exactly 9 or 14 digits.");
            return;
        }

        try {
            await createCompany(companyData);
            setMessage("Company created successfully.");
            setTimeout(() => (window.location.href = "/userProfile"), 2000);
        } catch (err) {
            console.error(err);
            setMessage(err || "Failed to create company.");
        }
    };

    return (
        <div className="centered">

            <h1>Create Company</h1>
            <CompanyCreateForm
                companyData={companyData}
                onInputChange={handleInputChange}
                onSubmit={handleSubmit}
            />
            <CancelButton/>
            {message && <p>{message}</p>}
        </div>
    );
};

export default CreateCompanyPage;
