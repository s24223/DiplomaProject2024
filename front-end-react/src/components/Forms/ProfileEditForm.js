import React, { useState } from "react";
import CancelButton from "../Buttons/CancelButton/CancelButton";
import AddressAutocomplete from "../AddressAutoComplete/AddressAutoComplete";

const ProfileEditForm = ({ profileData, allCharacteristics, onProfileUpdate }) => {
    const [formState, setFormState] = useState(profileData);
    const [message, setMessage] = useState("");

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        if (name === "birthDate") {
            const age = calculateAge(value);
            if (age < 18) {
                setMessage("You must be at least 18 years old.");
                return;
            } else {
                setMessage("");
            }
        }

        setFormState((prev) => ({ ...prev, [name]: value }));
    };

    const handleCharacteristicChange = (index, field, value) => {
        setFormState((prev) => {
            const updatedCharacteristics = [...prev.characteristics];
            updatedCharacteristics[index] = { ...updatedCharacteristics[index], [field]: value };
            return { ...prev, characteristics: updatedCharacteristics };
        });
    };

    const addCharacteristic = () => {
        setFormState((prev) => ({
            ...prev,
            characteristics: [...prev.characteristics, { characteristicId: "", qualityId: "" }],
        }));
    };

    const removeCharacteristic = (index) => {
        setFormState((prev) => ({
            ...prev,
            characteristics: prev.characteristics.filter((_, i) => i !== index),
        }));
    };

    const calculateAge = (birthDate) => {
        const today = new Date();
        const birthDateObj = new Date(birthDate);
        let age = today.getFullYear() - birthDateObj.getFullYear();
        const monthDiff = today.getMonth() - birthDateObj.getMonth();
        if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDateObj.getDate())) {
            age--;
        }
        return age;
    };

    const handleSubmit = (e) => {
        e.preventDefault();

        // Przygotowanie danych do aktualizacji
        const updatedCharacteristics = formState.characteristics
            .map((char) => {
                const matchedCharacteristic = allCharacteristics.find(
                    (item) => item.characteristic.id.toString() === char.characteristicId.toString()
                );
                if ([1, 2, 3, 4, 9].includes(matchedCharacteristic?.characteristicType.id)) {
                    return null;
                }

                if (matchedCharacteristic?.characteristicType.id !== 6) {
                    return { ...char, qualityId: null };
                }

                return char;
            })
            .filter((char) => char !== null);

        const [year, month, day] = formState.birthDate.split("-").map(Number);
        const updatedData = {
            ...formState,
            birthDate: { year, month, day },
            characteristics: updatedCharacteristics,
        };

        onProfileUpdate(updatedData);
    };

    return (
        <form onSubmit={handleSubmit}>
            <label>Name:</label><br />
            <input
                type="text"
                name="name"
                value={formState.name}
                onChange={handleInputChange}
                required
            /><br />
            <label>Surname:</label><br />
            <input
                type="text"
                name="surname"
                value={formState.surname}
                onChange={handleInputChange}
                required
            /><br />
            <label>Email:</label><br />
            <input
                type="email"
                name="contactEmail"
                value={formState.contactEmail}
                onChange={handleInputChange}
                required
            /><br />
            <label>Phone Number:</label><br />
            <input
                type="tel"
                name="contactPhoneNum"
                value={formState.contactPhoneNum}
                onChange={handleInputChange}
            /><br />
            <label>Birth Date:</label><br />
            <input
                type="date"
                name="birthDate"
                value={formState.birthDate}
                onChange={handleInputChange}
                required
            /><br />
            {message && <p style={{ color: "red" }}>{message}</p>}
            <label>Description:</label><br />
            <textarea
                name="description"
                value={formState.description}
                onChange={handleInputChange}
            ></textarea><br />
            <label>Address:</label><br />
            <AddressAutocomplete
                childToParent={(addressId) => setFormState((prev) => ({ ...prev, addressId }))}
                defaultValue={formState.addressId}
            /><br />
            <label>
                <input
                    type="checkbox"
                    name="isStudent"
                    checked={formState.isStudent}
                    onChange={(e) =>
                        setFormState((prev) => ({ ...prev, isStudent: e.target.checked }))
                    }
                />
                Student
            </label><br />
            <label>
                <input
                    type="checkbox"
                    name="isPublicProfile"
                    checked={formState.isPublicProfile}
                    onChange={(e) =>
                        setFormState((prev) => ({ ...prev, isPublicProfile: e.target.checked }))
                    }
                />
                Public Profile
            </label><br />

            <h3>Characteristics</h3>
            {formState.characteristics.map((char, index) => {
                const selectedCharacteristic = allCharacteristics.find(
                    (item) => item.characteristic.id.toString() === char.characteristicId.toString()
                );
                const isQualitySelectable = selectedCharacteristic?.characteristicType.id === 6;

                return (
                    <div key={index}>
                        <input
                            type="text"
                            list={`characteristics-${index}`}
                            value={
                                selectedCharacteristic
                                    ? selectedCharacteristic.characteristic.name
                                    : char.characteristicId || ""
                            }
                           
                            placeholder="Characteristic"
                            onChange={(e) =>
                                handleCharacteristicChange(index, "characteristicId", e.target.value)
                            }
                        />
                        <datalist id={`characteristics-${index}`}>
                            {allCharacteristics
                                .filter(
                                    (item) =>
                                        ![1, 2, 3, 4, 9].includes(item.characteristicType.id)
                                )
                                .map((item) => (
                                    <option key={item.characteristic.id} value={item.characteristic.id}>
                                        {item.characteristic.name}
                                    </option>
                                ))}
                        </datalist>
                        <select
                            value={char.qualityId || ""}
                            disabled={!isQualitySelectable}
                            onChange={(e) =>
                                handleCharacteristicChange(index, "qualityId", e.target.value)
                            }
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
            <button type="submit">Save Changes</button>
            <CancelButton />
        </form>
    );
};

export default ProfileEditForm;
