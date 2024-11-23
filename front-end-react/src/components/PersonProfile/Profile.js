// import React, { useState, useEffect } from 'react';
// import { fetchProfileGet } from '../../services/ProfileService/ProfilceService';
// import axios from 'axios';

// const Profile = () => {
//     const [profileData, setProfileData] = useState({
//         urlSegment: '',
//         contactEmail: '',
//         name: '',
//         surname: '',
//         birthDate: { year: '', month: '', day: '' },
//         contactPhoneNum: '',
//         description: '',
//         isStudent: false,
//         isPublicProfile: true,
//         addressId: null,
//     });

//     const [divisionId, setDivisionId] = useState(null); // Województwo
//     const [powiatId, setPowiatId] = useState(null);     // Powiat
//     const [gminaId, setGminaId] = useState(null);       // Gmina
//     const [streetId, setStreetId] = useState(null);     // Ulica

//     const [divisions, setDivisions] = useState([]);
//     const [powiats, setPowiats] = useState([]);
//     const [gminas, setGminas] = useState([]);
//     const [streets, setStreets] = useState([]);
// /*
//     // Fetch initial województwa
//     useEffect(() => {
//         axios.get('/api/Address/divisionsDown') // Adjust with your endpoint
//             .then(response => setDivisions(response.data.items))
//             .catch(error => console.error("Error fetching województwa:", error));
//     }, []);

//     // Fetch powiaty when divisionId changes
//     useEffect(() => {
//         if (divisionId) {
//             axios.get(`/api/Address/divisionsDown?id=${divisionId}`)
//                 .then(response => setPowiats(response.data.items))
//                 .catch(error => console.error("Error fetching powiats:", error));
//         }
//     }, [divisionId]);

//     // Fetch gminy when powiatId changes
//     useEffect(() => {
//         if (powiatId) {
//             axios.get(`/api/Address/divisionsDown?id=${powiatId}`)
//                 .then(response => setGminas(response.data.items))
//                 .catch(error => console.error("Error fetching gminas:", error));
//         }
//     }, [powiatId]);

//     // Fetch streets when gminaId changes
//     useEffect(() => {
//         if (gminaId) {
//             axios.get(`/api/Address/divisionsDown?id=${gminaId}`)
//                 .then(response => setStreets(response.data.items.flatMap(item => item.streets)))
//                 .catch(error => console.error("Error fetching streets:", error));
//         }
//     }, [gminaId]);
// */

//     useEffect(() => {
//         let dummyFetch = async () => {
//             let data = await fetchProfileGet()
//             console.log(data)
//             if (data.item.person === null)
//                 window.location.href="/userCreatePofile"
//         }
//         dummyFetch().catch(console.error)
//     }, [])

//     const handleInputChange = (e) => {
//         const { name, value } = e.target;
//         setProfileData(prevState => ({ ...prevState, [name]: value }));
//     };

//     const handleAddressSubmit = async () => {
//         try {
//             const addressResponse = await axios.post('/api/Address', {
//                 divisionId: gminaId,
//                 streetId: streetId,
//                 buildingNumber: profileData.buildingNumber,
//                 apartmentNumber: profileData.apartmentNumber,
//                 zipCode: profileData.zipCode,
//             });
//             setProfileData(prevState => ({ ...prevState, addressId: addressResponse.data.id }));
//             console.log("Address saved successfully");
//         } catch (error) {
//             console.error("Error saving address:", error);
//         }
//     };

//     const handleProfileSubmit = async () => {
//         try {
//             await axios.post('/api/Person', profileData);
//             console.log("Profile saved successfully");
//         } catch (error) {
//             console.error("Error saving profile:", error);
//         }
//     };

//     return (
//         <form onSubmit={(e) => { e.preventDefault(); handleProfileSubmit(); }}>
//             <h2>User Profile???</h2>
//             huh????

//             <input type="text" name="urlSegment" placeholder="Url" onChange={handleInputChange} />
//             <input type="text" name="contactEmail" placeholder="Email" onChange={handleInputChange} />
//             <input type="text" name="name" placeholder="Name" onChange={handleInputChange} />
//             <input type="text" name="surname" placeholder="Surname" onChange={handleInputChange} />
            
//             <h3>Birth Date</h3>
//             <input type="number" name="birthDate.year" placeholder="Year" onChange={handleInputChange} />
//             <input type="number" name="birthDate.month" placeholder="Month" onChange={handleInputChange} />
//             <input type="number" name="birthDate.day" placeholder="Day" onChange={handleInputChange} />

//             <h3>Address</h3>
//             <select onChange={(e) => setDivisionId(e.target.value)}>
//                 <option value="">Select Województwo</option>
//                 {divisions.map(division => <option key={division.division.id} value={division.division.id}>{division.division.name}</option>)}
//             </select>

//             <select onChange={(e) => setPowiatId(e.target.value)} disabled={!divisionId}>
//                 <option value="">Select Powiat</option>
//                 {powiats.map(powiat => <option key={powiat.division.id} value={powiat.division.id}>{powiat.division.name}</option>)}
//             </select>

//             <select onChange={(e) => setGminaId(e.target.value)} disabled={!powiatId}>
//                 <option value="">Select Gmina</option>
//                 {gminas.map(gmina => <option key={gmina.division.id} value={gmina.division.id}>{gmina.division.name}</option>)}
//             </select>

//             <select onChange={(e) => setStreetId(e.target.value)} disabled={!gminaId}>
//                 <option value="">Select Street</option>
//                 {streets.map(street => <option key={street.id} value={street.id}>{street.name}</option>)}
//             </select>

//             <input type="text" name="buildingNumber" placeholder="Building Number" onChange={handleInputChange} />
//             <input type="text" name="apartmentNumber" placeholder="Apartment Number" onChange={handleInputChange} />
//             <input type="text" name="zipCode" placeholder="ZIP Code" onChange={handleInputChange} />

//             <button type="button" onClick={handleAddressSubmit}>Save Address</button>
//             <button type="submit">Save Profile</button>
//         </form>
//     );
// };

// export default Profile;

import React, { useState, useEffect } from "react";
import axios from "axios";
import LoginButton from "../../components/LoginButton/LoginButton";
import MainPageButton from "../../components/MainPageButton/MainPageButton";


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
            <button onClick={handleProfileCreate} style={{ position: 'absolute', top: 40, right: 82 }}>
                    Create Profile
            </button>
            <button onClick={handleCompanyCreate} style={{ position: 'absolute', top: 80, right: 100 }}>
                    Create Company
            </button>
            <button onClick={handleCompanyEdit} style={{ position: 'absolute', top: 80, right: 10 }}>
                    Edit Company
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

export default Profile;
