# Simple Deployment Guide - GitHub Pages + AWS EC2

This guide will help you deploy your Wedding Gift List application using:
- **Frontend**: GitHub Pages (free)
- **Backend**: AWS EC2 Free Tier (free for 12 months)
- **Database**: SQLite (simple, no additional setup)
- **CI/CD**: GitHub Actions (free)

## Prerequisites

1. GitHub account
2. AWS account (free tier eligible)
3. Basic familiarity with SSH

## Step 1: AWS EC2 Setup

### 1.1 Create EC2 Instance

1. Login to AWS Console → EC2
2. Click "Launch Instance"
3. Choose **Ubuntu Server 22.04 LTS** (Free Tier Eligible)
4. Instance type: **t2.micro** (Free Tier Eligible)
5. Create new key pair or use existing one (**SAVE THE .pem FILE!**)
6. Security Group settings:
   - SSH (22): Your IP
   - HTTP (80): 0.0.0.0/0
   - HTTPS (443): 0.0.0.0/0
7. Launch instance

### 1.2 Setup Server

1. Connect to your EC2 instance via SSH:
   ```bash
   ssh -i your-key.pem ubuntu@YOUR_EC2_PUBLIC_IP
   ```

2. Run the setup script:
   ```bash
   wget https://raw.githubusercontent.com/YOUR_USERNAME/wedding-gift-list/main/backend/deploy-scripts/setup-ec2.sh
   chmod +x setup-ec2.sh
   ./setup-ec2.sh
   ```

3. Note down your **public IP address** (displayed at the end)

## Step 2: Update Configuration Files

### 2.1 Update Frontend Configuration

Replace placeholders in these files with your actual values:

1. **frontend/package.json**: 
   ```json
   "homepage": "https://YOUR_GITHUB_USERNAME.github.io/wedding-gift-list"
   ```

2. **frontend/src/services/giftService.ts**:
   ```typescript
   ? 'http://YOUR_EC2_PUBLIC_IP'  // Replace with your EC2 IP
   ```

### 2.2 Update Backend Configuration

1. **backend/appsettings.Production.json**:
   ```json
   "AllowedOrigins": [
     "https://YOUR_GITHUB_USERNAME.github.io"
   ]
   ```

2. **backend/Program.cs** (line 23):
   ```csharp
   ?? new[] { "https://YOUR_GITHUB_USERNAME.github.io" };
   ```

## Step 3: GitHub Setup

### 3.1 Enable GitHub Pages

1. Go to your GitHub repository
2. Settings → Pages
3. Source: "GitHub Actions"
4. Save

### 3.2 Configure GitHub Secrets

Go to Settings → Secrets and Variables → Actions, add these secrets:

**Frontend Secrets:**
- `REACT_APP_API_URL`: `http://YOUR_EC2_PUBLIC_IP`

**Backend Secrets:**
- `EC2_SSH_KEY`: Contents of your .pem key file
- `EC2_HOST`: Your EC2 public IP address
- `EC2_USER`: `ubuntu`

### 3.3 How to Add SSH Key Secret

1. Open your .pem file in a text editor
2. Copy the ENTIRE contents (including BEGIN/END lines)
3. Paste as `EC2_SSH_KEY` secret value

## Step 4: Deploy

### 4.1 Using the Deployment Script (Recommended)

1. Make the deployment script executable:
   ```bash
   chmod +x deploy.sh
   ```

2. Run the interactive deployment script:
   ```bash
   ./deploy.sh
   ```

3. Choose what to deploy:
   - Frontend only
   - Backend only  
   - Both frontend and backend

4. Enter a version number (e.g., 1.0.0)

The script will create and push a version tag, triggering GitHub Actions deployment.

### 4.2 Manual Tag Creation

Alternatively, create tags manually:

```bash
# Deploy both frontend and backend
git tag -a v1.0.0 -m "Release v1.0.0"
git push origin v1.0.0

# Deploy only frontend
git tag -a frontend-v1.0.1 -m "Frontend release v1.0.1"
git push origin frontend-v1.0.1

# Deploy only backend
git tag -a backend-v1.0.1 -m "Backend release v1.0.1"
git push origin backend-v1.0.1
```

### 4.2 Check Deployment

1. **Frontend**: Visit `https://YOUR_GITHUB_USERNAME.github.io/wedding-gift-list`
2. **Backend**: Visit `http://YOUR_EC2_PUBLIC_IP/api/gifts`

## Step 5: Domain Setup (Optional)

### 5.1 Custom Domain for Frontend

1. Buy a domain (e.g., from Route 53, Namecheap)
2. Add `CNAME` file to `frontend/public/` with your domain
3. Configure DNS to point to `YOUR_GITHUB_USERNAME.github.io`

### 5.2 Custom Domain for Backend

1. Use Route 53 or your DNS provider
2. Create A record pointing to your EC2 IP
3. Update CORS settings with your new domain

## Troubleshooting

### Common Issues

1. **Frontend not loading**: 
   - Check GitHub Pages is enabled
   - Verify homepage URL in package.json

2. **Backend not responding**:
   - SSH to EC2: `sudo systemctl status wedding-gift-api`
   - Check logs: `sudo journalctl -u wedding-gift-api -f`

3. **CORS errors**:
   - Verify AllowedOrigins in appsettings.Production.json
   - Make sure frontend URL matches exactly

4. **GitHub Actions failing**:
   - Check secrets are set correctly
   - Verify EC2 SSH key format

### Useful Commands

```bash
# SSH to EC2
ssh -i your-key.pem ubuntu@YOUR_EC2_IP

# Check service status
sudo systemctl status wedding-gift-api

# View logs
sudo journalctl -u wedding-gift-api -f

# Restart service
sudo systemctl restart wedding-gift-api

# Check Nginx status
sudo systemctl status nginx
```

## Cost Estimation

- **GitHub Pages**: Free
- **GitHub Actions**: Free (2000 minutes/month)
- **AWS EC2 t2.micro**: Free for 12 months, then ~$8/month
- **Domain** (optional): ~$12/year

## Security Notes

- Keep your .pem file secure
- Regularly update your EC2 instance
- Consider setting up SSL certificates for production
- Monitor AWS costs in your billing dashboard

## Updating Your App

### Option 1: Using the deployment script
```bash
git add .
git commit -m "Your changes"
git push origin main
./deploy.sh  # Creates a new version tag and deploys
```

### Option 2: Manual tag creation
```bash
git add .
git commit -m "Your changes"
git push origin main

# Then create a new version tag
git tag -a v1.0.1 -m "Release v1.0.1"
git push origin v1.0.1
```

**Important**: Deployment only happens when you push version tags, not on regular commits to main. This gives you control over when to deploy to production.

## Need Help?

1. Check GitHub Actions logs for deployment errors
2. SSH to EC2 and check service logs
3. Verify all configuration placeholders are replaced with actual values
