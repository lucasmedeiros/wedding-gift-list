# Deploy da API .NET 8 com Docker na EC2

Este guia mostra como buildar, rodar e atualizar a aplicação `.NET 8` usando **Docker** em uma instância EC2, com persistência do banco SQLite e mapeamento da porta interna 8080 para a porta 80 externa.

---

## 1️⃣ Build e run do container na EC2

```sh
cd ~/wedding-gift-list/backend
docker build -t wedding-gifts .
docker run -d --name wedding-gifts -p 80:8080 -v ~/wedding-gift-data:/opt/wedding-gift-api/data wedding-gifts
```

## 2️⃣ Sempre que houver alterações no código

```sh
cd ~/wedding-gift-list
git pull origin main
cd backend
docker build -t wedding-gifts .
docker stop wedding-gifts && docker rm wedding-gifts
docker run -d --name wedding-gifts -p 80:8080 -v ~/wedding-gift-data:/opt/wedding-gift-api/data wedding-gifts
```

## 3️⃣ Comandos úteis

```sh
docker ps # ver status do container
docker logs wedding-gifts # ver logs da aplicação
curl http://18.219.60.144/ # testar acesso à API
```
