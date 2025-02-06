import React, { useState, useEffect } from 'react';
import { useParams, Link } from 'react-router-dom';
import { fetchCompanyBranches } from '../../services/CompanyService/CompanyService';

const CompanyDetailsPublic = () => {
    const storedCompanyDetails = JSON.parse(localStorage.getItem('companyDetails') || '{}');
    const { companyName, companyDescription, companyUrl } = storedCompanyDetails;    
    const { companyId } = useParams();
    const [companyInfo, setCompanyInfo] = useState(null);
    const [branches, setBranches] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [page, setPage] = useState(1);
    const [maxItems, setMaxItems] = useState(5);
    const [wojewodztwa, setWojewodztwa] = useState([]);
    const [selectedWojewodztwo, setSelectedWojewodztwo] = useState('');

    useEffect(() => {
        const fetchCompanyDetails = async () => {
            setLoading(true);
            
                const queryParams = new URLSearchParams({
                    orderBy: 'hierarchy',
                    ascending: true,
                    maxItems,
                    page,
                    wojewodztwo: selectedWojewodztwo || '', // Filtr według województwa
                }).toString();
    
            try {
                const data = await fetchCompanyBranches(companyId, queryParams);
                if (data.error){
                    throw new Error(data.error)
                }
                const uniqueWojewodztwa = [
                    ...new Set(data.items.map((item) => item.branch.address.hierarchy[0]?.name)),
                ];
                setWojewodztwa(uniqueWojewodztwa);
                setBranches(data.items.map((item) => ({
                    ...item.branch,
                    branchOffersCount: item.branchOffersCount || 0,
                })));
                setCompanyInfo(data.company);
            } catch (err) {
                setError('Error fetching company details');
                console.error(err);
            } finally {
                setLoading(false);
            }
        };

        fetchCompanyDetails();
    }, [companyId, maxItems, page, selectedWojewodztwo]);

    const handleMaxItemsChange = (e) => {
        setMaxItems(Number(e.target.value));
        setPage(1); // Resetuje stronę na pierwszą przy zmianie liczby elementów
    };

    const handleWojewodztwoChange = (e) => {
        setSelectedWojewodztwo(e.target.value);
        setPage(1); // Resetuje stronę na pierwszą przy zmianie województwa
    };

    if (loading) return <p>Loading company details...</p>;
    if (error) return <p>{error}</p>;

    return (
        <div className="company-details">
            <h1>{companyName || companyInfo?.name}</h1>
            <p><strong>Description:</strong> {companyDescription || companyInfo?.description || 'Brak opisu'}</p>
            <p><strong>URL:</strong> {companyUrl || companyInfo?.urlSegment}</p>

            <h2>Branches</h2>

            {/* Filtr według województwa */}
            <label htmlFor="wojewodztwo">Filters:</label>
            <select
                id="wojewodztwo"
                value={selectedWojewodztwo}
                onChange={handleWojewodztwoChange}
            >
                <option value="">All voivodeships</option>
                {wojewodztwa.map((wojewodztwo) => (
                    <option key={wojewodztwo} value={wojewodztwo}>
                        {wojewodztwo}
                    </option>
                ))}
            </select>
            <br/>

            {/* Wybór liczby elementów na stronie */}
            <label htmlFor="maxItems">Amount of branches on the page:</label>
            <select id="maxItems" value={maxItems} onChange={handleMaxItemsChange}>
                <option value={5}>5</option>
                <option value={10}>10</option>
                <option value={20}>20</option>
                <option value={30}>30</option>
                <option value={50}>50</option>
                <option value={100}>100</option>

            </select>

            {branches.length > 0 ? (
                <ul>
                    {branches.map((branch) => (
                        <li key={branch.id}>
                            <p>
                                <strong>Name:</strong> {branch.name || 'Brak nazwy'}
                            </p>
                            <p>
                                <strong>Address:</strong> ul. {branch.address?.street?.name}, {branch.address?.buildingNumber}/{branch.address?.apartmentNumber}, {branch.address?.zipCode}, {branch.address?.hierarchy[1]?.name}
                            </p>
                            <p>
                                <strong>Offers:</strong> {branch.branchOffersCount || 0}
                            </p>
                            <Link to={`/public/branch/${branch.id}`} className="hidden-link">View branch details</Link>
                        </li>
                    ))}
                </ul>
            ) : (
                <p>No available branches.</p>
            )}

            {/* Paginacja */}
            <div className="pagination">
                <button
                    onClick={() => setPage((prev) => Math.max(prev - 1, 1))}
                    disabled={page === 1}
                >
                    Previous
                </button>
                <span>Page: {page}</span>
                <button onClick={() => setPage((prev) => prev + 1)}>Next</button>
            </div>
            <h1>CompanyDetailsPublic</h1>
        </div>
    );
};

export default CompanyDetailsPublic;
