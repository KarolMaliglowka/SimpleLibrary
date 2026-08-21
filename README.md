# SimpleLibrary

**PL:** Nowoczesny szablon aplikacji webowej do zarządzania biblioteką, zbudowany w oparciu o **ASP.NET Core 9**, **Entity Framework Core**, **PostgreSQL**, **Angular 19**, **PrimeNG** oraz **Tailwind CSS**.

**EN:** A modern web application template for library management, built with **ASP.NET Core 9**, **Entity Framework Core**, **PostgreSQL**, **Angular 19**, **PrimeNG**, and **Tailwind CSS**.

---

# Wersja polska

## 1. Opis projektu

SimpleLibrary jest kompletnym przykładowym projektem aplikacji webowej przeznaczonej do zarządzania biblioteką.

Projekt został przygotowany nie tylko jako gotowy system biblioteczny, ale również jako **uniwersalny szablon do budowy własnych aplikacji biznesowych**.

Frontend zawiera wiele gotowych przykładów interfejsów i sposobów budowania stron: dashboard, tabele danych, formularze, okna dialogowe, filtrowanie, sortowanie, paginację, komunikaty, formularze z walidacją, strony typu landing page oraz widoki administracyjne.

Dzięki temu projekt można łatwo dostosować do własnych potrzeb i wykorzystać jako bazę np. dla:

- systemu magazynowego,
- systemu CRM,
- panelu administracyjnego,
- systemu rezerwacji,
- systemu zarządzania produktami,
- systemu zarządzania użytkownikami,
- aplikacji biznesowej CRUD,
- własnego projektu SaaS.

**Istotą projektu jest pokazanie szerokiego zakresu możliwości budowania stron i komponentów Angular/PrimeNG, a nie ograniczenie się wyłącznie do funkcjonalności biblioteki.**

## 2. Najważniejsze funkcjonalności

### Backend

- ASP.NET Core 9 Web API
- architektura podzielona na warstwy:
  - `Library.Api`
  - `Library.Application`
  - `Library.Core`
  - `Library.Infrastructure`
- Entity Framework Core 9
- PostgreSQL
- migracje EF Core
- Repository Pattern
- Unit of Work
- DTO
- walidacja za pomocą FluentValidation
- globalna obsługa wyjątków
- Serilog
- CORS
- OpenAPI
- soft delete
- obsługa relacji pomiędzy encjami
- obsługa wypożyczeń
- endpointy REST dla głównych modułów aplikacji

### Frontend

- Angular 19
- TypeScript
- PrimeNG 19
- PrimeIcons
- Tailwind CSS
- RxJS
- Chart.js
- JsBarcode
- formularze Angular
- komponenty standalone
- routing
- serwisy komunikujące się z API
- dashboard
- tabele danych
- sortowanie
- filtrowanie
- paginacja
- formularze dodawania i edycji
- dialogi
- potwierdzenia operacji
- komunikaty Toast
- tooltipy
- obsługa list wyboru
- multi-select
- widoki administracyjne
- landing page
- responsywny layout

## 3. Moduły aplikacji

Aktualna aplikacja zawiera m.in.:

- Dashboard
- Books
- Available Books
- Borrowed Books
- Users
- Authors
- Categories
- Publishers
- Borrowings
- Settings
- Landing Page
- Not Found Page

Projekt pokazuje zarówno klasyczne widoki administracyjne, jak i elementy typowe dla nowoczesnych stron internetowych.

### Przykłady elementów UI

W projekcie można znaleźć przykłady:

- tabel z sortowaniem,
- filtrowania danych,
- paginacji,
- formularzy,
- formularzy reaktywnych,
- dialogów,
- przycisków akcji,
- potwierdzania usuwania,
- komunikatów sukcesu i błędów,
- dropdownów,
- multi-selectów,
- kart,
- statystyk,
- wykresów,
- dashboardu,
- menu bocznego,
- topbara,
- footera,
- landing page,
- sekcji Hero,
- sekcji Features,
- sekcji Highlights,
- sekcji Pricing,
- responsywnych komponentów.

