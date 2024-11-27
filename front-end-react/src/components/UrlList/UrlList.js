import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import axios from "axios";
import CreateUrl from "../UrlCreate/CreateUrl";

const UrlList = () => {
    const [urlList, setUrlList] = useState([]);
    const [error, setError] = useState(null);
    const [showAddForm, setShowAddForm] = useState(false);

    // Funkcja do pobierania listy URL-i
    const fetchUrlList = async () => {
        try {
            const response = await axios.get(
                "https://localhost:7166/api/User/urls?orderBy=created&ascending=true&itemsCount=100&page=1",
                {
                    headers: {
                        Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
                    },
                }
            );
            setUrlList(response.data.item.urls);
        } catch (err) {
            console.error("Error fetching URLs:", err);
            setError("Failed to load URLs. Please try again.");
        }
    };

    // Pobierz listę URL-i podczas inicjalizacji komponentu
    useEffect(() => {
        fetchUrlList();
    }, []);

    if (error) {
        return <p>{error}</p>;
    }

    return (
        <div>
            <h2>URL List</h2>
            {/* Przycisk do dodania nowego URL-a */}
            <button onClick={() => setShowAddForm(true)}>Add URL</button>
            {showAddForm && (
                <CreateUrl onClose={() => setShowAddForm(false)} refreshUrls={fetchUrlList} />
            )}

            {/* Lista URL-i */}
            {urlList.length > 0 ? (
                <ul>
                    {urlList.map((urlItem) => (
                        <li key={urlItem.userId}>
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
