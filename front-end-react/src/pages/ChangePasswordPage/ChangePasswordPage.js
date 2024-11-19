import React, { useState } from "react";
import axios from "axios";

const ChangePasswordPage = () => {
    const [newPassword, setNewPassword] = useState("");
    const [message, setMessage] = useState("");

    const handlePasswordChange = async (event) => {
        event.preventDefault();
        try {
            const response = await axios.put(
                "https://localhost:7166/api/User/password",
                { newPassword },
                {
                    headers: {
                        Authorization: `Bearer ${localStorage.getItem("jwt")}`,
                    },
                }
            );
            setMessage("Password changed successfully.");
        } catch (error) {
            console.error("Error changing password:", error);
            setMessage("Failed to change password. Please try again.");
        }
    };

    return (
        <div>
            <h1>Change Password</h1>
            <form onSubmit={handlePasswordChange}>
                <label>New Password:</label>
                <input
                    type="password"
                    value={newPassword}
                    onChange={(e) => setNewPassword(e.target.value)}
                    required
                />
                <button type="submit">Change Password</button>
            </form>
            {message && <p>{message}</p>}
        </div>
    );
};

export default ChangePasswordPage;
