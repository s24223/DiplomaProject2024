import React, { useState, useEffect } from "react";
import axios from "axios";

const EditProfile = () => {
    const [profileData, setProfileData] = useState({
        urlSegment: "",
        contactEmail: "",
        name: "",
        surname: "",
        birthDate: { year: 0, month: 0, day: 0 },
        contactPhoneNum: "",
        description: "",
        isStudent: false,
        isPublicProfile: false,
        addressId: "",
        characteristics: [],
    });

    const [message, setMessage] = useState("");

    useEffect(() => {
        const fetchProfileData = async () => {
            try {
                const response = await axios.get("https://localhost:7166/api/User", {
                    headers: {
                        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
                    },
                });
    
                const person = response.data.item.person || {};
                const defaultProfileData = {
                    urlSegment: person.urlSegment || "",
                    contactEmail: person.contactEmail || "",
                    name: person.name || "",
                    surname: person.surname || "",
                    birthDate: person.birthDate || { year: 0, month: 0, day: 0 },
                    contactPhoneNum: person.contactPhoneNum || "",
                    description: person.description || "",
                    isStudent: person.isStudent || false,
                    isPublicProfile: person.isPublicProfile || false,
                    addressId: person.addressId || "",
                    characteristics: person.characteristics || [],
                };
    
                setProfileData(defaultProfileData);
            } catch (error) {
                console.error("Error fetching profile data:", error);
            }
        };
    
        fetchProfileData();
    }, []);
    
    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setProfileData((prevData) => ({
            ...prevData,
            [name]: value,
        }));
    };


    const handleProfileUpdate = async (event) => {
        event.preventDefault();
    
        if (!profileData.name || !profileData.surname || !profileData.contactEmail) {
            setMessage("Please fill in all required fields.");
            return;
        }
    
        try {
            const response = await axios.put(
                "https://localhost:7166/api/User/person",
                profileData,
                {
                    headers: {
                        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
                    },
                }
            );
            setMessage("Profile updated successfully.");
        } catch (error) {
            console.error("Error updating profile:", error);
            setMessage("Failed to update profile. Please try again.");
        }
    };
    

    return (
        <div>
            <h1>Edit Profile</h1>
            <form onSubmit={handleProfileUpdate}>
                <label>Name:</label><br/>
                <input
                    type="text"
                    name="name"
                    value={profileData.name || ""}
                    onChange={handleInputChange}
                    required
                /><br/>
                <label>Surname:</label><br/>
                <input
                    type="text"
                    name="surname"
                    value={profileData.surname || ""}
                    onChange={handleInputChange}
                    required
                /><br/>
                <label>Email:</label><br/>
                <input
                    type="email"
                    name="contactEmail"
                    value={profileData.contactEmail || ""}
                    onChange={handleInputChange}
                    required
                /><br/>
                <label>Phone Number:</label><br/>
                <input
                    type="tel"
                    name="contactPhoneNum"
                    value={profileData.contactPhoneNum || ""}
                    onChange={handleInputChange}
                /><br/>
                <label>Description:</label><br/>
                <textarea
                    name="description"
                    value={profileData.description || ""}
                    onChange={handleInputChange}
                ></textarea>
                <label><br/>
                    <input
                        type="checkbox"
                        name="isStudent"
                        checked={profileData.isStudent || false}
                        onChange={(e) =>
                            setProfileData((prevData) => ({
                                ...prevData,
                                isStudent: e.target.checked,
                            }))
                        }
                    />
                    Student
                </label>
                <label><br/>
                    <input
                        type="checkbox"
                        name="isPublicProfile"
                        checked={profileData.isPublicProfile || false}
                        onChange={(e) =>
                            setProfileData((prevData) => ({
                                ...prevData,
                                isPublicProfile: e.target.checked,
                            }))
                        }
                    />
                    Public Profile
                </label><br/>
                <button type="submit">Save Changes</button>
            </form>
            {message && <p>{message}</p>}
        </div>
    );
    
};

export default EditProfile;
