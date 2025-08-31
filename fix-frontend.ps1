# Fix Frontend Package Dependencies Script

Write-Host "*** Fixing Frontend Dependencies ***" -ForegroundColor Cyan

# Navigate to frontend directory
if (Test-Path "frontend") {
    Write-Host ">> Navigating to frontend directory..." -ForegroundColor Yellow
    Set-Location "frontend"
} else {
    Write-Host "ERROR: frontend directory not found!" -ForegroundColor Red
    exit 1
}

# Remove old package-lock.json if it exists
if (Test-Path "package-lock.json") {
    Write-Host ">> Removing old package-lock.json..." -ForegroundColor Yellow
    Remove-Item "package-lock.json"
}

# Remove node_modules if it exists
if (Test-Path "node_modules") {
    Write-Host ">> Removing node_modules..." -ForegroundColor Yellow
    Remove-Item "node_modules" -Recurse -Force
}

# Fresh install
Write-Host ">> Running fresh npm install..." -ForegroundColor Yellow
npm install

if ($LASTEXITCODE -eq 0) {
    Write-Host "SUCCESS: Frontend dependencies fixed!" -ForegroundColor Green
    Write-Host ">> package-lock.json has been updated" -ForegroundColor Green
} else {
    Write-Host "ERROR: npm install failed!" -ForegroundColor Red
    exit 1
}

# Go back to root directory
Set-Location ".."

Write-Host ""
Write-Host "SUCCESS: Frontend is ready for deployment!" -ForegroundColor Green
Write-Host ">> Next: Commit these changes and redeploy" -ForegroundColor Blue
