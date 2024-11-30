import React from 'react'

const NotificationButton = () => {
    const handleClick = () => {
        window.location.href = "/notification"
    }

    return(
        <div className='notification-div'>
            <button onClick={handleClick}>Notification</button>
        </div>
    )
}

export default NotificationButton