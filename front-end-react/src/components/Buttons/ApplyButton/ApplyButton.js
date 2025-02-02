import React, { useState } from 'react';
import { useNavigate } from "react-router-dom";

const ApplyButton = ({ branchId }) => {
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [personMessage, setPersonMessage] = useState('');
    const [file, setFile] = useState();
    const navigate = useNavigate(); 

    // Otwórz lub zamknij modal
    const toggleModal = () => {
        const authToken = localStorage.getItem("jwt");

        if (!authToken) {
            // Zapisanie bieżącej ścieżki do przekierowania po zalogowaniu
            localStorage.setItem("redirectAfterLogin", window.location.pathname);
            alert("Log in to apply.");
            navigate("/login"); // Przekierowanie do logowania
            return;
        }
        
        setIsModalOpen(!isModalOpen);
    
    };

    const handleApply = async () => {
        console.log(file)
        try {
            const authToken = localStorage.getItem("jwt");
            if (!authToken) {
                // Zapisz bieżącą ścieżkę przed przekierowaniem na stronę logowania
                localStorage.setItem("redirectAfterLogin", window.location.pathname);
                alert("Brak tokenu uwierzytelniającego. Zaloguj się, aby aplikować.");
                navigate("/login");
                return;
            }

            const url = `https://localhost:7166/api/User/recruitment?branchOfferId=${branchId}`;

            let formData = new FormData()
            formData.append("PersonMessage", personMessage)
            formData.append("File", document.getElementById("file").files[0])

            const response = await fetch(url, {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${authToken}`,
                    'Access-Control-Allow-Origin': '*',
                },
                credentials: 'include',
                body: formData, // Wysyłanie wiadomości wpisanej przez użytkownika
            });

            if (!response.ok) throw new Error("Failed to apply for the offer");
            const result = await response.json();
            console.log("Successfully applied:", result);
            alert("Application successful!");

            // Zamknij modal po pomyślnym przesłaniu wiadomości
            toggleModal();
        } catch (error) {
            console.error("Error applying:", error);
            alert("Application failed. Please try again.");
        }
    };

    return (
        <div>
            <button onClick={toggleModal} className="apply-button">
                Apply
            </button>

            {isModalOpen && (
                <div className="modal-overlay">
                    <div className="modal-content">
                        <h2>Enter Your Message</h2>
                        <label>CV:</label>
                        <input id='file' type='file' accept='application/pdf' /><br />
                        <textarea
                            value={personMessage}
                            onChange={(e) => setPersonMessage(e.target.value)}
                            placeholder="Wpisz swoją wiadomość..."
                            rows="4"
                            cols="50"
                        />
                        <button onClick={handleApply} className="send-button">
                            Send
                        </button>
                        <button onClick={toggleModal} className="cancel-button">
                            Cancel
                        </button>
                    </div>
                </div>
            )}
        </div>
    );
};

export default ApplyButton;
