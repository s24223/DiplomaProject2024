import React, { useState } from 'react';
import { fetchBranchPut } from '../../services/BranchService/BranchService';

const BranchDetails = ({ branch, onEditSuccess }) => {
    const [editMode, setEditMode] = useState(false);
    const [name, setName] = useState(branch.name);
    const [description, setDescription] = useState(branch.description);
    const [urlSegment, setUrlSegment] = useState(branch.urlSegment);
    const [messageStatus, setMessageStatus] = useState('');

    const handleSave = async () => {
        try {
            const response = await fetchBranchPut([{
                branchId: branch.id,
                addressId: branch.addressId,
                urlSegment,
                name,
                description,
            }]);
            if (response.ok) {
                setMessageStatus('Branch updated successfully.');
                onEditSuccess();
            }
        } catch (error) {
            setMessageStatus('Failed to update branch.');
        } finally {
            setTimeout(() => setMessageStatus(''), 2000);
        }
    };

    return (
        <div className="branch-details">
            <button onClick={() => setEditMode(!editMode)}>
                {editMode ? "Cancel Edit" : "Edit Mode"}
            </button>
            <p>Name: {editMode ? <input value={name} onChange={(e) => setName(e.target.value)} /> : name}</p>
            <p>Description: {editMode ? <input value={description} onChange={(e) => setDescription(e.target.value)} /> : description}</p>
            <p>URL Segment: {editMode ? <input value={urlSegment} onChange={(e) => setUrlSegment(e.target.value)} /> : urlSegment}</p>
            <p>Address:</p>
            <ul>
                {branch.address.hierarchy.map(({ id, administrativeType, name }) => (
                    <li key={id}>
                        {administrativeType.name}: {name}
                    </li>
                ))}
            </ul>
            {editMode && <button onClick={handleSave}>Save</button>}
            {messageStatus && <p>{messageStatus}</p>}
        </div>
    );
};

export default BranchDetails;
