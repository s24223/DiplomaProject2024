import React, { useState } from "react";
import { GeoapifyGeocoderAutocomplete, GeoapifyContext } from '@geoapify/react-geocoder-autocomplete'
import '@geoapify/geocoder-autocomplete/styles/minimal.css'

const api_key = ""

const AddressAutocomplete = () => {
    const [language] = useState('pl')
    const [countryCodes] = useState('pl')
    const [place, setPlace] = useState(null)
    const [mapTile, setMapTile] = useState()

    function onPlaceSelect(value) {
        // console.log(value);
        setPlace(value)
        
        const lonlat = `${value.properties.lon},${value.properties.lat}`
        setMapTile(`https://maps.geoapify.com/v1/staticmap?style=osm-bright&width=600&height=400&center=lonlat:${lonlat}&zoom=15.7648&marker=lonlat:${lonlat};color:%23ff0000;size:medium&apiKey=${api_key}`)
    }
     
    function onSuggectionChange(value) {
        // console.log(value);
        setMapTile()
    }

    return(
        <div>
            <GeoapifyContext apiKey={api_key}>
                <GeoapifyGeocoderAutocomplete placeholder="Enter address here"
                  lang={language}
                  countryCodes={countryCodes}
                  placeSelect={onPlaceSelect}
                  suggestionsChange={onSuggectionChange}
                  />
            </GeoapifyContext>
            {mapTile && <img width="600" height="400" src={mapTile} />}
        </div>
    )
}

export default AddressAutocomplete