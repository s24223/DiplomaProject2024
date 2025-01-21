export const fetchLogout = () => {
    fetch('https://localhost:7166/api/User/logOut', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Access-Control-Allow-Origin': '*',
            'Authorization': `Bearer ${localStorage.getItem("jwt")}`
        }
    }).then(response => response.json())
    localStorage.removeItem("jwt")
    localStorage.removeItem("jwtValidTo")
    localStorage.removeItem("refreshToken")
    localStorage.removeItem("refreshTokenValidTo")
}