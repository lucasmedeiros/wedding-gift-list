# Wedding Gift List

An elegant wedding gift list website where guests can view and claim gifts with concurrency control to prevent conflicts.

## Features

- **Beautiful UI**: Clean, responsive design with soft shadows and elegant typography
- **Real-time Updates**: Gifts are immediately marked as taken to prevent conflicts
- **Concurrency Control**: Optimistic concurrency control prevents multiple guests from taking the same gift
- **Mobile Friendly**: Fully responsive design that works on all devices
- **Simple Deployment**: Containerized with Docker for easy deployment

## Tech Stack

- **Backend**: .NET 8 ASP.NET Core Web API
- **Database**: SQLite with Entity Framework Core
- **Frontend**: React 18 with TypeScript
- **Styling**: TailwindCSS
- **Containerization**: Docker

## Quick Start

### Prerequisites

- Docker and Docker Compose
- .NET 8 SDK (for development)
- Node.js 18+ (for development)

### Production Deployment (Docker)

1. **Build and run the backend:**
   ```bash
   cd backend
   docker build -t wedding-api .
   docker run -d -p 5000:5000 -v $(pwd)/data:/app/data --name wedding-api wedding-api
   ```

2. **Build and run the frontend:**
   ```bash
   cd frontend
   docker build -t wedding-frontend .
   docker run -d -p 3000:80 --name wedding-frontend wedding-frontend
   ```

3. **Access the application:**
   - Frontend: http://localhost:3000
   - Backend API: http://localhost:5000
   - API Documentation: http://localhost:5000/swagger

### Development Setup

1. **Backend Development:**
   ```bash
   cd backend
   dotnet restore
   dotnet run
   ```
   The API will be available at `https://localhost:5001` and `http://localhost:5000`

2. **Frontend Development:**
   ```bash
   cd frontend
   npm install
   npm start
   ```
   The React app will be available at `http://localhost:3000`

## API Endpoints

- `GET /api/gifts` - Get all gifts with their status
- `POST /api/gifts/{id}/take` - Take a gift (requires guest name)
- `POST /api/gifts/{id}/release` - Release a taken gift

## Database

The application uses SQLite for simplicity. The database file is stored in `/app/data/wedding_gifts.db` inside the container, which is mapped to a volume for persistence.

### Sample Data

The application comes with 6 pre-seeded gifts:
- Coffee Machine
- Kitchen Knife Set
- Silk Bed Sheets
- Wine Glasses Set
- Cast Iron Cookware
- Photo Album

## Concurrency Control

The application implements optimistic concurrency control using Entity Framework's `[Timestamp]` attribute. This prevents race conditions where two guests might try to take the same gift simultaneously.

## Architecture

```
┌─────────────────┐    HTTP/JSON    ┌──────────────────┐
│  React Frontend │ ◄──────────────► │ .NET Core Web API│
│  (TailwindCSS)  │                 │  (Controllers)   │
└─────────────────┘                 └──────────────────┘
                                             │
                                             ▼
                                    ┌──────────────────┐
                                    │ Entity Framework │
                                    │     (SQLite)     │
                                    └──────────────────┘
```

## Deployment Notes

- **Data Persistence**: Make sure to mount a volume for the SQLite database in production
- **HTTPS**: The backend is configured for HTTPS in production
- **CORS**: Configured to allow frontend access to the API
- **Error Handling**: Comprehensive error handling with user-friendly messages
- **Validation**: Input validation on both client and server sides

## Customization

- **Images**: Replace the Unsplash image URLs in `backend/Data/WeddingGiftListContext.cs`
- **Colors**: Modify the color palette in `frontend/tailwind.config.js`
- **Gifts**: Update the seed data in the DbContext to match your desired gifts

## License

This project is created for personal use. Feel free to adapt it for your own wedding!

---

Made with ❤️ for a special wedding day