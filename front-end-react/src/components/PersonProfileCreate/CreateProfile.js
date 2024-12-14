import React, { useState,useEffect } from "react";
import CancelButton from "../CancelButton/CancelButton";
import { fetchCharacteristics } from "../../services/CharacteristicsService/CharacteristicsService";
import { fetchProfilePost } from "../../services/ProfileService/ProfileService";
import LoginButton from "../LoginButton/LoginButton";
import AddressAutocomplete from "../AddressAutoComplete/AddressAutoComplete";

const CreateProfile = () => {
    const [urlSegmet, setUrlsegmet] = useState("string")
    const [contactEmail, setContactEmail] = useState()
    const [name, setName] = useState()
    const [surname, setSurname] = useState()
    const [birthDate, setBirthdate] = useState({
        year: null,
        month: null,
        day:null
    })
    const [contactPhoneNum, setContactPhoneNum] = useState()
    const [description, setDescription] = useState("")
    const [isStudent, setIsStudent] = useState(false)
    const [isPublicProfile, setIsPublicProfile] = useState(false)
    const [addressId, setAddressId] = useState(null)
    const [characteristics, setCharacteristics] = useState([])
    const [allCharacteristics, setAllCharacteristics] = useState([]);
    const [message, setMessage] = useState("");

    const handleBirthDate = (date) => {
        let dateSegmented = date.split("-")
        setBirthdate({
            year: dateSegmented[0],
            month: dateSegmented[1],
            day: dateSegmented[2]
        });
    }

    const [loading, setLoading] = useState(true);
    useEffect(() => {
        const loadCharacteristics = async() =>{
            try{
                const characteristic = await fetchCharacteristics();
                setAllCharacteristics(characteristic)
                setLoading(false);
            } catch (error) {
                console.error("Error fetching characteristics:", error);
                setMessage("Failed to load characteristics.");
                setLoading(false);
            }
       
        
        };
        loadCharacteristics();
    }, []);

    const handleCharacteristicChange = (index, field, value) => {
        setCharacteristics((prev) => {
            const updated = [...prev];
            updated[index] = { ...updated[index], [field]: value };
            return updated;
        });
    };

    const addCharacteristic = () => {
        setCharacteristics((prev) => [...prev, { characteristicId: "", qualityId: "" }]);
    };

    const removeCharacteristic = (index) => {
        setCharacteristics((prev) => prev.filter((_, i) => i !== index));
    };

    const childToParent = (addressIdFromChild) => {
        setAddressId(addressIdFromChild)
    }
    

    const handleSubmit = async (event) => {
        event.preventDefault()
        
        // Preprocess characteristics
        const processedCharacteristics = characteristics.map((char) => {
            const characteristic = allCharacteristics.find(
                (item) => item.characteristic.id.toString() === char.characteristicId
            );

            // Check characteristicType.id and adjust qualityId
            if (characteristic?.characteristicType.id !== 6) {
                return { ...char, qualityId: null }; // Set qualityId to null if not "Języki komunikacji"
            }

            return char; // Keep the original characteristic if type is "Języki komunikacji"
        });


        let body = {
            urlSegmet,
            contactEmail,
            name,
            surname,
            birthDate,
            contactPhoneNum,
            description,
            isStudent,
            isPublicProfile,
            addressId,
            characteristics: processedCharacteristics,
        }

        try {
            await fetchProfilePost(body)
            setMessage("Created successfully!"); // Ustawienie wiadomości o sukcesie
            setTimeout(() => {
                window.location.href = "/userProfile"; // Przekierowanie po 2 sekundach
            }, 2000);
        } catch(error){
            console.error(error)
        }
    }

    if (loading) {
        return <p>Loading characteristics...</p>;
    }
    return(
        <div className="form">
            <LoginButton/>
            <form onSubmit={handleSubmit}>
                <label>Name:</label><br />
                <input type="text" placeholder="Name" onChange={e => setName(e.target.value)} required /><br />
                <label>Surname:</label><br />
                <input type="text" placeholder="Surname" onChange={e => setSurname(e.target.value)} required /><br />
                <label>BirthDate:</label><br />
                <input type="date" onChange={e => handleBirthDate(e.target.value)} required /><br />
                <label>Email:</label><br />
                <input type="email" placeholder="Email" onChange={e => setContactEmail(e.target.value)} required /><br />
                <label>Phone Number (format: 123456789):</label><br />
                <input type="tel" placeholder="Phone number" pattern="[0-9]{9}" onChange={e => setContactPhoneNum(e.target.value)} required /><br />
                <label>xAddress:</label><br />
                <AddressAutocomplete childToParent={childToParent} />
                <input id="student" type="checkbox" onChange={e => setIsStudent(e.target.checked)} />
                <label htmlFor="student">a student</label><br />
                <input id="public" type="checkbox" onChange={e => setIsPublicProfile(e.target.checked)} />
                <label htmlFor="public">public profile</label><br />
                <label>Description:</label><br />
                <textarea className="description" placeholder="Description" onChange={e => setDescription(e.target.value)} /><br /> 
                <h3>Characteristics</h3>
                {characteristics.map((char, index) => {
                    // Find the selected characteristic to determine if quality should be selectable
                    const selectedCharacteristic = allCharacteristics.find(
                        (item) => item.characteristic.id.toString() === char.characteristicId
                    );

                    const isQualitySelectable = selectedCharacteristic?.characteristicType.id === 6;

                    return (
                        <div key={index} style={{ marginBottom: "10px" }}>
                            <input
                                type="text"
                                list={`characteristics-${index}`}
                                placeholder="Characteristic"
                                onChange={(e) =>
                                    handleCharacteristicChange(index, "characteristicId", e.target.value)
                                }
                                value={
                                    selectedCharacteristic
                                    ? selectedCharacteristic.characteristic.name 
                                    : char.characteristicId || ""}
                            />
                            <datalist id={`characteristics-${index}`}>
                                {allCharacteristics.map((item) => (
                                    <option key={item.characteristic.id} value={item.characteristic.id}>
                                        {item.characteristic.name}
                                    </option>
                                ))}
                            </datalist>

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
                            <button type="button" onClick={() => removeCharacteristic(index)}>
                                Remove
                            </button>
                        </div>
                    );
                })}
                <button type="button" onClick={addCharacteristic}>
                    Add Characteristic
                </button>
                <br />
                <br />

                <input type="submit" value="Create Profile" />
                <CancelButton/>
            </form>
            {message && <p style={{ color: message.includes("successfully") ? "green" : "red" }}>{message}</p>}
        </div>
    )
}

export default CreateProfile;