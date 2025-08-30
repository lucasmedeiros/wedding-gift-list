export interface Gift {
  id: number;
  name: string;
  description: string;
  takenByGuestName?: string;
  takenAt?: string;
  isTaken: boolean;
  version: number; // Version for optimistic concurrency
}

export interface TakeGiftRequest {
  guestName: string;
  version?: number;
}

export interface CreateGiftRequest {
  name: string;
  description?: string;
}
