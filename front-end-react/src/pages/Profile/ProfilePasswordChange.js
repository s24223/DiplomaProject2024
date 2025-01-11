import React, { useState, useEffect } from 'react'

const ProfileChnagePassword = () => {
    const [oldPassword, setOldPassword] = useState();
    const [newPassword, setNewPassword] = useState();
    const [newPasswordRepeat, setNewPasswordRepeat] = useState()
    const [message, setMessage] = useState('')

    useEffect(() => {
        if (!sessionStorage.getItem("jwt")){
            window.location.href='/'
        }
    }, [])

    const handleSubmit = (e) => {
        e.preventDefault()
        //check if old password match
        if(newPassword != newPasswordRepeat){
            setMessage('New passwords doesn\'t match')
            return
        }
        else{
            setMessage('')
        }

        const fetchDummy = async () => {
            let response = await fetch('https://localhost:7166/api/User/password', {
                method: 'PUT',
                headers:{
                    'Content-Type': 'application/json',
                    'Access-Control-Allow-Origin': '*',
                    Authorization: `Bearer ${sessionStorage.getItem("jwt")}`
                },
                body: JSON.stringify({newPassword:newPassword})
            })
            if(response.ok){
                window.location.href='/userProfile'
            }
        }

        fetchDummy()
    }

    return(
        <div className='centered'>
            <h1>Change password</h1>
            {message && <p style={{color:'red'}}>{message}</p>}
            <form onSubmit={handleSubmit}>
                <label>Old password:</label><br />
                <input type='password' onChange={e => setOldPassword(e.target.value)} required /><br />
                <label>New password:</label><br />
                <input type='password' onChange={e => setNewPassword(e.target.value)} required /><br />
                <label>Repeat new password:</label><br />
                <input type='password' onChange={e => setNewPasswordRepeat(e.target.value)} required /><br />
                <input type='submit' value="Change" />
            </form>
        </div>
    )
}

export default ProfileChnagePassword;