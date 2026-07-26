#!/bin/bash
rm -rf Migrations/
rm -f app.db
dotnet ef migrations add InitialCreate
dotnet ef database update