Dzięki temu projekt może być wykorzystany jako **baza lub katalog przykładów podczas tworzenia własnego frontendu**.

---

## 4. Struktura projektu

```text
SimpleLibrary
│
├── Library.sln
│
├── src
│   │
│   ├── Backend
│   │   ├── Library.Api
│   │   ├── Library.Application
│   │   ├── Library.Core
│   │   └── Library.Infrastructure
│   │
│   └── Frontend
│       ├── src
│       ├── angular.json
│       ├── package.json
│       └── tsconfig.json
│
├── compose.yaml
├── .env
└── .gitignore
```

### Backend

```text
Library.Api
    EndPoints/
    Extensions/
    GlobalHandlers/
    Program.cs

Library.Application
    DTO/
    Factories/
    Services/
    Validators/

Library.Core
    Entities/
    Exceptions/
    Repositories/
    IUnitOfWork.cs

Library.Infrastructure
    DAL/
        Configurations/
        Repositories/
        LibraryDbContext.cs
    Migrations/
    RepositoriesRegistration.cs
    ServicesRegistration.cs
```

Taki podział pozwala oddzielić API, logikę aplikacyjną, model domenowy oraz dostęp do danych.

---

# 5. Wymagania

Przed rozpoczęciem instalacji należy zainstalować:

- Git
- .NET SDK 9
- Node.js 20 LTS
- npm
- Angular CLI 19
- PostgreSQL 16 lub nowszy

Opcjonalnie:

- Docker Desktop / Docker Engine
- PostgreSQL GUI, np. pgAdmin
- Visual Studio / JetBrains Rider / Visual Studio Code

## Sprawdzenie instalacji

```bash
git --version
dotnet --version
node --version
npm --version
ng version
psql --version
```

---

# 6. Instalacja środowiska od zera

## 6.1. .NET 9

Zainstaluj **.NET 9 SDK**.

Po instalacji:

```bash
dotnet --version
```

Powinna zostać wyświetlona wersja `9.x`.

---

## 6.2. Node.js i npm

Zainstaluj **Node.js 20 LTS**.

Sprawdź:

```bash
node --version
npm --version
```

---

## 6.3. Angular CLI

Zainstaluj Angular CLI w wersji używanej przez projekt:

```bash
npm install -g @angular/cli@19
```

Sprawdź:

```bash
ng version
```

---

## 6.4. Entity Framework Core CLI

Zainstaluj narzędzie EF Core:

```bash
dotnet tool install --global dotnet-ef
```

Jeżeli narzędzie jest już zainstalowane:

```bash
dotnet tool update --global dotnet-ef
```

Sprawdź:

```bash
dotnet ef --version
```

Wersja narzędzia powinna odpowiadać głównej wersji EF Core używanej przez projekt, czyli `9.x`.

---

# 7. PostgreSQL

Projekt wykorzystuje PostgreSQL jako bazę danych.

Można użyć:

1. lokalnej instalacji PostgreSQL,
2. PostgreSQL uruchomionego w Dockerze,
3. zewnętrznego serwera PostgreSQL.

Projekt nie uruchamia kontenera PostgreSQL w obecnym `compose.yaml` — kontener Dockerowy dotyczy API, dlatego baza danych musi być dostępna niezależnie.

## Przykładowa konfiguracja

Utwórz bazę:

```sql
CREATE DATABASE LibraryDB;
```

oraz użytkownika:

```sql
CREATE USER LibraryUser WITH PASSWORD 'password';
```

Nadaj użytkownikowi odpowiednie uprawnienia:

```sql
GRANT ALL PRIVILEGES ON DATABASE LibraryDB TO LibraryUser;
```

W zależności od wersji PostgreSQL i konfiguracji serwera może być konieczne nadanie uprawnień również do schematu `public`.

---

# 8. Konfiguracja połączenia z bazą

W katalogu głównym projektu znajduje się plik:

```text
.env
```

Przykładowa konfiguracja:

```env
ASPNETCORE_ENVIRONMENT=Production

POSTGRES_HOST=localhost
POSTGRES_PORT=5432
POSTGRES_DB=LibraryDB
POSTGRES_USER=LibraryUser
POSTGRES_PASSWORD=password
```

