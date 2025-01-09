import React from "react";
import EditProfile from "../../components/PersonProfileEdit/EditProfile";


const ProfileEditPage = () =>{
    return(
        <div>

            <EditProfile/>
        </div>
    )
}

export default ProfileEditPage;

// import React, { useState, useEffect } from "react";
// import EditProfile from "../../components/PersonProfileEdit/EditProfile";
// import { fetchCharacteristics } from "../../services/CharacteristicsService/CharacteristicsService";
// import { fetchUserProfile, updateUserProfile } from "../../services/ProfileService/ProfileService";

// const ProfileEditPage = () => {
//     const [profileData, setProfileData] = useState(null);
//     const [allCharacteristics, setAllCharacteristics] = useState([]);
//     const [loading, setLoading] = useState(true);
//     const [message, setMessage] = useState("");

//     useEffect(() => {
//         const loadData = async () => {
//             try {
//                 const [characteristics, person] = await Promise.all([
//                     fetchCharacteristics(),
//                     fetchUserProfile(),
//                 ]);

//                 const formattedDate = person.birthDate
//                     ? new Date(person.birthDate).toISOString().split("T")[0]
//                     : "";

//                 setProfileData({
//                     urlSegment: person.urlSegment || "",
//                     contactEmail: person.contactEmail || "",
//                     name: person.name || "",
//                     surname: person.surname || "",
//                     birthDate: formattedDate,
//                     contactPhoneNum: person.contactPhoneNum || "",
//                     description: person.description || "",
//                     isStudent: person.isStudent || false,
//                     isPublicProfile: person.isPublicProfile || false,
//                     addressId: person.addressId || null,
//                     characteristics: person.characteristics.map((char) => ({
//                         characteristicId: char.characteristic.id,
//                         qualityId: char.quality?.id || "",
//                     })),
//                 });

//                 setAllCharacteristics(characteristics);
//                 setLoading(false);
//             } catch (error) {
//                 console.error("Error loading data:", error);
//                 setMessage("Failed to load profile data.");
//                 setLoading(false);
//             }
//         };

//         loadData();
//     }, []);

//     const handleProfileUpdate = async (event) => {
//             event.preventDefault();
//             try {
                
    
//                 // przygotowanie characteristics z quality=null kiedy potrzeba(nie są to języki mowy)  
//                 const updatedCharacteristics = profileData.characteristics.map((char) => {
//                     const matchedCharacteristic = allCharacteristics.find(
//                         (item) => item.characteristic.id.toString() === char.characteristicId.toString()
//                     );
//                     if ([1, 2, 3, 4, 9].includes(matchedCharacteristic?.characteristicType.id)) {
//                         return null;
//                     }
    
//                     if (matchedCharacteristic?.characteristicType.id !== 6) {
//                         // If not "Języki komunikacji", set qualityId to null
//                         return { ...char, qualityId: null };
//                     }
    
//                     return char;
//                 }).filter((char) => char !== null); // Usuwanie nulli;
    
//                 // Rozdzielenie daty na year, month, day
//                 const [year, month, day] = profileData.birthDate.split("-").map(Number);
    
//                 const updatedData = {
//                     ...profileData,
//                     addressId: profileData.addressId,
//                     birthDate: { year, month, day }, // Struktura zgodna z API
//                     characteristics: updatedCharacteristics,
//                 };
    
//                 await updateUserProfile(updatedData);
//                 setMessage("Profile updated successfully.");
//                 setTimeout(() => {
//                     window.location.href = "/userProfile"; 
//                 }, 2000);
//             } catch (error) {
//                 console.error("Error updating profile:", error);
//                 setMessage("Failed to update profile. Please try again.");
//             }
//         };

//     if (loading) return <p>Loading...</p>;

//     return (
//         <div>
//             <EditProfile
//                 profileData={profileData}
//                 allCharacteristics={allCharacteristics}
//                 onProfileUpdate={handleProfileUpdate}
//                 message={message}
//             />
//         </div>
//     );
// };

// export default ProfileEditPage;
