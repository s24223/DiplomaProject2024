import React, { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import axios from "axios";
import MainPageButton from "../../components/Buttons/MainPageButton/MainPageButton";
import ReturnButton from "../../components/Buttons/CancelButton/ReturnButton";
import LoginButton from "../../components/Buttons/LoginButton/LoginButton";

const EditUrlPage = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const url = location.state.urlItem;

    const [path, setPath] = useState(url.path);
    const [name, setName] = useState(url.name);
    const [description, setDescription] = useState(url.description);

    const handleSave = async () => {
        try {
            await axios.put(
                "https://localhost:7166/api/User//urls",
                [
                    {
                        urlTypeId: url.urlTypeId,
                        created: url.created,
                        data: {
                            path: path,
                            name: name,
                            description: description,
                        },
                    },
                ],
                {
                    headers: {
                        Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
                    },
                }
            );
            alert("URL updated successfully.");
            navigate(`/url/${url.userId}`, { state: { urlItem: { ...url, path, name, description } } });
        } catch (err) {
            console.error("Error updating URL:", err);
            alert("Failed to update URL.");
        }
    };

    const handleDelete = async () => {
        const confirmDelete = window.confirm(`Are you sure you want to delete "${name || path}"?`);
        if (!confirmDelete) return;

        try {
            await axios.delete(
                "https://localhost:7166/api/User/urls/urls",
                {
                    headers: {
                        Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
                    },
                    data: [{ urlTypeId: url.urlTypeId, created: url.created }],
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
        
        navigate(`/url/${url.userId}`, { state: { urlItem: url } });
    };

    return (
        <div>
            <MainPageButton /> <ReturnButton/>
            <LoginButton />
            <h1>Edit URL</h1>
            <form>
                <label>Path:</label>
                <input
                    type="text"
                    value={path}
                    onChange={(e) => setPath(e.target.value)}
                    style={{ display: "block", marginBottom: "10px" }}
                />
                <label>Name:</label>
                <input
                    type="text"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                    style={{ display: "block", marginBottom: "10px" }}
                />
                <label>Description:</label>
                <textarea
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                    style={{ display: "block", marginBottom: "10px" }}
                ></textarea>
            </form>
            <button onClick={handleSave} style={{ marginRight: "10px" }}>
                Save Changes
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

export default EditUrlPage;