Dla lokalnego uruchamiania API można również ustawić connection string w konfiguracji ASP.NET Core.

Aktualna konfiguracja infrastruktury korzysta z klucza:

```text
ConnectionString:default
```

Dlatego przy uruchamianiu bez Dockera należy upewnić się, że ASP.NET Core otrzymuje właściwy connection string, np. w `appsettings.json`, `appsettings.Development.json` albo przez zmienną środowiskową.

Przykład:

```json
{
  "ConnectionString": {
    "default": "Host=localhost;Port=5432;Database=LibraryDB;Username=LibraryUser;Password=password"
  }
}
```

---

# 9. Pobranie projektu

```bash
git clone <URL_REPOSITORY>
cd SimpleLibrary-develop
```

---

# 10. Instalacja zależności backendu

Przejdź do katalogu rozwiązania:

```bash
dotnet restore
```

Następnie skompiluj projekt:

```bash
dotnet build
```

Jeżeli oba polecenia wykonają się poprawnie, backend jest gotowy do dalszej konfiguracji.

---

# 11. Migracje Entity Framework Core

Projekt zawiera już migrację początkową:

```text
src/Backend/Library.Infrastructure/Migrations/
```

Znajduje się tam m.in.:

```text
20260809070814_initial.cs
LibraryDbContextModelSnapshot.cs
```

## 11.1. Utworzenie nowej migracji

Jeżeli zmienisz model encji, wykonaj:

```bash
dotnet ef migrations add NazwaMigracji \
  --project src/Backend/Library.Infrastructure \
  --startup-project src/Backend/Library.Api \
  --output-dir Migrations
```

Przykład:

```bash
dotnet ef migrations add AddBookIsbn \
  --project src/Backend/Library.Infrastructure \
  --startup-project src/Backend/Library.Api \
  --output-dir Migrations
```

## 11.2. Aktualizacja bazy danych

```bash
dotnet ef database update \
  --project src/Backend/Library.Infrastructure \
  --startup-project src/Backend/Library.Api
```

Polecenie utworzy brakujące tabele na podstawie migracji.

## 11.3. Usunięcie ostatniej migracji

Jeżeli migracja nie została jeszcze zastosowana do bazy:

```bash
dotnet ef migrations remove \
  --project src/Backend/Library.Infrastructure \
  --startup-project src/Backend/Library.Api
```

> Nie należy usuwać migracji, która została już zastosowana na współdzielonej lub produkcyjnej bazie danych bez odpowiedniego planu migracji.

---

# 12. Uruchomienie backendu

Przejdź do katalogu API:

```bash
cd src/Backend/Library.Api
```

Uruchom:

```bash
dotnet run
```

Domyślnie API jest skonfigurowane do działania pod:

```text
http://localhost:5280
```

Frontend korzysta z tego adresu poprzez:

```text
src/Frontend/src/environments/environment.ts
```

Wartość:

```typescript
export const environment = {
    baseUrl: 'http://localhost:5280/'
};
```

---

# 13. Uruchomienie frontendu

W drugim terminalu:

```bash
cd src/Frontend
```

Zainstaluj zależności:

```bash
npm install
```

Uruchom Angular:

```bash
npm start
```

lub:

```bash
ng serve
```

Aplikacja będzie dostępna pod:

```text
http://localhost:4200
```

---

# 14. Kolejność uruchamiania całego projektu

Najprostszy scenariusz lokalny:

### Terminal 1 — baza danych

Uruchom PostgreSQL i upewnij się, że baza `LibraryDB` istnieje.

### Terminal 2 — migracje

Z katalogu głównego:

```bash
dotnet ef database update \
  --project src/Backend/Library.Infrastructure \
  --startup-project src/Backend/Library.Api
```

### Terminal 3 — backend

```bash
cd src/Backend/Library.Api
dotnet run
```

### Terminal 4 — frontend

```bash
cd src/Frontend
npm install
npm start
```

Następnie otwórz:

```text
http://localhost:4200
```

---

# 15. Uruchomienie za pomocą Docker

Projekt zawiera:

```text
compose.yaml
```

