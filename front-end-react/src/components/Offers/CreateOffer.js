import React, { useState, useEffect } from "react";
import { assignOfferToBranch, createOffer } from "../../services/OffersService/OffersService";
import { fetchCharacteristics } from "../../services/CharacteristicsService/CharacteristicsService";

const CreateOffer = ({ branchId, onClose }) => {
    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [minSalary, setMinSalary] = useState("");
    const [maxSalary, setMaxSalary] = useState("");
    const [isNegotiatedSalary, setIsNegotiatedSalary] = useState(false);
    const [isForStudents, setIsForStudents] = useState(false);
    const [publishStart, setPublishStart] = useState("");
    const [publishEnd, setPublishEnd] = useState("");
    const [workStart, setWorkStart] = useState({ year: null, month: null, day: null });
    const [workEnd, setWorkEnd] = useState({ year: null, month: null, day: null });
    const [message, setMessage] = useState("");

    const [characteristics, setCharacteristics] = useState([])
    const [allCharacteristics, setAllCharacteristics] = useState([]);


    const [creationStartTime, setCreationStartTime] = useState(new Date());

    // Ustaw czas rozpoczęcia po pierwszym renderze
    useEffect(() => {
        setCreationStartTime(new Date());
    }, []);

    const validateDates = () => {
        const now = new Date();
        const publishStartDate = new Date(publishStart);
        const publishEndDate = new Date(publishEnd);
        const workStartDate = new Date(`${workStart.year}-${workStart.month}-${workStart.day}`);
        const workEndDate = new Date(`${workEnd.year}-${workEnd.month}-${workEnd.day}`);
    
        if (publishStartDate < new Date(creationStartTime.getTime() + 5 * 60 * 1000)) {
            setMessage("Publish Start must be at least 5 minutes from now.");
            return false;
        }
        if (publishEndDate <= publishStartDate) {
            setMessage("Publish End must be after Publish Start.");
            return false;
        }
        if (workStartDate <= new Date(publishEndDate.getTime() + 24 * 60 * 60 * 1000)) {
            setMessage("Work Start must be at least 1 day after Publish End.");
            return false;
        }
        if (workEndDate <= workStartDate) {
            setMessage("Work End must be after Work Start.");
            return false;
        }
    
        return true;
    };
    

    const handleWorkStart = (date) => {
        const dateSegmented = date.split("-");
        setWorkStart({
            year: dateSegmented[0],
            month: dateSegmented[1],
            day: dateSegmented[2],
        });
    };

    const handleWorkEnd = (date) => {
        const dateSegmented = date.split("-");
        setWorkEnd({
            year: dateSegmented[0],
            month: dateSegmented[1],
            day: dateSegmented[2],
        });
    };



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
        

    const handleSubmit = async (e) => {
        e.preventDefault();

        if(!minSalary){
            minSalary=0;
        }
        if(!maxSalary){
            maxSalary=0;
        }
        if (!name ||  !publishStart || !publishEnd) {//!minSalary || !maxSalary ||
            alert("All fields are required.");
            return;
        }
        if (!validateDates()) {
            return;
        }
    

        // Preprocess characteristics
        const processedCharacteristics = characteristics.map((char) => {
            const processedCharacteristics = allCharacteristics.find(
                (item) => item.characteristic.id.toString() === char.characteristicId.toString()
            );
            if ([1, 2, 3, 4].includes(processedCharacteristics?.characteristicType.id)) {
                return { ...char, qualityId: null };
            }

           
            return char; // Keep the original characteristic if type is "Języki komunikacji"
        }).filter((char) => char !== null);;




        const offerData = [
            {
                name,
                description,
                minSalary: parseFloat(minSalary),
                maxSalary: parseFloat(maxSalary),
                isNegotiatedSalary,
                isForStudents,
                characteristics: processedCharacteristics,
            },
        ];

        try {
            // Tworzenie oferty
            const createdOffer=await createOffer(offerData);

                // Przypisanie oferty do oddziału
                const publishData = [
                    {
                        branchId,
                        offerId: createdOffer.id,
                        publishStart,
                        publishEnd,
                        workStart,
                        workEnd,
                    },
                ];

                await assignOfferToBranch(publishData);

                setMessage("Offer created and added to branch successfully!");
                setTimeout(() => {
                    onClose();
                }, 2000);
            
        } catch (error) {
            
            setMessage(error);
        }
    };

    if (loading) {
        return <p>Loading characteristics...</p>;
    }

    return (
        <div>
            <h3>Create Offer</h3>
            <form onSubmit={handleSubmit}>
                <label>Name:</label>
                <input
                    type="text"
                    value={name}
                    onChange={(e) => setName(e.target.value)}
                    placeholder="Offer name"
                    required
                />
                <br />
                <label>Description:</label>
                <textarea
                    value={description}
                    onChange={(e) => setDescription(e.target.value)}
                    placeholder="Offer description"
                    required
                ></textarea>
                <br />
                <label>Min Salary:</label>
                <input
                    type="number"
                    value={minSalary}
                    onChange={(e) => setMinSalary(e.target.value)}
                    placeholder="Minimum salary"
                    required
                />
                <br />
                <label>Max Salary:</label>
                <input
                    type="number"
                    value={maxSalary}
                    onChange={(e) => setMaxSalary(e.target.value)}
                    placeholder="Maximum salary"
                    required
                />
                <br />
                <label>
                    <input
                        type="checkbox"
                        checked={isNegotiatedSalary}
                        onChange={(e) => setIsNegotiatedSalary(e.target.checked)}
                    />
                    Negotiable Salary
                </label>
                <br />
                <label>
                    <input
                        type="checkbox"
                        checked={isForStudents}
                        onChange={(e) => setIsForStudents(e.target.checked)}
                    />
                    For Students
                </label>
                <br />
                <label>Publish Start:</label>
                <input
                    type="datetime-local"
                    value={publishStart}
                    onChange={(e) => setPublishStart(e.target.value)}
                    required
                />
                <p style={{ color: "red" }}>
                    {publishStart && new Date(publishStart) < new Date(creationStartTime.getTime() + 5 * 60 * 1000) 
                        ? "Publish Start must be at least 5 minutes from now." 
                        : ""}
                </p>
                <br />
                <label>Publish End:</label>
                <input
                    type="datetime-local"
                    value={publishEnd}
                    onChange={(e) => setPublishEnd(e.target.value)}
                    required
                />
                <p style={{ color: "red" }}>
                    {publishEnd && new Date(publishEnd) <= new Date(publishStart) 
                        ? "Publish End must be after Publish Start." 
                        : ""}
                </p>
                <br />
                <label>Work Start:</label>
                <input
                    type="date"
                    onChange={(e) => handleWorkStart(e.target.value)}
                    required
                />
                <p style={{ color: "red" }}>
                    {workStart.year &&
                    new Date(`${workStart.year}-${workStart.month}-${workStart.day}`) <=
                        new Date(new Date(publishEnd).getTime() + 24 * 60 * 60 * 1000)
                        ? "Work Start must be at least 1 full day after Publish End."
                        : ""}
                </p>
                <br />
                <label>Work End:</label>
                <input
                    type="date"
                    onChange={(e) => handleWorkEnd(e.target.value)}
                    required
                />
                <p style={{ color: "red" }}>
                    {workEnd.year &&
                    new Date(`${workEnd.year}-${workEnd.month}-${workEnd.day}`) <=
                        new Date(`${workStart.year}-${workStart.month}-${workStart.day}`)
                        ? "Work End must be after Work Start."
                        : ""}
                </p>
                
                <h3>Characteristics</h3>
                {characteristics.map((char, index) => {
                    // Find the selected characteristic to determine if quality should be selectable
                    const selectedCharacteristic = allCharacteristics.find(
                        (item) => item.characteristic.id.toString() === char.characteristicId
                    );

                    const isQualitySelectable = ![1, 2, 3, 4].includes(selectedCharacteristic?.characteristicType.id);
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
                                {allCharacteristics
                                .map((item) => (
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
                                disabled={!isQualitySelectable}
                            >
                                <option value="">Select Quality</option>
                                {selectedCharacteristic?.possibleQualities.map((qual) => (
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
                <br />
                <button type="submit">Create Offer</button>
                <button type="button" onClick={onClose}>
                    Cancel
                </button>
            </form>
            {message && <p style={{ color: message.includes("successfully") ? "green" : "red" }}>{message}</p>}
        </div>
    );
};

export default CreateOffer;