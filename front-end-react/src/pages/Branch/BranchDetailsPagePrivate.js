import React, { useState, useEffect } from 'react'
import { Link, useLocation } from 'react-router-dom';
import { fetchBranchPut } from '../../services/BranchService/BranchService';
import CancelButton from '../../components/Buttons/CancelButton/CancelButton';
import CreateOffer from '../../components/OfferCreate/CreateOffer';
import { fetchBranchOffers } from '../../services/OffersService/OffersService';
import AddressImage from '../../components/AddressImage/AddressImage';

const BranchDetailPagePrivate = () => {
    const [editMode, setEditMode] = useState(false);
    const item = useLocation().state.item

    const [name, setName] = useState(item.name)
    const [description, setDescription] = useState(item.description)
    const [urlSegment, setUrlsegmet] = useState(item.urlSegment)

    const [messageStatus, setMessageStatus] = useState('')

    const [showCreateOffer, setShowCreateOffer] = useState(false);

    const [offers, setOffers] = useState([]);
    const [loadingOffers, setLoadingOffers] = useState(true);
    const [error, setError] = useState(null);


    const sleep = ms => new Promise(r => setTimeout(r, ms));


    const fetchOffers = async () => {
        try {
            setLoadingOffers(true);
            setError(null); // Resetujemy błędy przed nowym fetchowaniem
            const data = await fetchBranchOffers(item.id);
            setOffers(data.items || []); // Zapewniamy, że dane to tablica
            setLoadingOffers(false);
        } catch (err) {
            console.error("Error fetching offers:", err);
            setError("Failed to fetch offers for this branch.");
            setOffers([]); // Upewnij się, że lista jest pusta w przypadku błędu
            setLoadingOffers(false);
        }
    };
    

    useEffect(() => {
        fetchOffers();
    }, []);

    const handleChange = () => {
        const fetchDummy = async () => {
            let response = await fetchBranchPut([{
                "branchId": item.id,
                "addressId": item.addressId,
                "urlSegment": urlSegment,
                "name": name,
                "description": description
            }])

            if(response.ok){
                await response.json()
                item.name = name
                item.description = description
                item.urlSegment = urlSegment
                setMessageStatus('Success')
                await sleep(1000)
                setMessageStatus('')
            }
        }

        fetchDummy()
    }

    return(
        <div className='centered'>
            <h1>BranchDetailsPage</h1>
            {
            item && 
            <div>

                <button onClick={() => setEditMode(!editMode)}>Edit mode</button><br />
                Name: {editMode? <input type='text' placeholder={item.name} onChange={e => setName(e.target.value)} value={name} />: item.name}<br />
                Description: {editMode? <input type='text' placeholder={item.description} onChange={e => setDescription(e.target.value)} value={description} /> : item.description? item.description : <>NaN</>}<br />
                Url Segment: {editMode? <input type='text' placeholder={item.urlSegment} onChange={e => setUrlsegmet(e.target.value)} value={urlSegment} /> : item.urlSegment? item.urlSegment : <>NaN</>}<br />
                {editMode && <button onClick={handleChange}>Change</button>}
                <br/>
                
                {/* <p>comapnyID: {item.companyId}</p>*/}
                {/* <p>branchID: {item.id}</p>  */}
                <p>ul. {item.address.street.name}  {item.address.buildingNumber} m.  {item.address.apartmentNumber}</p>
                Address: {item.address.hierarchy.map((addressPart) => (
                    <p key={addressPart.id}>{addressPart.administrativeType.name}: {addressPart.name}</p>
                ))}
                <p><AddressImage lon={item.address.lon} lat={item.address.lat} /></p>
                
                <h2>Offers</h2>
                {loadingOffers ? (
                    <p>Loading offers...</p>
                ) : error ? (
                    <p style={{ color: "red" }}>{error}</p>
                ) : offers.length > 0 ? (
                    <ul>
                        {offers.map(({ offer }) => (
                            <li key={offer.id}>
                                <p>{offer.name}</p>
                                <Link to={`/offers/${offer.id}/edit`}>Edit</Link>
                            </li>
                        ))}
                    </ul>

                ) : (
                    <p>No offers available.</p>
                )}

                <label style={{color:'green'}}>{messageStatus}</label><br />
                
                <button onClick={() => setShowCreateOffer(true)}>Add Offer</button>
                <CancelButton/>
                {showCreateOffer && (
                    <CreateOffer
                        branchId={item.id}
                        
                        onClose={() =>{ setShowCreateOffer(false);
                        fetchOffers();
                    }}
                        />
                )}
                
            </div>
            }
        </div>
    )
}

export default BranchDetailPagePrivate;

