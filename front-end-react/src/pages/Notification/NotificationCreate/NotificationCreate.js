import React, { useState } from 'react'
import { fetchNotificationPostAuthorized, fetchNotificationPostUnAuthorized } from '../../../services/NotificationService/NotificationService'
import { useLocation } from 'react-router-dom'

const NotificationCreate = () => {
    const [userMessage, setUserMessage] = useState('')
    const [email, setEmail] = useState('')
    let body = {}

    const state = useLocation().state
    
    const handleSubmit = (event) => {
        event.preventDefault();
        
        if(state)
            if(state.elem.id)
                body.previousProblemId = state.elem.id
            if(state.elem.idAppProblem)
                body.idAppProblem = state.elem.idAppProblem
        
        body.userMessage = userMessage

        console.log(body)

        const fetchDummy = async () => {
            let response = await fetchNotificationPostAuthorized(body)
            if (response.ok)
                window.location.href = "/notification"
        }

        fetchDummy()
    }

    const handleSubmitUnAuth = (event) => {
        event.preventDefault();
        
        body.userMessage = userMessage
        body.email = email

        console.log(body)

        const fetchDummy = async () => {
            let response = await fetchNotificationPostUnAuthorized(body)
            if (response.ok)
                window.location.href = "/"
        }

        fetchDummy()
    }

    return(
        <div>
            {localStorage.getItem("jwt")?
                <form onSubmit={handleSubmit}>
                    <label htmlFor='userMessage'>Your message</label><br />
                    <input type='text' id='userMessage' onChange={e => {setUserMessage(e.target.value)}} required /><br />
                    <input type='submit' value='Create' />
                </form>
            :
                <form onSubmit={handleSubmitUnAuth}>
                    <label htmlFor='userMessage'>Your message</label><br />
                    <input type='text' id='userMessage' onChange={e => setUserMessage(e.target.value)} required /><br />
                    <label htmlFor='emailInp'>Email:</label><br />
                    <input type='email' id='emailInp' onChange={e => setEmail(e.target.value)} required /><br />
                    <input type='submit' value='Create' />
                </form>
            }
        </div>
    )
}

export default NotificationCreate