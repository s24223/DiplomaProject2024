import React, { useState } from 'react';
import { fetchBranchPut } from '../../services/BranchService/BranchService';

const EditBranchForm = ({ item, onSave }) => {
    const [name, setName] = useState(item.name);
    const [description, setDescription] = useState(item.description);
    const [urlSegment, setUrlSegment] = useState(item.urlSegment);
    const [message, setMessage] = useState('');

    const handleSubmit = async () => {
        try {
            const response = await fetchBranchPut([
                {
                    branchId: item.id,
                    addressId: item.addressId,
                    urlSegment,
                    name,
                    description,
                },
            ]);
            if (response.ok) {
                setMessage('Branch updated successfully!');
                onSave();
            } else {
                setMessage('Failed to update branch.');
            }
        } catch (error) {
            setMessage('An error occurred.');
        }
    };

    return (
        <div className="edit-branch-form">
            <label>Name:</label>
            <input type="text" value={name} onChange={(e) => setName(e.target.value)} />
            <label>Description:</label>
            <input
                type="text"
                value={description}
                onChange={(e) => setDescription(e.target.value)}
            />
            <label>URL Segment:</label>
            <input
                type="text"
                value={urlSegment}
                onChange={(e) => setUrlSegment(e.target.value)}
            />
            <button onClick={handleSubmit}>Save</button>
            {message && <p>{message}</p>}
        </div>
    );
};

export default EditBranchForm;
