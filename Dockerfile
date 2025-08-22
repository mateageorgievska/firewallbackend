FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY /Firewall/ Firewall/
COPY /Firewall.Repositories/ Firewall.Repositories/
COPY /Firewall.Services/ Firewall.Services
COPY /Model/ Model/
COPY /Persistence/ Persistence/

#COPY nuget.config .

RUN dotnet restore Firewall/Firewall.csproj

COPY . .

RUN dotnet publish Firewall/Firewall.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Firewall.dll"]
