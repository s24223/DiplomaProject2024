import React, {useState} from "react";
import { useLocation, useNavigate } from "react-router-dom";
import axios from "axios";
import MainPageButton from "../../components/MainPageButton/MainPageButton";
import LoginButton from "../../components/LoginButton/LoginButton";
import ReturnButton from "../../components/CancelButton/ReturnButton";

const UrlDetailsPage = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const { urlItem } = location.state;

    const [editMode, setEditMode] = useState(false);
    const [name, setName] = useState(urlItem.name || "");
    const [description, setDescription] = useState(urlItem.description || "");
    const [path, setPath] = useState(urlItem.path || "");
    const [messageStatus, setMessageStatus] = useState("");


    if (!urlItem) {
        return <p>No URL details available.</p>;
    }

    const handleSave = async () => {
        const updatedUrl = {
            urlTypeId: urlItem.urlTypeId,
            created: urlItem.created,
            data: {
                path,
                name,
                description,
            },
        };

        try {
            const response = await axios.put(
                "https://localhost:7166/api/User/urls",
                [updatedUrl],
                {
                    headers: {
                        Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
                    },
                }
            );

            if (response.status === 200) {
                setMessageStatus("Updated successfully.");
                setTimeout(() => setMessageStatus(""), 2000);
                setEditMode(false);
            }
        } catch (err) {
            console.error("Error updating URL:", err);
            alert("Failed to update URL.");
        }
    };


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



    return (
        <div>
            <MainPageButton /> <ReturnButton/>
            <LoginButton />
            <h1>URL Details</h1>
            {/* <p><strong>ID:</strong> {urlItem.urlTypeId}</p> */}
            <button onClick={() => setEditMode(!editMode)}>Edit Mode</button>
            {!editMode && (
                <button onClick={handleDelete} style={{ color: "red", marginLeft: "10px" }}>
                    Delete URL
                </button>
            )}
            <br />
            <p>
                Name:{" "}
                {editMode ? (
                    <input
                        type="text"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        placeholder="Enter name"
                    />
                ) : (
                    urlItem.name || "No Name"
                )}
            </p>
            <p>
                Path:{" "}
                {editMode ? (
                    <input
                        type="text"
                        value={path}
                        onChange={(e) => setPath(e.target.value)}
                        placeholder="Enter URL path"
                    />
                ) : (
                    urlItem.path || "No Path"
                )}
            </p>
            <p>
                Description:{" "}
                {editMode ? (
                    <textarea
                        value={description}
                        onChange={(e) => setDescription(e.target.value)}
                        placeholder="Enter description"
                    />
                ) : (
                    urlItem.description || "No Description"
                )}
            </p>
            <p>Type: {urlItem.type?.name}</p>
            <p>Created At: {new Date(urlItem.created).toLocaleString()}</p>
            <label style={{ color: "green" }}>{messageStatus}</label>
            <br />
            {editMode && (
                <button onClick={handleSave} style={{ marginRight: "10px" }}>
                    Save Changes
                </button>
            )}
            {editMode && (
                <button onClick={() => setEditMode(false)} style={{ backgroundColor: "#ccc" }}>
                    Cancel Edit
                </button>
            )}
            
        </div>
    );
};

export default UrlDetailsPage;
