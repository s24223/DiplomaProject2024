// import React, { useEffect } from "react";
// import { fetchProfileGet } from "../../services/ProfileService/ProfilceService";
// import Profile from "../../components/PersonProfile/Profile";

// const ProfilePage = () => {

//     return(
//         <div>
//             <Profile />
//         </div>
//     )
// }

// export default ProfilePage;
import React, { useState, useEffect } from "react";
import axios from "axios";
import LoginButton from "../../components/LoginButton/LoginButton";
import MainPageButton from "../../components/MainPageButton/MainPageButton";


const ProfilePage = () => {
    const [userData, setUserData] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchUserData = async () => {
            try {
                const response = await axios.get("https://localhost:7166/api/User", {
                    headers: {
                        Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
                    },
                });
                setUserData(response.data.item);
                setLoading(false);
            } catch (err) {
                setError("Failed to load user data. Please try again.");
                setLoading(false);
            }
        };

        fetchUserData();
    }, []);

    const handleProfileEdit = () => {
        window.location.href = '/userEditProfile';
    };
    const handleProfileCreate = () => {
        window.location.href = '/userCreateProfile';
    };

    if (loading) {
        return <p>Loading...</p>;
    }

    if (error) {
        return <p>{error}</p>;
    }

    if (!userData) {
        return <p>No user data available.</p>;
    }

    const {
        person,
        company,
        branchCount,
        activeOffersCount,
        companyCharacteristics,
    } = userData;

    const address = person?.address;

    return (
        <div className="user-profile">
            <MainPageButton/>
            <LoginButton />
            <button onClick={handleProfileEdit} style={{ position: 'absolute', top: 40, right: 10 }}>
                    Edit Profile
            </button>
            <button onClick={handleProfileCreate} style={{ position: 'absolute', top: 10, right: 74 }}>
                    Create Profile
            </button>
            <h1>User Profile</h1>
            <h2>Personal Information</h2>
            <p><strong>Name:</strong> {person?.name}</p>
            <p><strong>Surname:</strong> {person?.surname}</p>
            <p><strong>Email:</strong> {person?.contactEmail}</p>
            <p><strong>Phone:</strong> {person?.contactPhoneNum}</p>
            <p><strong>Birth Date:</strong> {new Date(person?.birthDate).toLocaleDateString()}</p>
            <p><strong>Is Student:</strong> {person?.isStudent ? "Yes" : "No"}</p>
            <p><strong>Public Profile:</strong> {person?.isPublicProfile ? "Yes" : "No"}</p>
            <p><strong>Description:</strong> {person?.description}</p>

            <h2>Address</h2>
            {address ? (
                <>
                    <p><strong>Street:</strong> {address.street?.name} {address.buildingNumber}/{address.apartmentNumber}</p>
                    <p><strong>ZIP Code:</strong> {address.zipCode}</p>
                    <p><strong>City:</strong> {address.hierarchy?.find(item => item.administrativeType.name === "miasto")?.name}</p>
                    <p><strong>Voivodeship:</strong> {address.hierarchy?.find(item => item.administrativeType.name === "województwo")?.name}</p>
                </>
            ) : (
                <p>No address provided.</p>
            )}

            {company && (
                <>
                    <h2>Company</h2>
                    <p><strong>Name:</strong> {company?.name}</p>
                    <p><strong>Email:</strong> {company?.contactEmail}</p>
                    <p><strong>Description:</strong> {company?.description}</p>
                    <p><strong>Branches:</strong> {branchCount}</p>
                    <p><strong>Active Offers:</strong> {activeOffersCount}</p>
                </>
            )}

            <h2>Characteristics</h2>
            {companyCharacteristics?.length > 0 ? (
                <ul>
                    {companyCharacteristics.map((char) => (
                        <li key={char.characteristic.id}>
                            <strong>{char.characteristic.name}</strong> ({char.type.name})
                        </li>
                    ))}
                </ul>
            ) : (
                <p>No characteristics available.</p>
            )}
        </div>
    );
};

export default ProfilePage;
