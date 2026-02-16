# 🐾 PetClinic – MVP Microservices Overview

This project is an MVP implementation of **PetClinic** using a microservices-based architecture.  
Each service owns its own domain, business rules, and (ideally) its own database.

The main goal of the MVP is to support:
- Managing owners and pets
- Managing veterinarians
- Scheduling and tracking visits
- Sending basic notifications

---

## Architecture at a Glance

- Web UI / Mobile Client  
- API Gateway (YARP??)  
- Owner Service  
- Pet Service  
- Vet Service  
- Visit Service  
- Notification Service  

Each service communicates through HTTP for MVP.  
Async messaging (event bus) can be introduced later.

---

## Web UI / Mobile Client

**Purpose:**  
User-facing application for clinic staff and pet owners.

**Responsibilities:**
- Display owners, pets, vets, and visits
- Create and edit owners and pets
- Schedule visits
- View visit history

---

## API Gateway (YARP??)

**Purpose:**  
Single entry point for all client requests.

**Responsibilities:**
- Route requests to appropriate services
- Basic request validation
- CORS and rate limiting (minimal for MVP)

---

## Owner Service

**Domain:** Owners

**Responsibilities:**
- Create, update, and retrieve owners
- Store owner contact information
- Search owners by name or phone

**Example Endpoints:**
- `POST /owners`
- `GET /owners/{id}`
- `GET /owners?query=...`
- `PUT /owners/{id}`

**Source of Truth For:** Owner data

---

## Pet Service

**Domain:** Pets

**Responsibilities:**
- Create, update, and retrieve pets
- Associate pets with owners
- Store pet type, breed, and birth date

**Example Endpoints:**
- `POST /pets`
- `GET /pets/{id}`
- `GET /pets?ownerId=...`
- `PUT /pets/{id}`

**Source of Truth For:** Pet data

---

## Vet Service

**Domain:** Veterinarians

**Responsibilities:**
- Manage vets and their specializations
- Provide list of available vets

**Example Endpoints:**
- `POST /vets`
- `GET /vets`
- `GET /vets/{id}`
- `PUT /vets/{id}`

**Source of Truth For:** Vet data

---

## Visit Service (Scheduling)

**Domain:** Visits / Appointments

**Responsibilities:**
- Schedule visits
- Validate time conflicts for vets
- Cancel visits
- Store visit history

**Example Endpoints:**
- `POST /visits`
- `GET /visits/{id}`
- `GET /visits?petId=...`
- `GET /visits?vetId=...&date=...`
- `POST /visits/{id}/cancel`

**Source of Truth For:** Visit and scheduling data

---

## Notification Service

**Domain:** Notifications

**Responsibilities:**
- Send email/SMS/in-app notifications
- Handle visit scheduled / cancelled events
- Store notification logs

**Implementation for MVP:**
- Console or fake sender
- Can be called synchronously from Visit Service

---

## Communication (MVP)

- Synchronous HTTP between services
- No event bus required initially

**Future Improvement:**
- Introduce message broker (RabbitMQ, Azure Service Bus, Kafka)
- Publish events such as:
  - `VisitScheduled`
  - `VisitCancelled`

---

## Data Ownership

| Entity  | Owning Service |
|-------|--------------|
| Owner | Owner Service |
| Pet | Pet Service |
| Vet | Vet Service |
| Visit | Visit Service |
| Notification | Notification Service |

---

## MVP Scope

Included:
- CRUD for owners, pets, vets
- Visit scheduling and cancellation
- Basic notifications

Out of Scope (for later):
- Authentication/authorization
- Payments/Billing
- Advanced scheduling rules
- Reporting and analytics

