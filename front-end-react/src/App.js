// export default App;
import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import MainPage from './pages/MainPage/MainPage';
import OfferDetailsPage from './pages/OfferDetailsPage/OfferDetailsPage';

function App() {
    return (
        <Router>
            <Routes>
                <Route path="/" element={<MainPage />} />
                <Route path="/offers" element={<MainPage />} />
                <Route path="/offers/:offerId" element={<OfferDetailsPage />} />
            </Routes>
        </Router>
    );
}

export default App;
