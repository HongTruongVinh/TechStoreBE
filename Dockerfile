# Sử dụng hình ảnh .NET SDK để build dự án
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Sao chép và build dự án
COPY . ./
RUN dotnet publish -c Release -o out

# Tạo container từ .NET runtime để chạy ứng dụng
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Mở cổng ứng dụng chạy trên cổng 80
EXPOSE 80

# Lệnh để chạy ứng dụng
ENTRYPOINT ["dotnet", "TechStoreAPI.dll"]