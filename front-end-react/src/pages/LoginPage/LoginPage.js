import React, {useState} from 'react'
import { fetchLogin } from '../../services/LoginService/LoginService';

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
            localStorage.setItem("jwtValidTo", response.jwtValidTo)
            localStorage.setItem("refreshToken", response.refereshToken)
            localStorage.setItem("refreshTokenValidTo", response.refereshTokenValidTo)
            window.history.back();
        } catch(error){
            console.log(error)
            //TODO: check error message and setmessage representive
            setErrorMsg('error')
        }
    }

    return(
        <div className='centered'>

            {errorMsg && errorMsg}
            <label className='title-text'>Log in</label>
            <form onSubmit={handleLogin}>
                <label>Email:</label><br />
                <input type="email" id="email" name="email" placeholder='Email' onChange={e => setEmail(e.target.value)} /><br />
                <label>Password:</label><br />
                <input type="password" id="password" name="password" placeholder='Password' onChange={e => setPassword(e.target.value)} /><br />
                <input type="submit" value="Log in" />
            </form>
        </div>
    )
}

export default LoginPage