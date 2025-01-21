import React from 'react';
import { jwtRefresh } from '../../services/JwtRefreshService/JwtRefreshService';

const TestPage = () => {
    jwtRefresh()
    return(
        <div>

        </div>
    )
}

export default TestPage