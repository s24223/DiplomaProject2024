import React, { useState } from 'react';
import { fetchBranchPost } from '../../services/BranchService/BranchService';
import CancelButton from '../../components/CancelButton/CancelButton';
import MainPageButton from '../../components/MainPageButton/MainPageButton';
import LoginButton from '../../components/LoginButton/LoginButton';
import AddressAutocomplete from '../../components/AddressAutoComplete/AddressAutoComplete';

const CreateBranchPage = () => {
    const [addressId, setAddressId] = useState();
    const [urlSegment, setUrlsegmet] = useState();
    const [name, setName] = useState();
    const [description, setDescription] = useState('');

    const childToParent = (addressIdFromChild) => {
        setAddressId(addressIdFromChild)
    }

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
        <div className='centered'>
            <MainPageButton/>
            <LoginButton/>
            <h2>Add branch</h2>
            <form className='form' onSubmit={handleSubmit}>
                <label htmlFor='address'>AddressId:</label><br />
                <AddressAutocomplete childToParent={childToParent} /><br />
                <label htmlFor='urlSegment'>UrlSegment:</label><br />
                <input type="text" id='urlSegment' onChange={e => setUrlsegmet(e.target.value)} required /><br />
                <label htmlFor='name'>Name:</label><br />
                <input type='text' id='name' onChange={e => setName(e.target.value)} required /><br />
                <label htmlFor='description'>Description:</label><br />
                <input type='text' id='description' onChange={e => setDescription(e.target.value)} /><br />
                <input type='submit' value="Add Branch" />
                <CancelButton/>
            </form>
        </div>
    )
}

export default CreateBranchPage;