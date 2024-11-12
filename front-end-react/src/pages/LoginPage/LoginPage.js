import React, {useState} from 'react'
import { fetchLogin } from '../../services/LoginService/LoginService';
import './LoginPage.css'

const LoginPage = () => {
    const [email, setEmail] = useState();
    const [password, setPassword] = useState();
    const [errorMsg, setErrorMsg] = useState();

    const handleLogin = async (event) => {
        event.preventDefault()
        console.log({email, password})
        try{
            const response = await fetchLogin({email, password})
            localStorage.setItem("jwt", response.jwt)
            window.location.href="/"
        } catch(error){
            console.log(error)
            //TODO: check error message and setmessage representive
            setErrorMsg('error')
        }
    }

    const handleRegistrationButton = () => {
        window.location.href="/register"
    }

    return(
        <div>
            {errorMsg && errorMsg}
            <form onSubmit={handleLogin}>
                <label>Email:</label><br />
                <input type="email" id="email" name="email" onChange={e => setEmail(e.target.value)} /><br />
                <label>Password:</label><br />
                <input type="password" id="password" name="password" onChange={e => setPassword(e.target.value)} /><br />
                <input type="submit" />
            </form>
            <button onClick={handleRegistrationButton}>Zarejestruj</button>
        </div>
    )
}

export default LoginPage