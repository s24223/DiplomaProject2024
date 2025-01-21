import React, {useState} from 'react'
import { fetchRegistration } from '../../services/RegisterService/RegisterService';
import { jwtRefresh } from '../../services/JwtRefreshService/JwtRefreshService';

const RegisterPage = () => {
    jwtRefresh();
    
    const [email, setEmail] = useState();
    const [password, setPassword] = useState();
    const [passwordRepeat, setPasswordRepeat] = useState();
    const [passwordError, setPasswordError] = useState('');
    const [errorMsg, setErrorMsg] = useState();

    const handleRegistration = async (event) => {
        event.preventDefault()

        if (password !== passwordRepeat){
            setPasswordError('Passwords do not match')
            return
        }
        else{
            setPasswordError('')
        }

        try{
            await fetchRegistration({email, password})
            window.location.href="/login"
        } catch(error) {
            console.log(error)
            setErrorMsg('error')
        }
    }

    return(
        <div className='centered'>
            {errorMsg && errorMsg}
            <label className='title-text'>Sign up</label>
            <form onSubmit={handleRegistration}>
                <label>Email:</label><br />
                <input type="email" id="email" name="email" placeholder='Email' onChange={e => setEmail(e.target.value)} /><br />
                <label>Password:</label><br />
                <input type="password" id="password" name="password" placeholder='Password' onChange={e => setPassword(e.target.value)} /><br />
                <label>Repeat password:</label><br />
                {passwordError && <>{passwordError}<br /></>}
                <input type='password' placeholder='Repeat password' onChange={e => setPasswordRepeat(e.target.value)} /><br />
                <input type="submit" value="Sign up" />
            </form>
        </div>
    )
}

export default RegisterPage