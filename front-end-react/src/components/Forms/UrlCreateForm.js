import React, { useState } from "react";

const CreateUrlForm = ({ onSubmit, urlTypes, onClose }) => {
    const [path, setPath] = useState("");
    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [urlTypeId, setUrlTypeId] = useState(urlTypes[0]?.id || ""); // Domyślny typ URL-a

    const handleSubmit = (e) => {
        e.preventDefault();

        const urlPattern = /^(https?:\/\/).+/i;
        if (!urlPattern.test(path)) {
            alert("URL must start with http:// or https://");
            return;
        }

        onSubmit({ urlTypeId, path, name, description });
    };

    return (
        <form onSubmit={handleSubmit}>
            <label>Name:</label>
            <input
                type="text"
                value={name}
                onChange={(e) => setName(e.target.value)}
                placeholder="Name"
                required
            />
            <br />
            <label>Path:</label>
            <input
                type="text"
                value={path}
                onChange={(e) => setPath(e.target.value)}
                placeholder="https://example.com"
                required
            />
            <br />
            <label>Type:</label>
            <select
                value={urlTypeId}
                onChange={(e) => setUrlTypeId(Number(e.target.value))}
                required
            >
                {urlTypes.map((type) => (
                    <option key={type.id} value={type.id}>
                        {type.name}
                    </option>
                ))}
            </select>
            <br />
            <label>Description:</label><br/>
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
    );
};

export default CreateUrlForm;
