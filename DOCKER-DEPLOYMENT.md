# Docker Deployment Guide

This guide explains how to deploy your Wedding Gift List application using Docker containers on AWS EC2.

## 🐋 **Docker Deployment Architecture**

- **Frontend**: GitHub Pages (Static files)
- **Backend**: Docker Container on AWS EC2
- **Database**: SQLite in Docker Volume (persistent)
- **Reverse Proxy**: Nginx on EC2 → Docker Container
- **CI/CD**: GitHub Actions with tag-based deployment

## ✅ **Benefits of Docker Deployment**

- 🔒 **Isolation**: Application runs in its own container
- 🔄 **Consistency**: Same environment in development and production
- 📦 **Easy Updates**: Simply replace container with new version
- 🛡️ **Security**: Non-root user inside container
- 📊 **Monitoring**: Built-in health checks
- 💾 **Persistent Data**: SQLite database survives container updates
- 🚀 **Fast Deployment**: No need to install .NET on EC2

## 🛠️ **Setup Instructions**

### **1. Prepare Your EC2 Instance**

#### **1.1 Launch EC2 Instance**
- **AMI**: Amazon Linux 2023 (ami-0b016c703b95ecbe4)
- **Type**: t2.micro (Free Tier)
- **Security Group**: Allow SSH (22), HTTP (80), HTTPS (443)

#### **1.2 Setup Docker Environment**
```bash
# SSH to your EC2 instance
ssh -i your-key.pem ec2-user@YOUR_EC2_IP

# Download and run Docker setup script
wget https://raw.githubusercontent.com/lucasmedeiros/wedding-gift-list/main/backend/deploy-scripts/setup-ec2-docker.sh
chmod +x setup-ec2-docker.sh
./setup-ec2-docker.sh

# IMPORTANT: Log out and log back in for Docker group changes
exit
ssh -i your-key.pem ec2-user@YOUR_EC2_IP
```

### **2. Configure GitHub Secrets**

Go to: `https://github.com/lucasmedeiros/wedding-gift-list/settings/secrets/actions`

| Secret Name | Value |
|-------------|-------|
| `EC2_SSH_KEY` | Contents of your .pem file |
| `EC2_HOST` | Your EC2 public IP |
| `EC2_USER` | `ec2-user` |
| `REACT_APP_API_URL` | `http://YOUR_EC2_IP` |

### **3. Deploy Your Application**

```powershell
# Use the new Docker deployment script
.\deploy-docker.ps1

# Choose your deployment option:
# 1) Frontend only
# 2) Backend only (Docker)
# 3) Both frontend and backend
```

## 📋 **Docker Container Details**

### **Container Configuration**
- **Image**: Built from `backend/Dockerfile`
- **Name**: `wedding-gift-api`
- **Port**: 5001 (mapped to host port 5001)
- **User**: Non-root `appuser` (UID 1000)
- **Restart Policy**: `unless-stopped`
- **Volume**: `wedding-gift-data` for SQLite database

### **Environment Variables**
```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:5001
ConnectionStrings__DefaultConnection=Data Source=/app/data/wedding_gifts.db
```

### **Health Checks**
- **Endpoint**: `http://localhost:5001/api/gifts`
- **Interval**: 30 seconds
- **Timeout**: 10 seconds
- **Retries**: 3

## 🔧 **Managing Your Docker Container**

### **Useful Commands (SSH to EC2)**

```bash
# View container status
docker ps

# View container logs
docker logs wedding-gift-api

# Follow logs in real-time
docker logs -f wedding-gift-api

# Restart container
docker restart wedding-gift-api

# Stop container
docker stop wedding-gift-api

# Start container
docker start wedding-gift-api

# View container stats
docker stats wedding-gift-api

# Execute commands inside container
docker exec -it wedding-gift-api bash
```

### **View Application Logs**
```bash
# View last 50 lines of logs
docker logs --tail 50 wedding-gift-api

# View logs from last 10 minutes
docker logs --since 10m wedding-gift-api

# Save logs to file
docker logs wedding-gift-api > app-logs.txt
```

### **Database Management**
```bash
# View database volume
docker volume inspect wedding-gift-data

# Backup database
docker run --rm -v wedding-gift-data:/data -v $(pwd):/backup alpine tar czf /backup/wedding-gifts-backup.tar.gz -C /data .

# Restore database (if needed)
docker run --rm -v wedding-gift-data:/data -v $(pwd):/backup alpine tar xzf /backup/wedding-gifts-backup.tar.gz -C /data
```

