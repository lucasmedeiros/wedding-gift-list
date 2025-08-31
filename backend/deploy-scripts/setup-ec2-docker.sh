#!/bin/bash

# AWS EC2 Initial Setup Script for Amazon Linux 2023 with Docker
# Run this once when setting up your EC2 instance

set -e

echo "Setting up AWS EC2 instance for Docker deployment..."

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
sudo dnf install -y wget curl unzip nginx git

# Install Docker
echo -e "${YELLOW}Installing Docker...${NC}"
sudo dnf install -y docker
sudo systemctl enable docker
sudo systemctl start docker

# Add ec2-user to docker group
sudo usermod -a -G docker ec2-user
echo -e "${GREEN}Added ec2-user to docker group${NC}"

# Install Docker Compose
echo -e "${YELLOW}Installing Docker Compose...${NC}"
sudo curl -L "https://github.com/docker/compose/releases/latest/download/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose

# Verify Docker installation
echo -e "${YELLOW}Verifying Docker installation...${NC}"
sudo docker --version
sudo docker-compose --version

# Configure firewall (firewalld)
echo -e "${YELLOW}Configuring firewall...${NC}"
sudo systemctl enable firewalld
sudo systemctl start firewalld
sudo firewall-cmd --permanent --add-service=ssh
sudo firewall-cmd --permanent --add-service=http
sudo firewall-cmd --permanent --add-service=https
sudo firewall-cmd --permanent --add-port=5001/tcp
sudo firewall-cmd --reload

# Configure Nginx as reverse proxy
echo -e "${YELLOW}Configuring Nginx reverse proxy...${NC}"
sudo tee /etc/nginx/conf.d/wedding-gift-api.conf > /dev/null <<EOF
server {
    listen 80;
    server_name _;

    location / {
        proxy_pass http://127.0.0.1:5001;
        proxy_set_header Host \$host;
        proxy_set_header X-Real-IP \$remote_addr;
        proxy_set_header X-Forwarded-For \$proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto \$scheme;
        proxy_connect_timeout 60s;
        proxy_send_timeout 60s;
        proxy_read_timeout 60s;
    }
}
EOF

# Start and enable nginx
echo -e "${YELLOW}Starting Nginx...${NC}"
sudo systemctl start nginx
sudo systemctl enable nginx

# Create application directory
echo -e "${YELLOW}Creating application directory...${NC}"
sudo mkdir -p /opt/wedding-gift-api
sudo chown -R ec2-user:ec2-user /opt/wedding-gift-api

# Create Docker network
echo -e "${YELLOW}Creating Docker network...${NC}"
sudo docker network create wedding-gift-network || echo "Network already exists"

echo -e "${GREEN}EC2 Docker setup completed successfully!${NC}"
echo -e "${GREEN}Your server is ready for Docker deployment.${NC}"
echo ""
echo -e "${YELLOW}Next steps:${NC}"
echo "1. Configure GitHub secrets with your EC2 details"
echo "2. Run deployment from your local machine"
echo "3. The API will be available at http://$(curl -s ifconfig.me)/"
echo ""
echo -e "${YELLOW}Server IP:${NC} $(curl -s ifconfig.me)"
echo -e "${YELLOW}Docker version:${NC} $(sudo docker --version)"
echo ""
echo -e "${GREEN}IMPORTANT: You need to log out and log back in for Docker group changes to take effect!${NC}"
