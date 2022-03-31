FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Ellie.VotesApi/Ellie.VotesApi.csproj", "Ellie.VotesApi/"]
RUN dotnet restore "src/Ellie.VotesApi/Ellie.VotesApi.csproj"
COPY . .
WORKDIR "/src/Ellie.VotesApi"
RUN dotnet build "Ellie.VotesApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ellie.VotesApi.csproj" -c Release -o /app/publish

FROM base AS file
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ellie.VotesApi.dll"]