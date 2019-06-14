# Pull image
FROM mcr.microsoft.com/dotnet/core/sdk:2.2
WORKDIR /src

# Add source
ADD ./Ho-Zyo /src/Ho-Zyo
WORKDIR Ho-Zyo

# Build
RUN mkdir /app
RUN dotnet publish -c Release -o /app

# Run
CMD dotnet /app/Ho-Zyo.dll 