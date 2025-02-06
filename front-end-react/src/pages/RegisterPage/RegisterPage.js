import React, { useState } from 'react'
import { fetchRegistration } from '../../services/RegisterService/RegisterService';
import { jwtRefresh } from '../../services/JwtRefreshService/JwtRefreshService';

const RegisterPage = () => {
    jwtRefresh();

    const [email, setEmail] = useState();
    const [password, setPassword] = useState();
    const [passwordRepeat, setPasswordRepeat] = useState();
    const [errorMsg, setErrorMsg] = useState();

    const handleRegistration = async (event) => {
        event.preventDefault()

        if (password !== passwordRepeat) {
            setErrorMsg('Passwords do not match')
            return
        }
        else {
            setErrorMsg('')
        }

        try {
            let response = await fetchRegistration({ email, password })
            if(response.error){
                setErrorMsg(response.error)
                return
            }
            window.location.href = "/login"
        } catch (error) {
            console.log(error)
            setErrorMsg('error')
        }
    }

    return (
        <div>
            {errorMsg && <header className='error-message'>{errorMsg}</header>}
            <div className='centered'>
                <label className='title-text'>Sign up</label>
                <form onSubmit={handleRegistration}>
                    <label>Email:</label><br />
                    <input type="email" id="email" name="email" placeholder='Email' onChange={e => setEmail(e.target.value)} /><br />
                    <label>Password:</label><br />
                    <input type="password" id="password" name="password" placeholder='Password' onChange={e => setPassword(e.target.value)} /><br />
                    <label>Repeat password:</label><br />
                    <input type='password' placeholder='Repeat password' onChange={e => setPasswordRepeat(e.target.value)} /><br />
                    <input type="submit" value="Sign up" />
                </form>
            </div>
        </div>
    )
}

export default RegisterPage