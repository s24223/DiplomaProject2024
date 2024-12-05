import React from 'react'
import { useLocation, Link } from 'react-router-dom'
import './NotificationDetailPage.css'

const NotificationDetailPgae = () => {
    const elem = useLocation().state.elem

    console.log(elem)

    return(
        <div>
            Created: {elem.created}<br />
            Completed: {elem.completed? elem.completed : "No"}<br />
            {elem.previousProblemId && <>Previous Id: {elem.previousProblemId}<br /></>}
            {elem.userMessage && <>Your message: {elem.userMessage}<br /></>}
            {elem.response && <>Response: {elem.response}<br /></>}
            Status: {elem.status.name}<br />
            <button><Link id='linkToCreateOnExistant' to={{pathname:'/notification/create'}} state={{elem}}>Add to notification</Link></button>
        </div>
    )
}

export default NotificationDetailPgae