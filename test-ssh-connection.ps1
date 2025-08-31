# SSH Connection Test Script for EC2

param(
    [string]$KeyPath,
    [string]$EC2Host
)

# Colors for output
$Red = "Red"
$Green = "Green" 
$Yellow = "Yellow"
$Blue = "Cyan"

Write-Host "*** EC2 SSH Connection Test ***" -ForegroundColor $Blue
Write-Host "================================="

# Get parameters if not provided
if (-not $KeyPath) {
    $KeyPath = Read-Host "Enter path to your .pem key file"
}

if (-not $EC2Host) {
    $EC2Host = Read-Host "Enter your EC2 IP address"
}

# Check if key file exists
if (-not (Test-Path $KeyPath)) {
    Write-Host "ERROR: Key file not found at: $KeyPath" -ForegroundColor $Red
    exit 1
}

# List of common EC2 users to test (ec2-user first for Amazon Linux)
$commonUsers = @("ec2-user", "ubuntu", "centos", "admin")

Write-Host ">> Testing SSH connection with different users..." -ForegroundColor $Yellow
Write-Host ""

$workingUser = $null

foreach ($user in $commonUsers) {
    Write-Host "Testing user: $user" -ForegroundColor $Blue
    
    # Test SSH connection
    $result = ssh -i $KeyPath -o ConnectTimeout=10 -o StrictHostKeyChecking=no $user@$EC2Host "echo 'Connection successful'; whoami; uname -a" 2>$null
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "SUCCESS: Connected with user '$user'!" -ForegroundColor $Green
        Write-Host "Server response:" -ForegroundColor $Green
        Write-Host $result
        $workingUser = $user
        break
    } else {
        Write-Host "FAILED: Could not connect with user '$user'" -ForegroundColor $Red
    }
    Write-Host ""
}

Write-Host ""
if ($workingUser) {
    Write-Host ">> RESULT: Working SSH user is '$workingUser'" -ForegroundColor $Green
    Write-Host ""
    Write-Host ">> Next steps:" -ForegroundColor $Blue
    Write-Host "1. Update GitHub Secret 'EC2_USER' to: $workingUser"
    Write-Host "2. Go to: https://github.com/lucasmedeiros/wedding-gift-list/settings/secrets/actions"
    Write-Host "3. Edit EC2_USER secret and change value to: $workingUser"
    Write-Host "4. Run deployment again with: .\deploy.ps1"
    Write-Host ""
    Write-Host ">> GitHub Secrets should be:" -ForegroundColor $Yellow
    Write-Host "   EC2_SSH_KEY: [Your .pem file contents]"
    Write-Host "   EC2_HOST: $EC2Host"
    Write-Host "   EC2_USER: $workingUser"
    Write-Host "   REACT_APP_API_URL: http://$EC2Host"
} else {
    Write-Host "ERROR: Could not connect with any common users!" -ForegroundColor $Red
    Write-Host ""
    Write-Host ">> Troubleshooting:" -ForegroundColor $Yellow
    Write-Host "1. Check if EC2 instance is running"
    Write-Host "2. Verify Security Group allows SSH (port 22) from your IP"
    Write-Host "3. Confirm the .pem key is correct for this EC2 instance"
    Write-Host "4. Try connecting via AWS Console's EC2 Instance Connect"
}

Write-Host ""
Write-Host "TIP: You can also check the correct user in AWS Console" -ForegroundColor $Blue
Write-Host "     → EC2 → Instances → Select instance → Connect → SSH client tab"
