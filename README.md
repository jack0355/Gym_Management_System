# Gym Management System

Membership and QR check-in system built for a real gym. Staff register members, assign plans, take cash payments, and scan a member's QR code at the door to get an instant green / yellow / red result.

## Features
- QR check-in: green = valid and paid, yellow = payment due or ending within 5 days, red = no valid subscription
- Members, plans, coaches (with a gender rule) and an optional training-program add-on
- Subscriptions with a late-renewal rule: a late payer keeps their cycle
- Cash payments with a "received by" audit trail; overpayments are refused
- Check-in history by date, with colour, plan-type and coach filters
- Arabic right-to-left staff screens; camera scanning or a USB scanner
- JWT login with roles; passwords hashed with PBKDF2 and a per-user salt

## Tech stack
ASP.NET Core 8 Web API · EF Core · PostgreSQL · HTML/CSS/JavaScript front-end

## Architecture
`Api → Infrastructure → Core`: a layered design inspired by Clean Architecture.
Core holds entities and the colour rule (no database). Infrastructure holds EF Core and services. Api holds controllers and the static pages.

## Screenshots
![Scan](docs/scan.png) ![Register](docs/register.png) ![History](docs/history.png)

## Design decisions
- The server decides the colour; the page only displays it
- `PriceCharged` is copied onto each subscription, so later price changes never rewrite history
- Amount owed is calculated from payments, never stored
- Foreign keys use `Restrict`: payment history can't be deleted by accident
- Dates are `DateOnly`, exact moments are UTC; "today" is the gym's local date

## Run locally
Requirements: .NET 8 SDK, PostgreSQL.
```bash
dotnet user-secrets init --project GymManagementSystem.Api
dotnet user-secrets set "ConnectionStrings:Default" "Host=localhost;Database=gym_db;Username=postgres;Password=<your-password>" --project GymManagementSystem.Api
dotnet user-secrets set "Jwt:Key" "<32+ random characters>" --project GymManagementSystem.Api
dotnet user-secrets set "Jwt:Issuer" "GymApi" --project GymManagementSystem.Api
dotnet user-secrets set "Jwt:Audience" "GymStaff" --project GymManagementSystem.Api
dotnet user-secrets set "Seed:OwnerPassword" "<choose a password>" --project GymManagementSystem.Api
dotnet ef database update --project GymManagementSystem.Infrastructure --startup-project GymManagementSystem.Api
dotnet run --project GymManagementSystem.Api
```
Sample plans and coaches: `docs/seed.sql`. Log in as `owner`.

## Known limitations / roadmap
- No automated tests yet (planned: tests for the colour rule)
- Login rate limiting, Docker and deployment are planned
- Single gym per deployment
