import React, { useState } from "react";
import axios from "axios";

const EditUrlButton = ({ url, onEditSuccess }) => {
    const [isEditing, setIsEditing] = useState(false);
    const [name, setName] = useState(url.name || "");
    const [description, setDescription] = useState(url.description || "");
    const [path, setPath] = useState(url.path || "");

    const handleSave = async () => {
        const updatedUrl = {
            urlTypeId: url.urlTypeId,
            created: url.created,
            data: {
                path: path,
                name: name,
                description: description,
            },
        };

        try {
            await axios.put(
                "https://localhost:7166/api/User/urls",
                [updatedUrl],
                {
                    headers: {
                        Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
                    },
                }
            );
            onEditSuccess(updatedUrl); // Callback to refresh or update parent state
            alert("URL updated successfully.");
            setIsEditing(false);
        } catch (err) {
            console.error("Error updating URL:", err);
            alert("Failed to update URL.");
        }
    };

    if (isEditing) {
        return (
            <div style={{ marginBottom: "10px" }}>
                <label>Name: </label>
                <input
                    type="text"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                    style={{ marginRight: "5px" }}
                />
                <label>Description: </label>
                <input
                    type="text"
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                    style={{ marginRight: "5px" }}
                />
                <label>Path: </label>
                <input
                    type="text"
                    value={path}
                    onChange={(e) => setPath(e.target.value)}
                    style={{ marginRight: "5px" }}
                />
                <button onClick={handleSave} style={{ marginRight: "10px" }}>
                    Save
                </button>
                {/* <br/><button onClick={() => setIsEditing(false)}>Cancel</button><br/> */}
            </div>
        );
    }

    return (
        <button onClick={() => setIsEditing(true)} style={{ marginLeft: "10px" }}>
            Edit
        </button>
    );
};

export default EditUrlButton;
