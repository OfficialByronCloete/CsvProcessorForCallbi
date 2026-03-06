# CsvProcessorForCallbi

## Docker setup

This repository now includes:
- `backend/Dockerfile`
- `frontend/Dockerfile`
- `docker-compose.yml` (SQLite + backend + frontend)

Run everything with:

```bash
docker compose up --build
```

Services:
- Frontend: `http://localhost:9000`
- Backend API: `http://localhost:5062`
- SQLite DB file is persisted in the named volume `sqlite-data` and mounted at `/data/transactions.db`.
