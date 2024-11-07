import React, {useState} from 'react'
import { fetchRegistration } from '../../services/RegisterService/RegisterService';
import './RegisterPage.css'

const RegisterPage = () => {
    const [email, setEmail] = useState();
    const [password, setPassword] = useState();
    const [errorMsg, setErrorMsg] = useState();

    const handleRegistration = async (event) => {
        event.preventDefault()
        try{
            await fetchRegistration({email, password})
            window.location.href="/login"
        } catch(error) {
            console.log(error)
            setErrorMsg('error')
        }
    }

    const handleLoginButton = () => {
        window.location.href="/login"
    }

    return(
        <div>
            {errorMsg && errorMsg}
            <form onSubmit={handleRegistration}>
                <label>Email:</label><br />
                <input type="email" id="email" name="email" onChange={e => setEmail(e.target.value)} /><br />
                <label>Password:</label><br />
                <input type="password" id="password" name="password" minlength="8" onChange={e => setPassword(e.target.value)} /><br />
                <input type="submit" />
            </form>
            <button onClick={handleLoginButton}>Login</button>
        </div>
    )
}

export default RegisterPage