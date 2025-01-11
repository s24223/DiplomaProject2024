import React from "react";
import { Link } from "react-router-dom";

const CompanyBranchList = ({ branches }) => {
    if (!branches || branches.length === 0) {
        return <p>Brak dostępnych oddziałów.</p>;
    }

    return (
        <ul>
            {branches.map((branch) => (
                <li key={branch.id}>
                    <p>
                        <strong>Nazwa:</strong> {branch.name || "Brak nazwy"}
                    </p>
                    <p>
                        <strong>Adres:</strong> ul. {branch.address?.street?.name}, {branch.address?.buildingNumber}/
                        {branch.address?.apartmentNumber}, {branch.address?.zipCode}, {branch.address?.hierarchy[1]?.name}
                    </p>
                    <p>
                        <strong>Oferty:</strong> {branch.branchOffersCount || 0}
                    </p>
                    <Link to={`/public/branch/${branch.id}`}>Zobacz szczegóły oddziału</Link>
                </li>
            ))}
        </ul>
    );
};

export default CompanyBranchList;
