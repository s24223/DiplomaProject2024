import React from 'react';
import { Link } from 'react-router-dom';

const CompanyDetails = ({ companyInfo, branches, wojewodztwa, selectedWojewodztwo, onWojewodztwoChange, onMaxItemsChange }) => {
    return (
        <div>
            <h1>{companyInfo.name}</h1>
            <p><strong>Opis:</strong> {companyInfo.description || 'Brak opisu'}</p>
            <p><strong>URL:</strong> {companyInfo.urlSegment}</p>
            <h2>Oddziały</h2>
            <label htmlFor="wojewodztwo">Filtry:</label>
            <select
                id="wojewodztwo"
                value={selectedWojewodztwo}
                onChange={onWojewodztwoChange}
            >
                <option value="">Wszystkie województwa</option>
                {wojewodztwa.map((woj) => (
                    <option key={woj} value={woj}>
                        {woj}
                    </option>
                ))}
            </select>
            <br />
            <label htmlFor="maxItems">Liczba oddziałów na stronie:</label>
            <select id="maxItems" onChange={onMaxItemsChange}>
                {[5, 10, 20, 30, 50, 100].map((num) => (
                    <option key={num} value={num}>
                        {num}
                    </option>
                ))}
            </select>
            {branches.length > 0 ? (
                <ul>
                    {branches.map((branch) => (
                        <li key={branch.id}>
                            <p>
                                <strong>Nazwa:</strong> {branch.name || 'Brak nazwy'}
                            </p>
                            <p>
                                <strong>Adres:</strong> ul. {branch.address?.street?.name}, {branch.address?.buildingNumber}/{branch.address?.apartmentNumber}, {branch.address?.zipCode}, {branch.address?.hierarchy[1]?.name}
                            </p>
                            <p>
                                <strong>Oferty:</strong> {branch.branchOffersCount || 0}
                            </p>
                            <Link to={`/public/branch/${branch.id}`}>Zobacz szczegóły oddziału</Link>
                        </li>
                    ))}
                </ul>
            ) : (
                <p>Brak dostępnych oddziałów.</p>
            )}
        </div>
    );
};

export default CompanyDetails;
