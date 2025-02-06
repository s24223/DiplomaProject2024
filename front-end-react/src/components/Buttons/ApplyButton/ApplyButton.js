import React, { useState } from 'react';
import { fetchApply } from '../../../services/ApplyService/ApplyService';

const ApplyButton = ({ branchId }) => {
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [personMessage, setPersonMessage] = useState('');

    // Otwórz lub zamknij modal
    const toggleModal = () => {
        const authToken = localStorage.getItem("jwt");

        if (!authToken) {
            // Zapisanie bieżącej ścieżki do przekierowania po zalogowaniu
            alert("Log in to apply.");
            window.location.href = "/login" // Przekierowanie do logowania
            return;
        }

        setIsModalOpen(!isModalOpen);

    };

    const handleApply = async () => {
        try {
            const authToken = localStorage.getItem("jwt");
            if (!authToken) {
                // Zapisz bieżącą ścieżkę przed przekierowaniem na stronę logowania
                alert("Missing authorization token. Please log in to apply.");
                window.location.href = "/login"
                return;
            }

            const url = `https://localhost:7166/api/User/recruitment?branchOfferId=${branchId}`;

            let formData = new FormData()
            formData.append("PersonMessage", personMessage)
            formData.append("File", document.getElementById("file").files[0])

            let response = await fetchApply({ url, formData });
            if(response.error){
                throw new Error("Failed to apply for the offer")
            }
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
                            placeholder="Enter your message..."
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
