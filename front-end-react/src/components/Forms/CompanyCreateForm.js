import React from "react";

const CompanyCreateForm = ({ companyData, onInputChange, onSubmit }) => (
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
        <label>REGON:</label><br/>
        <input
            type="text"
            name="regon"
            value={companyData.regon}
            onChange={onInputChange}
            pattern="[0-9]{9}|[0-9]{14}"
            title="REGON must have 9 or 14 digits"
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
        <button id="create-company-page-button" type="submit">Create Company</button>
    </form>
);

export default CompanyCreateForm;
