FROM dhi.io/aspnetcore:10.0.8

WORKDIR /app

COPY builds/AspNetCore/ .

EXPOSE 8080

ENTRYPOINT ["dotnet", "IWKits.Api.AspNetCore.dll"]