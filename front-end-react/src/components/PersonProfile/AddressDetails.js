import React from "react";
import AddressImage from "../AddressImage/AddressImage";

const AddressDetails = ({ address }) => (
    <div className="profile-details">
        <h2>Address</h2>
        <div className="bordered">
            <p><strong>Street:</strong> {address.street?.name} {address.buildingNumber}/{address.apartmentNumber}</p>
            <p><strong>ZIP Code:</strong> {address.zipCode}</p>
            <p><strong>City:</strong> {address.hierarchy?.find(item => item.administrativeType.name.includes("miasto"))?.name}</p>
            <p><strong>Voivodeship:</strong> {address.hierarchy?.find(item => item.administrativeType.name === "województwo")?.name}</p>
            <p><AddressImage lon={address.lon} lat={address.lat} /></p>
        </div>
    </div>
);

export default AddressDetails;
