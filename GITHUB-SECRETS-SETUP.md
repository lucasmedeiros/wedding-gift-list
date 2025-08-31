# GitHub Secrets Configuration Guide

Este guia detalha como configurar corretamente os secrets do GitHub para deployment automático.

## 🔑 **Secrets Necessários**

Vá para **Settings → Secrets and variables → Actions** no seu repositório GitHub e adicione:

### **Frontend Secrets:**
- `REACT_APP_API_URL`: URL da sua API (ex: `http://18.219.60.144`)

### **Backend Secrets:**
- `EC2_SSH_KEY`: Chave SSH privada (.pem)
- `EC2_HOST`: IP público da EC2 (ex: `18.219.60.144`)  
- `EC2_USER`: Usuário SSH (geralmente `ubuntu`)

## 🛠️ **Configuração Passo a Passo**

### **1. Preparar a Chave SSH**

#### **No Windows (PowerShell):**
```powershell
# Navegue para onde está sua chave .pem
cd C:\path\to\your\key

# Veja o conteúdo da chave
Get-Content your-key.pem

# Copie TODO o conteúdo (incluindo as linhas BEGIN/END)
```

#### **Formato correto da chave:**
```
-----BEGIN RSA PRIVATE KEY-----
MIIEpAIBAAKCAQEA1234567890abcdef...
...todas as linhas da chave...
...não pule nenhuma linha...
-----END RSA PRIVATE KEY-----
```

### **2. Configurar GitHub Secrets**

1. **Acesse:** `https://github.com/lucasmedeiros/wedding-gift-list/settings/secrets/actions`

2. **Clique:** "New repository secret"

3. **Adicione cada secret:**

#### **EC2_SSH_KEY:**
- Nome: `EC2_SSH_KEY`
- Valor: Cole **TODO** o conteúdo do arquivo .pem (incluindo BEGIN/END)

#### **EC2_HOST:**
- Nome: `EC2_HOST` 
- Valor: `18.219.60.144` (substitua pelo seu IP real)

#### **EC2_USER:**
- Nome: `EC2_USER`
- Valor: `ubuntu`

#### **REACT_APP_API_URL:**
- Nome: `REACT_APP_API_URL`
- Valor: `http://18.219.60.144` (substitua pelo seu IP real)

### **3. Verificar EC2 Configuration**

SSH na sua EC2 e verifique:

```bash
# SSH para EC2
ssh -i your-key.pem ubuntu@18.219.60.144

# Verificar se o usuário ubuntu pode usar sudo
sudo whoami
# Deve retornar: root

# Verificar se as pastas existem
ls -la /home/ubuntu/
ls -la /opt/

# Sair
exit
```

## 🧪 **Testar a Configuração SSH**

### **Teste Local (antes do deployment):**
```powershell
# Teste a conexão SSH localmente
ssh -i your-key.pem ubuntu@18.219.60.144

# Se conectar com sucesso, sua chave está correta
# Digite 'exit' para sair
```

### **Teste de Secrets no GitHub:**
Depois de configurar todos os secrets:
1. Vá para **Actions** no seu repositório
2. Execute um workflow manualmente
3. Veja os logs para verificar se conectou

## ❌ **Troubleshooting - Erros Comuns**

### **"Permission denied (publickey)"**
- ✅ Verifique se copiou a chave .pem COMPLETA (incluindo BEGIN/END)
- ✅ Confirme que o IP da EC2 está correto
- ✅ Teste SSH local primeiro

### **"Host key verification failed"**
- ✅ EC2 Security Group deve permitir SSH (porta 22) do seu IP
- ✅ Verifique se a EC2 está rodando

### **"sudo: command not found"**
- ✅ Use `ubuntu` como usuário, não `ec2-user`
- ✅ Verifique se a EC2 é Ubuntu 22.04

### **"File not found: deploy.zip"**
- ✅ Problema no workflow - rerun o deployment

## 🔍 **Verificar Status dos Secrets**

Depois de adicionar todos os secrets, você deve ver:
- ✅ `EC2_SSH_KEY` (Updated X minutes ago)
- ✅ `EC2_HOST` (Updated X minutes ago)  
- ✅ `EC2_USER` (Updated X minutes ago)
- ✅ `REACT_APP_API_URL` (Updated X minutes ago)

## 📋 **Checklist Final**

Antes de fazer deployment, confirme:

- [ ] **EC2 rodando** e acessível via SSH
- [ ] **Security Group** permite SSH (22) e HTTP (80)
- [ ] **Chave SSH** testada localmente
- [ ] **Todos os 4 secrets** configurados no GitHub
- [ ] **IP addresses** corretos nos secrets
- [ ] **Frontend** consegue acessar backend via HTTP

## 🚀 **Próximos Passos**

Depois de configurar tudo:
1. Execute `.\deploy.ps1`
2. Escolha opção 3 (Both frontend and backend)
3. Monitore os GitHub Actions
4. Verifique se o site funciona

## 🆘 **Se Ainda Não Funcionar**

1. **SSH Test Manual:**
```powershell
ssh -i your-key.pem ubuntu@YOUR_EC2_IP "echo 'SSH works!'"
```

2. **Check GitHub Actions Logs:**
- Vá para Actions → Workflow → Logs
- Procure por detalhes do erro

3. **EC2 Logs:**
```bash
# SSH para EC2
ssh -i your-key.pem ubuntu@YOUR_EC2_IP

# Verificar logs do serviço
sudo journalctl -u wedding-gift-api -f
```

## 💡 **Dicas**

- **Nunca** compartilhe sua chave SSH privada
- **Sempre** teste SSH localmente primeiro
- **Monitore** os logs do GitHub Actions para debuggar
- **Use** IPs específicos, não hostnames
- **Mantenha** sua EC2 atualizada: `sudo apt update && sudo apt upgrade`
