import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import ApplyButton from '../Buttons/ApplyButton/ApplyButton'
import AddressImage from "../AddressImage/AddressImage";
import { Link } from "react-router-dom";

const OfferDetails = () => {
    const { offerId } = useParams();
    const [offerDetails, setOfferDetails] = useState(null);

    
    

    useEffect(() => {
        const fetchOfferDetails = async () => {
            try {
                const response = await fetch(`https://localhost:7166/api/BranchOffers/offers/${offerId}`, {
                    method: 'GET',
                    withCredentials: true,
                    crossorigin: true,
                    headers: { 'Access-Control-Allow-Origin': '*' },
                });
                if (!response.ok) {
                    throw new Error(`Failed to fetch: ${response.status} ${response.statusText}`);
                }
            
                const data = await response.json();
                setOfferDetails(data.item);
            } catch (error) {
                console.error("Error fetching offer details:", error);
            }
        };
        fetchOfferDetails();
    }, [offerId]);

    if (!offerDetails) {
        return (
            <div className="loading">
                <p>Loading offer details...</p>
            </div>
        );
    }
    

    // Rozpakowywanie danych dla czytelności
    const { company, offer, branchOffers } = offerDetails;
    if (!branchOffers || branchOffers.length === 0) {
        return <p>No branch information available.</p>;
    }
    const branchOffer = branchOffers[0]; // Zakładając, że bierzemy tylko pierwszy oddział
    const { branch } = branchOffer;

    return (
        <div className="offer-details">
            
            <h1>{offer?.name}</h1>
            <p>
            <strong>Firma:</strong>{' '}
            <Link
                to={`/company/${branchOffer?.company?.companyId}`}
                onClick={() => {
                    localStorage.setItem(
                        'companyDetails',
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

            <div className='bordered'></div>
            <h2>Oddział:</h2>
            <p><strong>Nazwa oddziału:</strong> {branch?.name}</p>
            <p><strong>Adres:</strong></p>
            <p>ul. {branch?.address?.street?.name} {branch?.address?.buildingNumber}/{branch?.address?.apartmentNumber}  </p>
            <p>{branch?.address?.zipCode?.slice(0,2)}-{branch?.address?.zipCode?.slice(2)} {branch?.address?.hierarchy[2]?.name}</p>
            <p>{branch?.address?.hierarchy[1]?.name} </p>
            <br/>
            
            <p><strong>Województwo:</strong> {branch?.address?.hierarchy[0]?.name}</p>
            <p><strong>Opis oddziału:</strong> {branch?.description}</p>
            <p><AddressImage lon={branch?.address?.lon} lat={branch?.address?.lat} /></p>

            <div className='bordered'></div>
            <h2>Szczegóły oferty:</h2>
            
            <p><strong>Opis oferty:</strong> {offer?.description}</p>
            <p><strong>Wynagrodzenie:</strong> {offer?.minSalary} - {offer.maxSalary} PLN</p>
            <p><strong>Dla studentów:</strong> {offer?.isForStudents ? "Tak" : "Nie"}</p>
            <p><strong>Negocjowalne wynagrodzenie:</strong> {offer?.isNegotiatedSalary ? "Tak" : "Nie"}</p>
            <p><strong>Płatna oferta:</strong> {offer?.isPaid ? "Tak" : "Nie"}</p>

            <div className='bordered'></div>
            <h2>Informacje o zatrudnieniu:</h2>
            <p><strong>Id oferty:</strong> {branchOffer?.branchOffer.id}</p>
            <p><strong>Data publikacji:</strong> {new Date(branchOffer?.branchOffer?.publishStart).toLocaleDateString()}</p>
            <p><strong>Data Zakończenia:</strong> {new Date(branchOffer?.branchOffer?.publishEnd).toLocaleDateString()}</p>
            <p><strong>Ostatnia aktualizacja:</strong> {new Date(branchOffer?.branchOffer?.lastUpdate).toLocaleDateString()}</p>
            <p><strong>Okres zatrudnienia:</strong> {branchOffer?.branchOffer?.workDuration?.years} lat, {branch?.offerDetails?.workDuration?.months} miesięcy, {branch.offerDetails?.workDuration?.days} dni</p>
            <ApplyButton branchId={branchOffer?.branchOffer?.id} />

        </div>
    );
};

export default OfferDetails;




// import React, { useState, useEffect } from 'react';
// import { useParams } from 'react-router-dom';
// import ApplyButton from '../Buttons/ApplyButton/ApplyButton';
// import EditOfferForm from '../Forms/EditOfferForm';
// import AddressImage from '../AddressImage/AddressImage';
// import ReturnButton from '../Buttons/CancelButton/ReturnButton';

// const OfferDetails = ({ mode = 'view' }) => {
//     const { offerId } = useParams();
//     const [offerDetails, setOfferDetails] = useState(null);
//     const isEditing = mode === 'edit';

//     useEffect(() => {
//         const fetchOfferDetails = async () => {
//             try {
//                 const response = await fetch(
//                     `https://localhost:7166/api/BranchOffers/offers/${offerId}`,
//                     {
//                         method: 'GET',
//                         headers: { Authorization: `Bearer ${sessionStorage.getItem('jwt')}` },
//                     }
//                 );
//                 if (!response.ok) throw new Error('Failed to fetch offer details');
//                 const data = await response.json();
//                 setOfferDetails(data.item);
//             } catch (error) {
//                 console.error('Error fetching offer details:', error);
//             }
//         };

//         fetchOfferDetails();
//     }, [offerId]);

//     if (!offerDetails) {
//         return <p>Loading offer details...</p>;
//     }

//     const { offer, branchOffers, company } = offerDetails;

//     if (!branchOffers?.length) {
//         return <p>No branch information available.</p>;
//     }

//     const branchOffer = branchOffers[0];
//     const { branch } = branchOffer;

//     return (
//         <div className="offer-details">
//             <h1>{offer?.name}</h1>
//             <p><strong>Company:</strong> {company?.name}</p>
//             <p><strong>Email:</strong> {company?.contactEmail}</p>
//             <p><strong>Branch:</strong> {branch?.name}</p>
//             <p><strong>Address:</strong> ul. {branch?.address?.street?.name} {branch?.address?.buildingNumber}/{branch?.address?.apartmentNumber}</p>
//             <p><AddressImage lon={branch?.address?.lon} lat={branch?.address?.lat} /></p>

//             {isEditing ? (
//                 <EditOfferForm offer={offer} />
//             ) : (
//                 <>
//                     <p><strong>Description:</strong> {offer?.description}</p>
//                     <p><strong>Salary:</strong> {offer?.minSalary} - {offer?.maxSalary} PLN</p>
//                     <p><strong>For Students:</strong> {offer?.isForStudents ? 'Yes' : 'No'}</p>
//                     <ApplyButton branchId={branchOffer?.branchOffer?.id} />
//                 </>
//             )}
//         </div>
//     );
// };

// export default OfferDetails;
