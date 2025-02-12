import axios from "axios"

export const fetchActivateAccount = async (userId, secretString) => {
  return await axios.get(`https://localhost:7166/api/User/activate/${userId}/${secretString}`)
  .then(resposne => resposne.status === 200)
  .catch(error => {
    return false
  })
}