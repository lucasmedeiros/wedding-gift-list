# Quick Setup Guide for Amazon Linux 2023 Deployment

param(
    [string]$EC2Host,
    [string]$KeyPath
)

# Colors for output
$Red = "Red"
$Green = "Green" 
$Yellow = "Yellow"
$Blue = "Cyan"

Write-Host "*** Amazon Linux 2023 Deployment Setup ***" -ForegroundColor $Blue
Write-Host "==========================================="

# Get parameters if not provided
if (-not $EC2Host) {
    $EC2Host = Read-Host "Enter your EC2 IP address (e.g., 18.219.60.144)"
}

if (-not $KeyPath) {
    $KeyPath = Read-Host "Enter path to your .pem key file"
}

Write-Host ""
Write-Host ">> Updating project for Amazon Linux 2023..." -ForegroundColor $Yellow

# Test SSH connection first
Write-Host ">> Testing SSH connection..." -ForegroundColor $Blue
$sshTest = ssh -i $KeyPath -o ConnectTimeout=10 -o StrictHostKeyChecking=no ec2-user@$EC2Host "echo 'SSH OK'; whoami" 2>$null

if ($LASTEXITCODE -eq 0) {
    Write-Host "SUCCESS: SSH connection works!" -ForegroundColor $Green
    Write-Host "Server response: $sshTest" -ForegroundColor $Green
} else {
    Write-Host "WARNING: Could not connect via SSH" -ForegroundColor $Yellow
    Write-Host "Please check your EC2 instance is running and key file is correct" -ForegroundColor $Yellow
}

Write-Host ""
Write-Host ">> GitHub Secrets Configuration:" -ForegroundColor $Blue
Write-Host "You need to set these in GitHub repository settings:" -ForegroundColor $Yellow
Write-Host ""
Write-Host "1. Go to: https://github.com/lucasmedeiros/wedding-gift-list/settings/secrets/actions"
Write-Host ""
Write-Host "2. Add/Update these secrets:" -ForegroundColor $Green
Write-Host "   EC2_SSH_KEY: [Contents of $KeyPath file - entire file including BEGIN/END lines]"
Write-Host "   EC2_HOST: $EC2Host"
Write-Host "   EC2_USER: ec2-user"
Write-Host "   REACT_APP_API_URL: http://$EC2Host"
Write-Host ""

# Show key file path
Write-Host ">> Your SSH key file content (copy this EXACTLY):" -ForegroundColor $Blue
if (Test-Path $KeyPath) {
    Write-Host "File found at: $KeyPath" -ForegroundColor $Green
    Write-Host "COPY THIS ENTIRE CONTENT for EC2_SSH_KEY secret:" -ForegroundColor $Yellow
    Write-Host "----------------------------------------" -ForegroundColor $Yellow
    Get-Content $KeyPath
    Write-Host "----------------------------------------" -ForegroundColor $Yellow
    Write-Host ""
} else {
    Write-Host "ERROR: Key file not found at: $KeyPath" -ForegroundColor $Red
}

Write-Host ">> Next Steps:" -ForegroundColor $Blue
Write-Host "1. Configure GitHub Secrets (see above)" -ForegroundColor $Yellow
Write-Host "2. SSH to EC2 and run setup: ssh -i $KeyPath ec2-user@$EC2Host"
Write-Host "3. On EC2, run:"
Write-Host "   wget https://raw.githubusercontent.com/lucasmedeiros/wedding-gift-list/main/backend/deploy-scripts/setup-ec2.sh"
Write-Host "   chmod +x setup-ec2.sh"
Write-Host "   ./setup-ec2.sh"
Write-Host "4. Come back and run: .\deploy.ps1" -ForegroundColor $Green

Write-Host ""
Write-Host ">> Amazon Linux 2023 Details:" -ForegroundColor $Blue
Write-Host "   AMI: ami-0b016c703b95ecbe4 (64-bit x86)"
Write-Host "   Default User: ec2-user"
Write-Host "   Package Manager: dnf (yum)"
Write-Host "   Firewall: firewalld"
Write-Host "   Service User: nginx"

Write-Host ""
Write-Host "TIP: Test SSH connection first before deployment!" -ForegroundColor $Green
Write-Host "     ssh -i $KeyPath ec2-user@$EC2Host"
