@echo off
cls
echo.
echo ===============================================
echo      SIGORTA YONETIM SISTEMI BASLATICI
echo ===============================================
echo.

:: Backend terminali baslat
echo Backend baslatiliyor... (Port: 5293)
start "SIGORTA API BACKEND" cmd /k "cd /d SigortaYonetimAPI && echo Backend baslatiliyor... && dotnet run"

:: 2 saniye bekle
timeout /t 2 /nobreak >nul

:: Frontend terminali baslat  
echo Frontend baslatiliyor... (Port: 3000)
start "SIGORTA FRONTEND" cmd /k "cd /d sigorta-yonetim-frontend && echo Frontend baslatiliyor... && npm start"

echo.
echo Her iki servis de baslatildi!
echo.
echo PROJE BILGILERI:
echo - Backend API: http://localhost:5293
echo - Swagger UI: http://localhost:5293/swagger
echo - Frontend: http://localhost:3000
echo - Test Login: admin@adayazilim.com / admin123
echo.
echo Terminal pencerelerini acik birak!
echo Kapatmak icin bu pencereyi kapat.
echo.
pause 