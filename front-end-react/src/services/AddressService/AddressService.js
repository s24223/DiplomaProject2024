export const fetchAddressPost = async (passedBody) => {
    const response = await fetch(`https://localhost:7166/api/Address`, {
        method: "POST",
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
        body: JSON.stringify(passedBody)
    })

    return await response.json()
}