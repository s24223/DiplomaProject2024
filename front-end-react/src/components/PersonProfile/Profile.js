import React, { useState, useEffect } from "react";
import axios from "axios";
import LoginButton from "../../components/LoginButton/LoginButton";
import MainPageButton from "../../components/MainPageButton/MainPageButton";
import BranchList from "../BranchList/BranchList";
import UrlList from "../UrlList/UrlList";
import ReturnButton from "../CancelButton/ReturnButton";

const Profile = () => {
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
    const handleCompanyCreate = () => {
        window.location.href = '/userCreateCompany';
    };
    const handleCompanyEdit = () => {
        window.location.href = '/userEditCompany';
    };

    const handleAddBranch = () => {
        window.location.href = "/createBranch"
    }

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
        //companyCharacteristics,
    } = userData;

    const address = person?.address;

    return (
        <div className="user-profile">
            <MainPageButton/>
            <LoginButton />
            <ReturnButton/>
            
         
            
            
            <h1>User Profile</h1>
            <button className="button-one" onClick={handleProfileCreate} >
                    Create Profile
            </button>
            <button className="button-two" onClick={handleProfileEdit}>
                    Edit Profile
            </button>
            
            <h2>Personal Information</h2>
            <div className="bordered">
                <p><strong>Name:</strong> {person?.name}</p>
                <p><strong>Surname:</strong> {person?.surname}</p>
                <p><strong>Email:</strong> {person?.contactEmail}</p>
                <p><strong>Phone:</strong> {person?.contactPhoneNum}</p>
                <p><strong>Birth Date:</strong> {new Date(person?.birthDate).toLocaleDateString()}</p>
                <p><strong>Is Student:</strong> {person?.isStudent ? "Yes" : "No"}</p>
                <p><strong>Public Profile:</strong> {person?.isPublicProfile ? "Yes" : "No"}</p>
                <p><strong>Description:</strong> {person?.description}</p>
            </div>
            <h2>User Characteristics</h2>
            {person?.characteristics?.length > 0 ? (
                <div className="bordered">
                    <ul>
                        {person.characteristics.map((char) => (
                            <li key={char.characteristic.id}>
                              
                                <strong>{char.characteristic.name}</strong> {char?.quality?.name}
                            </li>
                        ))}
                    </ul>
                </div>
            ) : (
                <p>No characteristics available.</p>
            )}
           

            <h2>Address</h2>
            {address ? (
                <div className="bordered">
                    <p><strong>Street:</strong> {address.street?.name} {address.buildingNumber}/{address.apartmentNumber}</p>
                    <p><strong>ZIP Code:</strong> {address.zipCode}</p>
                    <p><strong>City:</strong> {address.hierarchy?.find(item => item.administrativeType.name === "miasto")?.name}</p>
                    <p><strong>Voivodeship:</strong> {address.hierarchy?.find(item => item.administrativeType.name === "województwo")?.name}</p>
                </div>
            ) : (
                <p>No address provided.</p>
            )}
            
            <UrlList/>
            <br/>
            <br/>
            <br/>
            <button onClick={handleCompanyCreate} >
                    Create Company
            </button>
            <button onClick={handleCompanyEdit} >
                    Edit Company
            </button>


            {company && (
                <div className="bordered">
                    <h2>Company</h2>
                    {/* <p><strong>Id:</strong> {company?.companyId}</p><br/> */}

                    <p><strong>Name:</strong> {company?.name}</p>
                    <p><strong>Email:</strong> {company?.contactEmail}</p>
                    <p><strong>Description:</strong> {company?.description}</p>
                    <p><strong>Branches:</strong> {branchCount}</p>
                    <p><strong>Active Offers:</strong> {activeOffersCount}</p>
                    <div className="bordered" />
                    
                    {branchCount > 0 && <BranchList />}
                    <button onClick={handleAddBranch} >
                    Add Branch
                    </button>
                    <br/>
                </div>
            )}


            {/* <h2>haracteristics</h2>
            {companyCharacteristics?.length > 0 ? (
                <div className="bordered">
                <ul>
                    {companyCharacteristics.map((char) => (
                        <li key={char.characteristic.id}>
                            <strong>{char.characteristic.name}</strong> ({char.quality.name})
                        </li>
                    ))}
                </ul>
                </div>
            ) : (
                <p>No characteristics available.</p>
            )} */}
            <ReturnButton/>
        </div>
    );
};

export default Profile;
