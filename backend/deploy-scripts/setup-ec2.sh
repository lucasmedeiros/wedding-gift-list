#!/bin/bash

# AWS EC2 Initial Setup Script for Amazon Linux 2023
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
sudo dnf update -y

# Install required packages
echo -e "${YELLOW}Installing required packages...${NC}"
sudo dnf install -y wget curl unzip nginx

# Install .NET 8 SDK
echo -e "${YELLOW}Installing .NET 8 SDK...${NC}"
sudo rpm --import https://packages.microsoft.com/keys/microsoft.asc
sudo wget -O /etc/yum.repos.d/microsoft-prod.repo https://packages.microsoft.com/config/rhel/8/prod.repo

sudo dnf update -y
sudo dnf install -y dotnet-sdk-8.0

# Verify .NET installation
echo -e "${YELLOW}Verifying .NET installation...${NC}"
dotnet --version

# Create nginx user if it doesn't exist (Amazon Linux uses nginx user)
echo -e "${YELLOW}Setting up nginx user...${NC}"
sudo useradd -r -s /bin/false nginx || echo "nginx user already exists"

# Configure firewall (firewalld)
echo -e "${YELLOW}Configuring firewall...${NC}"
sudo systemctl enable firewalld
sudo systemctl start firewalld
sudo firewall-cmd --permanent --add-service=ssh
sudo firewall-cmd --permanent --add-service=http
sudo firewall-cmd --permanent --add-service=https
sudo firewall-cmd --reload

# Start and enable nginx
echo -e "${YELLOW}Starting Nginx...${NC}"
sudo systemctl start nginx
sudo systemctl enable nginx

# Create application directory
echo -e "${YELLOW}Creating application directory...${NC}"
sudo mkdir -p /opt/wedding-gift-api
sudo chown -R nginx:nginx /opt/wedding-gift-api

echo -e "${GREEN}EC2 setup completed successfully!${NC}"
echo -e "${GREEN}Your server is ready for deployment.${NC}"
echo ""
echo -e "${YELLOW}Next steps:${NC}"
echo "1. Configure GitHub secrets with your EC2 details"
echo "2. Push your code to trigger deployment"
echo "3. Update your frontend API URL with this server's IP/domain"
echo ""
echo -e "${YELLOW}Server IP:${NC} $(curl -s ifconfig.me)"
