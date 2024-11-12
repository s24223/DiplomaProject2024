import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import LoginButton from '../../components/LoginButton/LoginButton';
import './OfferDetailsPage.css';
import MainPageButton from '../../components/MainPageButton/MainPageButton';
import ApplyButton from '../../components/ApplyButton/ApplyButton'

const OfferDetailsPage = () => {
    const { offerId } = useParams();
    const [offerDetails, setOfferDetails] = useState(null);

    

    useEffect(() => {
        const fetchOfferDetails = async () => {
            try {
                const response = await fetch(`https://localhost:7166/api/Offers/${offerId}`, {
                    method: 'GET',
                    withCredentials: true,
                    crossorigin: true,
                    headers: { 'Access-Control-Allow-Origin': '*' },
                });
                if (!response.ok) throw new Error("Error fetching offer details");
                const data = await response.json();
                setOfferDetails(data.item);
            } catch (error) {
                console.error("Error fetching offer details:", error);
            }
        };
        fetchOfferDetails();
    }, [offerId]);

    if (!offerDetails) return <p>Loading...</p>;

    // Rozpakowywanie danych dla czytelności
    const { company, offer, branches } = offerDetails;
    const branch = branches[0]; // Zakładając, że bierzemy tylko pierwszy oddział

    return (
        <div className="offer-details">
            <LoginButton />
            <MainPageButton/>
            <h1>{offer?.name}</h1>
            <p><strong>Firma:</strong> {company?.name}</p>
            <p><strong>Email kontaktowy:</strong> {company?.contactEmail}</p>
            <p><strong>Opis firmy:</strong> {company?.description}</p>
            
            <h2>Oddział:</h2>
            <p><strong>Nazwa oddziału:</strong> {branch?.branch?.name}</p>
            <p><strong>Adres:</strong></p>
            <p>ul. {branch?.branch?.address?.street?.name} {branch.branch?.address?.buildingNumber}/{branch.branch?.address?.apartmentNumber}  </p>
            <p>{branch.branch?.address?.zipCode?.slice(0,2)}-{branch.branch?.address?.zipCode?.slice(2)} {branch?.branch?.address?.hierarchy[2]?.name}</p>
            <br/>
            <p><strong>Województwo:</strong> {branch?.branch?.address?.hierarchy[0]?.name}</p>
            <p><strong>Opis oddziału:</strong> {branch?.branch?.description}</p>

            <h2>Szczegóły oferty:</h2>
            
            <p><strong>Opis oferty:</strong> {offer?.description}</p>
            <p><strong>Wynagrodzenie:</strong> {offer?.minSalary} - {offer.maxSalary} PLN</p>
            <p><strong>Dla studentów:</strong> {offer?.isForStudents ? "Tak" : "Nie"}</p>
            <p><strong>Negocjowalne wynagrodzenie:</strong> {offer?.isNegotiatedSalary ? "Tak" : "Nie"}</p>
            <p><strong>Płatna oferta:</strong> {offer?.isPaid ? "Tak" : "Nie"}</p>

            <h2>Informacje o zatrudnieniu:</h2>
            <p><strong>Id oferty:</strong> {branch.offerDetails.id}</p>
            <p><strong>Data publikacji:</strong> {new Date(branch?.offerDetails?.publishStart).toLocaleDateString()}</p>
            <p><strong>Data Zakończenia:</strong> {new Date(branch?.offerDetails?.publishEnd).toLocaleDateString()}</p>
            <p><strong>Ostatnia aktualizacja:</strong> {new Date(branch?.offerDetails?.lastUpdate).toLocaleDateString()}</p>
            <p><strong>Okres zatrudnienia:</strong> {branch?.offerDetails?.workDuration?.years} lat, {branch?.offerDetails?.workDuration?.months} miesięcy, {branch.offerDetails?.workDuration?.days} dni</p>
            {/* <ApplyButton/> */}
            <ApplyButton branchId={branch.offerDetails.id} />
            {/* <ApplyButton branchId={branch.offerDetails.id} authToken={userAuthToken} /> */}

        </div>
    );
};

export default OfferDetailsPage;
