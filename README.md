# Loan Management System — ASP.NET Core Web API

A clean, beginner-friendly **Loan Management REST API** built with **ASP.NET Core 9**, **Entity Framework Core 9**, and **Microsoft SQL Server**.

**Author:** [Md. Maksudul Hoque Khan](https://github.com/maksudulkhan328)

---

## Features

- **Customer Management** — CRUD operations for loan customers
- **Loan Applications** — Apply for loans with amount, interest rate, and term
- **EMI Calculation** — Accurate monthly EMI and total interest using the standard formula
- **Loan Workflow** — Approve or reject pending loan applications
- **Repayment Tracking** — Record repayments and track remaining balance
- **Auto-complete** — Loan marked Completed when fully repaid
- **Swagger UI** — Interactive API documentation
- **Unit Tests** — xUnit tests for loan calculation logic

---

## Tech Stack

- **.NET 9** / ASP.NET Core Web API
- **Entity Framework Core 9** (Code-First, SQL Server)
- **SQL Server / LocalDB**
- **xUnit** for unit testing
- **Swagger / OpenAPI**

---

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server or LocalDB (included with Visual Studio)

---

## Setup & Run

```bash
git clone https://github.com/maksudulkhan328/asp-net-core-web-api-loan-management-system.git
cd asp-net-core-web-api-loan-management-system
```

### SQL Server Configuration

The default connection string uses **LocalDB**. Edit `LoanManagement.Api/appsettings.json` if needed:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=LoanManagementDb;Trusted_Connection=True;TrustServerCertificate=True"
}
```

### Apply Migrations

The app auto-migrates on startup. To run manually:

```bash
dotnet ef database update --project LoanManagement.Api
```

### Run the API

```bash
cd LoanManagement.Api
dotnet run
```

- **API:** `http://localhost:5000`
- **Swagger UI:** `http://localhost:5000/swagger`

### Run Tests

```bash
dotnet test
```

---

## API Endpoints

### Customers

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/customers` | List all customers |
| GET | `/api/customers/{id}` | Get customer by ID |
| POST | `/api/customers` | Create customer |
| PUT | `/api/customers/{id}` | Update customer |
| DELETE | `/api/customers/{id}` | Delete customer |

### Loans

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/loans` | List all loans |
| GET | `/api/loans/{id}` | Get loan by ID |
| GET | `/api/loans/customer/{customerId}` | Get loans by customer |
| POST | `/api/loans` | Apply for a loan |
| POST | `/api/loans/{id}/approve` | Approve a pending loan |
| POST | `/api/loans/{id}/reject` | Reject a pending loan |
| POST | `/api/loans/{id}/repayments` | Record a repayment |
| GET | `/api/loans/{id}/repayments` | List repayments for a loan |

---

## Loan Status Flow

```
Pending → Approved (Active) → Completed
Pending → Rejected
```

---

## EMI Formula

```
EMI = P × r × (1+r)^n / ((1+r)^n − 1)
```

Where `P` = principal, `r` = monthly interest rate, `n` = term in months.

---

*Built by [Md. Maksudul Hoque Khan](https://github.com/maksudulkhan328)*
