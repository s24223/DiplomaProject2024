import React, { useState, useEffect } from "react";
import CreateOfferForm from "../../components/Offer/CreateOfferForm";
import { assignOfferToBranch, createOffer } from "../../api/OffersService";
import { fetchCharacteristics } from "../../api/CharacteristicsService";

const OfferCreatePage = ({ branchId, onClose }) => {
  const [characteristics, setCharacteristics] = useState([]);
  const [loading, setLoading] = useState(true);
  const [message, setMessage] = useState("");

  useEffect(() => {
    const loadCharacteristics = async () => {
      try {
        const data = await fetchCharacteristics();
        setCharacteristics(data);
        setLoading(false);
      } catch (error) {
        console.error("Error loading characteristics", error);
        setMessage("Failed to load characteristics.");
        setLoading(false);
      }
    };
    loadCharacteristics();
  }, []);

  const handleCreateOffer = async (offerData) => {
    try {
      const createdOffer = await createOffer([offerData]);
      const publishData = [
        {
          branchId,
          offerId: createdOffer.id,
          ...offerData.publishDetails,
        },
      ];
      await assignOfferToBranch(publishData);
      setMessage("Offer created and added to branch successfully!");
      setTimeout(onClose, 2000);
    } catch (error) {
      console.error("Error creating offer", error);
      setMessage("Failed to create offer.");
    }
  };

  if (loading) {
    return <p>Loading...</p>;
  }

  return (
    <div>
      <h1>Create Offer</h1>
      <CreateOfferForm
        characteristics={characteristics}
        onSubmit={handleCreateOffer}
        onClose={onClose}
      />
      {message && <p>{message}</p>}
    </div>
  );
};

export default OfferCreatePage;
