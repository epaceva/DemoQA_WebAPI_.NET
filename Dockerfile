# This image includes .NET SDK and pre-installed browsers (Chromium, Firefox, WebKit)
FROM mcr.microsoft.com/playwright/dotnet:v1.47.0-jammy

# Set the working directory inside the container
WORKDIR /app

# Copy all project files from your machine to the container
COPY . .

# Change directory to the test project folder
WORKDIR /app/AutomationTests

# Build the project
# This will also restore NuGet packages
RUN dotnet build

ENV HEADLESS=true

# Define the entry point
# When the container starts, it will run the tests
ENTRYPOINT ["dotnet", "test"]