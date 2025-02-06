import { useState } from "react"
import { deleteUserProfile } from "../../services/ProfileService/ProfileService"

const DeleteProfilePage = () => {
  const [password, setPassword] = useState()

  const handleSubmit = (e) => {
    e.preventDefault()
    let body = {
      password: password,
    } 
    const fetchDummy = async () => {
      await deleteUserProfile(body)
      localStorage.clear()
      window.location.href = "/"
    }
    fetchDummy()
  }

  return(
    <div className="centered">
      <form onSubmit={e => handleSubmit(e)}>
        <label>Password:</label><br />
        <input type="password" onChange={e => setPassword(e.target.value)} required /><br />
        <input type="submit" value="Confirm" />
      </form>
    </div>
  )
}

export default DeleteProfilePage;