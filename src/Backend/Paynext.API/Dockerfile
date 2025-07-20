# Acesse https://aka.ms/customizecontainer para saber como personalizar seu contêiner de depuração e como o Visual Studio usa este Dockerfile para criar suas imagens para uma depuração mais rápida.

# Esta fase é usada durante a execução no VS no modo rápido (Padrão para a configuração de Depuração)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Esta fase é usada para compilar o projeto de serviço
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Backend/Paynext.API/Paynext.API.csproj", "src/Backend/Paynext.API/"]
COPY ["src/Backend/Paynext.IOC/Paynext.IOC.csproj", "src/Backend/Paynext.IOC/"]
COPY ["src/Backend/Paynext.Infra/Paynext.Infra.csproj", "src/Backend/Paynext.Infra/"]
COPY ["src/Backend/Paynext.Application/Paynext.Application.csproj", "src/Backend/Paynext.Application/"]
COPY ["SharedKernel/SharedKernel.csproj", "SharedKernel/"]
COPY ["src/Backend/Paynext.Domain/Paynext.Domain.csproj", "src/Backend/Paynext.Domain/"]
RUN dotnet restore "./src/Backend/Paynext.API/Paynext.API.csproj"
COPY . .
WORKDIR "/src/src/Backend/Paynext.API"
RUN dotnet build "./Paynext.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Esta fase é usada para publicar o projeto de serviço a ser copiado para a fase final
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Paynext.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Esta fase é usada na produção ou quando executada no VS no modo normal (padrão quando não está usando a configuração de Depuração)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Paynext.API.dll"]