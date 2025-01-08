import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { fetchBranchDetails } from '../../services/BranchService/BranchService';
import BranchDetailsPublic from '../../components/BranchDetails/BranchDetailsPublic';

const BranchDetailsPagePublic = () => {
    const { branchId } = useParams();
    const [branchInfo, setBranchInfo] = useState(null);
    const [offers, setOffers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const loadBranchDetails = async () => {
            try {
                const data = await fetchBranchDetails(branchId);
                setBranchInfo({
                    ...data.items[0]?.branch,
                    branchOffersCount: data.items.length,
                });
                setOffers(data.items.map((item) => item.offer));
            } catch (err) {
                setError('Failed to load branch details');
            } finally {
                setLoading(false);
            }
        };
        loadBranchDetails();
    }, [branchId]);

    if (loading) return <p>Loading...</p>;
    if (error) return <p>{error}</p>;

    return <BranchDetailsPublic branchInfo={branchInfo} offers={offers} />;
};

export default BranchDetailsPagePublic;
