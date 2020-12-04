#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Pizza/Pizza.csproj", "Pizza/"]
COPY ["Entities/Entities.csproj", "Entities/"]
RUN dotnet restore "Pizza/Pizza.csproj"
COPY . .
WORKDIR "/src/Pizza"
RUN dotnet build "Pizza.csproj" -c Release -o /app/build
WORKDIR "/src/Entities"
RUN dotnet build "Entities.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR "/src/Pizza"
RUN dotnet publish "Pizza.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "Pizza.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Pizza.dll