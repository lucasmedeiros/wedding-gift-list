export interface Gift {
  id: number;
  name: string;
  description: string;
  imageUrl: string;
  takenByGuestName?: string;
  takenAt?: string;
  isTaken: boolean;
  rowVersion: string; // Base64 encoded byte array
}

export interface TakeGiftRequest {
  guestName: string;
  rowVersion?: string;
}
