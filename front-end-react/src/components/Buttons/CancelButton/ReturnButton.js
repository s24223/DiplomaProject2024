import React from 'react';
import { useNavigate } from "react-router-dom";


const ReturnButton = ({returnPath}) => {
    const navigate = useNavigate();
    const handleReturn = () => {
        if (returnPath) {
            navigate(returnPath); // Przejdź do konkretnej ścieżki
        } else {
            navigate(-1); // Cofnij się do poprzedniej strony
        }
    };

    return (
        <button onClick={handleReturn} style={{ margin: "10px", backgroundColor: "#ccc" }}>
            Return
        </button>
    );
};

export default ReturnButton;