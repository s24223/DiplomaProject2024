import React, { useEffect, useState } from "react";
import { fetchActivateAccount } from "../../services/AccountActivateService/AccountActivateService";

const AccountActivationPage = () => {
  const [message, setMessage] = useState('')
  const pathname = window.location.href.split('/')
  const secretString = pathname[pathname.length - 1]
  const userId = pathname[pathname.length - 2]

  useEffect(() => {
    const fetchDummy = async () => {
      const response = await fetchActivateAccount(userId, secretString)
      if (response) {
        setMessage('Account activated')
      }
      else {
        setMessage('Failed to activate')
      }
    }
    fetchDummy()
  }, [userId, secretString])

  return (
    <div className="centered">
      {message && message}
    </div>
  )
}

export default AccountActivationPage