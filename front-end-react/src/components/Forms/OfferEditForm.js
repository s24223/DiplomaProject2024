import React, { useState, useEffect } from "react";
import { fetchCharacteristics } from "../../services/CharacteristicsService/CharacteristicsService";
import { updateOffer } from "../../services/OffersService/OffersService";

const OfferEditForm = ({ offerDetails, onCancelEdit }) => {
    const [formData, setFormData] = useState({
        name: offerDetails.offer.name,
        description: offerDetails.offer.description,
        minSalary: offerDetails.offer.minSalary,
        maxSalary: offerDetails.offer.maxSalary,
        isForStudents: offerDetails.offer.isForStudents,
        isNegotiatedSalary: offerDetails.offer.isNegotiatedSalary,
        characteristics: offerDetails.offer.characteristics.map((char) => ({
            characteristicId: char.characteristic.id,
            qualityId: char.quality?.id || "",
        })),
    });

    const [allCharacteristics, setAllCharacteristics] = useState([]);
    const [loading, setLoading] = useState(true);
    const [message, setMessage] = useState("");

    useEffect(() => {
        const loadCharacteristics = async () => {
            try {
                const characteristics = await fetchCharacteristics();
                setAllCharacteristics(characteristics);
            } catch (error) {
                console.error("Error fetching characteristics:", error);
                setMessage("Failed to load characteristics.");
            } finally {
                setLoading(false);
            }
        };

        loadCharacteristics();
    }, []);

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData((prevData) => ({ ...prevData, [name]: value }));
    };

    const handleCharacteristicChange = (index, field, value) => {
        setFormData((prevData) => {
            const updatedCharacteristics = [...prevData.characteristics];
            updatedCharacteristics[index] = { ...updatedCharacteristics[index], [field]: value };
            return { ...prevData, characteristics: updatedCharacteristics };
        });
    };

    const addCharacteristic = () => {
        setFormData((prevData) => ({
            ...prevData,
            characteristics: [...prevData.characteristics, { characteristicId: "", qualityId: "" }],
        }));
    };

    const removeCharacteristic = (index) => {
        setFormData((prevData) => ({
            ...prevData,
            characteristics: prevData.characteristics.filter((_, i) => i !== index),
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        const processedCharacteristics = formData.characteristics
            .map((char) => {
                const matchedCharacteristic = allCharacteristics.find(
                    (item) => item.characteristic.id.toString() === char.characteristicId.toString()
                );
                if (!matchedCharacteristic) return null;

                // Dla typów nie wymagających jakości
                if ([1, 2, 3, 4].includes(matchedCharacteristic.characteristicType.id)) {
                    return { ...char, qualityId: null };
                }

                return char;
            })
            .filter((char) => char !== null);

        try {
            await updateOffer({
                offerId: offerDetails.offer.id,
                name: formData.name,
                description: formData.description,
                minSalary: parseFloat(formData.minSalary),
                maxSalary: parseFloat(formData.maxSalary),
                isForStudents: formData.isForStudents,
                isNegotiatedSalary: formData.isNegotiatedSalary,
                characteristics: processedCharacteristics,
            });
            alert("Offer updated successfully!");
            onCancelEdit();
        } catch (error) {
            console.error("Error updating offer:", error);
            setMessage("Failed to update the offer. Please try again.");
        }
    };

    if (loading) return <p>Loading characteristics...</p>;

    return (
        <form onSubmit={handleSubmit}>
            <label>
                Name:<br />
                <input
                    type="text"
                    name="name"
                    value={formData.name}
                    onChange={handleInputChange}
                />
            </label>
            <br />
            <label>
                Description:<br />
                <textarea
                    name="description"
                    value={formData.description}
                    onChange={handleInputChange}
                />
            </label>
            <br />
            <label>
                Min Salary:<br />
                <input
                    type="number"
                    name="minSalary"
                    value={formData.minSalary}
                    onChange={handleInputChange}
                />
            </label>
            <br />
            <label>
                Max Salary:<br />
                <input
                    type="number"
                    name="maxSalary"
                    value={formData.maxSalary}
                    onChange={handleInputChange}
                />
            </label>
            <br />
            <label>
                <input
                    type="checkbox"
                    name="isForStudents"
                    checked={formData.isForStudents}
                    onChange={(e) =>
                        setFormData((prevData) => ({ ...prevData, isForStudents: e.target.checked }))
                    }
                />
                For Students
            </label>
            <br />
            <label>
                <input
                    type="checkbox"
                    name="isNegotiatedSalary"
                    checked={formData.isNegotiatedSalary}
                    onChange={(e) =>
                        setFormData((prevData) => ({
                            ...prevData,
                            isNegotiatedSalary: e.target.checked,
                        }))
                    }
                />
                Negotiated Salary
            </label>
            <br />
            <h3>Characteristics</h3>
            {formData.characteristics.map((char, index) => {
                const selectedCharacteristic = allCharacteristics.find(
                    (item) => item.characteristic.id.toString() === char.characteristicId
                );


                const isQualitySelectable = ![1, 2, 3, 4].includes(selectedCharacteristic?.characteristicType.id);

                return (
                    <div key={index}>
                        <input
                            type="text"
                            list={`characteristics-${index}`}
                            value={selectedCharacteristic?.characteristic?.name ||
                                allCharacteristics.find(item => item.characteristic.id === char.characteristicId)?.characteristic.name || ""
                        }
                        placeholder="Characteristic"
                        onChange={(e) =>
                            handleCharacteristicChange(index, "characteristicId", e.target.value)
                            }
                        />
                        <datalist id={`characteristics-${index}`}>
                            {allCharacteristics.map((item) => (
                                <option
                                    key={item.characteristic.id}
                                    value={item.characteristic.id}
                                >
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
            {message && <p style={{ color: "red" }}>{message}</p>}
        </form>
    );
};

export default OfferEditForm;
