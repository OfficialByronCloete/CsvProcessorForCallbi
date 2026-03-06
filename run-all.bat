@echo off
cd /d "%~dp0"
cls
echo Starting services (sqlite, backend, frontend)...
docker compose up -d --build > compose-startup.log 2>&1
if errorlevel 1 (
  echo.
  echo Failed to start services. See compose-startup.log for details.
  pause
  exit /b 1
)
echo.
echo Services are running:
echo Frontend: http://localhost:9000
echo Backend : http://localhost:5062/swagger
echo.
echo Use "docker compose logs -f" to follow logs.
pause
