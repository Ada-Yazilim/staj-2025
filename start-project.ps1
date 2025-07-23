# =======================================================
#          SIGORTA YONETIM SISTEMI BASLATICI
# =======================================================

Write-Host ""
Write-Host "=============================================" -ForegroundColor Blue
Write-Host "     SIGORTA YONETIM SISTEMI BASLATICI      " -ForegroundColor Blue
Write-Host "=============================================" -ForegroundColor Blue
Write-Host ""

# Klasor varligini kontrol et
$backendPath = "SigortaYonetimAPI"
$frontendPath = "sigorta-yonetim-frontend"

if (-not (Test-Path $backendPath)) {
    Write-Host "HATA: Backend klasoru bulunamadi!" -ForegroundColor Red
    Write-Host "Beklenen: $backendPath" -ForegroundColor Yellow
    Read-Host "Devam etmek icin Enter'a basin..."
    exit
}

if (-not (Test-Path $frontendPath)) {
    Write-Host "HATA: Frontend klasoru bulunamadi!" -ForegroundColor Red
    Write-Host "Beklenen: $frontendPath" -ForegroundColor Yellow
    Read-Host "Devam etmek icin Enter'a basin..."
    exit
}

Write-Host "Proje klasorleri bulundu!" -ForegroundColor Green
Write-Host ""

# Backend baslat (Yeni terminal)
Write-Host "Backend baslatiliyor..." -ForegroundColor Yellow
Write-Host "Port: http://localhost:5000" -ForegroundColor Cyan
Write-Host "Swagger: http://localhost:5000/swagger" -ForegroundColor Cyan

$backendScript = @"
Write-Host '======================================' -ForegroundColor Green
Write-Host '    SIGORTA YONETIM API BACKEND      ' -ForegroundColor Green  
Write-Host '======================================' -ForegroundColor Green
Write-Host ''
Write-Host 'Backend baslatiliyor...' -ForegroundColor Yellow
Write-Host 'URL: http://localhost:5000' -ForegroundColor Cyan
Write-Host 'Swagger: http://localhost:5000/swagger' -ForegroundColor Cyan
Write-Host ''
Write-Host 'Durdurmak icin Ctrl+C kullaniniz' -ForegroundColor Gray
Write-Host ''
Set-Location '$((Get-Location).Path)\$backendPath'
dotnet run
"@

Start-Process powershell -ArgumentList "-NoExit", "-Command", $backendScript

Start-Sleep -Seconds 2

# Frontend baslat (Yeni terminal) 
Write-Host "Frontend baslatiliyor..." -ForegroundColor Yellow
Write-Host "Port: http://localhost:3000" -ForegroundColor Cyan

$frontendScript = @"
Write-Host '======================================' -ForegroundColor Blue
Write-Host '     SIGORTA YONETIM FRONTEND        ' -ForegroundColor Blue
Write-Host '======================================' -ForegroundColor Blue
Write-Host ''
Write-Host 'Frontend baslatiliyor...' -ForegroundColor Yellow
Write-Host 'URL: http://localhost:3000' -ForegroundColor Cyan
Write-Host ''
Write-Host 'Durdurmak icin Ctrl+C kullaniniz' -ForegroundColor Gray
Write-Host ''
Set-Location '$((Get-Location).Path)\$frontendPath'
npm start
"@

Start-Process powershell -ArgumentList "-NoExit", "-Command", $frontendScript

Write-Host ""
Write-Host "Her iki servis de baslatildi!" -ForegroundColor Green
Write-Host ""
Write-Host "PROJE DURUM BILGILERI:" -ForegroundColor White
Write-Host "- Backend API: http://localhost:5000" -ForegroundColor Cyan
Write-Host "- Swagger UI: http://localhost:5000/swagger" -ForegroundColor Cyan  
Write-Host "- Frontend: http://localhost:3000" -ForegroundColor Cyan
Write-Host "- Test Login: admin@test.com / Admin123!" -ForegroundColor Yellow
Write-Host ""
Write-Host "IPUCU: Her iki terminal de acik kalmali!" -ForegroundColor Gray
Write-Host "Durdurmak icin terminallerde Ctrl+C kullanin" -ForegroundColor Gray
Write-Host ""
Write-Host "Iyi gelistirmeler!" -ForegroundColor Magenta

# Script penceresi acik kalsin
Read-Host "Kapatmak icin Enter'a basin..." 