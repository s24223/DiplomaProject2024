import React, { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';

const BranchDetailsPublic = () => {
    const { branchId } = useParams();
    const [branchInfo, setBranchInfo] = useState(null);
    const [offers, setOffers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const fetchBranchDetails = async () => {
        try {
            setLoading(true);
            const response = await fetch(
                `https://localhost:7166/api/BranchOffers/branches/${branchId}/branchOffers`
            );
            if (!response.ok) throw new Error('Failed to fetch branch details');

            const data = await response.json();
            setBranchInfo({
                ...data.items[0]?.branch,
                branchOffersCount: data.items.length,
            });
            setOffers(data.items.map((item) => item.offer));
        } catch (err) {
            console.error("Error fetching branch details:", err);
            setError("Failed to load branch details.");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchBranchDetails();
    }, [branchId]);

    if (loading) return <p>Loading branch details...</p>;
    if (error) return <p>{error}</p>;

    return (
        <div className="branch-details-public">
            
            <h2>Oferty w tym oddziale</h2>
            {offers.length > 0 ? (
                <ul>
                    {offers.map((offer) => (
                        <li key={offer.id}>
                            <h3>
                            <Link to={`/offers/${offer.id}`}>
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
};

export default BranchDetailsPublic;
