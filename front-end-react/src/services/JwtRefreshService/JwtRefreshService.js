import axios from "axios";

export const jwtRefresh = async () => {
    if (!localStorage.getItem("jwt")) {
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

    if ((jwtExparation - currentTime) <= 10) {
        if ((refreshExparation - currentTime) <= 1) {
            // log out
            clearStorage();
        }
        else {
            // refresh
            return await axios.post("https://localhost:7166/api/User/refresh", { "refreshToken": localStorage.getItem("refreshToken") }, {
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${localStorage.getItem("jwt")}`,
                    "Access-Control-Allow-Origin": "*"
                },
            }).then((res) => {
                localStorage.setItem("jwt", res.data.jwt);
                localStorage.setItem("jwtValidTo", res.data.jwtValidTo);
                localStorage.setItem("refreshToken", res.data.refereshToken);
                localStorage.setItem("refreshTokenValidTo", res.data.refereshTokenValidTo);
            }).catch(error => {
                switch (error.response.status) {
                    case 500:
                        const idAppProblem = error.response.data.ProblemId
                        window.location.href = `/notification/create/${idAppProblem}`
                        break;
                    default:
                        return { error: error.response.data.message }
                }
            })
        }
    }

    // do nothing
}