export const fetchLogout = () => {
    fetch('https://localhost:7166/api/User/logOut', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*',
            'Authorization': `Bearer ${sessionStorage.getItem("jwt")}`
        }
    }).then(response => response.json())
    sessionStorage.removeItem("jwt")
    sessionStorage.removeItem("jwtValidTo")
    sessionStorage.removeItem("refreshToken")
    sessionStorage.removeItem("refreshTokenValidTo")
}