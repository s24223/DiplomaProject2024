export const fetchBranchPost = async (body) => {
    let response = await fetch('https://localhost:7166/api/User/company/branches', {
        method: 'POST',
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${sessionStorage.getItem("jwt")}`,
            "Access-Contorl-Allow-Origin": "*"
        },
        body: JSON.stringify(body)
    })
    return response;
}

export const fetchBranchGet = async () => {
    let response = await fetch('https://localhost:7166/api/User/company/branches',{
        method: 'GET',
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${sessionStorage.getItem("jwt")}`,
            "Access-Contorl-Allow-Origin": "*"
        }
    })
    return await response.json();
}

export const fetchBranchPut = async (body) => {
    let response = await fetch('https://localhost:7166/api/User/company/branches',{
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${sessionStorage.getItem("jwt")}`,
            "Access-Contorl-Allow-Origin": "*"
        },
        body: JSON.stringify(body)
    })
    return response
}