// import React, { useState, useEffect } from "react";
// import axios from "axios";

// const EditProfile = () => {
//     const [profileData, setProfileData] = useState({
//         urlSegment: "",
//         contactEmail: "",
//         name: "",
//         surname: "",
//         birthDate: { year: 0, month: 0, day: 0 },
//         contactPhoneNum: "",
//         description: "",
//         isStudent: false,
//         isPublicProfile: false,
//         addressId: "",
//         characteristics: [],
//     });

//     const [message, setMessage] = useState("");

//     useEffect(() => {
//         const fetchProfileData = async () => {
//             try {
//                 const response = await axios.get("https://localhost:7166/api/User", {
//                     headers: {
//                         Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
//                     },
//                 });
//                 const person = response.data.item.person || {};
//                 setProfileData({
//                     urlSegment: person.urlSegment || "",
//                     contactEmail: person.contactEmail || "",
//                     name: person.name || "",
//                     surname: person.surname || "",
//                     birthDate: person.birthDate || { year: 0, month: 0, day: 0 },
//                     contactPhoneNum: person.contactPhoneNum || "",
//                     description: person.description || "",
//                     isStudent: person.isStudent || false,
//                     isPublicProfile: person.isPublicProfile || false,
//                     addressId: person.addressId || "",
//                     characteristics: person.characteristics || [],
//                 });
//             } catch (error) {
//                 console.error("Error fetching profile data:", error);
//             }
//         };

//         fetchProfileData();
//     }, []);

//     const handleInputChange = (e) => {
//         const { name, value } = e.target;
//         setProfileData((prevData) => ({
//             ...prevData,
//             [name]: value,
//         }));
//     };

//     const handleProfileUpdate = async (event) => {
//         event.preventDefault();
//         try {
//             const updatedData = {
//                 ...profileData,
//                 birthDate: {
//                     year: Number(profileData.birthDate.year),
//                     month: Number(profileData.birthDate.month),
//                     day: Number(profileData.birthDate.day),
//                 },
//             };

//             await axios.put("https://localhost:7166/api/User/person", updatedData, {
//                 headers: {
//                     Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
//                 },
//             });
//             setMessage("Profile updated successfully.");
//         } catch (error) {
//             console.error("Error updating profile:", error.response?.data || error.message);
//             setMessage("Failed to update profile. Please try again.");
//         }
//     };

