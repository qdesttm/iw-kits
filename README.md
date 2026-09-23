# Instant Wellness Kits

**Order Management System with Geolocation-Based Tax Calculation.**

## Challenges & Solutions

A brief description of the challenges our team faced during the development of this solution.

| **Challenge**                                                | **Solution**                                                                                                                                                                                                                                 |
| ------------------------------------------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Restrictions on Third-Party APIs**                         | Midway through development, we decided to phase out external services due to API usage restrictions. We transitioned to working with local, pre-processed datasets, ensuring total system autonomy.                                          |
| **Handling Out-of-State Points**                             | To verify coordinates, we implemented a **GeoJSON MultiPolygon** covering all land and water boundaries of New York, allowing for precise regional identification.                                                                           |
| **Tax Calculation for Remote Areas (Forests, Rivers, etc.)** | We implemented an algorithm that anchors the tax rate to the nearest available Zip Code. This ensures seamless financial calculations for any point in New York, regardless of existing postal infrastructure.                               |
| **Slow Import of Large Datasets**                            | The loading process was optimized by implementing caching mechanisms and parallel database writes, significantly reducing CSV file processing time.                                                                                          |
| **Zip Code Ambiguity (One code spanning two states)**        | To avoid tax rate errors, the system uses combined identification. Calculations are based not only on the Zip Code but also on a State Identifier, accurately separating jurisdictions with different tax levels even within the same index. |

> **Note:** Since physical delivery to certain zones (e.g., dense forests or the middle of a river) may be logistically complex, actual transportation to such locations is a separate case requiring additional research at the courier service level. Our primary goal was to ensure the stability of mathematical calculations for any geographic position.

> **Important:** The solution architecture allows for polygon editing. This enables the automatic exclusion of forests, parks, or water bodies during the validation stage where delivery is known to be impossible.

## Flexibility & Scalability

The application architecture is designed with future expansion in mind. The system can adapt to calculate tax rates not only for New York but for other US states as well.

**Technical Implementation of Scaling:**

- **Geospatial Independence:** The system uses the GeoJSON standard to store state and county boundaries in MongoDB. To add a new region, simply upload the corresponding coordinate array (MultiPolygon) to the geozone collection without changing the source code.
- **Unified Data Model:** The tax rates collection (`tax_rates`) is designed on a "key-value" principle, where each key is a unique identifier (Zip Code + County Code). This allows for easy importing of new datasets for any state via the existing API endpoint.

## Tech Stack

- **Frontend:** React, TypeScript, Vite, Ant Design
- **Backend:** C# .NET 10 Minimal API, MediatR (CQS)
- **Database:** MongoDB (Production-isolated access & indexes)
- **DevOps:** Docker, Docker Compose, automated VS Code tasks

## Frontend Features

- **Orders Table:** Server-side pagination and multidirectional sorting.
- **Filters:** Date range and financial amount boundary filters.
- **Creation:** Geolocation-based order entry with real-time tax validation.
- **CSV Import:** Stream-based bulk upload for large order datasets.

## How to Run

**Prerequisites:**
- VS Code (Recommended editor environment)
- Docker Desktop installed and running
- Git

**Step-by-step Setup:**

1. **Clone the Repository**

```bash
git clone https://github.com
cd iw-kits
```

2. **Prepare Database Data**

Download the archive (`.zip` or `.rar`) from the **Releases** tab and extract the `mongodb` folder into the project root. It contains the necessary data and configurations for the backend calculations.

3. **Build Images via VS Code Tasks**

Open the root folder in VS Code, press `Ctrl+Shift+P` (`Cmd+Shift+P` on Mac), select **Tasks: Run Task**, and run:
- `Docker.Build.AspNetCore` – Compiles the .NET 10 API and builds `dev.kitstech.local/iwkits/api`.
- `Docker.Build.Client` – Builds production frontend assets and builds `dev.kitstech.local/iwkits/client`.

4. **Launch Infrastructure**

Create a `.env` file in the root directory (either copy and edit `.env.example` or download a pre-configured `.env` file from the **Releases** tab), then run:

```bash
docker compose up -d
```

## Authorization Data

| **Role / Service** | **Login** | **Password** | **Access Method** |
| :--- | :--- | :--- | :--- |
| **Web Admin** | `admin` | `admin` | Web App (Frontend) |
| **DB Root Admin** | `admin` | `admin` | MongoDB Compass |
| **DB API Service** | `api_service` | `8888` | MongoDB Compass |

### Services

| **Service** | **URL** | **Description** |
| :--- | :--- | :--- |
| **Frontend** | `http://localhost:3000` | React SPA (Nginx) |
| **Backend API** | `http://localhost:5152` | .NET 10 Minimal API |
| **MongoDB** | `http://localhost:27017` | Isolated Database Instance |
| **Swagger UI** | `http://localhost:5152/swagger` | Interactive API Documentation |

### Bulk CSV Import Format

The CSV file for bulk import (`POST /v1/orders/import`) must use this layout:

```csv
id,latitude,longitude,subtotal,timestamp
1001,40.7580,-73.9855,99.99,2026-01-15T10:30:00Z
1002,40.7128,-74.0060,149.50,2026-01-15T11:15:00Z
```

> **Note:** The `id` column is **optional and completely ignored** by the server. The database will always generate its own unique identifiers for all imported rows.

> **Note:** All API endpoints, request/response models, and schemas are fully documented and testable in **Swagger UI**: `http://localhost:5152/swagger`.