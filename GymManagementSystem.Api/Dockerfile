FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GymManagementSystem.Api/GymManagementSystem.Api.csproj", "GymManagementSystem.Api/"]
COPY ["GymManagementSystem.Infrastructure/GymManagementSystem.Infrastructure.csproj", "GymManagementSystem.Infrastructure/"]
COPY ["Gym_Management_System.Core/GymManagementSystem.Core.csproj", "Gym_Management_System.Core/"]
RUN dotnet restore "GymManagementSystem.Api/GymManagementSystem.Api.csproj"
COPY . .
RUN dotnet publish "GymManagementSystem.Api/GymManagementSystem.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "GymManagementSystem.Api.dll"]