import { Gift, TakeGiftRequest } from '../types/Gift';

// Use environment variable for production, localhost for development
const API_BASE_URL = process.env.REACT_APP_API_URL || 
  (process.env.NODE_ENV === 'production' 
    ? 'https://api.projectslucas.dev'
    : 'http://localhost:5139');

export class GiftService {
  private static async handleResponse<T>(response: Response): Promise<T> {
    if (!response.ok) {
      const errorData = await response.json().catch(() => ({}));
      throw new Error(errorData.message || `HTTP error! status: ${response.status}`);
    }
    return response.json();
  }

  static async getGifts(): Promise<Gift[]> {
    const response = await fetch(`${API_BASE_URL}/api/gifts`);
    return this.handleResponse<Gift[]>(response);
  }

  static async takeGift(giftId: number, request: TakeGiftRequest): Promise<Gift> {
    const response = await fetch(`${API_BASE_URL}/api/gifts/${giftId}/take`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(request),
    });
    return this.handleResponse<Gift>(response);
  }

  static async releaseGift(giftId: number): Promise<Gift> {
    const response = await fetch(`${API_BASE_URL}/api/gifts/${giftId}/release`, {
      method: 'POST',
    });
    return this.handleResponse<Gift>(response);
  }
}
