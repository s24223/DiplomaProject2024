import React, { useState } from "react";
import { updateUrl } from "../../services/URLService/UrlService";

const UrlForm = ({ urlItem, onSaveSuccess, onCancelEdit }) => {
    const [name, setName] = useState(urlItem.name || "");
    const [description, setDescription] = useState(urlItem.description || "");
    const [path, setPath] = useState(urlItem.path || "");

    const handleSave = async () => {
        const updatedUrl = {
            urlTypeId: urlItem.urlTypeId,
            created: urlItem.created,
            data: { path, name, description },
        };

        try {
            let res = await updateUrl(updatedUrl);
            if(res.error){
                throw new Error(res.error)
            }
            onSaveSuccess();
        } catch (err) {
            console.error("Error updating URL:", err);
            alert("Failed to update URL.");
        }
    };

    return (
        <div>
            <p>
                <strong>Name:</strong>
                <input
                    type="text"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                    placeholder="Enter name"
                />
            </p>
            <p>
                <strong>Path:</strong>
                <input
                    type="text"
                    value={path}
                    onChange={(e) => setPath(e.target.value)}
                    placeholder="Enter URL path"
                />
            </p>
            <p>
                <strong>Description:</strong>
                <textarea
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                    placeholder="Enter description"
                />
            </p>
            <button onClick={handleSave} style={{ marginRight: "10px" }}>
                Save Changes
            </button>
            <button onClick={onCancelEdit} style={{ backgroundColor: "#ccc" }}>
                Cancel Edit
            </button>
        </div>
    );
};

export default UrlForm;
