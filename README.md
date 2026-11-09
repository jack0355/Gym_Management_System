# 🏋️ Gym Management System - QR Check-in & Membership

A production-ready, full-stack membership and QR check-in system built for real-world gyms. Staff can register members, assign plans, track cash payments, and scan members' QR codes at the door for an instant green/yellow/red status result.

## 🚀 Live Demo
*   **URL:** [https://gym-system.hostless.app](https://gym-system.hostless.app)
*   *Credentials provided upon request.*

## ✨ Key Features
*   **QR Check-in:** Green = valid and paid, Yellow = payment due or ending within 5 days, Red = no valid subscription.
*   **Offline Member Scanning:** Members only need a static photo of their QR code (no app or internet required for them). Staff scans it using the web app.
*   **Smart Subscription Rules:** Late renewals keep the original cycle. Overpayments are refused. `PriceCharged` is locked at the time of sale to prevent history rewriting.
*   **Cash Payment Auditing:** Tracks which staff member received the payment.
*   **Advanced Filtering:** Check-in history by date, status color, plan type, and coach.
*   **Arabic RTL Interface:** Fully responsive UI designed for mobile phones (camera scanning) and desktop (management).
*   **Security:** JWT authentication, rate-limiting on login, and PBKDF2 password hashing with per-user salts.

## 🛠️ Tech Stack
*   **Backend:** C# / ASP.NET Core 8 Web API
*   **Database:** PostgreSQL (via Entity Framework Core)
*   **Frontend:** HTML5, CSS3, Vanilla JavaScript (served directly from the API)
*   **Deployment:** Docker, Hostless.cloud, Supabase (PostgreSQL)

## 🏗️ Architecture
Strictly layered following Clean Architecture principles: `Api → Infrastructure → Core`.
*   **Core:** Holds domain entities and the pure business logic (e.g., the color status rule). Zero dependencies on databases or frameworks.
*   **Infrastructure:** Holds EF Core `AppDbContext`, migrations, and external services (QR generation, password hashing).
*   **Api:** Controllers, DTOs, middleware, and static frontend files.

## 📸 Screenshots
### Login
<img width="1912" height="873" alt="Login" src="https://github.com/user-attachments/assets/8610f42d-d82d-4d73-bdd1-7c8706248c0d" />

### Dashboard & Scanner
<img width="1910" height="885" alt="Main" src="https://github.com/user-attachments/assets/110d14f1-93a1-4822-80bd-308bedd01eb8" />
<img width="1908" height="885" alt="Scanner" src="https://github.com/user-attachments/assets/92cb1dbc-669b-4582-9271-3a0bb4f469b3" />
<img width="1153" height="762" alt="Scan successed " src="https://github.com/user-attachments/assets/9af57405-d6cd-465d-8fdd-053a8936ef4c" />


### Member Registration & QR
<img width="1095" height="847" alt="Register" src="https://github.com/user-attachments/assets/cd63f699-716b-4b76-a8a5-8c09d2cbc826" />
<img width="467" height="765" alt="QR" src="https://github.com/user-attachments/assets/d5d9e135-5ee1-471c-b933-0cdba85646cc" />


## coaches Stats . 
<img width="1861" height="881" alt="Coach Stats " src="https://github.com/user-attachments/assets/13229016-b1eb-47d5-af8e-7fe59a03a524" />


### Check-in History & Member Management
<img width="1897" height="877" alt="History" src="https://github.com/user-attachments/assets/012c6774-8c63-478a-916c-7ac237e35686" />
<img width="1893" height="873" alt="Members" src="https://github.com/user-attachments/assets/3a279ee7-7a21-4b7f-a679-01a69a53b91e" />

## 🧠 Design Decisions
*   **The server decides the status color.** The frontend simply displays it. This prevents tampering.
*   **Amount owed is calculated dynamically** from payment history; it is never stored as a static number.
*   **UTC vs. DateOnly:** Exact moments (check-ins, payments) are stored in UTC. Subscriptions use `DateOnly` based on the gym's local timezone.
*   **Foreign Keys use `Restrict`:** A member or plan cannot be deleted if they have payment history, protecting the gym's financial data.

## 🧪 Testing
- **Unit Tests:** The core business logic, including the Check-In color rules (Green/Yellow/Red) and Member search filtering, is fully covered by automated tests.
- **Frameworks:** xUnit and Entity Framework Core In-Memory Database.
- **Run Locally:** Execute `dotnet test` in the terminal to run the test suite.

## 💻 Running Locally (Docker)
The easiest way to run this project is via Docker.
```bash
# 1. Clone the repository
git clone https://github.com/jack0355/Gym_Management_System.git
cd Gym_Management_System

# 2. Create your .env file
echo "JWT_KEY=YourSuperSecretKey1234567890!" > .env
echo "OWNER_PASSWORD=YourOwnerPassword123!" >> .env

# 3. Build and run
docker compose up --build -d

# 4. Seed the database with initial plans and coaches
docker cp .\docs\seed.sql gym_management_system-db-1:/seed.sql
docker exec -it gym_management_system-db-1 psql -U gym -d gym_db -f /seed.sql
