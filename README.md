# Exam Submission System

This repository contains a microservices Exam Submission System built with .NET 9.0. The solution is organized into multiple services, each following a clean architecture approach, and includes API, Application, Domain, Infrastructure, and Test projects for each service.

## Solution Structure

- **ExamService**
  - `ExamService.API`: REST API for exam management
  - `ExamService.Application`: Application logic and use cases
  - `ExamService.Domain`: Domain models and interfaces
  - `ExamService.Infrastructure`: Data access and external integrations
  - `ExamService.Test`: Unit and integration tests

- **SubmissionService**
  - `SubmissionService.API`: REST API for submission management
  - `SubmissionService.Application`: Application logic and use cases
  - `SubmissionService.Domain`: Domain models and interfaces
  - `SubmissionService.Infrastructure`: Data access and external integrations
  - `SubmissionService.Test`: Unit and integration tests

- **NotificationService**
  - `NotificationService.API`: REST API for notifications
  - `NotificationService.Application`: Application logic and use cases
  - `NotificationService.Domain`: Domain models and interfaces
  - `NotificationService.Infrastructure`: Data access and external integrations
  - `NotificationService.Test`: Unit and integration tests

- **Shared**
  - Common code and utilities shared across services

#