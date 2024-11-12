import React from 'react';
// import './ApplyButton.css';

const ApplyButton = ({ branchId }) => {
    const handleApply = async () => {
        try {
            const url = `https://localhost:7166/api/Internship/recruitment?branchOfferId=${branchId}`;
            const response = await fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Access-Control-Allow-Origin': '*',
                },
                //body: JSON.stringify({ branchOfferId: branchId }), // Przesyłanie branchId jako branchOfferId
                credentials: 'include', // Zapewnia przesyłanie informacji o sesji zalogowanego użytkownika
            });

            if (!response.ok) throw new Error("Failed to apply for the offer");
            const result = await response.json();
            console.log("Successfully applied:", result);
            alert("Application successful!");
        } catch (error) {
            console.error("Error applying:", error);
            alert("Application failed. Please try again. "+branchId);
        }
    };

    return (
        <button onClick={handleApply} className="apply-button">
            Apply
        </button>
    );
};

export default ApplyButton;

// import React from 'react';

// const ApplyButton = ({ branchId, authToken }) => {
//     const handleApply = async () => {
//         try {
//             // Tworzenie URL z parametrem branchOfferId
//             const url = `https://localhost:7166/api/Internship/recruitment?branchOfferId=${branchId}`;

//             const response = await fetch(url, {
//                 method: 'POST',
//                 headers: {
//                     'Content-Type': 'application/json',
//                     'Authorization': `Bearer ${authToken}`, // Dodanie nagłówka Authorization
//                     'Access-Control-Allow-Origin': '*',
//                 },
//                 credentials: 'include',
//                 body: JSON.stringify({
//                     personMessage: "string", // Dodanie body, jeśli wymagane
//                 }),
//             });

//             if (!response.ok) throw new Error("Failed to apply for the offer");
//             const result = await response.json();
//             console.log("Successfully applied:", result);
//             alert("Application successful!");
//         } catch (error) {
//             console.error("Error applying:", error);
//             alert("Application failed. Please try again.");
//         }
//     };

//     return (
//         <button onClick={handleApply} className="apply-button">
//             Apply
//         </button>
//     );
// };

// export default ApplyButton;
