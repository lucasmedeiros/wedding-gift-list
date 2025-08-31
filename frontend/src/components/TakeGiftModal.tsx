import React, { useState } from 'react';
import { Gift } from '../types/Gift';

interface TakeGiftModalProps {
  gift: Gift | null;
  isOpen: boolean;
  onClose: () => void;
  onConfirm: (guestName: string) => void;
  isLoading: boolean;
}

export const TakeGiftModal: React.FC<TakeGiftModalProps> = ({
  gift,
  isOpen,
  onClose,
  onConfirm,
  isLoading
}) => {
  const [guestName, setGuestName] = useState('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (guestName.trim()) {
      onConfirm(guestName.trim());
    }
  };

  if (!isOpen || !gift) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center p-4 z-50">
      <div className="bg-white rounded-xl shadow-2xl max-w-md w-full max-h-[90vh] overflow-y-auto">
        <div className="p-6">
          <div className="flex justify-between items-start mb-4">
            <h2 className="text-2xl font-elegant font-semibold text-secondary-900">
              Escolher Presente
            </h2>
            <button
              onClick={onClose}
              className="text-secondary-400 hover:text-secondary-600 transition-colors"
              disabled={isLoading}
            >
              <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>
          
          <div className="mb-6">
            <h3 className="text-lg font-semibold text-secondary-900 mb-2">
              {gift.name}
            </h3>
            <p className="text-secondary-600 text-sm">
              {gift.description}
            </p>
          </div>
          
          <form onSubmit={handleSubmit} className="space-y-4">
            <div>
              <label htmlFor="guestName" className="block text-sm font-medium text-secondary-700 mb-2">
                Seu Nome
              </label>
              <input
                type="text"
                id="guestName"
                value={guestName}
                onChange={(e) => setGuestName(e.target.value)}
                placeholder="Digite seu nome completo"
                className="w-full px-4 py-3 border border-secondary-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent outline-none transition-all"
                required
                disabled={isLoading}
                maxLength={100}
              />
            </div>
            
            <div className="flex space-x-3 pt-4">
              <button
                type="button"
                onClick={onClose}
                className="flex-1 px-4 py-3 border border-secondary-300 text-secondary-700 rounded-lg hover:bg-secondary-50 transition-colors font-medium"
                disabled={isLoading}
              >
                Cancelar
              </button>
              <button
                type="submit"
                disabled={!guestName.trim() || isLoading}
                className="flex-1 px-4 py-3 bg-gradient-to-r from-primary-500 to-primary-600 hover:from-primary-600 hover:to-primary-700 disabled:from-secondary-300 disabled:to-secondary-400 text-white rounded-lg transition-all font-medium disabled:cursor-not-allowed flex items-center justify-center"
              >
                {isLoading ? (
                  <>
                    <svg className="animate-spin -ml-1 mr-2 h-4 w-4 text-white" fill="none" viewBox="0 0 24 24">
                      <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
                      <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
                    </svg>
                    Escolhendo...
                  </>
                ) : (
                  'Confirmar'
                )}
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};
