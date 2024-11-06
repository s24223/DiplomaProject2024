export const fetchRegistration = async (body) => {
    let responseData;
    await fetch('https://localhost:7166/api/User',{
        method: 'POST',
        headers:{
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
        body: JSON.stringify(body)
    }).then((res) => {
        if (!res.ok) throw new Error(res.body)
            else return res.json()
    }).then(data => responseData = data)
    return responseData
}