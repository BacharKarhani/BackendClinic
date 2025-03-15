# Use the official ASP.NET runtime image as a base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the .NET SDK for building the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj file and restore dependencies
COPY backendclinic.csproj ./
RUN dotnet restore "backendclinic.csproj"

# Copy the remaining application files
COPY . ./
WORKDIR "/src"

# Build the application
RUN dotnet build "backendclinic.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "backendclinic.csproj" -c Release -o /app/publish

# Create the final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set the entry point for the application
ENTRYPOINT ["dotnet", "backendclinic.dll"]
