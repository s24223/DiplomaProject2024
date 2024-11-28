import React, { useState } from 'react'
import { useLocation } from 'react-router-dom';
import { fetchBranchPut } from '../../services/BranchService/BranchService';
import ProfileButton from '../../components/ProfileButton/ProfileButton';
import CancelButton from '../../components/CancelButton/CancelButton';

const BranchDetailPage = () => {
    const [editMode, setEditMode] = useState(false);
    const item = useLocation().state.item

    const [name, setName] = useState(item.name)
    const [description, setDescription] = useState(item.description)
    const [urlSegment, setUrlsegmet] = useState(item.urlSegment)

    const [messageStatus, setMessageStatus] = useState('')

    const sleep = ms => new Promise(r => setTimeout(r, ms));

    const handleChange = () => {
        const fetchDummy = async () => {
            let response = await fetchBranchPut([{
                "branchId": item.id,
                "addressId": item.addressId,
                "urlSegment": urlSegment,
                "name": name,
                "description": description
            }])

            if(response.ok){
                await response.json()
                item.name = name
                item.description = description
                item.urlSegment = urlSegment
                setMessageStatus('Success')
                await sleep(1000)
                setMessageStatus('')
            }
        }

        fetchDummy()
    }

    return(
        <div>
            {
            item && 
            <div>
                <ProfileButton/>
                <button onClick={() => setEditMode(!editMode)}>Edit mode</button><br />
                Name: {editMode? <input type='text' placeholder={item.name} onChange={e => setName(e.target.value)} value={name} />: item.name}<br />
                Description: {editMode? <input type='text' placeholder={item.description} onChange={e => setDescription(e.target.value)} value={description} /> : item.description? item.description : <>NaN</>}<br />
                Url Segment: {editMode? <input type='text' placeholder={item.urlSegment} onChange={e => setUrlsegmet(e.target.value)} value={urlSegment} /> : item.urlSegment? item.urlSegment : <>NaN</>}<br />
                Address: {item.address.hierarchy.map((addressPart) => (
                    <p key={addressPart.id}>{addressPart.administrativeType.name}: {addressPart.name}</p>
                ))}
                <p>Ulica: {item.address.street.name}</p>
                <p>Dom: {item.address.buildingNumber}</p>
                <p>Apartment number: {item.address.apartmentNumber}</p>
                <label style={{color:'green'}}>{messageStatus}</label><br />
                {editMode && <button onClick={handleChange}>Change</button>}
                <CancelButton/>
            </div>
            }
        </div>
    )
}

export default BranchDetailPage;