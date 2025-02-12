import React, { useState, useEffect } from "react";
import axios from "axios";
import { useParams } from "react-router-dom";
import { jwtRefresh } from "../../services/JwtRefreshService/JwtRefreshService";

const OfferApplicationsPage = () => {
  jwtRefresh();

  const { offerId } = useParams();
  const [applications, setApplications] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [filterStatus, setFilterStatus] = useState(""); // Filtr statusu aplikacji

  useEffect(() => {
    const fetchApplications = async () => {
      try {
        const response = await axios.get("https://localhost:7166/api/User/company/recruitment", {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("jwt")}`,
          },
          params: {
            orderBy: "created",
            ascending: true,
            maxItems: 100,
            page: 1,
            filterOfferId: offerId, // Filtrowanie po konkretnej ofercie
            filterStatus: filterStatus || null,
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
  }, [offerId, filterStatus]);

  const handleResponse = async (recruitmentId, isAccepted) => {
    const companyResponse = isAccepted ? "Your application has been accepted." : "Your application has been rejected.";
    try {
      await axios.put(
        `https://localhost:7166/api/User/recruitment/${recruitmentId}/answer`,
        { companyResponse, isAccepted },
        {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("jwt")}`,
          },
        }
      );
      alert(isAccepted ? "Application accepted!" : "Application rejected!");
      setApplications((prev) => prev.filter((app) => app.recruitment.id !== recruitmentId));
    } catch (error) {
      console.error("Error updating application status:", error);
      alert("Failed to update application status.");
    }
  };

  if (loading) return <p>Loading applications...</p>;
  if (error) return <p>{error}</p>;

  return (
    <div className="applications">
      <h1>Applications for Offer</h1>
      <div>
        <label>Filter by status: </label>
        <select value={filterStatus} onChange={(e) => setFilterStatus(e.target.value)}>
          <option value="">All</option>
          <option value="true">Accepted</option>
          <option value="false">Rejected</option>
        </select>
      </div>
      {applications.length > 0 ? (
        <ul>
          {applications.map((app) => (
            <li key={app.recruitment.id}>
              <h3>{app.person.name} {app.person.surname}</h3>
              <p>Email: {app.person.contactEmail}</p>
              <p>Phone: {app.person.contactPhoneNum}</p>
              <p>Message: {app.recruitment.personMessage || "No message provided"}</p>
              {app.recruitment.url && (
                <p>
                  <a
                    href={`https://localhost:7166/api/User/cv/${app.recruitment.url}`}
                    target="_blank"
                    rel="noopener noreferrer"
                    style={{ textDecoration: "underline", color: "blue" }}
                  >
                    Download CV
                  </a>
                </p>
              )}
              <button onClick={() => handleResponse(app.recruitment.id, true)}>Accept</button>
              <button onClick={() => handleResponse(app.recruitment.id, false)}>Reject</button>
            </li>
          ))}
        </ul>
      ) : (
        <p>No applications found.</p>
      )}
    </div>
  );
};

export default OfferApplicationsPage;
