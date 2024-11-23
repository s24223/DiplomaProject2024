import React, { useState } from 'react';
import { fetchBranchPost } from '../../services/BranchService/BranchService';

const CreateBranchPage = () => {
    const [addressId, setAddressId] = useState();
    const [urlSegment, setUrlsegmet] = useState();
    const [name, setName] = useState();
    const [description, setDescription] = useState('');

    const handleSubmit = (event) => {
        event.preventDefault();
        const fetchDummy = async () => {
            let response = await fetchBranchPost([{
                "addressId": addressId,
                "urlSegment": urlSegment,
                "name": name,
                "description": description
            }]);
            if(response.ok){
                window.location.href = "/userProfile"
            }
        }
        fetchDummy()
    }

    return(
        <div>
            <form onSubmit={handleSubmit}>
                <label htmlFor='address'>AddressId:</label><br />
                <input type="text" id='address' onChange={e => setAddressId(e.target.value)} required /><br />
                <label htmlFor='urlSegment'>UrlSegment:</label><br />
                <input type="text" id='urlSegment' onChange={e => setUrlsegmet(e.target.value)} required /><br />
                <label htmlFor='name'>Name:</label><br />
                <input type='text' id='name' onChange={e => setName(e.target.value)} required /><br />
                <label htmlFor='description'>Description:</label><br />
                <input type='text' id='description' onChange={e => setDescription(e.target.value)} /><br />
                <input type='submit' value="Add Branch" />
            </form>
        </div>
    )
}

export default CreateBranchPage;