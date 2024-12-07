import React, { useState } from "react";
import { assignOfferToBranch, createOffer } from "../../services/OffersService/OffersService";

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

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!name || !description || !minSalary || !maxSalary || !publishStart || !publishEnd) {
            alert("All fields are required.");
            return;
        }

        const offerData = [
            {
                name,
                description,
                minSalary: parseFloat(minSalary),
                maxSalary: parseFloat(maxSalary),
                isNegotiatedSalary,
                isForStudents,
                characteristics: [
                    {
                        characteristicId: 1,
                        qualityId: 11,
                    },
                ],
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
                <br />
                <label>Publish End:</label>
                <input
                    type="datetime-local"
                    value={publishEnd}
                    onChange={(e) => setPublishEnd(e.target.value)}
                    required
                />
                <br />
                <label>Work Start:</label>
                <input
                    type="date"
                    onChange={(e) => handleWorkStart(e.target.value)}
                    required
                />
                <br />
                <label>Work End:</label>
                <input
                    type="date"
                    onChange={(e) => handleWorkEnd(e.target.value)}
                    required
                />
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
