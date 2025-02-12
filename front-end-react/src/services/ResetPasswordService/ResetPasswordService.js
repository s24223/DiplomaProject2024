import axios from "axios"

export const fetchResetPassword = async (userId, secretString, newPassword) => {
  return await axios.post(`https://localhost:7166/api/User/reset/${userId}/${secretString}`, {"newPassword": newPassword}, {
    headers: {
      'Access-Control-Allow-Origin': '*',
  }
  })
  .then(resposne => resposne.status === 200)
  .catch(error => {
    return false
  })
}

export const fetchResetPasswordEmail = async (email) => {
  return axios.post('https://localhost:7166/api/User/reset', {"email": email}, {
    headers: {
      'Access-Control-Allow-Origin': '*',
  }
  })
  .then(resposne => resposne.status === 200)
  .catch(error => {
    return false
  })
}