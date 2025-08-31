#!/bin/bash

# Docker Deployment Script for Wedding Gift List API

set -e

echo "Starting Docker deployment of Wedding Gift List API..."

# Configuration
APP_NAME="wedding-gift-api"
APP_DIR="/opt/$APP_NAME"
CONTAINER_NAME="wedding-gift-api"
IMAGE_NAME="wedding-gift-api:latest"

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m'

echo -e "${YELLOW}Current user: $(whoami)${NC}"
echo -e "${YELLOW}Current directory: $(pwd)${NC}"

# Navigate to application directory
echo -e "${YELLOW}Navigating to application directory...${NC}"
cd $APP_DIR

# Stop and remove existing container if running
echo -e "${YELLOW}Stopping existing container...${NC}"
docker stop $CONTAINER_NAME || echo "Container not running"
docker rm $CONTAINER_NAME || echo "Container not found"

# Remove old image to ensure fresh build
echo -e "${YELLOW}Removing old Docker image...${NC}"
docker rmi $IMAGE_NAME || echo "Image not found"

# Build new Docker image
echo -e "${YELLOW}Building new Docker image...${NC}"
docker build -t $IMAGE_NAME .

# Run new container
echo -e "${YELLOW}Starting new container...${NC}"
docker run -d \
  --name $CONTAINER_NAME \
  --restart unless-stopped \
  -p 5001:5001 \
  -v wedding-gift-data:/app/data \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ASPNETCORE_URLS=http://+:5001 \
  -e "ConnectionStrings__DefaultConnection=Data Source=/app/data/wedding_gifts.db" \
  $IMAGE_NAME

# Wait for container to start
echo -e "${YELLOW}Waiting for container to start...${NC}"
sleep 10

# Check container status
echo -e "${YELLOW}Checking container status...${NC}"
if docker ps | grep -q $CONTAINER_NAME; then
    echo -e "${GREEN}✅ Container is running successfully!${NC}"
    
    # Show container logs (last 20 lines)
    echo -e "${YELLOW}Container logs (last 20 lines):${NC}"
    docker logs --tail 20 $CONTAINER_NAME
    
    # Test API endpoint
    echo -e "${YELLOW}Testing API endpoint...${NC}"
    sleep 5
    if curl -f -s http://localhost:5001/api/gifts > /dev/null; then
        echo -e "${GREEN}✅ API is responding successfully!${NC}"
    else
        echo -e "${YELLOW}⚠️  API not responding yet (this is normal on first run)${NC}"
    fi
    
else
    echo -e "${RED}❌ Container failed to start!${NC}"
    echo -e "${YELLOW}Container logs:${NC}"
    docker logs $CONTAINER_NAME
    exit 1
fi

# Show running containers
echo -e "${YELLOW}Running containers:${NC}"
docker ps --filter "name=$CONTAINER_NAME"

# Clean up unused images and containers
echo -e "${YELLOW}Cleaning up unused Docker resources...${NC}"
docker system prune -f

echo ""
echo -e "${GREEN}🎉 Docker deployment completed successfully!${NC}"
echo ""
echo -e "${GREEN}Container Status:${NC}"
echo "  Name: $CONTAINER_NAME"
echo "  Image: $IMAGE_NAME"
echo "  Port: 5001 → 5001"
echo "  Data Volume: wedding-gift-data"
echo ""
echo -e "${GREEN}API Endpoints:${NC}"
echo "  Local: http://localhost:5001/api/gifts"
echo "  Public: http://$(curl -s ifconfig.me)/api/gifts (via Nginx)"
echo ""
echo -e "${YELLOW}Useful Commands:${NC}"
echo "  View logs: docker logs -f $CONTAINER_NAME"
echo "  Stop container: docker stop $CONTAINER_NAME"
echo "  Start container: docker start $CONTAINER_NAME"
echo "  Remove container: docker rm $CONTAINER_NAME"
