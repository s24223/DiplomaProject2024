import React, { useState, useEffect } from "react";
import { fetchUserProfile } from "../../services/UserService/UserService";
import ProfileDetails from "./ProfileDetails";
import CompanyDetails from "./CompanyDetails";
import AddressDetails from "./AddressDetails";

const Profile = () => {
    const [userData, setUserData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const loadUserData = async () => {
            try {
                const data = await fetchUserProfile();
                setUserData(data);
                setLoading(false);
            } catch (err) {
                setError("Failed to load user data. Please try again.");
                setLoading(false);
            }
        };

        loadUserData();
    }, []);

    if (loading) return <p>Loading...</p>;
    if (error) return <p>{error}</p>;
    if (!userData) return <p>No user data available.</p>;

    const { person, company, branchCount, activeOffersCount } = userData;

    return (
        <div className="user-profile">
            <h1>User Profile</h1>
            <div className="profile-actions">
                {! company && (
                <button onClick={() => (window.location.href = "/personRecruitment")}>
                    View Applications
                </button> )}
                {person ? (
                    <button onClick={() => (window.location.href = "/userEditProfile")}>
                        Edit Profile
                    </button>
                ) : (
                    <button onClick={() => (window.location.href = "/userCreateProfile")}>
                        Create Profile
                    </button>
                )}
                {company ? (
                    <button onClick={() => (window.location.href = "/userEditCompany")}>
                        Edit Company
                    </button>
                ) : (
                    <button onClick={() => (window.location.href = "/userCreateCompany")}>
                        Create Company
                    </button>
                )}
                <button onClick={() => (window.location.href='/changePassword')}>
                    Change password
                </button>
            </div>

            {person && <ProfileDetails person={person} />}
            {person?.address && <AddressDetails address={person.address} />}
            
            {company && (
                <CompanyDetails
                    company={company}
                    branchCount={branchCount}
                    activeOffersCount={activeOffersCount}
                />
            )}
        </div>
    );
};

export default Profile;