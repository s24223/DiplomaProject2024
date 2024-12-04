// import React, { useState } from "react";
// import axios from "axios";

// const CreateOffer = ({ branchId, onClose, refreshOffers }) => {
//     const [name, setName] = useState("");
//     const [description, setDescription] = useState("");
//     const [minSalary, setMinSalary] = useState("");
//     const [maxSalary, setMaxSalary] = useState("");
//     const [isNegotiatedSalary, setIsNegotiatedSalary] = useState(false);
//     const [isForStudents, setIsForStudents] = useState(false);

//     const handleSubmit = async (e) => {
//         e.preventDefault();

//         // Walidacja
//         if (!name || !description || !minSalary || !maxSalary) {
//             alert("All fields are required.");
//             return;
//         }

//         const offerData = [
//             {
//                 name,
//                 description,
//                 minSalary: parseFloat(minSalary),
//                 maxSalary: parseFloat(maxSalary),
//                 isNegotiatedSalary,
//                 isForStudents,
//                 characteristics: [
//                     {
//                         characteristicId: 1,
//                         qualityId: 11
//                     }
//                 ], 
//             },
//         ];

//         try {
//             const response = await axios.post(
//                 `https://localhost:7166/api/User/company/offers`,
//                 offerData,
//                 {
//                     headers: {
//                         Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
//                     },
//                 }
//             );

//             if (response.status === 201) {
//                 alert("Offer created successfully.");
//                 refreshOffers(); // Odśwież listę ofert
//                 onClose(); // Zamknij formularz
//             }
//         } catch (error) {
//             console.error("Error creating offer:", error);
//             alert("Failed to create offer.");
//         }
//     };

//     return (
//         <div>
//             <h3>Create Offer</h3>
//             <form onSubmit={handleSubmit}>
//                 <label>Name:</label>
//                 <input
//                     type="text"
//                     value={name}
//                     onChange={(e) => setName(e.target.value)}
//                     placeholder="Offer name"
//                     required
//                 />
//                 <br />
//                 <label>Description:</label>
//                 <textarea
//                     value={description}
//                     onChange={(e) => setDescription(e.target.value)}
//                     placeholder="Offer description"
//                     required
//                 ></textarea>
//                 <br />
//                 <label>Min Salary:</label>
//                 <input
//                     type="number"
//                     value={minSalary}
//                     onChange={(e) => setMinSalary(e.target.value)}
//                     placeholder="Minimum salary"
//                     required
//                 />
//                 <br />
//                 <label>Max Salary:</label>
//                 <input
//                     type="number"
//                     value={maxSalary}
//                     onChange={(e) => setMaxSalary(e.target.value)}
//                     placeholder="Maximum salary"
//                     required
//                 />
//                 <br />
//                 <label>
//                     <input
//                         type="checkbox"
//                         checked={isNegotiatedSalary}
//                         onChange={(e) => setIsNegotiatedSalary(e.target.checked)}
//                     />
//                     Negotiable Salary
//                 </label>
//                 <br />
//                 <label>
//                     <input
//                         type="checkbox"
//                         checked={isForStudents}
//                         onChange={(e) => setIsForStudents(e.target.checked)}
//                     />
//                     For Students
//                 </label>
//                 <br />
//                 <button type="submit">Create Offer</button>
//                 <button type="button" onClick={onClose}>
//                     Cancel
//                 </button>
//             </form>
//         </div>
//     );
// };

// export default CreateOffer;

import React, { useState } from "react";
import axios from "axios";

const CreateOffer = ({ branchId, onClose, refreshOffers }) => {
    const [name, setName] = useState("");
    const [description, setDescription] = useState("");
    const [minSalary, setMinSalary] = useState("");
    const [maxSalary, setMaxSalary] = useState("");
    const [isNegotiatedSalary, setIsNegotiatedSalary] = useState(false);
    const [isForStudents, setIsForStudents] = useState(false);
    const [publishStart, setPublishStart] = useState("");
    const [publishEnd, setPublishEnd] = useState("");
    const [workStart, setWorkStart] = useState({ year: "", month: "", day: "" });
    const [workEnd, setWorkEnd] = useState({ year: "", month: "", day: "" });

    const handleSubmit = async (e) => {
        e.preventDefault();

        // Walidacja
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
                ], // Na stałe ustawione wartości
            },
        ];

        try {
            // Tworzenie oferty
            const offerResponse = await axios.post(
                `https://localhost:7166/api/User/company/offers`,
                offerData,
                {
                    headers: {
                        Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
                    },
                }
            );

            if (offerResponse.status === 201) {
                const createdOffer = offerResponse.data.item[0]; // Pobranie pierwszej utworzonej oferty

                // Wysłanie dodatkowych informacji o publikacji i okresie pracy
                const publishData = [
                    {
                        branchId,
                        offerId: createdOffer.offerId,
                        publishStart,
                        publishEnd,
                        workStart: {
                            year: parseInt(workStart.year),
                            month: parseInt(workStart.month),
                            day: parseInt(workStart.day),
                        },
                        workEnd: {
                            year: parseInt(workEnd.year),
                            month: parseInt(workEnd.month),
                            day: parseInt(workEnd.day),
                        },
                    },
                ];

                await axios.post(
                    `https://localhost:7166/api/User/company/branches&offers`,
                    publishData,
                    {
                        headers: {
                            Authorization: `Bearer ${sessionStorage.getItem("jwt")}`,
                        },
                    }
                );

                alert("Offer created and published successfully.");
                refreshOffers(); // Odśwież listę ofert
                onClose(); // Zamknij formularz
            }
        } catch (error) {
            console.error("Error creating or publishing offer:", error);
            alert("Failed to create or publish offer.");
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
                    type="number"
                    value={workStart.year}
                    onChange={(e) => setWorkStart((prev) => ({ ...prev, year: e.target.value }))}
                    placeholder="Year"
                    required
                />
                <input
                    type="number"
                    value={workStart.month}
                    onChange={(e) => setWorkStart((prev) => ({ ...prev, month: e.target.value }))}
                    placeholder="Month"
                    required
                />
                <input
                    type="number"
                    value={workStart.day}
                    onChange={(e) => setWorkStart((prev) => ({ ...prev, day: e.target.value }))}
                    placeholder="Day"
                    required
                />
                <br />
                <label>Work End:</label>
                <input
                    type="number"
                    value={workEnd.year}
                    onChange={(e) => setWorkEnd((prev) => ({ ...prev, year: e.target.value }))}
                    placeholder="Year"
                    required
                />
                <input
                    type="number"
                    value={workEnd.month}
                    onChange={(e) => setWorkEnd((prev) => ({ ...prev, month: e.target.value }))}
                    placeholder="Month"
                    required
                />
                <input
                    type="number"
                    value={workEnd.day}
                    onChange={(e) => setWorkEnd((prev) => ({ ...prev, day: e.target.value }))}
                    placeholder="Day"
                    required
                />
                <br />
                <button type="submit">Create Offer</button>
                <button type="button" onClick={onClose}>
                    Cancel
                </button>
            </form>
        </div>
    );
};

export default CreateOffer;
