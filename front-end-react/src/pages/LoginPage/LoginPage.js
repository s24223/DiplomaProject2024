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
            sessionStorage.setItem("jwt", response.jwt)
            sessionStorage.setItem("jwtValidTo", response.jwtValidTo)
            sessionStorage.setItem("refreshToken", response.refereshToken)
            sessionStorage.setItem("refreshTokenValidTo", response.refereshTokenValidTo)
            const redirectPath = sessionStorage.getItem("redirectAfterLogin") || "/";
            sessionStorage.removeItem("redirectAfterLogin"); // Usuń po użyciu
            window.location.href = redirectPath;
            //window.location.href="/"
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
        <div className='centered'>

            {errorMsg && errorMsg}
            <label className='title-text'>Logowanie</label>
            <form onSubmit={handleLogin}>
                <label>Email:</label><br />
                <input type="email" id="email" name="email" placeholder='Email' onChange={e => setEmail(e.target.value)} /><br />
                <label>Password:</label><br />
                <input type="password" id="password" name="password" placeholder='Password' onChange={e => setPassword(e.target.value)} /><br />
                <input type="submit" value="Zaloguj" />
            </form>
            <button onClick={handleRegistrationButton}>Zarejestruj</button>
        </div>
    )
}

export default LoginPage