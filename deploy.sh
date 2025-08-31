#!/bin/bash

# Wedding Gift List - Tag-based Deployment Script
# Creates and pushes version tags to trigger deployments

set -e

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

echo -e "${BLUE}🎉 Wedding Gift List - Tag Deployment${NC}"
echo "========================================="

# Check if we're in a git repository
if [ ! -d ".git" ]; then
    echo -e "${RED}❌ Error: Not in a git repository${NC}"
    exit 1
fi

# Check for uncommitted changes
if ! git diff --quiet || ! git diff --cached --quiet; then
    echo -e "${RED}❌ Error: You have uncommitted changes${NC}"
    echo "Please commit or stash your changes before creating a release tag"
    git status --short
    exit 1
fi

# Make sure we're up to date with remote
echo -e "${YELLOW}📥 Fetching latest changes from remote...${NC}"
git fetch --tags

# Get the current version or ask for it
if [ -z "$1" ]; then
    echo ""
    echo -e "${YELLOW}📝 Please provide a version number${NC}"
    echo "Format: X.Y.Z (e.g., 1.0.0, 2.1.3)"
    echo ""
    # Show existing tags for reference
    echo -e "${BLUE}Existing tags:${NC}"
    git tag -l --sort=-version:refname | head -10 || echo "No tags found"
    echo ""
    read -p "Version: " VERSION
else
    VERSION=$1
fi

# Validate version format
if [[ ! $VERSION =~ ^[0-9]+\.[0-9]+\.[0-9]+$ ]]; then
    echo -e "${RED}❌ Error: Version must be in format X.Y.Z (e.g., 1.0.0)${NC}"
    exit 1
fi

# Ask what to deploy
echo ""
echo -e "${BLUE}🚀 What would you like to deploy?${NC}"
echo "1) 🌐 Frontend only"
echo "2) 🔧 Backend only"
echo "3) 📦 Both frontend and backend"
echo ""
read -p "Choose (1-3): " CHOICE

case $CHOICE in
    1)
        TAG="frontend-v${VERSION}"
        DESCRIPTION="🌐 Frontend deployment v${VERSION}"
        DEPLOY_TYPE="Frontend"
        ;;
    2)
        TAG="backend-v${VERSION}"
        DESCRIPTION="🔧 Backend deployment v${VERSION}"
        DEPLOY_TYPE="Backend"
        ;;
    3)
        TAG="v${VERSION}"
        DESCRIPTION="📦 Full application deployment v${VERSION}"
        DEPLOY_TYPE="Full application"
        ;;
    *)
        echo -e "${RED}❌ Invalid choice${NC}"
        exit 1
        ;;
esac

# Check if tag already exists
if git tag -l | grep -q "^${TAG}$"; then
    echo -e "${RED}❌ Error: Tag ${TAG} already exists${NC}"
    echo ""
    echo "Existing tags matching this pattern:"
    git tag -l | grep "${VERSION}" || echo "None found"
    echo ""
    echo "Use 'git tag -d ${TAG}' to delete the existing tag if needed"
    echo "Or choose a different version number"
    exit 1
fi

# Confirm deployment
echo ""
echo -e "${GREEN}✅ Ready to deploy:${NC}"
echo "   Tag: ${TAG}"
echo "   Type: ${DEPLOY_TYPE}"
echo "   Description: ${DESCRIPTION}"
echo ""
echo -e "${YELLOW}This will trigger GitHub Actions to deploy to production!${NC}"
echo ""
read -p "Are you sure you want to create and push this tag? (y/N): " CONFIRM

if [[ ! $CONFIRM =~ ^[Yy]$ ]]; then
    echo -e "${YELLOW}🚫 Deployment cancelled${NC}"
    exit 0
fi

# Create and push the tag
echo ""
echo -e "${BLUE}🏷️  Creating tag...${NC}"
git tag -a "$TAG" -m "$DESCRIPTION"

echo -e "${BLUE}📤 Pushing tag to remote...${NC}"
git push origin "$TAG"

echo ""
echo -e "${GREEN}🎉 Success! Tag ${TAG} created and pushed!${NC}"
echo ""

# Show deployment status information
REPO_URL=$(git config --get remote.origin.url | sed 's/git@github.com:/https:\/\/github.com\//' | sed 's/\.git$//')
ACTIONS_URL="${REPO_URL}/actions"

echo -e "${BLUE}📊 Deployment Status:${NC}"
echo "   • GitHub Actions: ${ACTIONS_URL}"

case $CHOICE in
    1)
        echo "   • Frontend will deploy to: https://lucasmedeiros.github.io/wedding-gift-list"
        echo "   • Expected deploy time: ~2-3 minutes"
        ;;
    2)
        echo "   • Backend will deploy to your AWS EC2 instance"
        echo "   • Expected deploy time: ~3-5 minutes"
        ;;
    3)
        echo "   • Frontend will deploy to: https://lucasmedeiros.github.io/wedding-gift-list"
        echo "   • Backend will deploy to your AWS EC2 instance"
        echo "   • Expected total deploy time: ~5-8 minutes"
        ;;
esac

echo ""
echo -e "${GREEN}🚀 Deployment initiated! Check the GitHub Actions link above for progress.${NC}"
echo ""

# Offer to open GitHub Actions
if command -v xdg-open &> /dev/null || command -v open &> /dev/null; then
    read -p "Open GitHub Actions in browser? (y/N): " OPEN_BROWSER
    if [[ $OPEN_BROWSER =~ ^[Yy]$ ]]; then
        if command -v xdg-open &> /dev/null; then
            xdg-open "$ACTIONS_URL"
        elif command -v open &> /dev/null; then
            open "$ACTIONS_URL"
        fi
    fi
fi

echo -e "${BLUE}💡 Next time, you can run: ${NC}./deploy.sh ${VERSION}"
