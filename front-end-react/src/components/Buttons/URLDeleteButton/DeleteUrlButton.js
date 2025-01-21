import React from "react";
import axios from "axios";

const DeleteUrlButton = ({ url, onDeleteSuccess }) => {
    const handleDelete = async () => {
        const confirmDelete = window.confirm("Are you sure you want to delete this URL?");
        if (!confirmDelete) return;

        try {
            await axios.delete(
                "https://localhost:7166/api/User/urls",
                {
                    headers: {
                        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
                    },
                    data: [{ urlTypeId: url.urlTypeId, created: url.created }],
                }
            );
            onDeleteSuccess(url); // Callback to refresh the list or update the parent state
            alert("URL deleted successfully.");
        } catch (err) {
            console.error("Error deleting URL:", err);
            alert("Failed to delete URL.");
        }
    };

    return (
        <button onClick={handleDelete} style={{ marginLeft: "10px", color: "red" }}>
            Delete
        </button>
    );
};

export default DeleteUrlButton;
