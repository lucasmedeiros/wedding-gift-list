import React from 'react';
import { Gift } from '../types/Gift';

interface GiftCardProps {
  gift: Gift;
  onTake: (gift: Gift) => void;
  onRelease: (gift: Gift) => void;
}

export const GiftCard: React.FC<GiftCardProps> = ({ gift, onTake, onRelease }) => {
  const formatDate = (dateString?: string) => {
    if (!dateString) return '';
    return new Date(dateString).toLocaleDateString('pt-BR', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  };

  return (
    <div className={`bg-white rounded-xl shadow-lg overflow-hidden transition-all duration-300 hover:shadow-xl transform hover:-translate-y-1 ${
      gift.isTaken ? 'opacity-60' : ''
    }`}>
      <div className="p-6">
        <div className="flex justify-between items-start mb-2">
          <h3 className="text-xl font-elegant font-semibold text-secondary-900 leading-tight">
            {gift.name}
          </h3>
          {gift.isTaken && (
            <span className="bg-secondary-200 text-secondary-700 px-2 py-1 rounded-full text-xs font-medium">
              Escolhido
            </span>
          )}
        </div>
        
        <p className="text-secondary-600 text-sm mb-4 leading-relaxed">
          {gift.description}
        </p>
        
        {gift.isTaken ? (
          <div className="space-y-2">
            <p className="text-sm text-secondary-500">
              <span className="font-medium">Escolhido por:</span> {gift.takenByGuestName}
            </p>
            <p className="text-sm text-secondary-500">
              <span className="font-medium">Data:</span> {formatDate(gift.takenAt)}
            </p>
            <button
              // onClick={() => onRelease(gift)}
              disabled={true}
              className="w-full bg-secondary-100 text-secondary-700 font-medium py-2 px-4 rounded-lg transition-colors duration-200 text-sm disabled:cursor-not-allowed"
            >
              Presente escolhido
            </button>
          </div>
        ) : (
          <button
            onClick={() => onTake(gift)}
            className="w-full bg-gradient-to-r from-primary-500 to-primary-600 hover:from-primary-600 hover:to-primary-700 text-white font-medium py-3 px-4 rounded-lg transition-all duration-200 transform hover:scale-105 active:scale-95 shadow-md hover:shadow-lg"
          >
            Escolher
          </button>
        )}
      </div>
    </div>
  );
};
