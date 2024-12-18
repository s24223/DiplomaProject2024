import React, { useState, useEffect } from "react";
import axios from "axios";
import LoginButton from "../../components/LoginButton/LoginButton";
import MainPageButton from "../../components/MainPageButton/MainPageButton";
import ReturnButton from "../../components/CancelButton/ReturnButton";

const PersonRecruitmentPage = () => {
    const [applications, setApplications] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchApplications = async () => {
            try {
                const response = await axios.get("https://localhost:7166/api/User/person/recruitment", {
                    headers: {
                        Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
                    },
                    params: {
                        filterStatus: false,
                        orderBy: "created",
                        ascending: true,
                        maxItems: 100,
                        page: 1,
                    },
                });
                setApplications(response.data.items);
                setLoading(false);
            } catch (err) {
                console.error("Error fetching applications:", err);
                setError("Failed to load applications.");
                setLoading(false);
            }
        };

        fetchApplications();
    }, []);

    if (loading) {
        return <p>Loading applications...</p>;
    }

    if (error) {
        return <p>{error}</p>;
    }

    return (
        <div className="applications">
            <MainPageButton/>
            <LoginButton/>
            <ReturnButton/>
            <h1>My Applications</h1>
            {applications.length > 0 ? (
                <ul>
                    {applications.map((application) => (
                        <li key={application.recruitment.id}>
                            <h3>Company: {application.company.name}</h3>
                            <p>Offer: {application.offer.name}</p>
                            <p>Branch: {application.branch.name}</p>
                            <p>Status: {application.recruitment.isAccepted === null
                                ? "Pending"
                                : application.recruitment.isAccepted
                                ? "Accepted"
                                : "Rejected"
                            }</p>
                            <p>Company Response: {application.recruitment.companyResponse || "No response yet"}</p>
                        </li>
                    ))}
                </ul>
            ) : (
                <p>No applications found.</p>
            )}
        </div>
    );
};

export default PersonRecruitmentPage;
