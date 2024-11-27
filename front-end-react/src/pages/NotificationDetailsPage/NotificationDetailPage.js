import React from 'react'
import { useLocation } from 'react-router-dom'

const NotificationDetailPgae = () => {
    const elem = useLocation().state.elem

    return(
        <div>
            Created: {elem.created}<br />
            Completed: {elem.completed? elem.completed : "No"}<br />
            {elem.userMessage && <>Your message: {elem.userMessage}<br /></>}
            {elem.response && <>Response: {elem.response}<br /></>}
            Status: {elem.status.name}<br />
        </div>
    )
}

export default NotificationDetailPgae