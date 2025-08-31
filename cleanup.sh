#!/bin/bash

# Wedding Gift List Project Cleanup Script
# This script removes unnecessary files and configurations

set -e

echo "🧹 Cleaning up unnecessary files..."

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

echo -e "${BLUE}Starting cleanup of wedding-gift-list project...${NC}"

# 1. Remove Docker files (not used with GitHub Pages + EC2 deployment)
echo -e "${YELLOW}📦 Removing unused Docker files...${NC}"
if [ -f "frontend/Dockerfile" ]; then
    rm frontend/Dockerfile
    echo "✅ Removed frontend/Dockerfile"
fi

if [ -f "frontend/nginx.conf" ]; then
    rm frontend/nginx.conf
    echo "✅ Removed frontend/nginx.conf"
fi

if [ -f "backend/Dockerfile" ]; then
    rm backend/Dockerfile
    echo "✅ Removed backend/Dockerfile"
fi

# 2. Remove IDE-specific files
echo -e "${YELLOW}🔧 Removing IDE-specific files...${NC}"
if [ -f "backend/Folder.DotSettings.user" ]; then
    rm backend/Folder.DotSettings.user
    echo "✅ Removed backend/Folder.DotSettings.user"
fi

# 3. Remove empty/unnecessary files and directories
echo -e "${YELLOW}📁 Removing empty directories and unnecessary files...${NC}"
if [ -d "frontend/src/config" ] && [ ! "$(ls -A frontend/src/config)" ]; then
    rmdir frontend/src/config
    echo "✅ Removed empty frontend/src/config directory"
fi

if [ -f "package-lock.json" ]; then
    # Check if it's the mostly empty root-level one
    if [ $(wc -l < package-lock.json) -lt 10 ]; then
        rm package-lock.json
        echo "✅ Removed unnecessary root-level package-lock.json"
    fi
fi

# 4. Remove development-only files (optional - you might want to keep locally)
echo -e "${YELLOW}🔬 Removing development-only files...${NC}"
read -p "Remove HTTP test file (backend/WeddingGiftList.Api.http)? It's useful for development but not needed in production. (y/N): " REMOVE_HTTP

if [[ $REMOVE_HTTP =~ ^[Yy]$ ]]; then
    if [ -f "backend/WeddingGiftList.Api.http" ]; then
        rm backend/WeddingGiftList.Api.http
        echo "✅ Removed backend/WeddingGiftList.Api.http"
    fi
else
    echo "⏭️  Kept backend/WeddingGiftList.Api.http (useful for API testing)"
fi

# 5. Clean up build artifacts (if they somehow made it past .gitignore)
echo -e "${YELLOW}🏗️  Checking for build artifacts...${NC}"
if [ -d "backend/bin" ]; then
    echo "⚠️  Found backend/bin directory - this should be gitignored"
    echo "   Run: git rm -r --cached backend/bin && git commit -m 'Remove build artifacts'"
fi

if [ -d "backend/obj" ]; then
    echo "⚠️  Found backend/obj directory - this should be gitignored"
    echo "   Run: git rm -r --cached backend/obj && git commit -m 'Remove build artifacts'"
fi

if [ -d "frontend/node_modules" ]; then
    echo "✅ frontend/node_modules exists (should be gitignored - that's correct)"
fi

# 6. Check for unnecessary database files in git
echo -e "${YELLOW}🗄️  Checking for database files in git...${NC}"
if [ -f "backend/wedding_gifts.db" ]; then
    echo "⚠️  Found database file in git - this should be gitignored"
    echo "   Run: git rm --cached backend/wedding_gifts.db* && git commit -m 'Remove database files'"
fi

echo ""
echo -e "${GREEN}✅ Cleanup completed!${NC}"
echo ""
echo -e "${BLUE}📋 Summary of cleaned files:${NC}"
echo "   • Docker files (not needed for GitHub Pages + EC2)"
echo "   • IDE-specific files"
echo "   • Empty directories"
echo "   • Unnecessary package files"
echo ""
echo -e "${YELLOW}💡 Additional recommendations:${NC}"
echo "   1. Make sure build artifacts (bin/, obj/) are not committed to git"
echo "   2. Database files should not be in version control"
echo "   3. Consider keeping HTTP test files locally for development"
echo ""
echo -e "${GREEN}🎉 Your project is now cleaner and more production-ready!${NC}"
