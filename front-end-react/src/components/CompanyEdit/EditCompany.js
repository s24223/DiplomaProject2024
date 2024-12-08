import React, { useState, useEffect } from "react";
import CancelButton from "../CancelButton/CancelButton";
import { fetchCompanyData, updateCompany } from "../../services/CompanyService/ComapnyService";

const EditCompany = () => {
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

                if (company) {
                    setCompanyData({
                        urlSegment: company.urlSegment || "",
                        contactEmail: company.contactEmail || "",
                        name: company.name || "",
                        description: company.description || "",
                    });
                }

                setLoading(false);
            } catch (error) {
                console.error("Error fetching company data:", error);
                setMessage("Failed to load company data. Please try again.");
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

    const handleEditCompany = async (event) => {
        event.preventDefault();

        try {
            await updateCompany(companyData);

            // Przekierowanie po sukcesie
            setMessage("Company updated successfully.");
            setTimeout(() => {
                window.location.href = "/userProfile";
            }, 2000); // Czas na wyświetlenie wiadomości (2 sekundy)
        } catch (erroMessage) {
            setMessage(erroMessage);           
            // Przekierowanie w przypadku błędu, jeśli `urlSegment` jest `null`
            if (companyData.urlSegment === null) {
                setTimeout(() => {
                    window.location.href = "/userProfile";
                }, 2000);
            }
        }
    };
    if (loading) {
        return <p>Loading company data...</p>;
    }

    return (
        <div className="centered">
            <h1>Edit Company</h1>
            <form onSubmit={handleEditCompany}>
                <label>Name:</label><br/>
                <input
                    type="text"
                    name="name"
                    value={companyData.name}
                    onChange={handleInputChange}
                    required
                /><br/>
                <label>Email:</label><br/>
                <input
                    type="email"
                    name="contactEmail"
                    value={companyData.contactEmail}
                    onChange={handleInputChange}
                    required
                /><br/>
                <label>Description:</label><br/>
                <textarea
                    name="description"
                    value={companyData.description}
                    onChange={handleInputChange}
                ></textarea>
                <br/>
                <label>URL Segment (optional):</label><br/>
                <input
                    type="text"
                    name="urlSegment"
                    value={companyData.urlSegment || ""}
                    onChange={(e) =>
                        setCompanyData((prevData) => ({
                            ...prevData,
                            urlSegment: e.target.value || null,
                        }))
                    }
                /><br/><br/>
                <button type="submit">Save Changes</button>
                <CancelButton/>
            </form>
            {message && <p>{message}</p>}
        </div>
    );
};

export default EditCompany;
