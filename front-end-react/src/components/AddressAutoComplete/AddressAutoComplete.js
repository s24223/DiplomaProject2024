import React, { useState } from "react";
import { GeoapifyGeocoderAutocomplete, GeoapifyContext } from '@geoapify/react-geocoder-autocomplete'
import '@geoapify/geocoder-autocomplete/styles/minimal.css'
import { fetchAddressPost } from "../../services/AddressService/AddressService";

const api_key = "17217a7c061e45d28862b5f3c4e2a91f"

const AddressAutocomplete = ({ childToParent }) => {
    const [language] = useState('pl')
    const [apartmentNumber, setApratmentNumber] = useState(null)
    const [houseNumber, setHouseNumber] = useState(null)
    const [mapTile, setMapTile] = useState()
    const [address, setAddress] = useState()

    function onPlaceSelect(value) {
        if (!value)
            return
        const lonlat = `${value.properties.lon},${value.properties.lat}`
        setMapTile(`https://maps.geoapify.com/v1/staticmap?style=osm-bright&width=600&height=400&center=lonlat:${lonlat}&zoom=15.7648&marker=lonlat:${lonlat};color:%23ff0000;size:medium&apiKey=${api_key}`)
        setAddress(value.properties)
    }

    function onSuggectionChange(value) {
        setMapTile()
    }

    function confirmAddress(e) {
        e.preventDefault()

        if (!address)
            return

        const body = {
            "country": address.country,
            "state": address.state,
            "county": address.county,
            "municipality": address.municipality,
            "city": address.city,
            "postcode": address.postcode,
            "suburb": address.suburb,
            "street": address.street,
            "lon": address.lon,
            "lat": address.lat,
            "houseNumber": houseNumber,
            "apartmentNumber": apartmentNumber
        }

        Object.keys(body).forEach(key => body[key] === undefined ? delete body[key] : {});

        const fetchDummy = async () => {
            const res = await fetchAddressPost(body)
            if(res.error){
                throw new Error(res.error)
            }
            childToParent(res.item.addressId)
        }

        fetchDummy()
    }

    return (
        <div className="address-div">
            <GeoapifyContext apiKey={api_key}>
                <GeoapifyGeocoderAutocomplete id='address-api' placeholder="Enter address here"
                    lang={language}
                    filterByCountryCode={['pl']}
                    placeSelect={onPlaceSelect}
                    suggestionsChange={onSuggectionChange}
                />
            </GeoapifyContext>
            <input type="text" placeholder="House Number" onChange={(e) => setHouseNumber(e.target.value)} /><br />
            <input type="text" placeholder="Apartment Number" onChange={(e) => setApratmentNumber(e.target.value)} /><br />
            {mapTile && <><img alt="" width="600" height="400" src={mapTile} /><br /></>}
            <button id="address-page-button" onClick={e => confirmAddress(e)}>Confirm address</button>
        </div>
    )
}

export default AddressAutocomplete