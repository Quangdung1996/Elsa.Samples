#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["Elsa.Samples/Elsa.Samples.csproj", "Elsa.Samples/"]
RUN dotnet restore "Elsa.Samples/Elsa.Samples.csproj"
COPY . .
WORKDIR "/src/Elsa.Samples"
RUN dotnet build "Elsa.Samples.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Elsa.Samples.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Elsa.Samples.dll"]
