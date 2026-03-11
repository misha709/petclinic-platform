# Project Requirements: PetClinic CI/CD Platform

## Overview

Build a CI/CD solution for the [spring-petclinic](https://github.com/spring-projects/spring-petclinic) Java application (or a custom application of your choice), containerize it, and deploy it to a cloud environment.

---

## Deliverables

| # | Deliverable              | Status |
|---|--------------------------|--------|
| 1 | Architecture diagram      | [ ]    |
| 2 | Infrastructure automation | [ ]    |
| 3 | CI/CD pipeline            | [ ]    |
| 4 | Monitoring dashboard      | [ ]    |

---

## Technology Constraints

| Concern                        | Allowed Options                                                                                     |
|-------------------------------|------------------------------------------------------------------------------------------------------|
| Source control                 | GitHub, GitLab                                                                                       |
| Cloud provider                 | AWS, GCP, Azure                                                                                      |
| IaC tool                       | Terraform                                                                                            |
| Terraform remote state backend | Amazon S3, Azure Blob Storage, Google Cloud Storage, GitLab-managed Terraform state                  |
| Configuration management       | Ansible                                                                                              |
| CI/CD automation               | GitHub Actions, GitLab CI, Jenkins                                                                   |
| Artifact format                | Docker images                                                                                        |
| Artifact registry              | Nexus, GitLab Container Registry, Docker Hub, ECR, GCR, or ACR                                     |
| Application database           | Amazon RDS for MySQL, Azure Database for MySQL, Cloud SQL (GCP)                                     |
| Scripting languages            | Python and/or Bash                                                                                   |

---

## 1. Infrastructure Automation

### Repositories

- Infrastructure code must live in a **separate repository** from application source code.
- Use an **Infrastructure as Code** approach throughout.

### Bootstrap Steps (may be done manually or via local scripts)

- [ ] Create the Terraform remote state backend (S3 / Azure Blob / GCS) before running any pipeline.
- [ ] If using GitLab/GitHub SaaS, register a CI runner (can run locally at first).
- [ ] If using Jenkins, provision a VM, install Jenkins, and configure agents.

### Terraform Pipeline Stages

| Stage                       | Trigger       |
|-----------------------------|---------------|
| Format (`terraform fmt`)    | Automatic     |
| Validate & scan (optional)  | Automatic     |
| Plan (`terraform plan`)     | Automatic     |
| Apply / deploy resources    | Manual        |
| Destroy resources           | Manual        |

### Terraform-Managed Cloud Resources

- **Application VM** — with all required networking (VPC/VNet, subnets, security groups) and a load balancer.
- **Managed database** — persistent database instance for the application.
- **Tooling VM** *(optional)* — separate VM for supporting software such as GitLab runners or Nexus, with its own networking.

### Ansible Responsibilities

Install and configure software on provisioned VMs:

- [ ] Docker and docker-compose
- [ ] Jenkins workers **or** GitLab runners
- [ ] Nexus repository manager *(if used)*
- [ ] Any other required packages

---

## 2. CI/CD Solution

### Application Repository Requirements

The application repository must include:

- Maven or Gradle configuration files
- `Dockerfile`

### 2a. Pull Request / Merge Request Pipeline

Runs on every PR/MR targeting any branch.

| Step                   | Notes                                               |
|------------------------|-----------------------------------------------------|
| Static code analysis   | e.g. Checkstyle, SonarQube, SpotBugs                |
| Tests                  | Unit and/or integration tests                       |
| Build                  | Compile and package the application                 |
| Create artifact        | Docker image tagged with the short commit SHA       |
| Push artifact          | Push image to the artifact registry                 |

### 2b. Main Branch Pipeline

Runs on every commit merged to the main/master branch.

| Step                       | Notes                                                                                        |
|----------------------------|----------------------------------------------------------------------------------------------|
| Create Git tag             | Use Semantic Versioning; increment **minor** version on each commit. Use a Python script with [`semver`](https://pypi.org/project/semver/) from PyPI. |
| Create artifact            | Docker image tagged with the new SemVer Git tag                                              |
| Push artifact              | Push image to the artifact registry                                                          |
| Manual deploy              | SSH into the VM, stop and remove the previous container/image, pull the new image, start the application, print the application URL |

---

## 3. Monitoring

- Use a **cloud-native monitoring solution** (e.g. CloudWatch, Azure Monitor, Google Cloud Monitoring).
- Create a dashboard displaying VM resource consumption metrics:
  - CPU utilization
  - Memory usage
  - Disk I/O
  - Network I/O

---

## 4. Architecture Diagram

Create an architecture diagram covering all components of the solution.

**Recommended tools:** [draw.io](https://draw.io), [Lucidchart](https://lucid.app)

**Reference architectures for inspiration:**
- [AWS Reference Architecture Diagrams](https://aws.amazon.com/architecture/)
- [GCP Cloud Reference Architectures](https://cloud.google.com/architecture)
- [Azure Architectures](https://learn.microsoft.com/en-us/azure/architecture/)

---

## 5. Evaluation & Presentation

Maximum score: **30 points**

During the demo, be prepared to:

- [ ] Walk through the architecture diagram and explain all design decisions.
- [ ] Make a live code change and demonstrate the full CI/CD pipeline running end-to-end.
- [ ] Prove the application is connected to and using the managed database.
- [ ] Review the codebase and configuration files, explaining how each part of the solution is implemented.
- [ ] Answer questions from the instructor.
