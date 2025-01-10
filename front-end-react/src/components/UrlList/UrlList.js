import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import CreateUrl from "../UrlCreate/CreateUrl";
import { fetchUrls } from "../../services/URLService/UrlService";

const UrlList = () => {
    const [urlList, setUrlList] = useState([]);
    const [error, setError] = useState(null);
    const [showAddForm, setShowAddForm] = useState(false);

    // Funkcja do pobierania listy URL-i
    const loadUrlList = async () => {
        try {
            const urls = await fetchUrls();
            setUrlList(urls);
        } catch (err) {
            console.error("Error fetching URLs:", err);
            setError("Failed to load URLs. Please try again.");
        }
    };

    // Pobierz listę URL-i podczas inicjalizacji komponentu
    useEffect(() => {
        loadUrlList();
    }, []);

    if (error) {
        return <p>{error}</p>;
    }

    return (
        <div className="bordered">
            <h2>URL List</h2>
            {/* Przycisk do dodania nowego URL-a */}
            <button onClick={() => setShowAddForm(true)}>Add URL</button>
            {showAddForm && (
                <CreateUrl onClose={() => setShowAddForm(false)} refreshUrls={loadUrlList} />
            )}

            {/* Lista URL-i */}
            {urlList.length > 0 ? (
                <ul>
                    {urlList.map((urlItem) => (
                        <li key={urlItem.userId} className="url">
                            <Link 
                                to={{
                                    pathname: `/url/${urlItem.userId}`,
                                }}
                                state={{ urlItem }}
                                style={{ textDecoration: "none", color: "white", marginBottom: "10px" }}
                            >
                                {urlItem.name || "Unnamed URL"}
                            </Link>
                        </li>
                    ))}
                </ul>
            ) : (
                <p>No URLs available.</p>
            )}
        </div>
    );
};

export default UrlList;
