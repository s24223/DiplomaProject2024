import React, { useState, useEffect } from 'react'
import { fetchNotificationGetAuthorized } from '../../../services/NotificationService/NotificationService'
import { Link } from 'react-router-dom';
import { jwtRefresh } from '../../../services/JwtRefreshService/JwtRefreshService';

const NotificationPage = () => {
    const [errorMessage, setErrorMessage] = useState();

    if (!localStorage.getItem("jwt"))
        window.location.href = '/notification/create'

    jwtRefresh();

    const fetchDummy = async () => {
        let response = await fetchNotificationGetAuthorized()
        if (response) {
            if (response.error) {
                throw new Error(response.error)
            }
            if (response.item) {

                setNotificationList(response.item.urls)
            }
        }
        else{
            setErrorMessage('Something went wrong when trying to retrive your notifications')
        }
    }

    fetchDummy()

    const [notificationList, setNotificationList] = useState([])

    useEffect(() => {

    }, [])

    const handleCreateButton = () => {
        window.location.href = "/notification/create"
    }

    return (
        <div id='notification-list'>
            {errorMessage && <header className='error-message'>{errorMessage}</header>}
            <nav>
                <ul>
                    {notificationList && notificationList.map((elem) => (
                        <li key={elem.id}><Link id="notificationLink"
                            to={{ pathname: `/notification/${elem.id}` }}
                            state={{ elem }}>
                            {elem.created}</Link></li>
                    ))}
                </ul>
            </nav>
            <button onClick={handleCreateButton}>Create notification</button>
        </div>
    )
}

export default NotificationPage;