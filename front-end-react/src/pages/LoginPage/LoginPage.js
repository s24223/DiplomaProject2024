import React, { useState } from 'react'
import { fetchLogin } from '../../services/LoginService/LoginService';
import { Link } from 'react-router-dom';

const LoginPage = () => {
    const [email, setEmail] = useState();
    const [password, setPassword] = useState();
    const [errorMsg, setErrorMsg] = useState();

    const handleLogin = async (event) => {
        event.preventDefault()
        try {
            const response = await fetchLogin({ email, password })

            if (response.error) {
                setErrorMsg(response.error)
                return
            }

            localStorage.setItem("jwt", response.jwt)
            localStorage.setItem("jwtValidTo", response.jwtValidTo)
            localStorage.setItem("refreshToken", response.refereshToken)
            localStorage.setItem("refreshTokenValidTo", response.refereshTokenValidTo)
            window.history.back();
        } catch (error) {
            setErrorMsg('error')
        }
    }

    return (
        <div>
            {errorMsg && <header className='error-message'>
                {errorMsg}
            </header>}
            <div className='centered'>
                <label className='title-text'>Log in</label>
                <form onSubmit={handleLogin}>
                    <label>Email:</label><br />
                    <input type="email" id="email" name="email" placeholder='Email' onChange={e => setEmail(e.target.value)} /><br />
                    <label>Password:</label><br />
                    <input type="password" id="password" name="password" placeholder='Password' onChange={e => setPassword(e.target.value)} /><br />
                    <input id='log-in-page-button' type="submit" value="Log in" />
                </form>
                
                <Link to="/reset" className="hidden-link">Forgot password </Link>
            </div>
        </div>
    )
}

export default LoginPage