import { Navigate } from "react-router-dom";

export const jwtRefresh = async () => {
    if (!localStorage.getItem("jwt")){
        return
    }

    let clearStorage = () => {
        localStorage.removeItem("jwt");
        localStorage.removeItem("jwtValidTo");
        localStorage.removeItem("refreshTokenValidTo");
        localStorage.removeItem("refreshToken");
        window.location.reload();
    }

    let jwtExparation = Date.parse(localStorage.getItem("jwtValidTo"))
    let refreshExparation = Date.parse(localStorage.getItem("refreshTokenValidTo"))
    let currentTime = Date.parse(new Date())

    if ((jwtExparation - currentTime) <= 10){
        if((refreshExparation - currentTime) <= 1){
            // log out
            clearStorage();
        }
        else{
            // refresh
            let response = await fetch("https://localhost:7166/api/User/refresh", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${localStorage.getItem("jwt")}`,
                    "Access-Control-Allow-Origin": "*"
                },
                body: JSON.stringify({"refreshToken": localStorage.getItem("refreshToken")})
            })
            if (response.ok){
                response = await response.json();
                localStorage.setItem("jwt", response.jwt);
                localStorage.setItem("jwtValidTo", response.jwtValidTo);
                localStorage.setItem("refreshToken", response.refereshToken);
                localStorage.setItem("refreshTokenValidTo", response.refereshTokenValidTo);
            }
            else{
                if (response.status === 500){
                    response = await response.json();
                    Navigate({
                        to: "/notification/create", 
                        state:{
                            idAppProblem: response.idAppProblem
                        }
                    })
                }
            }
        }
    }

    // do nothing
}