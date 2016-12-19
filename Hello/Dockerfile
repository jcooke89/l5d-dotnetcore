FROM microsoft/dotnet:latest

COPY . /app

WORKDIR /app

RUN ["dotnet", "restore"]

RUN ["dotnet", "build"]

EXPOSE 7777/tcp

CMD ["dotnet", "run", "--server.urls", "http://0.0.0.0:7777"]
