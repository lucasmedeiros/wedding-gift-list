#!/bin/bash

# AWS EC2 Initial Setup Script for Ubuntu 22.04
# Run this once when setting up your EC2 instance

set -e

echo "Setting up AWS EC2 instance for .NET 8 API hosting..."

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

# Update system
echo -e "${YELLOW}Updating system packages...${NC}"
sudo apt update && sudo apt upgrade -y

# Install required packages
echo -e "${YELLOW}Installing required packages...${NC}"
sudo apt install -y wget curl unzip nginx

# Install .NET 8 SDK
echo -e "${YELLOW}Installing .NET 8 SDK...${NC}"
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

sudo apt update
sudo apt install -y dotnet-sdk-8.0

# Verify .NET installation
echo -e "${YELLOW}Verifying .NET installation...${NC}"
dotnet --version

# Create www-data user if it doesn't exist
echo -e "${YELLOW}Setting up www-data user...${NC}"
sudo useradd -r -s /bin/false www-data || echo "www-data user already exists"

# Configure firewall (UFW)
echo -e "${YELLOW}Configuring firewall...${NC}"
sudo ufw --force enable
sudo ufw allow ssh
sudo ufw allow 80/tcp
sudo ufw allow 443/tcp

# Start and enable nginx
echo -e "${YELLOW}Starting Nginx...${NC}"
sudo systemctl start nginx
sudo systemctl enable nginx

# Create application directory
echo -e "${YELLOW}Creating application directory...${NC}"
sudo mkdir -p /opt/wedding-gift-api
sudo chown -R www-data:www-data /opt/wedding-gift-api

echo -e "${GREEN}EC2 setup completed successfully!${NC}"
echo -e "${GREEN}Your server is ready for deployment.${NC}"
echo ""
echo -e "${YELLOW}Next steps:${NC}"
echo "1. Configure GitHub secrets with your EC2 details"
echo "2. Push your code to trigger deployment"
echo "3. Update your frontend API URL with this server's IP/domain"
echo ""
echo -e "${YELLOW}Server IP:${NC} $(curl -s ifconfig.me)"
