FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["GrpcClientConsole.csproj", "GrpcClientConsole/"]
RUN dotnet restore "GrpcClientConsole/GrpcClientConsole.csproj"
WORKDIR "/src/GrpcClientConsole"
COPY . .
RUN dotnet build "GrpcClientConsole.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GrpcClientConsole.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrpcClientConsole.dll"]
