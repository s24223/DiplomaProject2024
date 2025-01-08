import React from "react";

const UrlDetailsView = ({ urlItem }) => (
    <div>
        <p><strong>Name:</strong> {urlItem.name || "No Name"}</p>
        <p><strong>Path:</strong> {urlItem.path || "No Path"}</p>
        <p><strong>Description:</strong> {urlItem.description || "No Description"}</p>
        <p><strong>Type:</strong> {urlItem.type?.name || "Unknown"}</p>
        <p><strong>Created At:</strong> {new Date(urlItem.created).toLocaleString()}</p>
    </div>
);

export default UrlDetailsView;
