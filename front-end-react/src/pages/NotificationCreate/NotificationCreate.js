import React, { useState } from 'react'
import { fetchNotificationPostAuthorized } from '../../services/NotificationService/NotificationService'
import { useLocation } from 'react-router-dom'

const NotificationCreate = () => {
    const [userMessage, setUserMessage] = useState('')
    let body = {}

    const previd = useLocation().state.elem.id
    
    const handleSubmit = (event) => {
        event.preventDefault();
        
        if(previd)
            body.previousProblemId = previd
        
        body.userMessage = userMessage

        console.log(body)

        const fetchDummy = async () => {
            let response = await fetchNotificationPostAuthorized(body)
            if (response.ok)
                window.location.href = "/notification"
        }

        fetchDummy()

        console.log(userMessage)
    }

    return(
        <div>
            <form onSubmit={handleSubmit}>
                <label htmlFor='userMessage'>Your message</label><br />
                <input type='text' id='userMessage' onChange={e => {setUserMessage(e.target.value)}} /><br />
                <input type='submit' value='Create' />
            </form>
        </div>
    )
}

export default NotificationCreate