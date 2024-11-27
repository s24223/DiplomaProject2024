export const fetchNotificationGetAuthorized = async () =>{
    let response = await fetch('https://localhost:7166/api/User/notifications/authorized', {
        method: 'GET',
        headers: {
            "Content-Type": "application/json",
            "Access-Contorl-Allow-Origin": "*",
            "Authorization": `Bearer ${sessionStorage.getItem("jwt")}`
        }
    })

    return await response.json()
}

export const fetchNotificationPostAuthorized = async (body) => {
    return await fetch('https://localhost:7166/api/User/notifications/authorized', {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Access-Contorl-Allow-Origin": "*",
            "Authorization": `Bearer ${sessionStorage.getItem("jwt")}`
        },
        body: JSON.stringify(body)
    })
}