## 📊 **Monitoring Your Application**

### **Health Check**
```bash
# Check if API is responding
curl http://localhost:5001/api/gifts

# Check via Nginx (public)
curl http://YOUR_EC2_IP/api/gifts
```

### **Container Health**
```bash
# View container health status
docker inspect --format='{{.State.Health.Status}}' wedding-gift-api

# View health check logs
docker inspect --format='{{range .State.Health.Log}}{{.Output}}{{end}}' wedding-gift-api
```

### **System Resources**
```bash
# View real-time container resource usage
docker stats wedding-gift-api

# View disk usage
docker system df

# View volume usage
docker volume ls
```

## 🚨 **Troubleshooting**

### **Common Issues**

#### **Container Won't Start**
```bash
# Check container logs for errors
docker logs wedding-gift-api

# Check if port is available
sudo netstat -tlnp | grep :5001

# Restart Docker service
sudo systemctl restart docker
```

#### **API Not Responding**
```bash
# Check container is running
docker ps | grep wedding-gift-api

# Check container health
docker inspect wedding-gift-api | grep -A 10 Health

# Check nginx configuration
sudo nginx -t
sudo systemctl status nginx
```

#### **Database Issues**
```bash
# Check volume exists
docker volume ls | grep wedding-gift-data

# Check volume mount inside container
docker exec wedding-gift-api ls -la /app/data/

# Check database file permissions
docker exec wedding-gift-api ls -la /app/data/wedding_gifts.db
```

#### **Deployment Failed**
```bash
# Clean up failed deployment
docker stop wedding-gift-api || true
docker rm wedding-gift-api || true
docker rmi wedding-gift-api:latest || true

# Rebuild and restart
cd /opt/wedding-gift-api/backend
docker build -t wedding-gift-api:latest .
./deploy-scripts/deploy-docker.sh
```

### **Performance Tuning**

```bash
# Clean up unused Docker resources
docker system prune -f

# Remove old images
docker image prune -a -f

# Optimize container resource limits
docker update --memory=512m --cpus=0.5 wedding-gift-api
```

## 🔄 **Rollback Process**

If you need to rollback to a previous version:

```bash
# Stop current container
docker stop wedding-gift-api
docker rm wedding-gift-api

# Deploy previous version using GitHub tag
# Go to GitHub Actions and re-run a previous successful deployment
```

## 📈 **Scaling Options**

### **Vertical Scaling (Upgrade EC2)**
- Change EC2 instance type (t2.micro → t2.small → t2.medium)
- Adjust Docker container resource limits

### **Horizontal Scaling (Multiple Containers)**
```bash
# Run multiple instances with load balancer
docker run -d --name wedding-gift-api-1 -p 5001:5001 wedding-gift-api:latest
docker run -d --name wedding-gift-api-2 -p 5002:5001 wedding-gift-api:latest

# Update Nginx to load balance between containers
```

## 💡 **Best Practices**

1. **Regular Backups**: Backup your database volume regularly
2. **Log Management**: Rotate Docker logs to prevent disk space issues
3. **Security Updates**: Keep EC2 and Docker updated
4. **Monitoring**: Set up CloudWatch or monitoring tools
5. **SSL**: Consider adding SSL certificate for HTTPS

## 📱 **Quick Reference**

### **Deployment Commands**
```powershell
# Windows (Local)
.\deploy-docker.ps1              # Interactive deployment
.\deploy-docker.ps1 1.2.3       # Quick deployment with version
```

### **Management Commands**
```bash
# EC2 (SSH)
docker logs -f wedding-gift-api           # View logs
docker restart wedding-gift-api           # Restart app
sudo systemctl restart nginx              # Restart proxy
./backend/deploy-scripts/deploy-docker.sh  # Manual redeploy
```

### **URLs**
- **Frontend**: https://lucasmedeiros.github.io/wedding-gift-list
- **Backend API**: http://YOUR_EC2_IP/api/gifts
- **GitHub Actions**: https://github.com/lucasmedeiros/wedding-gift-list/actions

---

**🐋 Your Docker deployment is now ready! Happy containerizing! 🚀**
