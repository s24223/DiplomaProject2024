import React from "react";
import { useLocation, useNavigate } from "react-router-dom";
import MainPageButton from "../../components/MainPageButton/MainPageButton";
import LoginButton from "../../components/LoginButton/LoginButton";
import ProfileCancelButton from "../../components/CancelButton/ReturnButton";

const UrlDetailsPage = () => {
    const location = useLocation();
    const navigate = useNavigate();
    const { urlItem } = location.state;

    if (!urlItem) {
        return <p>No URL details available.</p>;
    }

    return (
        <div>
            <MainPageButton />
            <LoginButton />
            <h1>URL Details</h1>
            {/* <p><strong>ID:</strong> {urlItem.urlTypeId}</p> */}
            <p><strong>Path:</strong> {urlItem.path}</p>
            <p><strong>Type:</strong> {urlItem.type?.name}</p>
            <p><strong>Description:</strong> {urlItem.description || "No description provided."}</p>
            <p><strong>Created At:</strong> {new Date(urlItem.created).toLocaleString()}</p>
            <button onClick={() => navigate(`/url/edit/${urlItem.userId}`, { state: { urlItem } })}>
                Edit URL
            </button>
            <ProfileCancelButton/>
        </div>
    );
};

export default UrlDetailsPage;
