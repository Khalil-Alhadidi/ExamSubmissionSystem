# 🧠 Exam Platform Backend – Microservice Architecture

This project is a backend-only exam platform built using clean architecture and microservices principles, designed for evaluation purposes. It includes exam management, student submission, and simulated notification/processing.

## 📦 Technology Stack

- ASP.NET Core 9
- Entity Framework Core
- RabbitMQ (message broker)
- SQL Server (per service DB)
- Serilog (logging)
- OpenTelemetry (tracing)
- xUnit (unit testing planned)
- Docker / Docker Compose

---

## 🏗️ Microservices

| Service | Purpose |
|--------|---------|
| **ExamService** | Manage subjects, questions, and exam configuration (admin-only) |
| **SubmissionService** | Allow students to submit exam responses |
| **NotificationService** | Simulate notification to grading pipeline (fire-and-forget, async) |

Each service has its own database and runs independently.

---

## 🔐 Authentication & Authorization

- JWT-based auth with hardcoded tokens (no real UserService).
- JWTs are manually generated with `sub` (user ID) and `role` (`admin` or `student`).
- Role-based access is enforced using `[Authorize(Roles = "admin")]`, etc.
