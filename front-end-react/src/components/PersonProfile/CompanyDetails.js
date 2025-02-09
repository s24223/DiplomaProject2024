import React from "react";
import BranchList from "../Branch/ProfileBranchList";
import UrlList from "../URL/UrlList";


const CompanyDetails = ({ company, branchCount, activeOffersCount }) => {
    const handleAddBranch = () => {
        window.location.href = "/createBranch";
    };

    return (
        <div className="profile-details">
            <h2>Company</h2>
            <div className="bordered">
                <p><strong>Name:</strong> {company.name}</p>
                <p><strong>Email:</strong> {company.contactEmail}</p>
                <p><strong>Description:</strong> {company.description}</p>
                <p><strong>Branches:</strong> {branchCount}</p>
                <p><strong>Active Offers:</strong> {activeOffersCount}</p>
            </div>
            <UrlList />
            {branchCount > 0 && <BranchList />}
            &nbsp;
            <button onClick={handleAddBranch}>Add Branch</button>
            &nbsp;
            <button onClick={() => (window.location.href = "/companyRecruitment")}>
                Manage Applications
            </button>
        </div>
    );
};

export default CompanyDetails;
