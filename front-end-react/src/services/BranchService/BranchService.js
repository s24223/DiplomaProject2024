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
    let response = await fetch('https://localhost:7166/api/User/company/branches/core',{
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

export const fetchBranchDetails = async (branchId) => {
    const response = await fetch(
        `https://localhost:7166/api/BranchOffers/branches/${branchId}/branchOffers`
    );
    if (!response.ok) {
        throw new Error('Failed to fetch branch details');
    }
    return await response.json();
};

// export const updateBranchDetails = async (branchId, updatedData) => {
//     const response = await fetch(
//         `https://localhost:7166/api/Branches/${branchId}`,
//         {
//             method: 'PUT',
//             headers: {
//                 'Content-Type': 'application/json',
//                 Authorization: `Bearer ${sessionStorage.getItem('jwt')}`,
//             },
//             body: JSON.stringify(updatedData),
//         }
//     );
//     if (!response.ok) {
//         throw new Error('Failed to update branch details');
//     }
//     return await response.json();
// };
