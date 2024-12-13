import React, { useState, useEffect } from "react";
import CancelButton from "../CancelButton/CancelButton";
import { fetchCharacteristics } from "../../services/CharacteristicsService/CharacteristicsService";
import { fetchUserProfile, updateUserProfile } from "../../services/ProfileService/ProfileService";

const EditProfile = () => {
    const [profileData, setProfileData] = useState({
        urlSegment: "",
        contactEmail: "",
        name: "",
        surname: "",
        birthDate: "", // Zmienione na string w formacie YYYY-MM-DD
        contactPhoneNum: "",
        description: "",
        isStudent: false,
        isPublicProfile: false,
        addressId: "",
        characteristics: [],
    });
    const [allCharacteristics, setAllCharacteristics] = useState([])
    const [message, setMessage] = useState("");
    const [loading, setLoading] = useState(true);

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


                // console.log("Characteristics:", characteristics); // Debugging line

                // Formatowanie daty na "YYYY-MM-DD"
                const formattedDate = person.birthDate
                    ? new Date(person.birthDate).toISOString().split("T")[0]
                    : "";

                setAllCharacteristics(characteristics);
                setProfileData({
                    urlSegment: person.urlSegment || "" ,
                    contactEmail: person.contactEmail || "",
                    name: person.name || "",
                    surname: person.surname || "",
                    birthDate: formattedDate, // Ustawiona sformatowana data
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

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setProfileData((prevData) => ({
            ...prevData,
            [name]: value,
        }));
    };

    const handleCharacteristicChange = (index, field, value) => {
        setProfileData((prevData) => {
            const updatedCharacteristics = [...prevData.characteristics];
            updatedCharacteristics[index] = { ...updatedCharacteristics[index], [field]: value };
            return { ...prevData, characteristics: updatedCharacteristics };
        });
    };

    const addCharacteristic = () => {
        setProfileData((prevData) => ({
            ...prevData,
            characteristics: [...prevData.characteristics, { characteristicId: "", qualityId: "" }],
        }));
    };

    const removeCharacteristic = (index) => {
        setProfileData((prevData) => ({
            ...prevData,
            characteristics: prevData.characteristics.filter((_, i) => i !== index),
        }));
    };

    const handleProfileUpdate = async (event) => {
        event.preventDefault();
        try {
            const token = sessionStorage.getItem("jwt") || localStorage.getItem("jwt");

            // przygotowanie characteristics z wuality=null kiedy potrzeba(nie są to języki mowy)  
            const updatedCharacteristics = profileData.characteristics.map((char) => {
                const matchedCharacteristic = allCharacteristics.find(
                    (item) => item.characteristic.id.toString() === char.characteristicId.toString()
                );

                if (matchedCharacteristic?.characteristicType.id !== 6) {
                    // If not "Języki komunikacji", set qualityId to null
                    return { ...char, qualityId: null };
                }

                return char;
            });

            // Rozdzielenie daty na year, month, day
            const [year, month, day] = profileData.birthDate.split("-").map(Number);

            const updatedData = {
                ...profileData,
                birthDate: { year, month, day }, // Struktura zgodna z API
                characteristics: updatedCharacteristics,
            };

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
            <form onSubmit={handleProfileUpdate}>
                <label>Name:</label><br/>
                <input
                    type="text"
                    name="name"
                    value={profileData.name}
                    onChange={handleInputChange}
                    required
                /><br/>
                <label>Surname:</label><br/>
                <input
                    type="text"
                    name="surname"
                    value={profileData.surname}
                    onChange={handleInputChange}
                    required
                /><br/>
                <label>Email:</label><br/>
                <input
                    type="email"
                    name="contactEmail"
                    value={profileData.contactEmail}
                    onChange={handleInputChange}
                    required
                /><br/>
                <label>Phone Number:</label><br/>
                <input
                    type="tel"
                    name="contactPhoneNum"
                    value={profileData.contactPhoneNum}
                    onChange={handleInputChange}
                /><br/>
                <label>Birth Date:</label><br/>
                <input
                    type="date" // Pole daty w formacie YYYY-MM-DD
                    name="birthDate"
                    value={profileData.birthDate}
                    onChange={handleInputChange}
                /><br/>
                <label>Description:</label><br/>
                <textarea
                    name="description"
                    value={profileData.description}
                    onChange={handleInputChange}
                ></textarea><br />
                <label>
                    <input
                        type="checkbox"
                        name="isStudent"
                        checked={profileData.isStudent}
                        onChange={(e) =>
                            setProfileData((prevData) => ({
                                ...prevData,
                                isStudent: e.target.checked,
                            }))
                        }
                    />
                    Student
                </label><br/>
                <label>
                    <input
                        type="checkbox"
                        name="isPublicProfile"
                        checked={profileData.isPublicProfile}
                        onChange={(e) =>
                            setProfileData((prevData) => ({
                                ...prevData,
                                isPublicProfile: e.target.checked,
                            }))
                        }
                    />
                    Public Profile

                    <h3>Characteristics</h3>
                {profileData.characteristics.map((char, index) => {
                    // Find the selected characteristic to determine if quality should be selectable
                    const selectedCharacteristic = allCharacteristics.find(
                        (item) => item.characteristic.id.toString() === char.characteristicId.toString()
                    );

                    const isQualitySelectable = selectedCharacteristic?.characteristicType.id === 6;

                    return (
                        <div key={index} style={{ marginBottom: "10px" }}>
                            {/* Input for characteristic with text search */}
                            <input
                                type="text"
                                list={`characteristics-${index}`}
                                placeholder="Characteristic"
                                onChange={(e) =>
                                    handleCharacteristicChange(index, "characteristicId", e.target.value)
                                }
                                value={
                                    selectedCharacteristic
                                        ? selectedCharacteristic.characteristic.name // Show the name of the characteristic
                                        : char.characteristicId || ""
                                }
                            />
                            <datalist id={`characteristics-${index}`}>
                                {allCharacteristics.map((item) => (
                                    <option key={item.characteristic.id} value={item.characteristic.id}>
                                        {item.characteristic.name}
                                    </option>
                                ))}
                            </datalist>

                            {/* Select for quality, disabled if quality is not selectable */}
                            <select
                                onChange={(e) =>
                                    handleCharacteristicChange(index, "qualityId", e.target.value)
                                }
                                value={char.qualityId || ""}
                                disabled={!isQualitySelectable} // Disable if quality cannot be selected
                            >
                                <option value="">Select Quality</option>
                                {isQualitySelectable &&
                                    selectedCharacteristic?.possibleQualities.map((qual) => (
                                        <option key={qual.id} value={qual.id}>
                                            {qual.name}
                                        </option>
                                    ))}
                            </select>

                            {/* Remove characteristic button */}
                            <button type="button" onClick={() => removeCharacteristic(index)}>
                                Remove
                            </button>
                        </div>
                    );
                })}
                <button type="button" onClick={addCharacteristic}>
                    Add Characteristic
                </button>
                <br/><br/>
                </label><br/>
                <button type="submit">Save Changes</button><br/>
                <CancelButton/>
            </form>
            {message && <p style={{ color: message.includes("successfully") ? "green" : "red" }}>{message}</p>}
        </div>
    );
};

export default EditProfile;

