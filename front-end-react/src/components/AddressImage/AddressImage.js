import React from "react";

const AddressImage = ({lon, lat}) => {
    const api_key = "17217a7c061e45d28862b5f3c4e2a91f"
    const imgSrc = `https://maps.geoapify.com/v1/staticmap?style=osm-bright&width=600&height=400&center=lonlat:${lon},${lat}&zoom=15.7648&marker=lonlat:${lon},${lat};color:%23ff0000;size:medium&apiKey=${api_key}`

    return(
        <img width="600" height="400" src={imgSrc} />
    )
}

export default AddressImage