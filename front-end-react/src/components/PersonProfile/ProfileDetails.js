import React from "react";

const ProfileDetails = ({ person }) => (
    <div>
        <h2>Personal Information</h2>
        <div className="bordered">
            <p><strong>Name:</strong> {person.name}</p>
            <p><strong>Surname:</strong> {person.surname}</p>
            <p><strong>Email:</strong> {person.contactEmail}</p>
            <p><strong>Phone:</strong> {person.contactPhoneNum}</p>
            <p><strong>Birth Date:</strong> {new Date(person.birthDate).toLocaleDateString()}</p>
            <p><strong>Is Student:</strong> {person.isStudent ? "Yes" : "No"}</p>
            <p><strong>Public Profile:</strong> {person.isPublicProfile ? "Yes" : "No"}</p>
            <p><strong>Description:</strong> {person.description}</p>
        </div>
        <h2>User Characteristics</h2>
        {person.characteristics?.length > 0 ? (
            <div className="bordered">
                <ul>
                    {person.characteristics.map((char) => (
                        <li key={char.characteristic.id}>
                            <strong>{char.characteristic.name}</strong> {char?.quality?.name}
                        </li>
                    ))}
                </ul>
            </div>
        ) : (
            <p>No characteristics available.</p>
        )}
    </div>
);

export default ProfileDetails;
