FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443
EXPOSE 9300

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TestGrpcWebsite.csproj", "TestGrpcWebsite/"]
RUN dotnet restore "TestGrpcWebsite/TestGrpcWebsite.csproj"
WORKDIR "/src/TestGrpcWebsite"
COPY . .
RUN dotnet build "TestGrpcWebsite.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestGrpcWebsite.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestGrpcWebsite.dll"]
