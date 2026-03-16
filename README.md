# RShopOnline

**Simple ASP.NET Core WebAPI for an online shop**

## Features
- PostgreSQL as the main persistent storage
- Authentication using JWT Token
- Full Custom Role-based authorization
- Saving logs in OpenSearch
- Saving images in Minio

## How to build && run locally

1. Download .NET SDK from the official source
2. Download Docker for your OS
3. Clone the project from github to your PC
```bash
https://github.com/gritsruslan/RShopOnline.git
```
4. Use docker to initialize all the dependencies
```bash
cd docker
docker compose up -d
```
5. Create and apply migrations for Postgres:
```
cd ..
dotnet ef migrations add Initial -p .\src\RShopOnline.Storage\ -s .\src\RShopOnline.API\
dotnet ef database update -p .\src\RShopOnline.Storage\ -s .\src\RShopOnline.API\
```
5. Build the project in release mode && Run API:
```bash
dotnet build .\src\RShopOnline.API\RShopOnline.API.csproj -c Release
dotnet run --project .\src\RShopOnline.API\RShopOnline.API.csproj -c Release
```
- Optional: Run tests
```
dotnet test
```

- Swagger API is avaliable at: `http://localhost:7777/swagger`
- OpenSearch Dashboards for logs at: `http://localhost:5601`

**Fully Educational project**
