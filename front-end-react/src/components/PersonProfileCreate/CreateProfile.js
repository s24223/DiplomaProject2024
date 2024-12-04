import React, { useState } from "react";
import { fetchProfilePost } from "../../services/ProfileService/ProfilceService";
import CancelButton from "../CancelButton/CancelButton";

const CreateProfile = () => {
    const [urlSegmet, setUrlsegmet] = useState("string")
    const [contactEmail, setContactEmail] = useState()
    const [name, setName] = useState()
    const [surname, setSurname] = useState()
    const [birthDate, setBirthdate] = useState({
        year: null,
        month: null,
        day:null
    })
    const [contactPhoneNum, setContactPhoneNum] = useState()
    const [description, setDescription] = useState("")
    const [isStudent, setIsStudent] = useState(false)
    const [isPublicProfile, setIsPublicProfile] = useState(false)
    const [addressId, setAddressId] = useState(null)
    const [characteristics, setCharacteristics] = useState([])

    const handleBirthDate = (date) => {
        let dateSegmented = date.split("-")
        setBirthdate({
            year: dateSegmented[0],
            month: dateSegmented[1],
            day: dateSegmented[2]
        });
    }

    const handleSubmit = async (event) => {
        event.preventDefault()
        let body = {
            urlSegmet,
            contactEmail,
            name,
            surname,
            birthDate,
            contactPhoneNum,
            description,
            isStudent,
            isPublicProfile,
            addressId,
            characteristics
        }

        try {
            await fetchProfilePost(body)
            //window.location.href="/userProfile"
        } catch(error){
            console.error(error)
        }
    }

    return(
        <div>
            <form onSubmit={handleSubmit}>
                <label>Name:</label><br />
                <input type="text" placeholder="Name" onChange={e => setName(e.target.value)} required /><br />
                <label>Surname:</label><br />
                <input type="text" placeholder="Surname" onChange={e => setSurname(e.target.value)} required /><br />
                <label>BirthDate:</label><br />
                <input type="date" onChange={e => handleBirthDate(e.target.value)} required /><br />
                <label>Email:</label><br />
                <input type="email" placeholder="Email" onChange={e => setContactEmail(e.target.value)} required /><br />
                <label>Phone Number (format: 123456789):</label><br />
                <input type="tel" placeholder="Phone number" pattern="[0-9]{9}" onChange={e => setContactPhoneNum(e.target.value)} required /><br />
                <input id="student" type="checkbox" onChange={e => setIsStudent(e.target.checked)} />
                <label htmlFor="student">a student</label><br />
                <input id="public" type="checkbox" onChange={e => setIsPublicProfile(e.target.checked)} />
                <label htmlFor="public">public profile</label><br />
                <label>Description:</label><br />
                <textarea className="description" placeholder="Description" onChange={e => setDescription(e.target.value)} /><br /> 
                <input type="submit" value="Create Profile" />
                <CancelButton/>
            </form>
        </div>
    )
}

export default CreateProfile;