import React, { useState, useEffect } from 'react'
import { fetchNotificationGetAuthorized } from '../../services/NotificationService/NotificationService'
import { Link } from 'react-router-dom';
import './NotificationPage.css'

const NotificationPage = () => {
    const [notificationList, setNotificationList] = useState([])
    let count = 0;

    useEffect(() => {
        if(!sessionStorage.getItem("jwt"))
            window.location.href = '/login'

        const fetchDummy = async () => {
            let response = await fetchNotificationGetAuthorized()
            setNotificationList(response.item.urls)
        }

        fetchDummy()
    }, [])

    const handleCreateButton = () => {
        window.location.href = "/notification/create"
    }

    return(
        <div>
            <nav>
                <ul>
                    {notificationList && notificationList.map((elem) => (
                        <li key={count++}><Link id="notificationLink"
                        to={{ pathname: `/notification/${count}` }}
                        state = {{ elem }}>
                            {elem.created}</Link></li>
                    ))}
                </ul>
            </nav>
            <button onClick={handleCreateButton}>Create notification</button>
        </div>
    )
}

export default NotificationPage;