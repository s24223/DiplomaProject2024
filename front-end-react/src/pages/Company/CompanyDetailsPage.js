import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { fetchCompanyDetails } from '../../services/CompanyService/CompanyService';
import CompanyDetails from '../../components/Public/CompanyDetails';

const CompanyDetailsPage = () => {
    const { companyId } = useParams();
    const [companyInfo, setCompanyInfo] = useState(null);
    const [branches, setBranches] = useState([]);
    const [wojewodztwa, setWojewodztwa] = useState([]);
    const [selectedWojewodztwo, setSelectedWojewodztwo] = useState('');
    const [page, setPage] = useState(1);
    const [maxItems, setMaxItems] = useState(5);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const loadCompanyDetails = async () => {
            try {
                const data = await fetchCompanyDetails(companyId, page, maxItems, selectedWojewodztwo);
                setCompanyInfo(data.company);
                setBranches(data.items.map((item) => ({
                    ...item.branch,
                    branchOffersCount: item.branchOffersCount || 0,
                })));
                setWojewodztwa([
                    ...new Set(data.items.map((item) => item.branch.address.hierarchy[0]?.name)),
                ]);
            } catch (err) {
                setError('Failed to load company details');
            } finally {
                setLoading(false);
            }
        };
        loadCompanyDetails();
    }, [companyId, page, maxItems, selectedWojewodztwo]);

    if (loading) return <p>Loading...</p>;
    if (error) return <p>{error}</p>;

    return (
        <CompanyDetails
        
            companyInfo={companyInfo}
            branches={branches}
            wojewodztwa={wojewodztwa}
            selectedWojewodztwo={selectedWojewodztwo}
            onWojewodztwoChange={(e) => setSelectedWojewodztwo(e.target.value)}
            onMaxItemsChange={(e) => setMaxItems(Number(e.target.value))}
        />
    );
};

export default CompanyDetailsPage;
