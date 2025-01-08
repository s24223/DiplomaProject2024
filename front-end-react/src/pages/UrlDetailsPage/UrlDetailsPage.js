import React, { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import UrlDetailsView from "../../components/URL/UrlDetailsView";
import UrlForm from "../../components/Forms/UrlDetailsForm";
import { deleteUrl } from "../../services/URLService/UrlService";

const UrlDetailsPage = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const { urlItem } = location.state;

    const [editMode, setEditMode] = useState(false);
    const [messageStatus, setMessageStatus] = useState("");

    if (!urlItem) {
        return <p>No URL details available.</p>;
    }

    const handleDelete = async () => {
        const confirmDelete = window.confirm(`Are you sure you want to delete "${urlItem.name}"?`);
        if (!confirmDelete) return;

        try {
            await deleteUrl(urlItem);
            alert("URL deleted successfully.");
            navigate("/userProfile");
        } catch (err) {
            console.error("Error deleting URL:", err);
            alert("Failed to delete URL.");
        }
    };

    const handleSaveSuccess = () => {
        setMessageStatus("Updated successfully.");
        setTimeout(() => setMessageStatus(""), 2000);
        setEditMode(false);
    };

    return (
        <div>
            <h1>URL Details</h1>
            <button onClick={() => setEditMode(!editMode)}>
                {editMode ? "Cancel Edit" : "Edit Mode"}
            </button>
            {!editMode && (
                <button onClick={handleDelete} style={{ color: "red", marginLeft: "10px" }}>
                    Delete URL
                </button>
            )}
            <br />
            {editMode ? (
                <UrlForm
                    urlItem={urlItem}
                    onSaveSuccess={handleSaveSuccess}
                    onCancelEdit={() => setEditMode(false)}
                />
            ) : (
                <UrlDetailsView urlItem={urlItem} />
            )}
            <label style={{ color: "green" }}>{messageStatus}</label>
        </div>
    );
};

export default UrlDetailsPage;
