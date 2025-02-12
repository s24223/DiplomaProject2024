import React, { useState } from "react";
import { fetchResetPassword } from "../../services/ResetPasswordService/ResetPasswordService";

const ResetPasswordPage = () => {
  const [message, setMessage] = useState('')
  const [newPassword, setNewPassword] = useState('')
  const pathname = window.location.href.split('/')
  const secretString = pathname[pathname.length - 1]
  const userId = pathname[pathname.length - 2]

  const handleSubmit = async (event) => {
    event.preventDefault()
    const response = await fetchResetPassword(userId, secretString, newPassword)
    if (response) {
      setMessage('Password reseted')
    }
    else {
      setMessage('Failed to reset password')
    }
  }

  return (
    <div className="centered">
      <h1>Password reset</h1>
      {message && message}
      <form onSubmit={handleSubmit}>
        <input type="password" onChange={e => setNewPassword(e.target.value)} placeholder="New password" required /><br />
        <input type="submit" value="Reset" />
      </form>
    </div>
  )
}

export default ResetPasswordPage