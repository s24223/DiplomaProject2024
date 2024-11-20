export const fetchLogin = async (body) => {
    let responseData = ''
    await fetch('https://localhost:7166/api/User/login',{
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*'
        },
        body: JSON.stringify(body)
    }).then((res) => {
        if (!res.ok) throw new Error(res.status)
            else return res.json();
    })
    .then(data => responseData = data).then(res => console.log(res))
    return responseData
}