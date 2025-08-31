# Wedding Gift List - Tag-based Deployment Script (PowerShell)
# Creates and pushes version tags to trigger deployments on Windows

param(
    [string]$Version
)

# Colors for output
$Red = "Red"
$Green = "Green" 
$Yellow = "Yellow"
$Blue = "Cyan"

Write-Host "*** Wedding Gift List - Tag Deployment ***" -ForegroundColor $Blue
Write-Host "=========================================="

# Check if we're in a git repository
if (-not (Test-Path ".git")) {
    Write-Host "ERROR: Not in a git repository" -ForegroundColor $Red
    exit 1
}

# Check for uncommitted changes
$gitStatus = git status --porcelain
if ($gitStatus) {
    Write-Host "ERROR: You have uncommitted changes" -ForegroundColor $Red
    Write-Host "Please commit or stash your changes before creating a release tag" -ForegroundColor $Red
    git status --short
    exit 1
}

# Make sure we're up to date with remote
Write-Host ">> Fetching latest changes from remote..." -ForegroundColor $Yellow
git fetch --tags

# Get the current version or ask for it
if (-not $Version) {
    Write-Host ""
    Write-Host ">> Please provide a version number" -ForegroundColor $Yellow
    Write-Host "Format: X.Y.Z (e.g., 1.0.0, 2.1.3)"
    Write-Host ""
    # Show existing tags for reference
    Write-Host "Existing tags:" -ForegroundColor $Blue
    try {
        $existingTags = git tag -l --sort=-version:refname | Select-Object -First 10
        if ($existingTags) {
            $existingTags | ForEach-Object { Write-Host "  $_" }
        } else {
            Write-Host "  No tags found"
        }
    } catch {
        Write-Host "  No tags found"
    }
    Write-Host ""
    $Version = Read-Host "Version"
}

# Validate version format
if ($Version -notmatch "^[0-9]+\.[0-9]+\.[0-9]+$") {
    Write-Host "ERROR: Version must be in format X.Y.Z (e.g., 1.0.0)" -ForegroundColor $Red
    exit 1
}

# Ask what to deploy
Write-Host ""
Write-Host ">> What would you like to deploy?" -ForegroundColor $Blue
Write-Host "1) Frontend only"
Write-Host "2) Backend only" 
Write-Host "3) Both frontend and backend"
Write-Host ""
$Choice = Read-Host "Choose (1-3)"

switch ($Choice) {
    "1" {
        $Tag = "frontend-v$Version"
        $Description = "Frontend deployment v$Version"
        $DeployType = "Frontend"
    }
    "2" {
        $Tag = "backend-v$Version"
        $Description = "Backend deployment v$Version"
        $DeployType = "Backend"
    }
    "3" {
        $Tag = "v$Version"
        $Description = "Full application deployment v$Version"
        $DeployType = "Full application"
    }
    default {
        Write-Host "ERROR: Invalid choice" -ForegroundColor $Red
        exit 1
    }
}

# Check if tag already exists
$existingTag = git tag -l | Where-Object { $_ -eq $Tag }
if ($existingTag) {
    Write-Host "ERROR: Tag $Tag already exists" -ForegroundColor $Red
    Write-Host ""
    Write-Host "Existing tags matching this pattern:"
    git tag -l | Where-Object { $_ -match $Version }
    Write-Host ""
    Write-Host "Use 'git tag -d $Tag' to delete the existing tag if needed"
    Write-Host "Or choose a different version number"
    exit 1
}

# Confirm deployment
Write-Host ""
Write-Host ">> Ready to deploy:" -ForegroundColor $Green
Write-Host "   Tag: $Tag"
Write-Host "   Type: $DeployType"  
Write-Host "   Description: $Description"
Write-Host ""
Write-Host "This will trigger GitHub Actions to deploy to production!" -ForegroundColor $Yellow
Write-Host ""
$Confirm = Read-Host "Are you sure you want to create and push this tag? (y/N)"

if ($Confirm -notmatch "^[Yy]$") {
    Write-Host ">> Deployment cancelled" -ForegroundColor $Yellow
    exit 0
}

# Create and push the tag
Write-Host ""
Write-Host ">> Creating tag..." -ForegroundColor $Blue
git tag -a $Tag -m $Description

Write-Host ">> Pushing tag to remote..." -ForegroundColor $Blue
git push origin $Tag

Write-Host ""
Write-Host "SUCCESS! Tag $Tag created and pushed!" -ForegroundColor $Green
Write-Host ""

# Show deployment status information
try {
    $repoUrl = git config --get remote.origin.url
    if ($repoUrl -match "github\.com[:/](.+)\.git") {
        $repoPath = $matches[1]
        $actionsUrl = "https://github.com/$repoPath/actions"
    } else {
        $actionsUrl = "Check your GitHub repository actions page"
    }
} catch {
    $actionsUrl = "Check your GitHub repository actions page"
}

Write-Host ">> Deployment Status:" -ForegroundColor $Blue
Write-Host "   - GitHub Actions: $actionsUrl"

switch ($Choice) {
    "1" {
        Write-Host "   - Frontend will deploy to: https://lucasmedeiros.github.io/wedding-gift-list"
        Write-Host "   - Expected deploy time: ~2-3 minutes"
    }
    "2" {
        Write-Host "   - Backend will deploy to your AWS EC2 instance"
        Write-Host "   - Expected deploy time: ~3-5 minutes"
        Write-Host "   - Make sure GitHub secrets are configured (see GITHUB-SECRETS-SETUP.md)"
    }
    "3" {
        Write-Host "   - Frontend will deploy to: https://lucasmedeiros.github.io/wedding-gift-list"
        Write-Host "   - Backend will deploy to your AWS EC2 instance"
        Write-Host "   - Expected total deploy time: ~5-8 minutes"
        Write-Host "   - Make sure GitHub secrets are configured (see GITHUB-SECRETS-SETUP.md)"
    }
}

Write-Host ""
Write-Host ">> Deployment initiated! Check the GitHub Actions link above for progress." -ForegroundColor $Green
Write-Host ""
Write-Host ">> If deployment fails:" -ForegroundColor $Yellow
Write-Host "   1. Check GitHub Actions logs for detailed errors"
Write-Host "   2. For SSH errors: verify GITHUB-SECRETS-SETUP.md"
Write-Host "   3. Test SSH connection manually: ssh -i your-key.pem ec2-user@YOUR_EC2_IP"
Write-Host ""

# Offer to open GitHub Actions
$OpenBrowser = Read-Host "Open GitHub Actions in browser? (y/N)"
if ($OpenBrowser -match "^[Yy]$") {
    try {
        Start-Process $actionsUrl
    } catch {
        Write-Host "Could not open browser automatically. Please visit: $actionsUrl"
    }
}

Write-Host "TIP: Next time, you can run: " -ForegroundColor $Blue -NoNewline
Write-Host ".\deploy.ps1 $Version"
