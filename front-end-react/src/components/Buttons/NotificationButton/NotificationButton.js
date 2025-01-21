import React from 'react'

const NotificationButton = () => {
    const handleClick = () => {
        if(!localStorage.getItem("jwt"))
            window.location.href = '/notification/create'
        else
            window.location.href = "/notification"
    }

    return(
        <div className='notification-div'>
            <button onClick={handleClick}>Notification</button>
        </div>
    )
}

export default NotificationButton