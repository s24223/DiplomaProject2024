import React, { useState } from 'react';
import { updateOffer } from '../../services/OffersService/OffersService';

const EditOfferForm = ({ offer }) => {
    const [formData, setFormData] = useState({
        name: offer.name,
        description: offer.description,
        minSalary: offer.minSalary,
        maxSalary: offer.maxSalary,
        isForStudents: offer.isForStudents,
    });

    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;
        setFormData((prevData) => ({
            ...prevData,
            [name]: type === 'checkbox' ? checked : value,
        }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            await updateOffer({ ...formData, offerId: offer.id });
            alert('Offer updated successfully!');
        } catch (error) {
            console.error('Error updating offer:', error);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <label>Name:</label>
            <input type="text" name="name" value={formData.name} onChange={handleChange} />
            <label>Description:</label>
            <textarea name="description" value={formData.description} onChange={handleChange}></textarea>
            <label>Min Salary:</label>
            <input type="number" name="minSalary" value={formData.minSalary} onChange={handleChange} />
            <label>Max Salary:</label>
            <input type="number" name="maxSalary" value={formData.maxSalary} onChange={handleChange} />
            <label>For Students:</label>
            <input type="checkbox" name="isForStudents" checked={formData.isForStudents} onChange={handleChange} />
            <button type="submit">Save</button>
        </form>
    );
};

export default EditOfferForm;
