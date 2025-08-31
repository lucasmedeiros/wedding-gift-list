import React, { useState, useEffect } from 'react';
import { Gift } from './types/Gift';
import { GiftService } from './services/giftService';
import { GiftCard } from './components/GiftCard';
import { TakeGiftModal } from './components/TakeGiftModal';

function App() {
  const [gifts, setGifts] = useState<Gift[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [selectedGift, setSelectedGift] = useState<Gift | null>(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isProcessing, setIsProcessing] = useState(false);

  const loadGifts = async () => {
    try {
      setLoading(true);
      setError(null);
      const giftsData = await GiftService.getGifts();
      setGifts(giftsData);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to load gifts');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadGifts();
  }, []);

  const handleTakeGift = (gift: Gift) => {
    setSelectedGift(gift);
    setIsModalOpen(true);
  };

  const handleConfirmTakeGift = async (guestName: string) => {
    if (!selectedGift) return;

    try {
      setIsProcessing(true);
      const updatedGift = await GiftService.takeGift(selectedGift.id, {
        guestName,
        version: selectedGift.version
      });
      
      setGifts(prev => prev.map(g => g.id === updatedGift.id ? updatedGift : g));
      setIsModalOpen(false);
      setSelectedGift(null);
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to take gift');
    } finally {
      setIsProcessing(false);
    }
  };

  const handleReleaseGift = async (gift: Gift) => {
    try {
      setError(null);
      const updatedGift = await GiftService.releaseGift(gift.id);
      setGifts(prev => prev.map(g => g.id === updatedGift.id ? updatedGift : g));
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Failed to release gift');
    }
  };

  const handleCloseModal = () => {
    if (!isProcessing) {
      setIsModalOpen(false);
      setSelectedGift(null);
    }
  };

  const handleRefresh = () => {
    loadGifts();
  };

  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-primary-500 mx-auto mb-4"></div>
          <p className="text-secondary-600">Carregando presentes de casamento...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gradient-to-br from-primary-50 via-white to-secondary-50">
      <div className="container mx-auto px-4 py-8">
        {/* Header */}
        <div className="text-center mb-12">
          <h1 className="text-4xl md:text-5xl font-elegant font-bold text-secondary-900 mb-4">
            Lista de Presentes
          </h1>
          <p className="text-lg text-secondary-600 max-w-2xl mx-auto leading-relaxed">
            Escolha um presente que você gostaria de nos dar para o nosso dia especial. Obrigado por celebrar conosco! 💕
          </p>
          <div className="flex justify-center mt-6">
            <button
              onClick={handleRefresh}
              className="inline-flex items-center px-4 py-2 bg-white border border-secondary-300 rounded-lg hover:bg-secondary-50 transition-colors text-secondary-700 font-medium"
            >
              <svg className="w-4 h-4 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
              </svg>
              Atualizar
            </button>
          </div>
        </div>

        {/* Error Message */}
        {error && (
          <div className="bg-red-50 border border-red-200 rounded-lg p-4 mb-8 max-w-2xl mx-auto">
            <div className="flex items-center">
              <svg className="w-5 h-5 text-red-500 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
              </svg>
              <p className="text-red-700">{error}</p>
              <button
                onClick={() => setError(null)}
                className="ml-auto text-red-400 hover:text-red-600"
              >
                <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
                </svg>
              </button>
            </div>
          </div>
        )}

        {/* Gift Stats */}
        <div className="flex justify-center mb-8">
          <div className="bg-white rounded-lg shadow-md px-6 py-4 border border-secondary-200">
            <div className="flex items-center space-x-6 text-sm">
              <div className="text-center">
                <div className="text-2xl font-bold text-secondary-900">
                  {gifts.length}
                </div>
                <div className="text-secondary-600">Total de Presentes</div>
              </div>
              <div className="text-center">
                <div className="text-2xl font-bold text-green-600">
                  {gifts.filter(g => !g.isTaken).length}
                </div>
                <div className="text-secondary-600">Disponíveis</div>
              </div>
              <div className="text-center">
                <div className="text-2xl font-bold text-primary-600">
                  {gifts.filter(g => g.isTaken).length}
                </div>
                <div className="text-secondary-600">Escolhidos</div>
              </div>
            </div>
          </div>
        </div>

        {/* Gifts Grid */}
        {gifts.length === 0 ? (
          <div className="text-center py-12">
            <svg className="w-16 h-16 text-secondary-300 mx-auto mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1} d="M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M9 12l6-3" />
            </svg>
            <p className="text-secondary-600 text-lg">Nenhum presente disponível</p>
          </div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8 max-w-7xl mx-auto">
            {gifts.map((gift) => (
              <GiftCard
                key={gift.id}
                gift={gift}
                onTake={handleTakeGift}
                onRelease={handleReleaseGift}
              />
            ))}
          </div>
        )}

        {/* Take Gift Modal */}
        <TakeGiftModal
          gift={selectedGift}
          isOpen={isModalOpen}
          onClose={handleCloseModal}
          onConfirm={handleConfirmTakeGift}
          isLoading={isProcessing}
        />
      </div>
    </div>
  );
}

export default App;
