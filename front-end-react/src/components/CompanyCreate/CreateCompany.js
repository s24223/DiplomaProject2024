import React, { useState } from "react";
import axios from "axios";
import CancelButton from "../CancelButton/ReturnButton";
import { createCompany } from "../../services/CompanyService/ComapnyService";

const CreateCompany = () => {
    const [companyData, setCompanyData] = useState({
        urlSegment: null, // może być null
        contactEmail: "",
        name: "",
        regon: "",
        description: "",
        branches: [], // Oddziały można dodawać później
    });

    const [message, setMessage] = useState("");

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setCompanyData((prevData) => ({
            ...prevData,
            [name]: value,
        }));
    };

    const handleCreateCompany = async (event) => {
        event.preventDefault();

        // Walidacja REGON
        const regonPattern = /^[0-9]{9}$|^[0-9]{14}$/;
        if (!regonPattern.test(companyData.regon)) {
            setMessage("REGON must have exactly 9 or 14 digits.");
            return;
        }

        try {
            await createCompany(companyData);

            // Przekierowanie po sukcesie
            setMessage("Company created successfully.");
            setTimeout(() => {
                window.location.href = "/userProfile";
            }, 2000); // Czas na wyświetlenie wiadomości (2 sekundy)
        } catch (error) {
            console.error("Error creating company:", error.response?.data || error.message);

            if (error.response?.data?.Message) {
                setMessage(error.response.data.Message);
            } else {
                setMessage("Failed to create company. Please try again.");
            }

            // Przekierowanie, jeśli dane wejściowe są `null`
            if (companyData.urlSegment === null) {
                setTimeout(() => {
                    window.location.href = "/userProfile";
                }, 2000);
            }
        }
    };

    return (
        <div className="centered">
            <h1>Create Company</h1>
            <form onSubmit={handleCreateCompany}>
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
                <label>REGON:</label><br/>
                <input
                    type="text"
                    name="regon"
                    value={companyData.regon}
                    onChange={handleInputChange}
                    pattern="[0-9]{9}|[0-9]{14}" // Walidacja po stronie przeglądarki
                    title="REGON must have 9 or 14 digits"
                    required
                /><br/>
                <label>Description:</label><br/>
                <textarea
                    name="description"
                    value={companyData.description}
                    onChange={handleInputChange}
                ></textarea><br/>
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
                /><br/>
                <button type="submit">Create Company</button><br/>
                <CancelButton/>
            </form>
            {message && <p>{message}</p>}
        </div>
    );
};

export default CreateCompany;