oraz Dockerfile dla API.

Przed uruchomieniem ustaw właściwe dane w:

```text
.env
```

Następnie:

```bash
docker compose up -d --build
```

Sprawdzenie kontenerów:

```bash
docker compose ps
```

Logi:

```bash
docker compose logs -f
```

Zatrzymanie:

```bash
docker compose down
```

> W obecnej konfiguracji Docker Compose uruchamia API. PostgreSQL powinien być dostępny pod adresem ustawionym w `POSTGRES_HOST`.

---

# 16. Dostosowanie projektu do własnych potrzeb

Projekt został zaprojektowany tak, aby można było potraktować go jako **starter/template**, a nie tylko jako gotową bibliotekę.

Przykładowy proces dostosowania:

### 1. Zmień domenę

Encje:

```text
Book
Author
Publisher
Category
User
Borrow
```

można zastąpić własnymi modelami biznesowymi.

Przykładowo:

```text
Product
Customer
Order
Category
Warehouse
Invoice
```

### 2. Dodaj własną encję

Utwórz encję w:

```text
Library.Core/Entities
```

Dodaj konfigurację w:

```text
Library.Infrastructure/DAL/Configurations
```

Dodaj repozytorium:

```text
Library.Infrastructure/DAL/Repositories
```

Dodaj DTO i serwis w warstwie Application.

Na końcu utwórz migrację:

```bash
dotnet ef migrations add AddProduct \
  --project src/Backend/Library.Infrastructure \
  --startup-project src/Backend/Library.Api \
  --output-dir Migrations
```

i zastosuj ją:

```bash
dotnet ef database update \
  --project src/Backend/Library.Infrastructure \
  --startup-project src/Backend/Library.Api
```

### 3. Dodaj endpoint

Endpointy znajdują się w:

```text
src/Backend/Library.Api/EndPoints
```

### 4. Dodaj serwis Angular

Serwisy frontendowe znajdują się w:

```text
src/Frontend/src/app/service
```

### 5. Dodaj stronę

Strony znajdują się w:

```text
src/Frontend/src/app/pages
```

Następnie dodaj trasę w:

```text
src/Frontend/src/app/pages/pages.routes.ts
```

---

# 17. Przykładowy przepływ tworzenia nowego modułu

Dla przykładowego modułu `Product`:

```text
Core
 └── Entities
      └── Product.cs

Application
 ├── DTO
 │    └── ProductDto.cs
 ├── Services
 │    └── ProductService.cs
 └── Validators
      └── ProductDtoValidator.cs

Infrastructure
 ├── DAL
 │    ├── Configurations
 │    │    └── ProductConfiguration.cs
 │    └── Repositories
 │         └── ProductRepository.cs
 └── Migrations

Api
 └── EndPoints
      └── ProductEndpoints.cs

Frontend
 ├── pages
 │    └── product
 │         ├── product.component.ts
 │         └── product.component.html
 └── service
      └── product.service.ts
```

Taki schemat można powtarzać dla kolejnych modułów.

---

# 18. Testowe dane

Projekt może być zasilony przykładowymi danymi książkowymi.

W repozytorium dostępny jest również plik:

```text
books-100-test-dataset-year-string.json
```

Zawiera przykładowe rekordy książek wraz z autorami, kategoriami, wydawcami, liczbą stron, rokiem wydania i ISBN.

Plik może być wykorzystany do testowania endpointów oraz interfejsu aplikacji.

---

# 19. Dobre praktyki przy dalszym rozwoju

Przy rozbudowie projektu warto zachować istniejący podział odpowiedzialności:

```text
API
 ↓
Application
 ↓
Core

Infrastructure → Core / Application
```

W szczególności:

- logika biznesowa nie powinna być umieszczana bezpośrednio w endpointach,
- dostęp do bazy danych powinien pozostać w Infrastructure,
- DTO powinny oddzielać API od encji domenowych,
- walidacja powinna być wykonywana przed wykonaniem operacji biznesowych,
- każda zmiana modelu bazy danych powinna być obsługiwana przez migrację,
- komponenty Angular powinny korzystać z serwisów do komunikacji z API,
- konfiguracja adresu API powinna być oddzielona od kodu komponentów.

