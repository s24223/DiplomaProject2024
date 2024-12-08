import React, { useState } from "react";
import { GeoapifyGeocoderAutocomplete, GeoapifyContext } from '@geoapify/react-geocoder-autocomplete'
import '@geoapify/geocoder-autocomplete/styles/minimal.css'

const AddressAutocomplete = () => {
    const [language] = useState('pl')
    const [countryCodes] = useState('pl')

    const [place, setPlace] = useState(null)

    function onPlaceSelect(value) {
        console.log(value);
        setPlace(value)
    }
     
    function onSuggectionChange(value) {
        console.log(value);
    }

    return(
        <div>
            <GeoapifyContext apiKey="">
                <GeoapifyGeocoderAutocomplete placeholder="Enter address here"
                  lang={language}
                  countryCodes={countryCodes}
                  placeSelect={onPlaceSelect}
                  suggestionsChange={onSuggectionChange}
                  />
            </GeoapifyContext>
        </div>
    )
}

export default AddressAutocomplete