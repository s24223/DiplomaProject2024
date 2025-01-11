import React from "react";
import { Link } from "react-router-dom";
import ApplyButton from "../Buttons/ApplyButton/ApplyButton";
import AddressImage from "../AddressImage/AddressImage";

const OfferDetails = ({ offerDetails }) => {
    const { company, offer, branchOffers } = offerDetails;
    if (!branchOffers || branchOffers.length === 0) {
        return <p>No branch information available.</p>;
    }
    const branchOffer = branchOffers[0];
    const { branch } = branchOffer;

    return (
        <div className="offer-details">
            
            <h1>{offer?.name}</h1>
            <p>
                <strong>Firma:</strong>{" "}
                <Link
                    to={`/company/${branchOffer?.company?.companyId}`}
                    onClick={() => {
                        localStorage.setItem(
                            "companyDetails",
                            JSON.stringify({
                                companyName: branchOffer?.company?.name,
                                companyDescription: branchOffer?.company?.description,
                                companyUrl: branchOffer?.company?.urlSegment,
                            })
                        );
                    }}
                >
                    {branchOffer?.company?.name}
                </Link>
            </p>
            <p><strong>Email kontaktowy:</strong> {branchOffer?.company?.contactEmail}</p>
            <p><strong>Opis firmy:</strong> {branchOffer?.company?.description}</p>
            <div className="bordered"></div>
            <h2>Oddział:</h2>
            <p><strong>Nazwa oddziału:</strong> {branch?.name}</p>
            <p><strong>Adres:</strong></p>
            <p>BranchID: {branch.id}</p>
            <p>ul. {branch?.address?.street?.name} {branch?.address?.buildingNumber}/{branch?.address?.apartmentNumber}</p>
            <p>{branch?.address?.zipCode?.slice(0, 2)}-{branch?.address?.zipCode?.slice(2)} {branch?.address?.hierarchy[2]?.name}</p>
            <p>{branch?.address?.hierarchy[1]?.name}</p>
            <p><strong>Województwo:</strong> {branch?.address?.hierarchy[0]?.name}</p>
            <p><AddressImage lon={branch?.address?.lon} lat={branch?.address?.lat} /></p>
            <div className="bordered"></div>
            <h2>Szczegóły oferty:</h2>
            <p><strong>Opis oferty:</strong> {offer?.description}</p>
            <p><strong>Wynagrodzenie:</strong> {offer?.minSalary} - {offer.maxSalary} PLN</p>
            <p><strong>Dla studentów:</strong> {offer?.isForStudents ? "Tak" : "Nie"}</p>
            <p><strong>Negocjowalne wynagrodzenie:</strong> {offer?.isNegotiatedSalary ? "Tak" : "Nie"}</p>
            <div className="bordered"></div>
            <h2>Charakterystyki oferty:</h2>
            {offer?.characteristics?.length > 0 ? (
                <ul>
                    {offer.characteristics.map((char, index) => (
                        <li key={index}>
                            <p>
                                <strong>{char.characteristic.name}</strong>{" "}
                                {char.quality?.name || ""}
                            </p>
                        </li>
                    ))}
                </ul>
            ) : (
                <p>Brak charakterystyk dla tej oferty.</p>
            )}
            <div className="bordered"></div>
            <h2>Informacje o zatrudnieniu:</h2>
            
            <p><strong>Id oferty:</strong> {offer.id}</p>
            <p><strong>branchOfferId oferty:</strong> {branchOffer?.branchOffer.id}</p> 
            <p><strong>Data publikacji:</strong> {new Date(branchOffer?.branchOffer?.publishStart).toLocaleDateString()}</p>
            <p><strong>Data zakończenia:</strong> {new Date(branchOffer?.branchOffer?.publishEnd).toLocaleDateString()}</p>
            <p><strong>Okres zatrudnienia:</strong> {branchOffer?.branchOffer?.workDuration?.years} lat, {branch?.offerDetails?.workDuration?.months} miesięcy, {branch.offerDetails?.workDuration?.days} dni</p>
            <ApplyButton branchId={branchOffer?.branchOffer?.id} />
        <h1>OfferDetails</h1>
        </div>
    );
};

export default OfferDetails;