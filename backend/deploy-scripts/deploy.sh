#!/bin/bash

# AWS EC2 Deployment Script for .NET 8 Wedding Gift List API

set -e

echo "Starting deployment of Wedding Gift List API..."

# Configuration
APP_NAME="wedding-gift-api"
APP_DIR="/opt/$APP_NAME"
SERVICE_NAME="$APP_NAME.service"
PUBLISH_DIR="./publish"

# Detect current user for proper paths
CURRENT_USER=$(whoami)
echo "Current user: $CURRENT_USER"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

echo -e "${YELLOW}Stopping existing service if running...${NC}"
sudo systemctl stop $SERVICE_NAME || echo "Service not running"

echo -e "${YELLOW}Creating application directory...${NC}"
sudo mkdir -p $APP_DIR

echo -e "${YELLOW}Copying application files...${NC}"
sudo cp -r $PUBLISH_DIR/* $APP_DIR/

echo -e "${YELLOW}Creating data directory for SQLite...${NC}"
sudo mkdir -p $APP_DIR/data

echo -e "${YELLOW}Setting up service configuration...${NC}"
sudo tee /etc/systemd/system/$SERVICE_NAME > /dev/null <<EOF
[Unit]
Description=Wedding Gift List API
After=network.target

[Service]
Type=notify
User=nginx
Group=nginx
WorkingDirectory=$APP_DIR
ExecStart=/usr/bin/dotnet $APP_DIR/WeddingGiftList.Api.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=$APP_NAME
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://0.0.0.0:5001
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
EOF

echo -e "${YELLOW}Setting permissions...${NC}"
sudo chown -R nginx:nginx $APP_DIR
sudo chmod +x $APP_DIR/WeddingGiftList.Api.dll || true
sudo chmod 755 $APP_DIR/data

echo -e "${YELLOW}Reloading systemd and starting service...${NC}"
sudo systemctl daemon-reload
sudo systemctl enable $SERVICE_NAME
sudo systemctl start $SERVICE_NAME

echo -e "${YELLOW}Configuring Nginx reverse proxy...${NC}"
sudo tee /etc/nginx/sites-available/$APP_NAME > /dev/null <<EOF
server {
    listen 80;
    server_name _;

    location / {
        proxy_pass http://127.0.0.1:5001;
        proxy_set_header Host \$host;
        proxy_set_header X-Real-IP \$remote_addr;
        proxy_set_header X-Forwarded-For \$proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto \$scheme;
    }
}
EOF

echo -e "${YELLOW}Enabling Nginx site...${NC}"
sudo ln -sf /etc/nginx/sites-available/$APP_NAME /etc/nginx/sites-enabled/
sudo rm -f /etc/nginx/sites-enabled/default
sudo nginx -t && sudo systemctl reload nginx

echo -e "${GREEN}Deployment completed successfully!${NC}"
echo -e "${GREEN}Service status:${NC}"
sudo systemctl status $SERVICE_NAME --no-pager

echo -e "${GREEN}API should be available at: http://$(curl -s ifconfig.me):80${NC}"
