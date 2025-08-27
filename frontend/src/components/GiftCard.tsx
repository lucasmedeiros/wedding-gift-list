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
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric'
    });
  };

  return (
    <div className={`bg-white rounded-xl shadow-lg overflow-hidden transition-all duration-300 hover:shadow-xl transform hover:-translate-y-1 ${
      gift.isTaken ? 'opacity-60' : ''
    }`}>
      <div className="aspect-w-16 aspect-h-12 overflow-hidden">
        <img 
          src={gift.imageUrl} 
          alt={gift.name}
          className={`w-full h-48 object-cover transition-all duration-300 ${
            gift.isTaken ? 'grayscale' : 'hover:scale-105'
          }`}
          onError={(e) => {
            const target = e.target as HTMLImageElement;
            target.src = `https://images.unsplash.com/photo-1513475382585-d06e58bcb0e0?w=400&h=300&fit=crop&auto=format`;
          }}
        />
      </div>
      
      <div className="p-6">
        <div className="flex justify-between items-start mb-2">
          <h3 className="text-xl font-elegant font-semibold text-secondary-900 leading-tight">
            {gift.name}
          </h3>
          {gift.isTaken && (
            <span className="bg-secondary-200 text-secondary-700 px-2 py-1 rounded-full text-xs font-medium">
              Taken
            </span>
          )}
        </div>
        
        <p className="text-secondary-600 text-sm mb-4 leading-relaxed">
          {gift.description}
        </p>
        
        {gift.isTaken ? (
          <div className="space-y-2">
            <p className="text-sm text-secondary-500">
              <span className="font-medium">Taken by:</span> {gift.takenByGuestName}
            </p>
            <p className="text-sm text-secondary-500">
              <span className="font-medium">Date:</span> {formatDate(gift.takenAt)}
            </p>
            <button
              onClick={() => onRelease(gift)}
              className="w-full bg-secondary-100 hover:bg-secondary-200 text-secondary-700 font-medium py-2 px-4 rounded-lg transition-colors duration-200 text-sm"
            >
              Release Gift
            </button>
          </div>
        ) : (
          <button
            onClick={() => onTake(gift)}
            className="w-full bg-gradient-to-r from-primary-500 to-primary-600 hover:from-primary-600 hover:to-primary-700 text-white font-medium py-3 px-4 rounded-lg transition-all duration-200 transform hover:scale-105 active:scale-95 shadow-md hover:shadow-lg"
          >
            I'll Take This Gift
          </button>
        )}
      </div>
    </div>
  );
};