---

# 20. Licencja i wykorzystanie

Projekt może być traktowany jako baza do tworzenia własnych aplikacji.

Możesz:

- zmienić wygląd,
- zmienić nazwy modułów,
- zmienić model danych,
- dodać własne moduły,
- usunąć niepotrzebne funkcje,
- zmienić backend,
- rozbudować frontend,
- wykorzystać istniejące komponenty jako przykłady,
- dostosować aplikację do konkretnego procesu biznesowego.

Przed wykorzystaniem projektu komercyjnie należy sprawdzić licencje zastosowanych bibliotek oraz elementów zewnętrznych.

---

# English version

## 1. Project overview

SimpleLibrary is a complete example of a modern web application for library management.

The project is designed not only as a ready-to-use library system, but also as a **reusable starter/template for building custom business applications**.

The frontend contains many examples of how different types of pages and UI components can be built: dashboards, data tables, forms, dialogs, filtering, sorting, pagination, notifications, validation, landing pages and administration views.

Because of this, the project can be adapted to many different use cases, for example:

- inventory management,
- CRM systems,
- administration panels,
- booking systems,
- product management,
- user management,
- CRUD business applications,
- SaaS applications.

**The main purpose of the project is to demonstrate a broad range of Angular/PrimeNG page and component possibilities rather than being limited to library-specific functionality.**

## 2. Main features

### Backend

- ASP.NET Core 9 Web API
- layered architecture:
  - `Library.Api`
  - `Library.Application`
  - `Library.Core`
  - `Library.Infrastructure`
- Entity Framework Core 9
- PostgreSQL
- EF Core migrations
- Repository Pattern
- Unit of Work
- DTOs
- FluentValidation
- global exception handling
- Serilog
- CORS
- OpenAPI
- soft delete
- entity relationships
- borrowing management
- REST endpoints for the main application modules

### Frontend

- Angular 19
- TypeScript
- PrimeNG 19
- PrimeIcons
- Tailwind CSS
- RxJS
- Chart.js
- JsBarcode
- Angular forms
- standalone components
- routing
- API services
- dashboard
- data tables
- sorting
- filtering
- pagination
- create/edit forms
- dialogs
- confirmation dialogs
- toast notifications
- tooltips
- dropdowns
- multi-select controls
- administration views
- landing page
- responsive layout

## 3. Application modules

The current application contains, among others:

- Dashboard
- Books
- Available Books
- Borrowed Books
- Users
- Authors
- Categories
- Publishers
- Borrowings
- Settings
- Landing Page
- Not Found Page

The project demonstrates both classic administration interfaces and modern website sections.

### UI examples

The project contains examples of:

- sortable tables,
- data filtering,
- pagination,
- forms,
- reactive forms,
- dialogs,
- action buttons,
- delete confirmations,
- success/error notifications,
- dropdowns,
- multi-select controls,
- cards,
- statistics,
- charts,
- dashboards,
- side navigation,
- topbars,
- footers,
- landing pages,
- Hero sections,
- Features sections,
- Highlights sections,
- Pricing sections,
- responsive components.

This makes the project useful as a **frontend starter and a reference implementation for building custom Angular applications**.

---

# 4. Project structure

```text
SimpleLibrary
│
├── Library.sln
│
├── src
│   │
│   ├── Backend
│   │   ├── Library.Api
│   │   ├── Library.Application
│   │   ├── Library.Core
│   │   └── Library.Infrastructure
│   │
│   └── Frontend
│       ├── src
│       ├── angular.json
│       ├── package.json
│       └── tsconfig.json
│
├── compose.yaml
├── .env
└── .gitignore
```

---

# 5. Requirements

Install the following:

- Git
- .NET SDK 9
- Node.js 20 LTS
- npm
- Angular CLI 19
- PostgreSQL 16 or newer

Optional:

- Docker Desktop / Docker Engine
- pgAdmin
- Visual Studio / JetBrains Rider / Visual Studio Code

Verify the installation:

```bash
git --version
dotnet --version
node --version
npm --version
ng version
psql --version
```

