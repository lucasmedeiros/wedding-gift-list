# Wedding Gift List Project Cleanup Script (PowerShell)
# This script removes unnecessary files and configurations

Write-Host "*** Cleaning up unnecessary files ***" -ForegroundColor Cyan

Write-Host ">> Starting cleanup of wedding-gift-list project..." -ForegroundColor Cyan

# 1. Remove Docker files (not used with GitHub Pages + EC2 deployment)
Write-Host ">> Removing unused Docker files..." -ForegroundColor Yellow

$dockerFiles = @(
    "frontend\Dockerfile",
    "frontend\nginx.conf", 
    "backend\Dockerfile"
)

foreach ($file in $dockerFiles) {
    if (Test-Path $file) {
        Remove-Item $file
        Write-Host "REMOVED: $file" -ForegroundColor Green
    }
}

# 2. Remove IDE-specific files
Write-Host ">> Removing IDE-specific files..." -ForegroundColor Yellow

if (Test-Path "backend\Folder.DotSettings.user") {
    Remove-Item "backend\Folder.DotSettings.user"
    Write-Host "REMOVED: backend\Folder.DotSettings.user" -ForegroundColor Green
}

# 3. Remove empty/unnecessary files and directories
Write-Host ">> Removing empty directories and unnecessary files..." -ForegroundColor Yellow

if ((Test-Path "frontend\src\config") -and ((Get-ChildItem "frontend\src\config").Count -eq 0)) {
    Remove-Item "frontend\src\config" -Recurse
    Write-Host "REMOVED: empty frontend\src\config directory" -ForegroundColor Green
}

if (Test-Path "package-lock.json") {
    # Check if it's the mostly empty root-level one
    $lineCount = (Get-Content "package-lock.json" | Measure-Object -Line).Lines
    if ($lineCount -lt 10) {
        Remove-Item "package-lock.json"
        Write-Host "REMOVED: unnecessary root-level package-lock.json" -ForegroundColor Green
    }
}

# 4. Remove development-only files (optional)
Write-Host ">> Removing development-only files..." -ForegroundColor Yellow
$removeHttp = Read-Host "Remove HTTP test file (backend\WeddingGiftList.Api.http)? It's useful for development but not needed in production. (y/N)"

if ($removeHttp -match "^[Yy]$") {
    if (Test-Path "backend\WeddingGiftList.Api.http") {
        Remove-Item "backend\WeddingGiftList.Api.http"
        Write-Host "REMOVED: backend\WeddingGiftList.Api.http" -ForegroundColor Green
    }
} else {
    Write-Host "KEPT: backend\WeddingGiftList.Api.http (useful for API testing)" -ForegroundColor Yellow
}

# 5. Clean up build artifacts (if they somehow made it past .gitignore)
Write-Host ">> Checking for build artifacts..." -ForegroundColor Yellow

if (Test-Path "backend\bin") {
    Write-Host "WARNING: Found backend\bin directory - this should be gitignored" -ForegroundColor Yellow
    Write-Host "   Run: git rm -r --cached backend/bin && git commit -m 'Remove build artifacts'"
}

if (Test-Path "backend\obj") {
    Write-Host "WARNING: Found backend\obj directory - this should be gitignored" -ForegroundColor Yellow  
    Write-Host "   Run: git rm -r --cached backend/obj && git commit -m 'Remove build artifacts'"
}

if (Test-Path "frontend\node_modules") {
    Write-Host "OK: frontend\node_modules exists (should be gitignored - that's correct)" -ForegroundColor Green
}

# 6. Check for unnecessary database files in git
Write-Host ">> Checking for database files in git..." -ForegroundColor Yellow

$dbFiles = Get-ChildItem "backend" -Filter "wedding_gifts.db*" -ErrorAction SilentlyContinue
if ($dbFiles) {
    Write-Host "WARNING: Found database file(s) in git - these should be gitignored" -ForegroundColor Yellow
    Write-Host "   Run: git rm --cached backend/wedding_gifts.db* && git commit -m 'Remove database files'"
}

Write-Host ""
Write-Host "SUCCESS: Cleanup completed!" -ForegroundColor Green
Write-Host ""
Write-Host ">> Summary of cleaned files:" -ForegroundColor Blue
Write-Host "   - Docker files (not needed for GitHub Pages + EC2)"
Write-Host "   - IDE-specific files"
Write-Host "   - Empty directories" 
Write-Host "   - Unnecessary package files"
Write-Host ""
Write-Host ">> Additional recommendations:" -ForegroundColor Yellow
Write-Host "   1. Make sure build artifacts (bin/, obj/) are not committed to git"
Write-Host "   2. Database files should not be in version control"
Write-Host "   3. Consider keeping HTTP test files locally for development"
Write-Host ""
Write-Host "SUCCESS: Your project is now cleaner and more production-ready!" -ForegroundColor Green
