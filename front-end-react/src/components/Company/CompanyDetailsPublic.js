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

    useEffect(() => {
        fetchCompanyDetails();
    }, [page, maxItems, selectedWojewodztwo]);

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
            <p><strong>Opis:</strong> {companyDescription || companyInfo?.description || 'Brak opisu'}</p>
            <p><strong>URL:</strong> {companyUrl || companyInfo?.urlSegment}</p>

            <h2>Oddziały</h2>

            {/* Filtr według województwa */}
            <label htmlFor="wojewodztwo">Filtry:</label>
            <select
                id="wojewodztwo"
                value={selectedWojewodztwo}
                onChange={handleWojewodztwoChange}
            >
                <option value="">Wszystkie województwa</option>
                {wojewodztwa.map((wojewodztwo) => (
                    <option key={wojewodztwo} value={wojewodztwo}>
                        {wojewodztwo}
                    </option>
                ))}
            </select>
            <br/>

            {/* Wybór liczby elementów na stronie */}
            <label htmlFor="maxItems">Liczba oddziałów na stronie:</label>
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
                                <strong>Nazwa:</strong> {branch.name || 'Brak nazwy'}
                            </p>
                            <p>
                                <strong>Adres:</strong> ul. {branch.address?.street?.name}, {branch.address?.buildingNumber}/{branch.address?.apartmentNumber}, {branch.address?.zipCode}, {branch.address?.hierarchy[1]?.name}
                            </p>
                            <p>
                                <strong>Oferty:</strong> {branch.branchOffersCount || 0}
                            </p>
                            <Link to={`/public/branch/${branch.id}`} className="hidden-link">Zobacz szczegóły oddziału</Link>
                        </li>
                    ))}
                </ul>
            ) : (
                <p>Brak dostępnych oddziałów.</p>
            )}

            {/* Paginacja */}
            <div className="pagination">
                <button
                    onClick={() => setPage((prev) => Math.max(prev - 1, 1))}
                    disabled={page === 1}
                >
                    Poprzednia
                </button>
                <span>Strona: {page}</span>
                <button onClick={() => setPage((prev) => prev + 1)}>Następna</button>
            </div>
            <h1>CompanyDetailsPublic</h1>
        </div>
    );
};

export default CompanyDetailsPublic;