//     return (
//         <div>
//             <h1>Edit Profile</h1>
//             <form onSubmit={handleProfileUpdate}>
//                 <label>URL Segment:</label>
//                 <input
//                     type="text"
//                     name="urlSegment"
//                     value={profileData.urlSegment}
//                     onChange={handleInputChange}
//                 /><br/>
//                 <label>Email:</label>
//                 <input
//                     type="email"
//                     name="contactEmail"
//                     value={profileData.contactEmail}
//                     onChange={handleInputChange}
//                     required
//                 /><br/>
//                 <label>Name:</label>
//                 <input
//                     type="text"
//                     name="name"
//                     value={profileData.name}
//                     onChange={handleInputChange}
//                     required
//                 /><br/>
//                 <label>Surname:</label>
//                 <input
//                     type="text"
//                     name="surname"
//                     value={profileData.surname}
//                     onChange={handleInputChange}
//                     required
//                 /><br/>
//                 <label>Birth Date:</label>
//                 <input
//                     type="number"
//                     name="birthDate.year"
//                     placeholder="Year"
//                     value={profileData.birthDate.year || ""}
//                     onChange={(e) =>
//                         setProfileData((prevData) => ({
//                             ...prevData,
//                             birthDate: { ...prevData.birthDate, year: e.target.value },
//                         }))
//                     }
//                 /><br/>
//                 <input
//                     type="number"
//                     name="birthDate.month"
//                     placeholder="Month"
//                     value={profileData.birthDate.month || ""}
//                     onChange={(e) =>
//                         setProfileData((prevData) => ({
//                             ...prevData,
//                             birthDate: { ...prevData.birthDate, month: e.target.value },
//                         }))
//                     }
//                 /><br/>
//                 <input
//                     type="number"
//                     name="birthDate.day"
//                     placeholder="Day"
//                     value={profileData.birthDate.day || ""}
//                     onChange={(e) =>
//                         setProfileData((prevData) => ({
//                             ...prevData,
//                             birthDate: { ...prevData.birthDate, day: e.target.value },
//                         }))
//                     }
//                 /><br/>
//                 <label>Phone Number:</label>
//                 <input
//                     type="tel"
//                     name="contactPhoneNum"
//                     value={profileData.contactPhoneNum}
//                     onChange={handleInputChange}
//                 /><br/>
//                 <label>Description:</label>
//                 <textarea
//                     name="description"
//                     value={profileData.description}
//                     onChange={handleInputChange}
//                 ></textarea>
//                 <label><br/>
//                     <input
//                         type="checkbox"
//                         name="isStudent"
//                         checked={profileData.isStudent}
//                         onChange={(e) =>
//                             setProfileData((prevData) => ({
//                                 ...prevData,
//                                 isStudent: e.target.checked,
//                             }))
//                         }
//                     />
//                     Student
//                 </label>
//                 <label><br/>
//                     <input
//                         type="checkbox"
//                         name="isPublicProfile"
//                         checked={profileData.isPublicProfile}
//                         onChange={(e) =>
//                             setProfileData((prevData) => ({
//                                 ...prevData,
//                                 isPublicProfile: e.target.checked,
//                             }))
//                         }
//                     />
//                     Public Profile
//                 </label><br/>
//                 <button type="submit">Save Changes</button>
//             </form>
//             {message && <p>{message}</p>}
//         </div>
//     );
// };

// export default EditProfile;

import React, { useState, useEffect } from "react";
import axios from "axios";
import ProfileCencelButton from "../CencelButtonProfile/profileCencelButton";

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
    const [message, setMessage] = useState("");

    useEffect(() => {
        const token = sessionStorage.getItem("jwt") || localStorage.getItem("jwt");

        if (!token) {
            alert("You must be logged in to edit your profile.");
            window.location.href = "/login";
            return;
        }

        const fetchProfileData = async () => {
            try {
                const response = await axios.get("https://localhost:7166/api/User", {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });

                const person = response.data.item.person || {};

                // Formatowanie daty na "YYYY-MM-DD"
                const formattedDate = person.birthDate
                    ? new Date(person.birthDate).toISOString().split("T")[0]
                    : "";

                setProfileData({
                    urlSegment: person.urlSegment || "",
                    contactEmail: person.contactEmail || "",
                    name: person.name || "",
                    surname: person.surname || "",
                    birthDate: formattedDate, // Ustawiona sformatowana data
                    contactPhoneNum: person.contactPhoneNum || "",
                    description: person.description || "",
                    isStudent: person.isStudent || false,
                    isPublicProfile: person.isPublicProfile || false,
                    addressId: person.addressId || "",
                    characteristics: person.characteristics || [],
                });
            } catch (error) {
                console.error("Error fetching profile data:", error);
                setMessage("Failed to load profile data.");
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
        try {
            const token = sessionStorage.getItem("jwt") || localStorage.getItem("jwt");

            // Rozdzielenie daty na year, month, day
            const [year, month, day] = profileData.birthDate.split("-").map(Number);

            const updatedData = {
                ...profileData,
                birthDate: { year, month, day }, // Struktura zgodna z API
            };

            await axios.put("https://localhost:7166/api/User/person", updatedData, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });
            setMessage("Profile updated successfully.");
        } catch (error) {
            console.error("Error updating profile:", error.response?.data || error.message);
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
                ></textarea>
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
                    /><br/>
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
                </label><br/>
                <button type="submit">Save Changes</button><br/>
                <ProfileCencelButton/>
            </form>
            {message && <p>{message}</p>}
        </div>
    );
};

export default EditProfile;
