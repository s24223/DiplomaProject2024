import React from 'react';
import { Link } from 'react-router-dom';

const BranchDetailsPublic = ({ branchInfo, offers }) => (
    <div>
        <h1>Branch DetailsPublic</h1>
        {/* <h1>Oddział: {branchInfo.name}</h1> */}
        {/* <p><strong>Opis:</strong> {branchInfo.description || 'Brak opisu'}</p> */}
        <h2>Oferty w tym oddziale</h2>
        {offers.length > 0 ? (
            <ul>
                {offers.map((offer) => (
                    <li key={offer.id}>
                        <h3>
                            <Link to={`/offers/${offer.id}`} className="hidden-link">
                                {offer.name || 'Brak nazwy'}
                            </Link>
                        </h3>
                        <p><strong>Opis:</strong> {offer.description || 'Brak opisu'}</p>
                        <p>
                            <strong>Wynagrodzenie:</strong> {offer.minSalary} - {offer.maxSalary} PLN
                        </p>
                    </li>
                ))}
            </ul>
        ) : (
            <p>Brak ofert w tym oddziale.</p>
        )}
    </div>
);

export default BranchDetailsPublic;
