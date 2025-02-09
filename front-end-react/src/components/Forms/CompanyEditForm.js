import React from "react";

const CompanyEditForm = ({ companyData, onInputChange, onSubmit }) => (
    <form onSubmit={onSubmit}>
        <label>Name:</label><br/>
        <input
            type="text"
            name="name"
            value={companyData.name}
            onChange={onInputChange}
            required
        /><br/>
        <label>Email:</label><br/>
        <input
            type="email"
            name="contactEmail"
            value={companyData.contactEmail}
            onChange={onInputChange}
            required
        /><br/>
        <label>Description:</label><br/>
        <textarea
            name="description"
            value={companyData.description}
            onChange={onInputChange}
        ></textarea><br/>
        <label>URL Segment (optional):</label><br/>
        <input
            type="text"
            name="urlSegment"
            value={companyData.urlSegment || ""}
            onChange={onInputChange}
        /><br/>
        <button id="edit-company-page-button" type="submit">Save Changes</button>
    </form>
);

export default CompanyEditForm;
