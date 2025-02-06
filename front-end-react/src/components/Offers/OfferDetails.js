import React from "react";
import { Link } from "react-router-dom";
import AddressImage from "../AddressImage/AddressImage";

const OfferDetails = ({ offerDetails }) => {
    const { /*company,*/ offer, branchOffers } = offerDetails;
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
                    to={`/company/${branchOffer?.company?.companyId}`}  className="hidden-link"
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
            <p><strong>Contact e-mail:</strong> {branchOffer?.company?.contactEmail}</p>
            <p><strong>Business description:</strong> {branchOffer?.company?.description}</p>
            <div className="bordered"></div>
            <h2>Branch:</h2>
            <p><strong>Branch name:</strong> {branch?.name}</p>
            <p><strong>Address:</strong></p>
            {/* <p>BranchID: {branch.id}</p> */}
            <p>{branch?.address?.street?.name} street {branch?.address?.buildingNumber}/{branch?.address?.apartmentNumber}</p>
            <p>{branch?.address?.zipCode?.slice(0, 2)}-{branch?.address?.zipCode?.slice(2)} {branch?.address?.hierarchy[2]?.name}</p>
            <p>{branch?.address?.hierarchy[1]?.name}</p>
            <p><strong>voivodeship:</strong> {branch?.address?.hierarchy[0]?.name}</p>
            <p><AddressImage lon={branch?.address?.lon} lat={branch?.address?.lat} /></p>
            <div className="bordered"></div>
            <h2>Offer details:</h2>
            <p><strong>Offer description:</strong> {offer?.description}</p>
            <p><strong>Salary:</strong> {offer?.minSalary} - {offer.maxSalary} PLN</p>
            <p><strong>For students:</strong> {offer?.isForStudents ? "Yes" : "No"}</p>
            <p><strong>Negotiable salary:</strong> {offer?.isNegotiatedSalary ? "Yes" : "No"}</p>
            <div className="bordered"></div>
            <h2>Offer characteristics:</h2>
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
                <p>No characteristics for this offer.</p>
            )}
            <div className="bordered"></div>
            <h2>Employment information:</h2>
            
            {/* <p><strong>Id oferty:</strong> {offer.id}</p>
            <p><strong>branchOfferId oferty:</strong> {branchOffer?.branchOffer.id}</p>  */}
            <p><strong>Publish date:</strong> {new Date(branchOffer?.branchOffer?.publishStart).toLocaleDateString()}</p>
            <p><strong>Publish end date:</strong> {new Date(branchOffer?.branchOffer?.publishEnd).toLocaleDateString()}</p>
            <p><strong>Employment period:</strong> {branchOffer?.branchOffer?.workDuration.years} years, {branchOffer?.branchOffer?.workDuration?.months} monthes, {branchOffer.branchOffer?.workDuration?.days} days</p>
            {/* <p>branchOffer?.branchOffer?.id: {branchOffer?.branchOffer?.id}</p> */}
        {/* <h1>OfferDetails</h1> */}
        </div>
    );
};

export default OfferDetails;