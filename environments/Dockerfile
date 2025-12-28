# ビルドステージ
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine3.23 AS build
WORKDIR /src

# プロジェクトファイルをコピーして依存関係を復元
COPY app/Api/Api.csproj ./Api/
RUN dotnet restore Api/Api.csproj

# ソースコードをコピーしてPublish
COPY app/Api/. ./Api/
WORKDIR /src/Api
RUN dotnet publish Api.csproj -c Release -o /app/publish /p:UseAppHost=true

# ランタイムステージ
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine3.23 AS final
WORKDIR /app

# 公開されたファイルをコピー
COPY --from=build /app/publish .

# ポートを公開（ASP.NET Coreのデフォルトポート）
EXPOSE 8080

# エントリーポイント
ENTRYPOINT ["dotnet", "./Api.dll"]

