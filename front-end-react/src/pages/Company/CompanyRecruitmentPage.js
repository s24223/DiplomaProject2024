import React, { useState, useEffect } from "react";
import axios from "axios";

const CompanyRecruitmentPage = () => {
    const [applications, setApplications] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    // Fetch applications on mount
    useEffect(() => {
        const fetchApplications = async () => {
            try {
                const response = await axios.get("https://localhost:7166/api/User/company/recruitment", {
                    headers: {
                        Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
                    },
                    params: {
                        filterStatus: true,
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

    // Function to handle responses
    const handleResponse = async (recruitmentId, isAccepted) => {
        const companyResponse = isAccepted
            ? "Your application has been accepted."
            : "Your application has been rejected.";

        try {
            await axios.put(
                `https://localhost:7166/api/User/recruitment/${recruitmentId}/answer`,
                { companyResponse, isAccepted },
                {
                    headers: {
                        Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
                    },
                }
            );
            alert(isAccepted ? "Application accepted!" : "Application rejected!");
            // Refresh applications list after response
            setApplications((prevApplications) =>
                prevApplications.filter((application) => application.recruitment.id !== recruitmentId)
            );
        } catch (error) {
            console.error("Error updating application status:", error);
            alert("Failed to update application status.");
        }
    };

    if (loading) {
        return <p>Loading applications...</p>;
    }

    if (error) {
        return <p>{error}</p>;
    }

    return (
        <div className="applications">
            
            <h1>Applications</h1>
            {applications.length > 0 ? (
                <ul>
                    {applications.map((application) => (
                        <li key={application.recruitment.id}>
                            <h3>{application.person.name} {application.person.surname}</h3>
                            {/* <p>id recruitment: {application.recruitment.id} </p> */}
                            {/* <p>id branch: {application.branch.id} </p> */}
                            <p>Email: {application.person.contactEmail}</p>
                            <p>Phone: {application.person.contactPhoneNum}</p>
                            <p>Message: {application.recruitment.personMessage || "No message provided"}</p>
                            <p>Offer: {application.offer.name}</p>
                            <p>Branch: {application.branch.name}</p>
                            <button
                                onClick={() => handleResponse(application.recruitment.id, true)}
                            >
                                Accept
                            </button>
                            <button
                                onClick={() => handleResponse(application.recruitment.id, false)}
                            >
                                Reject
                            </button>
                        </li>
                    ))}
                </ul>
            ) : (
                <p>No applications found.</p>
            )}
        </div>
    );
};

export default CompanyRecruitmentPage;
