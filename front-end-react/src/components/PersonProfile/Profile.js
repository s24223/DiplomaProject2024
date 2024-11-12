import React, { useState, useEffect } from 'react';
import axios from 'axios';

const Profile = () => {
    const [profileData, setProfileData] = useState({
        urlSegment: '',
        contactEmail: '',
        name: '',
        surname: '',
        birthDate: { year: '', month: '', day: '' },
        contactPhoneNum: '',
        description: '',
        isStudent: false,
        isPublicProfile: true,
        addressId: null,
    });

    const [divisionId, setDivisionId] = useState(null); // Województwo
    const [powiatId, setPowiatId] = useState(null);     // Powiat
    const [gminaId, setGminaId] = useState(null);       // Gmina
    const [streetId, setStreetId] = useState(null);     // Ulica

    const [divisions, setDivisions] = useState([]);
    const [powiats, setPowiats] = useState([]);
    const [gminas, setGminas] = useState([]);
    const [streets, setStreets] = useState([]);

    // Fetch initial województwa
    useEffect(() => {
        axios.get('/api/Address/divisionsDown') // Adjust with your endpoint
            .then(response => setDivisions(response.data.items))
            .catch(error => console.error("Error fetching województwa:", error));
    }, []);

    // Fetch powiaty when divisionId changes
    useEffect(() => {
        if (divisionId) {
            axios.get(`/api/Address/divisionsDown?id=${divisionId}`)
                .then(response => setPowiats(response.data.items))
                .catch(error => console.error("Error fetching powiats:", error));
        }
    }, [divisionId]);

    // Fetch gminy when powiatId changes
    useEffect(() => {
        if (powiatId) {
            axios.get(`/api/Address/divisionsDown?id=${powiatId}`)
                .then(response => setGminas(response.data.items))
                .catch(error => console.error("Error fetching gminas:", error));
        }
    }, [powiatId]);

    // Fetch streets when gminaId changes
    useEffect(() => {
        if (gminaId) {
            axios.get(`/api/Address/divisionsDown?id=${gminaId}`)
                .then(response => setStreets(response.data.items.flatMap(item => item.streets)))
                .catch(error => console.error("Error fetching streets:", error));
        }
    }, [gminaId]);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setProfileData(prevState => ({ ...prevState, [name]: value }));
    };

    const handleAddressSubmit = async () => {
        try {
            const addressResponse = await axios.post('/api/Address', {
                divisionId: gminaId,
                streetId: streetId,
                buildingNumber: profileData.buildingNumber,
                apartmentNumber: profileData.apartmentNumber,
                zipCode: profileData.zipCode,
            });
            setProfileData(prevState => ({ ...prevState, addressId: addressResponse.data.id }));
            console.log("Address saved successfully");
        } catch (error) {
            console.error("Error saving address:", error);
        }
    };

    const handleProfileSubmit = async () => {
        try {
            await axios.post('/api/Person', profileData);
            console.log("Profile saved successfully");
        } catch (error) {
            console.error("Error saving profile:", error);
        }
    };

    return (
        <form onSubmit={(e) => { e.preventDefault(); handleProfileSubmit(); }}>
            <h2>User Profile</h2>

            <input type="text" name="urlSegment" placeholder="Url" onChange={handleInputChange} />
            <input type="text" name="contactEmail" placeholder="Email" onChange={handleInputChange} />
            <input type="text" name="name" placeholder="Name" onChange={handleInputChange} />
            <input type="text" name="surname" placeholder="Surname" onChange={handleInputChange} />
            
            <h3>Birth Date</h3>
            <input type="number" name="birthDate.year" placeholder="Year" onChange={handleInputChange} />
            <input type="number" name="birthDate.month" placeholder="Month" onChange={handleInputChange} />
            <input type="number" name="birthDate.day" placeholder="Day" onChange={handleInputChange} />

            <h3>Address</h3>
            <select onChange={(e) => setDivisionId(e.target.value)}>
                <option value="">Select Województwo</option>
                {divisions.map(division => <option key={division.division.id} value={division.division.id}>{division.division.name}</option>)}
            </select>

            <select onChange={(e) => setPowiatId(e.target.value)} disabled={!divisionId}>
                <option value="">Select Powiat</option>
                {powiats.map(powiat => <option key={powiat.division.id} value={powiat.division.id}>{powiat.division.name}</option>)}
            </select>

            <select onChange={(e) => setGminaId(e.target.value)} disabled={!powiatId}>
                <option value="">Select Gmina</option>
                {gminas.map(gmina => <option key={gmina.division.id} value={gmina.division.id}>{gmina.division.name}</option>)}
            </select>

            <select onChange={(e) => setStreetId(e.target.value)} disabled={!gminaId}>
                <option value="">Select Street</option>
                {streets.map(street => <option key={street.id} value={street.id}>{street.name}</option>)}
            </select>

            <input type="text" name="buildingNumber" placeholder="Building Number" onChange={handleInputChange} />
            <input type="text" name="apartmentNumber" placeholder="Apartment Number" onChange={handleInputChange} />
            <input type="text" name="zipCode" placeholder="ZIP Code" onChange={handleInputChange} />

            <button type="button" onClick={handleAddressSubmit}>Save Address</button>
            <button type="submit">Save Profile</button>
        </form>
    );
};

export default Profile;
