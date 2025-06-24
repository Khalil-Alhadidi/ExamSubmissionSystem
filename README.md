# 🧠 Exam Platform Backend – Microservice Architecture

This project is a backend-only exam platform built using clean architecture and microservices principles, designed for evaluation purposes. It includes exam management, student submission, and simulated notification/processing.

## 📦 Technology Stack (Clean Architecture)

- ASP.NET Core 9
- Entity Framework Core
- RabbitMQ (message broker) and MassTransit
- Databases - SQL Server and InMemory (per service DB)
- Serilog (logging)
- OpenTelemetry (tracing)
- xUnit (unit testing planned)
- Docker / Docker Compose

---

## 🏗️ Microservices

| Service | Purpose |
|--------|---------|
| **ExamService** | Manage subjects, questions, and exam configuration (admin-only) | SQL Server DB
| **SubmissionService** | Allow students to submit exam responses | SQL Server DB
| **NotificationService** | Simulate notification to grading pipeline (fire-and-forget, async) | InMemory DB

Each service has its own database and runs independently.

---

## 🔐 Authentication & Authorization

- JWT-based auth with hardcoded tokens (no real UserService), In a real-world setup, authentication would be handled by a dedicated UserService that issues JWTs. For simplicity and time constraints, this project simulates authentication with generated tokens that include roles like 'admin' and 'student' using dev-token api in each service
- Role-based access is enforced using Roles (Admin,User)

## 🐳 Docker
This system is Dockerized and can be orchestrated via Kubernetes for scaling, but for simplicity and rapid development, Docker Compose is used here.

## ✅  Time Zone
The system uses a UTC-first strategy. All submission times are handled and stored in UTC to avoid inconsistencies across global clients. Clients may optionally send their local timezone offset for contextual logging.”

## 🔄 Inter-Service Communication
SubmissionService retrieves exam config from ExamService via synchronous HTTP calls, this ensures data consistency and simplicity. The architecture is ready to extend with an event-driven approach using RabbitMQ and a local cache (planned as a future enhancement).

## 🧭 Caching
The SubmissionService uses in-memory caching to reduce repeated calls to ExamService for the same exam ID. This improves performance for high-volume exam periods. In a production deployment, this would be replaced with a distributed cache like Redis to provide cross-instance consistency and offline fallback.


## 🔧 API Gateway
This system currently uses direct service-to-service communication. In a production deployment, I would introduce an API Gateway (such as YARP for .NET environments or an NGINX Ingress controller for Kubernetes) to manage cross-cutting concerns like routing, authentication, rate limiting, observability, and service discovery.


## 🚀 How to Run & Use the Application
Prerequisites
•	.NET 9 SDK
•	Docker Desktop (with Docker Compose)
•	(Optional) curl or Postman for API testing

1- Clone the Repo

2- Open a terminal and navigate to the project root directory

3- Run docker-compose up --build

This will build and start all services and their dependencies.

4- Use the following endpoints to interact with the services:

This will start:

•	ExamService (http://localhost:8080)

•	SubmissionService (http://localhost:8081)

•	NotificationService (http://localhost:8082)

•	RabbitMQ Management UI (http://localhost:15672, user: rabbitmq, pass: Admin@1234)

•	SQL Server for ExamService (port 11433, password: Admin@1234)

•	SQL Server for SubmissionService (port 21433, password: Admin@1234)


5. Accessing the APIs

Each service exposes a Swagger UI for easy API exploration:

•	ExamService: http://localhost:8080/swagger

•	SubmissionService: http://localhost:8081/swagger

•	NotificationService: http://localhost:8082/swagger

6. Authentication

•	Use the /dev-token endpoint (if enabled) in each service to generate JWT tokens for roles like admin or student.

•	Add the token as a Bearer token in the Authorization header in SwaggerUI when making API requests.

7. Example Workflow (Make sure to Authenticate first)

•	Admin: Use ExamService to create subjects, questions, and configure exams (there is one exam config that is seeded you can use that by calling api/v1/exam-configs).

•	Student: Use SubmissionService to submit exam responses (requires a student token).

•	Notification: SubmissionService will trigger NotificationService for grading simulation.

 
8. To stop the services, run: docker compose down