---

# 6. Install the development environment from scratch

## 6.1. .NET 9

Install the **.NET 9 SDK**.

Verify:

```bash
dotnet --version
```

The output should be a `9.x` version.

## 6.2. Node.js and npm

Install **Node.js 20 LTS**.

Verify:

```bash
node --version
npm --version
```

## 6.3. Angular CLI

Install Angular CLI 19:

```bash
npm install -g @angular/cli@19
```

Verify:

```bash
ng version
```

## 6.4. Entity Framework Core CLI

Install the EF Core command-line tool:

```bash
dotnet tool install --global dotnet-ef
```

If it is already installed:

```bash
dotnet tool update --global dotnet-ef
```

Verify:

```bash
dotnet ef --version
```

The major version should match the project's EF Core version: `9.x`.

---

# 7. PostgreSQL setup

The project uses PostgreSQL as its database.

You can use:

1. a local PostgreSQL installation,
2. PostgreSQL running in Docker,
3. an external PostgreSQL server.

The current `compose.yaml` starts the API container but does not define a PostgreSQL container. Therefore, PostgreSQL must be available separately.

Example:

```sql
CREATE DATABASE LibraryDB;
```

Create the database user:

```sql
CREATE USER LibraryUser WITH PASSWORD 'password';
```

Grant access:

```sql
GRANT ALL PRIVILEGES ON DATABASE LibraryDB TO LibraryUser;
```

Depending on the PostgreSQL version and configuration, you may also need to grant permissions on the `public` schema.

---

# 8. Database connection configuration

The root directory contains:

```text
.env
```

Example:

```env
ASPNETCORE_ENVIRONMENT=Production

POSTGRES_HOST=localhost
POSTGRES_PORT=5432
POSTGRES_DB=LibraryDB
POSTGRES_USER=LibraryUser
POSTGRES_PASSWORD=password
```

When running the API outside Docker, make sure ASP.NET Core receives a valid connection string.

The infrastructure layer currently reads:

```text
ConnectionString:default
```

For example:

```json
{
  "ConnectionString": {
    "default": "Host=localhost;Port=5432;Database=LibraryDB;Username=LibraryUser;Password=password"
  }
}
```

This can be placed in an appropriate `appsettings` file or provided through environment variables.

---

# 9. Clone the project

```bash
git clone <REPOSITORY_URL>
cd SimpleLibrary-develop
```

---

# 10. Install backend dependencies

From the solution root:

```bash
dotnet restore
```

Build the solution:

```bash
dotnet build
```

If both commands complete successfully, the backend is ready.

---

# 11. Entity Framework Core migrations

The project already contains an initial migration:

```text
src/Backend/Library.Infrastructure/Migrations/
```

including:

```text
20260809070814_initial.cs
LibraryDbContextModelSnapshot.cs
```

## 11.1. Create a new migration

After changing the entity model:

```bash
dotnet ef migrations add MigrationName \
  --project src/Backend/Library.Infrastructure \
  --startup-project src/Backend/Library.Api \
  --output-dir Migrations
```

Example:

```bash
dotnet ef migrations add AddBookIsbn \
  --project src/Backend/Library.Infrastructure \
  --startup-project src/Backend/Library.Api \
  --output-dir Migrations
```

## 11.2. Apply migrations

```bash
dotnet ef database update \
  --project src/Backend/Library.Infrastructure \
  --startup-project src/Backend/Library.Api
```

## 11.3. Remove the last migration

If it has not been applied to a shared/production database:

```bash
dotnet ef migrations remove \
  --project src/Backend/Library.Infrastructure \
  --startup-project src/Backend/Library.Api
```

---

# 12. Run the backend

```bash
cd src/Backend/Library.Api
dotnet run
```

The API is configured to run on:

```text
http://localhost:5280
```

The Angular environment configuration points to this address:

```text
src/Frontend/src/environments/environment.ts
```

```typescript
export const environment = {
    baseUrl: 'http://localhost:5280/'
};
```

---

# 13. Run the frontend

Open another terminal:

```bash
cd src/Frontend
```

Install dependencies:

```bash
npm install
```

