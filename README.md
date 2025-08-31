# Wedding Gift List 💕

A beautiful, responsive wedding gift list application where guests can view and select gifts for your special day.

![Wedding Gift List](https://img.shields.io/badge/Status-Ready%20for%20Deployment-green)
![.NET 8](https://img.shields.io/badge/.NET-8.0-blue)
![React](https://img.shields.io/badge/React-18.2-blue)
![TypeScript](https://img.shields.io/badge/TypeScript-4.9-blue)

## Features

- 🎁 **Beautiful Gift Display**: Clean, modern interface showing available gifts
- 📱 **Responsive Design**: Works perfectly on desktop, tablet, and mobile
- 🔒 **Gift Reservation**: Guests can reserve gifts with their name
- 🇧🇷 **Portuguese Language**: Fully translated to Brazilian Portuguese
- ⚡ **Real-time Updates**: See gift availability in real-time
- 🎨 **Elegant UI**: Designed with Tailwind CSS for a modern look
- 🐋 **Docker Deployment**: Containerized backend for easy deployment and scaling

## Architecture

- **Frontend**: React 18 with TypeScript, styled with Tailwind CSS
- **Backend**: .NET 8 Web API with Entity Framework Core (Containerized)
- **Database**: SQLite in persistent Docker volume
- **Deployment**: GitHub Pages (frontend) + Docker on AWS EC2 (backend)
- **Proxy**: Nginx reverse proxy to Docker container

## Quick Start (Development)

### Prerequisites
- Node.js 18+
- .NET 8 SDK
- Git

### Run Locally

1. **Clone the repository**
   ```bash
   git clone https://github.com/YOUR_USERNAME/wedding-gift-list.git
   cd wedding-gift-list
   ```

2. **Start the backend**
   ```bash
   cd backend
   dotnet run
   ```
   Backend will run on http://localhost:5139

3. **Start the frontend** (in a new terminal)
   ```bash
   cd frontend
   npm install
   npm start
   ```
   Frontend will run on http://localhost:3000

## Production Deployment

We've configured a simple, cost-effective, **Docker-based deployment** strategy:

- **Frontend**: GitHub Pages (Free)
- **Backend**: Docker Container on AWS EC2 Free Tier (Free for 12 months)
- **Database**: SQLite in persistent Docker volume
- **CI/CD**: GitHub Actions (Free) - triggered by version tags
- **Deployment**: Use `.\deploy-docker.ps1` (PowerShell) script for containerized releases

**👉 [Complete Docker Deployment Guide](DOCKER-DEPLOYMENT.md)**

### Quick Docker Deploy (Windows)
```powershell
.\deploy-docker.ps1  # Interactive Docker deployment with version tagging
```

### Quick EC2 Docker Setup
```bash
# SSH to your EC2 and run:
wget https://raw.githubusercontent.com/lucasmedeiros/wedding-gift-list/main/backend/deploy-scripts/setup-ec2-docker.sh
chmod +x setup-ec2-docker.sh
./setup-ec2-docker.sh
```

## Project Structure

```
wedding-gift-list/
├── frontend/                 # React TypeScript frontend
│   ├── src/
│   │   ├── components/      # React components
│   │   ├── services/        # API service layer
│   │   ├── types/          # TypeScript type definitions
│   │   └── App.tsx         # Main application component
│   └── package.json
├── backend/                 # .NET 8 Web API
│   ├── Controllers/        # API controllers
│   ├── Models/            # Data models and DTOs
│   ├── Services/          # Business logic services
│   ├── Data/              # Entity Framework context
│   ├── deploy-scripts/    # AWS deployment scripts
│   └── Program.cs         # Application entry point
├── .github/workflows/     # GitHub Actions CI/CD
└── DEPLOYMENT.md         # Detailed deployment guide
```

## API Endpoints

- `GET /api/gifts` - Get all gifts
- `POST /api/gifts/{id}/take` - Reserve a gift
- `POST /api/gifts/{id}/release` - Release a reserved gift

## Development Scripts

### Frontend
```bash
cd frontend
npm start          # Start development server
npm run build      # Build for production
npm test           # Run tests
```

### Backend
```bash
cd backend
dotnet run         # Start development server
dotnet build       # Build the application
dotnet test        # Run tests (when added)
```

## Customization

### Adding Gifts

Gifts are automatically seeded in the database. To modify the gift list, edit the `WeddingGiftListContext.cs` file in the Data folder.

### Styling

The frontend uses Tailwind CSS. You can customize colors, fonts, and styling by modifying the Tailwind configuration and component files.

### Language

The app is currently in Brazilian Portuguese. To change the language, update the text strings in the React components.

## Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## Support

If you encounter any issues:

1. Check the [Deployment Guide](DEPLOYMENT.md)
2. Review GitHub Actions logs
3. Check EC2 instance logs (if using AWS deployment)
4. Open an issue with detailed error information

## License

This project is open source and available under the [MIT License](LICENSE).

---

**Made with ❤️ for your special day**

Happy coding and congratulations on your wedding! 🎉