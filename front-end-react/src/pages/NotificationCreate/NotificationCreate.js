import React, { useState } from 'react'
import { fetchNotificationPostAuthorized } from '../../services/NotificationService/NotificationService'

const NotificationCreate = () => {
    const [userMessage, setUserMessage] = useState('')

    const handleSubmit = (event) => {
        event.preventDefault();
        const fetchDummy = async () => {
            let response = await fetchNotificationPostAuthorized({ userMessage: userMessage })
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