# Diagnose Deployment Issues Script

param(
    [string]$EC2Host,
    [string]$KeyPath
)

# Colors for output
$Red = "Red"
$Green = "Green" 
$Yellow = "Yellow"
$Blue = "Cyan"

Write-Host "*** Deployment Diagnostics ***" -ForegroundColor $Blue
Write-Host "==============================="

# Get parameters if not provided
if (-not $EC2Host) {
    $EC2Host = "18.219.60.144"  # Your known IP
}

if (-not $KeyPath) {
    $KeyPath = Read-Host "Enter path to your .pem key file"
}

# Test SSH Connection
Write-Host ">> Testing SSH Connection..." -ForegroundColor $Blue
$sshTest = ssh -i $KeyPath -o ConnectTimeout=10 -o StrictHostKeyChecking=no ec2-user@$EC2Host "echo 'SSH OK'; whoami; uname -a" 2>$null

if ($LASTEXITCODE -eq 0) {
    Write-Host "SUCCESS: SSH connection works!" -ForegroundColor $Green
    Write-Host $sshTest
} else {
    Write-Host "ERROR: SSH connection failed!" -ForegroundColor $Red
    exit 1
}

# Check GitHub Secrets
Write-Host ""
Write-Host ">> GitHub Secrets Check:" -ForegroundColor $Blue
Write-Host "Go to: https://github.com/lucasmedeiros/wedding-gift-list/settings/secrets/actions"
Write-Host ""
Write-Host "Required secrets:" -ForegroundColor $Yellow
Write-Host "✓ EC2_SSH_KEY: [Your .pem file content]"
Write-Host "✓ EC2_HOST: $EC2Host"
Write-Host "✓ EC2_USER: ec2-user"
Write-Host "✓ REACT_APP_API_URL: http://$EC2Host"

# Check GitHub Pages Settings
Write-Host ""
Write-Host ">> GitHub Pages Check:" -ForegroundColor $Blue
Write-Host "1. Go to: https://github.com/lucasmedeiros/wedding-gift-list/settings/pages"
Write-Host "2. Source should be: 'GitHub Actions'"
Write-Host "3. Make sure Pages is enabled"

# Check System Users on EC2
Write-Host ""
Write-Host ">> Checking EC2 System Users..." -ForegroundColor $Blue
$userCheck = ssh -i $KeyPath -o StrictHostKeyChecking=no ec2-user@$EC2Host "
echo 'Checking system users...';
id nginx 2>/dev/null && echo 'nginx user: EXISTS' || echo 'nginx user: MISSING';
id wedding-gift 2>/dev/null && echo 'wedding-gift user: EXISTS' || echo 'wedding-gift user: MISSING';
id ec2-user 2>/dev/null && echo 'ec2-user: EXISTS' || echo 'ec2-user: MISSING';
echo 'Current user:' \$(whoami);
ls -la /opt/wedding-gift-api/ 2>/dev/null || echo '/opt/wedding-gift-api does not exist yet';
" 2>$null

if ($LASTEXITCODE -eq 0) {
    Write-Host $userCheck
} else {
    Write-Host "Could not check users on EC2" -ForegroundColor $Yellow
}

Write-Host ""
Write-Host ">> Next Steps:" -ForegroundColor $Blue
Write-Host "1. Make sure GitHub secrets are correct (especially EC2_USER = ec2-user)"
Write-Host "2. SSH to EC2 and run setup: ssh -i $KeyPath ec2-user@$EC2Host"
Write-Host "3. On EC2, run the setup script to create users:"
Write-Host "   wget https://raw.githubusercontent.com/lucasmedeiros/wedding-gift-list/main/backend/deploy-scripts/setup-ec2.sh"
Write-Host "   chmod +x setup-ec2.sh"
Write-Host "   ./setup-ec2.sh"
Write-Host "4. Come back and run: .\deploy.ps1" -ForegroundColor $Green

Write-Host ""
Write-Host "TIP: Run setup-ec2.sh on your EC2 instance first!" -ForegroundColor $Yellow
