import React, { useState } from "react";
import axios from "axios";

const CreateUrl = ({ onClose, refreshUrls }) => {
    const [path, setPath] = useState("");
    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [urlTypeId, setUrlTypeId] = useState(7); // Domyślny typ URL-a

    const handleSubmit = async (e) => {
        e.preventDefault();

        // Walidacja path
        const urlPattern = /^(https?:\/\/).+/i;
        if (!urlPattern.test(path)) {
            alert("URL must start with http:// or https://");
            return;
        }

        try {
            const response = await axios.post(
                "https://localhost:7166/api/User/urls",
                [
                    {
                        urlTypeId,
                        path,
                        name,
                        description,
                    },
                ],
                {
                    headers: {
                        Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
                    },
                }
            );

            if (response.status === 201) {
                alert("URL added successfully.");
                refreshUrls(); // Odśwież listę URL-i
                onClose(); // Zamknij formularz
            }
        } catch (error) {
            console.error("Error adding URL:", error);
            alert("Failed to add URL.");
        }
    };

    return (
        <div>
            <h3>Add URL</h3>
            <form onSubmit={handleSubmit}>
                <label>Path:</label>
                <input
                    type="text"
                    value={path}
                    onChange={(e) => setPath(e.target.value)}
                    placeholder="https://example.com"
                    required
                />
                <br />
                <label>Name:</label>
                <input
                    type="text"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                    placeholder="Name"
                    required
                />
                <br />
                <label>Description:</label>
                <textarea
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                    placeholder="Description"
                ></textarea>
                <br />
                <button type="submit">Add URL</button>
                <button type="button" onClick={onClose}>
                    Cancel
                </button>
            </form>
        </div>
    );
};

export default CreateUrl;
