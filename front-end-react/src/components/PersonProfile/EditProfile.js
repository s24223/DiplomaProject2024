import React, { useState, useEffect } from "react";
import EditProfileForm from "../Forms/ProfileEditForm";
import { fetchCharacteristics } from "../../services/CharacteristicsService/CharacteristicsService";
import { fetchUserProfile, updateUserProfile } from "../../services/ProfileService/ProfileService";

const EditProfile = () => {
    const [profileData, setProfileData] = useState(null);
    const [allCharacteristics, setAllCharacteristics] = useState([]);
    const [loading, setLoading] = useState(true);
    const [message, setMessage] = useState("");

    useEffect(() => {
        const token = sessionStorage.getItem("jwt") || localStorage.getItem("jwt");

        if (!token) {
            alert("You must be logged in to edit your profile.");
            window.location.href = "/login";
            return;
        }

        const loadProfile = async () => {
            try {
                const characteristics = await fetchCharacteristics();
                const person = await fetchUserProfile();

                // Formatowanie daty na "YYYY-MM-DD"
                const formattedDate = person.birthDate
                    ? new Date(person.birthDate).toISOString().split("T")[0]
                    : "";

                setAllCharacteristics(characteristics);
                setProfileData({
                    urlSegment: person.urlSegment || "",
                    contactEmail: person.contactEmail || "",
                    name: person.name || "",
                    surname: person.surname || "",
                    birthDate: formattedDate,
                    contactPhoneNum: person.contactPhoneNum || "",
                    description: person.description || "",
                    isStudent: person.isStudent || false,
                    isPublicProfile: person.isPublicProfile || false,
                    addressId: person.addressId || null,
                    characteristics: person.characteristics.map((char) => ({
                        characteristicId: char.characteristic.id,
                        qualityId: char.quality?.id || "",
                    })),
                });
                setLoading(false);
            } catch (error) {
                console.error("Error fetching profile data:", error);
                setMessage("Failed to load profile data.");
                setLoading(false);
            }
        };

        loadProfile();
    }, []);

    const handleProfileUpdate = async (updatedData) => {
        try {
            await updateUserProfile(updatedData);
            setMessage("Profile updated successfully.");
            setTimeout(() => {
                window.location.href = "/userProfile";
            }, 2000);
        } catch (error) {
            console.error("Error updating profile:", error);
            setMessage("Failed to update profile. Please try again.");
        }
    };

    if (loading) {
        return <p>Loading profile data...</p>;
    }

    return (
        <div className="centered">
            <h1>Edit Profile</h1>
            <EditProfileForm
                profileData={profileData}
                allCharacteristics={allCharacteristics}
                onProfileUpdate={handleProfileUpdate}
            />
            {message && <p style={{ color: message.includes("successfully") ? "green" : "red" }}>{message}</p>}
        </div>
    );
};

export default EditProfile;