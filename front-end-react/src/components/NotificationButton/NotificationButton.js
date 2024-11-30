import React from 'react'

const NotificationButton = () => {
    const handleClick = () => {
        window.location.href = "/notification"
    }

    return(
        <div>
            <button onClick={handleClick} style={{ position: 'absolute', top: 36, right: 10 }}>Notification</button>
        </div>
    )
}

export default NotificationButton