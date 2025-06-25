# 🧠 Exam Platform Backend – Microservice Architecture

This project is a backend-only exam platform built using clean architecture and microservices principles, designed for evaluation purposes. It includes exam management, student submission, and simulated notification/processing.

## 📦 Technology Stack (Clean Architecture)

- ASP.NET Core 9
- Entity Framework Core
- RabbitMQ (message broker) and MassTransit
- Databases (per service DB)
- Serilog (logging)
- OpenTelemetry (tracing) without Jaeger which can be added later
- Seq (Collecting and visualizing logs)
- xUnit (testing some handlers)
- Docker / Docker Compose

---

![Image](https://github.com/user-attachments/assets/8064881d-ae3a-45a7-ba3f-5d756b22ad46)

---

## 🏗️ Microservices

| Service | Purpose |
|--------|---------|
| **ExamService** | Manage subjects, questions, and exam configuration (admin-only) - SQL Server DB
| **SubmissionService** | Allow students to submit exam responses - SQL Server DB
| **NotificationService** | Simulate notification to grading pipeline (fire-and-forget, async) - InMemory DB

Each service has its own database and runs independently.

---

## ⚙️ Design Decisions & Production Caveats
This project was developed under time constraints and focused on demonstrating core architectural patterns. The following decisions were made intentionally for simplicity and should be revisited for production environments:

## 🔐 HTTP Communication Between Services
-Services currently communicate over HTTP only within Docker Compose.

-SubmissionService retrieves exam config from ExamService via synchronous HTTP calls, this ensures data consistency and simplicity. The architecture is ready to extend with an event-driven approach using RabbitMQ and a local cache (planned as a future enhancement).

✔️ This is safe and meant for local development (isolated network).

⚠️ In production, traffic should be secured using HTTPS, mTLS, or routed via a secure Ingress/API Gateway.


## 🔑 API Keys and Secrets
API keys and secrets (e.g., inter-service keys, connection strings) are hardcoded or stored in appsettings.json.

⚠️ In production, it is recommended to use a secure secret management system such as:
	-Azure Key Vault
	-AWS Secrets Manager
	-Kubernetes Secrets
	-Environment variables

## 🗃️ Database Design
 - Each service has its own database schema, ensuring loose coupling.
 - Entities are designed with soft delete logic to prevent data loss.

## 🧑‍💻 Libraries & Dependencies
- I didn't use AutoMapper or Mediator/CQRS - given that these libiraries will need a commercial license for production use.
- I have used MassTransit (for the sake of time), knowing that it will also need a commercial license for production use.
- FluentValidation as been used for input validation in the API layer (one handler to show case the usage, but not all handlers are using it for simplicity)



## 🔐 Authentication & Authorization

- JWT-based auth with hardcoded tokens (not real UserService), In a real-world setup, authentication would be handled by a dedicated UserService that issues JWTs. For simplicity and time constraints, this project simulates authentication with generated tokens that include roles like 'admin' and 'student' using dev-token api in each service
- Role-based access is enforced using Roles (Admin,User)
- API Key is used for inter-service communication, ensuring that only authorized services can communicate with each other (this is hardcoded in the shared project, for a production enviroment this should be stored securly in a KeyVault or any similar service )
- ⚠️ In production, authentication should be delegated to a dedicated Identity Provider (e.g., IdentityServer/Duende , KeyCloak, Azure EntraID).

## 🐳 Docker
-This system is Dockerized and can be orchestrated via Kubernetes for scaling, but for simplicity and rapid development, Docker Compose is used here.

-In a production setup, I would configure GitHub Actions to build and push Docker images to DockerHub 

## ✅  Time Zone
-The system uses a UTC-first strategy. All submission times are handled and stored in UTC to avoid inconsistencies across global clients.

## 🧭 Caching
-The SubmissionService uses in-memory caching to reduce repeated calls to ExamService for the same exam ID. This improves performance for high-volume exam periods. 

-In a production deployment, this would be replaced with a distributed cache like Redis to provide cross-instance consistency and offline fallback.

## 🗑️ Deletion Behavior

-Entities implement soft delete logic to avoid permanent data loss.

-Records are flagged instead of removed from the database.

## 🧠 Observability
-Logging is handled via Serilog.

-Logs are visualized using Seq (via Docker).

-Traces are generated using OpenTelemetry (console-only).

-⚠️ For production:

	-Considering a distributed tracing with Jaeger/Zipkin
	-Enable log correlation across services
	-Store logs in a centralized, queryable system

## 🌐 API Gateway & Ingress
-This system uses direct service-to-service calls.

-⚠️ In production, we might consider introducing an API Gateway (e.g., YARP, or NGINX, etc) for:

	-Routing
	-Authentication
	-Rate limiting
	-TLS termination
	-Observability


---

## 🚀 How to Run & Use the Application
Prerequisites
	
	 .NET 9 SDK

	 Docker Desktop (with Docker Compose)

	 You can use SwaggerUi for testing, (Optional) curl or Postman or Insomnia

1- Clone the Repo

2- Open a terminal and navigate to the project root directory

3- Run docker-compose up --build

	This will build and start all services and their dependencies.

4- Use the following endpoints to interact with the services:

This will start:

		ExamService (http://localhost:8080)
	
		SubmissionService (http://localhost:8081)
	
		NotificationService (http://localhost:8082)
	
		RabbitMQ Management UI (http://localhost:15672, user: rabbitmq, pass: Admin@1234)
	
		SQL Server 2019 Express for ExamService (port 11433, password: Admin@1234)
	
		SQL Server 2019 Express for SubmissionService (port 21433, password: Admin@1234)

	        Seq for logging (http://localhost:5341, user: admin, pass: Admin@1234) - it will ask you to change the password


5. Accessing the APIs

Each service exposes a Swagger UI for easy API exploration:

		ExamService: http://localhost:8080/swagger
	
		SubmissionService: http://localhost:8081/swagger
	
		NotificationService: http://localhost:8082/swagger

6. Authentication
	
     	 -Use the /dev-token endpoint in each service (except Notification) to generate JWT tokens for roles like admin or student.
  
	     -Add the token as a Bearer token in the Authorization header in SwaggerUI when making API requests.

7. Example Workflow (Make sure to Authenticate first)	
	  		
	   -Admin: Use ExamService to create subjects, questions, and configure exams (there is one exam config that is seeded you can use that by calling api/v1/exam-configs).
 
       -Student: Use SubmissionService to submit exam responses (requires a student token).
 	  
	   -Notification: SubmissionService will trigger NotificationService for grading simulation.


For simple testing (testing the full happy path)
 
 - Go to Exam Service Swagger UI and Create a token by calling /dev-token endpoint and authenticate with the token in the Swagger UI

 - Within Exam service, call the /api/v1/exam-configs endpoint to create an exam config (you can use the seeded data).

 - Go to Submission Service Swagger UI and create a token by calling /dev-token endpoint and authenticate with the token in the Swagger UI

 - Copy the data from the seeded exam config *you will need examId, and the two question Ids* and use it to submit an exam response in Submission Service by calling the /api/v1/submit/submissions/{examId} endpoint.

 - Sample Json 
              	{
                     "answers": [
                  {
                    "questionId": "1d992702-31e9-4baa-b9f2-8ed9ed4cee5f",
                    "answerValue": "answer 1"
                  },
                  {
                    "questionId": "72373b12-065f-4ca1-a787-7c72aee78926",
                    "answerValue": "answer2"
                  }
                  ]
                 }

	-  Go to Notification Service Swagger UI and call /logs to see the simulated notification logs for grading.
	-  To do another submition, you need to create a new token in the Submission Service Swagger UI, becasue each token represents different student, no need to create a new token in the ExamService, you can use the same json from your previous submission and it will work for the new token/student
	-  To view the logs, go to Seq url as mentioned above (http://localhost:5341) and login with the credentials provided. You can filter logs by service name or severity level.
 
8. To stop the services, run: 
       
       docker compose down


