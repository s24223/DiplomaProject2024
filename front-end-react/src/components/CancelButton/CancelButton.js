import React from 'react';
import { useNavigate } from "react-router-dom";


const CancelButton = ({returnPath}) => {
    const navigate = useNavigate();
    const handleCancel = () => {
        if (returnPath) {
            navigate(returnPath); // Przejdź do konkretnej ścieżki
        } else {
            navigate(-1); // Cofnij się do poprzedniej strony
        }
    };

    return (
        <button onClick={handleCancel} style={{ margin: "10px", backgroundColor: "#ccc" }}>
            Cancel
        </button>
    );
};

export default CancelButton;