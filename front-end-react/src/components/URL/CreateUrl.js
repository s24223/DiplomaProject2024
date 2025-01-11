import React, { useState, useEffect } from "react";
import CreateUrlForm from "../Forms/UrlCreateForm";
import { fetchUrlTypes, addUrl } from "../../services/URLService/UrlService";

const CreateUrl = ({ onClose, refreshUrls }) => {
    const [urlTypes, setUrlTypes] = useState([]);

    useEffect(() => {
        const loadUrlTypes = async () => {
            try {
                const types = await fetchUrlTypes();
                setUrlTypes(types);
            } catch (err) {
                console.error("Error fetching URL types:", err);
                alert("Failed to load URL types.");
            }
        };

        loadUrlTypes();
    }, []);

    const handleSubmit = async (urlData) => {
        try {
            await addUrl(urlData);
            alert("URL added successfully.");
            refreshUrls(); // Odśwież listę URL-i
            onClose(); // Zamknij formularz
        } catch (error) {
            console.error("Error adding URL:", error);
            alert("Failed to add URL.");
        }
    };

    return (
        <div>
            <h3>Add URL</h3>
            <CreateUrlForm onSubmit={handleSubmit} urlTypes={urlTypes} onClose={onClose} />
        </div>
    );
};

export default CreateUrl;