Start Angular:

```bash
npm start
```

or:

```bash
ng serve
```

Open:

```text
http://localhost:4200
```

---

# 14. Complete local startup sequence

### Terminal 1 — PostgreSQL

Start PostgreSQL and make sure that `LibraryDB` exists.

### Terminal 2 — database migration

From the project root:

```bash
dotnet ef database update \
  --project src/Backend/Library.Infrastructure \
  --startup-project src/Backend/Library.Api
```

### Terminal 3 — backend

```bash
cd src/Backend/Library.Api
dotnet run
```

### Terminal 4 — frontend

```bash
cd src/Frontend
npm install
npm start
```

Then open:

```text
http://localhost:4200
```

---

# 15. Docker

The repository contains:

```text
compose.yaml
```

and an API Dockerfile.

Configure:

```text
.env
```

and run:

```bash
docker compose up -d --build
```

Check running containers:

```bash
docker compose ps
```

View logs:

```bash
docker compose logs -f
```

Stop the application:

```bash
docker compose down
```

> The current Docker Compose configuration runs the API. PostgreSQL must be available separately at the host specified by `POSTGRES_HOST`.

---

# 16. Customizing the project

The project is intended to be used as a **starter/template**, not only as a library management application.

You can:

- replace the domain model,
- rename modules,
- add new entities,
- remove unused functionality,
- add new REST endpoints,
- create new Angular pages,
- replace the database model,
- redesign the UI,
- reuse existing PrimeNG components,
- use the existing pages as implementation examples.

For example, library entities:

```text
Book
Author
Publisher
Category
User
Borrow
```

can be replaced or extended with:

```text
Product
Customer
Order
Category
Warehouse
Invoice
```

---

# 17. Creating a new module

A typical new `Product` module could look like this:

```text
Core
 └── Entities
      └── Product.cs

Application
 ├── DTO
 │    └── ProductDto.cs
 ├── Services
 │    └── ProductService.cs
 └── Validators
      └── ProductDtoValidator.cs

Infrastructure
 ├── DAL
 │    ├── Configurations
 │    │    └── ProductConfiguration.cs
 │    └── Repositories
 │         └── ProductRepository.cs
 └── Migrations

Api
 └── EndPoints
      └── ProductEndpoints.cs

Frontend
 ├── pages
 │    └── product
 │         ├── product.component.ts
 │         └── product.component.html
 └── service
      └── product.service.ts
```

This structure can be repeated for additional business modules.

---

# 18. Test data

The repository can be populated with sample book data.

The project also contains:

```text
books-100-test-dataset-year-string.json
```

The file contains sample books with authors, categories, publishers, page counts, release years and ISBN values.

It can be used for testing API endpoints and the frontend.

---

# 19. Development guidelines

When extending the application, keep the existing separation of responsibilities:

```text
API
 ↓
Application
 ↓
Core

Infrastructure → Core / Application
```

Recommended practices:

- keep business logic out of API endpoints,
- keep database access inside Infrastructure,
- use DTOs between the API and domain entities,
- validate input before executing business operations,
- create EF Core migrations for model changes,
- use Angular services for API communication,
- keep API configuration outside individual components.

---

# 20. License and usage

The project can be used as a foundation for creating custom applications.

You may:

- redesign the UI,
- rename modules,
- change the data model,
- add new modules,
- remove unused features,
- extend the backend,
- extend the frontend,
- reuse existing components as examples,
- adapt the application to a specific business process.

Before commercial distribution, verify the licenses of all third-party libraries, templates and external assets used by the project. See the **Third-party licenses and commercial use** section above.

---

## Technology stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET Core 9 |
| Language | C# |
| ORM | Entity Framework Core 9 |
| Database | PostgreSQL / Npgsql |
| Validation | FluentValidation |
| Logging | Serilog |
| API documentation | OpenAPI |
| Frontend | Angular 19 |
| Frontend language | TypeScript |
| UI | PrimeNG 19 |
| CSS | Tailwind CSS |
| Charts | Chart.js |
| Barcode | JsBarcode |
| Reactive programming | RxJS |
| Containers | Docker |
