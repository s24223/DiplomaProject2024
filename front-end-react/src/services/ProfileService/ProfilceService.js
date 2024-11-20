export const fetchProfileGet = async () => {
    const response = await fetch("https://localhost:7166/api/User",
        {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Access-Contorl-Allow-Origin": "*",
                "Authorization": `Bearer ${sessionStorage.getItem("jwt")}`
            }
        }
    );
    if (!response.ok) 
        throw new Error("Error fetching profile");
    return await response.json();
}

export const fetchProfilePost = async (body) => {
    await fetch("https://localhost:7166/api/User/person",
        {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Access-Contorl-Allow-Origin": "*",
                "Authorization": `Bearer ${sessionStorage.getItem("jwt")}`
            },
            body: JSON.stringify(body)
        }
    ).then((response) => {
        if (!response.ok)
            throw new Error(response.status)
        else console.log(response.json())
    });
}