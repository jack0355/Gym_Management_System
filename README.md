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
##[Log-in Page]
<img width="1912" height="873" alt="تسجيل_دخول" src="https://github.com/user-attachments/assets/8610f42d-d82d-4d73-bdd1-7c8706248c0d" />

##[main-page] 
<img width="1910" height="885" alt="الصفحة الرئيسية" src="https://github.com/user-attachments/assets/110d14f1-93a1-4822-80bd-308bedd01eb8" />

##[scan-page]
<img width="1908" height="885" alt="صفحة البحث" src="https://github.com/user-attachments/assets/92cb1dbc-669b-4582-9271-3a0bb4f469b3" />

##[Register-New-Member]
<img width="1095" height="847" alt="تسجيل لاعب جديد" src="https://github.com/user-attachments/assets/cd63f699-716b-4b76-a8a5-8c09d2cbc826" />

##[Successfully - Registered] , [QR Photo ] , [QR Image (Downloading -  sharing)]
<img width="1095" height="847" alt="تسجيل لاعب جديد" src="https://github.com/user-attachments/assets/64dbe7ac-d055-46dd-a955-d27c2b61c0dc" />
<img width="467" height="765" alt="QrToken" src="https://github.com/user-attachments/assets/d5d9e135-5ee1-471c-b933-0cdba85646cc" />
<img width="520" height="276" alt="تحميل صورةQr" src="https://github.com/user-attachments/assets/d83ee24e-ed79-43ce-ae94-e44a932000bc" />

##[Member - (Check-In) - Daily ]
<img width="1897" height="877" alt="سجل الحضور " src="https://github.com/user-attachments/assets/012c6774-8c63-478a-916c-7ac237e35686" />

##[Page - Members Table -Info]
<img width="1893" height="873" alt="صفحة البحث عن العضو" src="https://github.com/user-attachments/assets/3a279ee7-7a21-4b7f-a679-01a69a53b91e" />





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
