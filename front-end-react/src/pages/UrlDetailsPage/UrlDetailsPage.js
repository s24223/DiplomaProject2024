import React from "react";
import { useLocation, useNavigate } from "react-router-dom";
import axios from "axios";
import MainPageButton from "../../components/MainPageButton/MainPageButton";
import LoginButton from "../../components/LoginButton/LoginButton";
import ReturnButton from "../../components/CancelButton/ReturnButton";
import CancelButton from "../../components/CancelButton/CancelButton";

const UrlDetailsPage = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const { urlItem } = location.state;

    if (!urlItem) {
        return <p>No URL details available.</p>;
    }
    const handleDelete = async () => {
        const confirmDelete = window.confirm(`Are you sure you want to delete "${urlItem.type.urlTypeId}"?`);
        if (!confirmDelete) return;

        try {
            await axios.delete(
                "https://localhost:7166/api/User/urls",
                {
                    headers: {
                        Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
                    },
                    data: [{ urlTypeId: urlItem.urlTypeId, created: urlItem.created }],
                }
            );
            alert("URL deleted successfully.");
            navigate("/userProfile");
        } catch (err) {
            console.error("Error deleting URL:", err);
            alert("Failed to delete URL.");
        }
    };

    const handleCancel = () => {
        
        navigate(`/userProfile`);
    };


    return (
        <div>
            <MainPageButton /> <ReturnButton/>
            <LoginButton />
            <h1>URL Details</h1>
            {/* <p><strong>ID:</strong> {urlItem.urlTypeId}</p> */}
            <p><strong>Name:</strong> {urlItem.name}</p>
            <p><strong>Path:</strong> {urlItem.path}</p>
            <p><strong>Type:</strong> {urlItem.type?.name}</p>
            <p><strong>Description:</strong> {urlItem.description || "No description provided."}</p>
            <p><strong>Created At:</strong> {new Date(urlItem.created).toLocaleString()}</p>
            <button onClick={() => navigate(`/url/edit/${urlItem.userId}`, { state: { urlItem } })}>
                Edit URL
            </button>
            <button onClick={handleDelete} style={{ color: "red", marginRight: "10px" }}>
                Delete URL
            </button><br/>
            <button onClick={handleCancel} style={{ backgroundColor: "#ccc" }}>
                Cancel
            </button>
        </div>
    );
};

export default UrlDetailsPage;
