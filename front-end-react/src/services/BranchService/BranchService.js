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
    return response
}