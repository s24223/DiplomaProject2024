export const fetchLogin = async ({branchId}) => {
    let responseData = ''
    await fetch(`https://localhost:7166/api/Internship/recruitment?branchOfferId=${branchId}`,{
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
    .then(data => responseData = data)
    return responseData
}