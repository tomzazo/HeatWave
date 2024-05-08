FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /runtime
COPY ./app /runtime

EXPOSE 8080

CMD [ "dotnet", "bin/Debug/net8.0/HeatWave.dll" ]
