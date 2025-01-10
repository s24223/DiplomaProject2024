import React, { useState } from "react";

const OfferEditForm = ({ offerDetails, onCancelEdit }) => {
    const [formData, setFormData] = useState({
        name: offerDetails.offer.name,
        description: offerDetails.offer.description,
        minSalary: offerDetails.offer.minSalary,
        maxSalary: offerDetails.offer.maxSalary,
    });

    const handleInputChange = (e) => {
        const { name, value } = e.target;
        setFormData((prevData) => ({ ...prevData, [name]: value }));
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        console.log("Updated Offer Data:", formData);
        // TODO: Add API call to save the changes
        onCancelEdit();
    };

    return (
        <form onSubmit={handleSubmit}>
            <label>
                Name:<br/>
                <input
                    type="text"
                    name="name"
                    value={formData.name}
                    onChange={handleInputChange}
                />
            </label><br/>
            <label>
                Description:<br/>
                <textarea
                    name="description"
                    value={formData.description}
                    onChange={handleInputChange}
                />
            </label><br/>
            <label>
                Min Salary:<br/>
                <input
                    type="number"
                    name="minSalary"
                    value={formData.minSalary}
                    onChange={handleInputChange}
                />
            </label><br/>
            <label>
                Max Salary:<br/>
                <input
                    type="number"
                    name="maxSalary"
                    value={formData.maxSalary}
                    onChange={handleInputChange}
                />
            </label><br/>
            <button type="submit">Save Changes</button>
            <button type="button" onClick={onCancelEdit}>
                Cancel
            </button>
        </form>
    );
};

export default OfferEditForm;
