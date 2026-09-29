# 📋 UserApp

Aplikacja do zarządzania zadaniami i tablicami (bilboardami), zbudowana w architekturze **mikroserwisów** na .NET 8 z frontendem w Vue 3 / Quasar.

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)
![Vue](https://img.shields.io/badge/Vue-3-4FC08D?logo=vuedotjs&logoColor=white)
![Quasar](https://img.shields.io/badge/Quasar-2-1976D2?logo=quasar&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=white)
![RabbitMQ](https://img.shields.io/badge/RabbitMQ-FF6600?logo=rabbitmq&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?logo=docker&logoColor=white)

## ✨ Funkcje

- Rejestracja, logowanie, aktywacja konta i reset hasła (JWT + refresh token)
- Tworzenie, edycja, wyszukiwanie i usuwanie zadań wraz z załącznikami
- Tablice (bilboardy) z zadaniami i zmianą statusów
- Komunikacja asynchroniczna między serwisami przez RabbitMQ (MassTransit)
- Powiadomienia e-mail
- Dokumentacja API w Swaggerze dla każdego serwisu

## 🏗️ Architektura

```
                ┌──────────────┐
  Przeglądarka  │     WWW      │  Vue 3 + Quasar
       │        └──────┬───────┘
       ▼               ▼
              ┌──────────────────┐
              │   Gateway.Api    │  jedyny punkt wejścia, sesja, JWT
              └───┬────────┬─────┘
                  │  HTTP  │
        ┌─────────┘        └──────────┐──────────────┐
        ▼                  ▼          ▼
  ┌──────────┐      ┌──────────┐  ┌──────────────┐
  │ User.Api │      │ Task.Api │  │ Bilboard.Api │
  └────┬─────┘      └────┬─────┘  └──────┬───────┘
       └─────── RabbitMQ (MassTransit) ──┘
                          │
                    SQL Server (osobna baza na serwis)
```

| Projekt | Rola |
|---|---|
| `Gateway.Api` | Brama API – kieruje żądania do serwisów, zarządza sesją i tokenami |
| `User.Api` | Użytkownicy, autoryzacja, tokeny JWT i refresh |
| `Task.Api` | Zadania, statusy, pliki |
| `Bilboard.Api` | Tablice i ich elementy |
| `Common` | Wspólny kod: dostęp do bazy (Dapper), filtry, middleware, migracje |
| `Models` | Wspólne modele i zdarzenia RabbitMQ |
| `WWW` | Frontend (Vue 3, Quasar, Pinia, TypeScript) |

## 🧰 Stack

**Backend:** .NET 8, ASP.NET Core, Dapper, SQL Server, MassTransit + RabbitMQ, JWT, Serilog / log4net, Swagger
**Frontend:** Vue 3, Quasar, Pinia, Vue Router, Axios, TypeScript
**Infrastruktura:** Docker, Docker Compose

## 🚀 Uruchomienie

### Wymagania

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- Node.js 18/20 (frontend)
- Dostęp do instancji SQL Server (trzy bazy: `Training_User`, `Training_Task`, `Training_Bilboard`)

### 1. Konfiguracja sekretów

Sekrety **nie są** w repozytorium. Skopiuj wzór i uzupełnij go własnymi wartościami:

```bash
cp .env.example .env
```

| Zmienna | Opis |
|---|---|
| `USER_DB_CONNECTION`, `TASK_DB_CONNECTION`, `BILBOARD_DB_CONNECTION` | Connection stringi do baz SQL Server |
| `JWT_PRIVATE_KEY` | Prywatny klucz RSA (base64) używany przez `User.Api`; klucz publiczny jest w `appsettings.json` |
| `EMAIL_USER`, `EMAIL_PASSWORD` | Konto SMTP (Gmail – użyj hasła aplikacji) |
| `CERT_PASSWORD` | Hasło certyfikatu HTTPS dla Kestrel |

Plik `.env` jest w `.gitignore`. Przy uruchamianiu bez Dockera (np. z Visual Studio) ustaw te same wartości przez [`dotnet user-secrets`](https://learn.microsoft.com/aspnet/core/security/app-secrets) lub zmienne środowiskowe, np. `ConnectionStrings__DefaultConnection`.

### 2. Backend (Docker Compose)

Ścieżki wolumenów w `docker-compose.yml` (`E:\UserApp\Files`, `C:\certs`) dostosuj do swojego środowiska, a w `C:\certs` umieść certyfikat `appcert.pfx`.

```bash
docker compose up --build
```

| Serwis | HTTP | HTTPS |
|---|---|---|
| User.Api | 8000 | 8010 |
| Task.Api | 8001 | 8011 |
| Gateway.Api | 8002 | 8012 |
| Bilboard.Api | 8003 | 8013 |
| RabbitMQ (panel) | 15672 | – |

Swagger każdego serwisu jest dostępny pod `/swagger`.

### 3. Frontend

```bash
cd WWW
npm install
npm run dev
```

Produkcyjny build: `npm run build`.

## 📁 Struktura repozytorium

```
UserApp/
├── Bilboard.Api/      # mikroserwis tablic
├── Task.Api/          # mikroserwis zadań
├── User.Api/          # mikroserwis użytkowników
├── Gateway.Api/       # brama API
├── Common/            # współdzielona biblioteka
├── Models/            # modele i zdarzenia
├── WWW/               # frontend Quasar
├── Files/             # wgrywane pliki (poza repo)
├── docker-compose.yml
└── .env.example
```

## 🔐 Bezpieczeństwo

- Hasła użytkowników przechowywane jako hash HMAC-SHA512 z solą
- Autoryzacja JWT (RS256) z odświeżaniem tokenów, ochrona CSRF
- Sanityzacja HTML (HtmlSanitizer)
- **Nigdy nie commituj** plików `.env`, certyfikatów ani danych logowania

## 📄 Licencja

Projekt edukacyjny. Dodaj plik `LICENSE`, jeśli chcesz określić warunki użycia.
