FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["src/Services/Discount/Discount.Api/Discount.Api.csproj", "src/Services/Discount/Discount.Api/"]
RUN dotnet restore "src/Services/Discount/Discount.Api/Discount.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Discount/Discount.Api"
RUN dotnet build "Discount.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Discount.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Discount.Api.dll"]
