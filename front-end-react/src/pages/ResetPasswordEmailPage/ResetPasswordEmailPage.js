import React, { useState } from "react";
import { fetchResetPasswordEmail } from "../../services/ResetPasswordService/ResetPasswordService";

const ResetPasswordEmailPage = () => {
  const [email, setEmail] = useState('')
  const [message, setMessage] = useState('')

  const handleSubmit = async (event) => {
    event.preventDefault()
    const response = await fetchResetPasswordEmail(email)
    if (response){
      setMessage('Reset link sent to your email')
    }
    else{
      setMessage('Failed to find your account')
    }
  }

  return(
    <div className="centered">
      <h1>Reset password</h1>
      {message && message}
      <form onSubmit={handleSubmit}>
        <input type="email" placeholder="Email" onChange={e => setEmail(e.target.value)} required /><br />
        <input type="submit" value="Reset" />
      </form>
    </div>
  )
}

export default ResetPasswordEmailPage