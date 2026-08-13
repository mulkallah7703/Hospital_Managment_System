# Hospital Management System

A web-based Hospital Management System developed as a Cooperative Training Project for managing hospital operations, patient information, appointments, medical records, prescriptions, billing, and reports.

## Project Overview

The Hospital Management System provides a centralized platform for managing essential hospital operations through a secure and user-friendly web interface.

The system is designed to simplify hospital workflows and provide organized access to healthcare-related information for authorized users.

## Live Demo

🌐 **Live Website:**  
https://mulkallah-001-site1.itempurl.com/

## Project Type

- Cooperative Training Project
- Web Application
- ASP.NET Web Forms
- .NET Framework 4.8
- Microsoft SQL Server

## Main Features

### 🔐 Authentication
- User Login
- User Registration
- Session-based authentication
- Secure access to the system

### 👨‍⚕️ Patient Management
- Add patients
- Edit patient information
- Delete patients
- Search patients
- Manage National ID, phone, email, and gender information

### 🩺 Doctor Management
- Add and manage doctors
- Assign doctors to departments
- Search and manage doctor information

### 🏥 Department Management
- Create departments
- Manage department information
- Associate doctors with departments

### 📅 Appointment Management
- Schedule appointments
- Manage patient appointments
- Associate appointments with doctors
- View today's appointments

### 📋 Medical Records
- Manage patient medical records
- Store medical information
- Associate records with patients and doctors

### 💊 Prescription Management
- Create prescriptions
- Manage prescription information
- Associate prescriptions with patients and doctors

### 💳 Billing
- Create and manage bills
- Track billing information
- Associate bills with patients

### 📊 Reports
- Generate and view hospital reports
- Provide an overview of hospital activities

### 👥 Team Information
- About Our Team page
- Student information
- Academic information
- Supervisors and training information

### 📧 Contact Us
- Contact form
- Email validation
- Message submission
- Email notification functionality

## Technology Stack

### Frontend
- HTML5
- CSS3
- ASP.NET Web Forms
- Responsive CSS

### Backend
- C#
- ASP.NET Web Forms
- .NET Framework 4.8

### Database
- Microsoft SQL Server
- ADO.NET

### Development Tools
- Visual Studio
- Git
- GitHub
- FileZilla

### Hosting
- Windows/IIS-based hosting
- Production deployment through FTP

## Database

The system uses Microsoft SQL Server.

### Main Tables

- `Users`
- `Patients`
- `Doctors`
- `Departments`
- `Appointments`
- `MedicalRecords`
- `Prescriptions`
- `Bills`

The database script is included in the project documentation/deployment files.

## Project Structure

```text
HospitalManagementSystem
│
├── Deployment/
│
├── Documentation/
│
├── HospitalManagementSystem.Web/
│   │
│   ├── Pages/
│   │   ├── Patients/
│   │   ├── Doctors/
│   │   ├── Departments/
│   │   ├── Appointments/
│   │   ├── MedicalRecords/
│   │   ├── Prescriptions/
│   │   └── Bills/
│   │
│   ├── Services/
│   ├── Security/
│   ├── Helpers/
│   │
│   ├── About.aspx
│   ├── ContactUs.aspx
│   ├── Default.aspx
│   ├── Login.aspx
│   ├── Register.aspx
│   ├── Reports.aspx
│   ├── Site.Master
│   ├── Global.asax
│   └── Web.config
│
├── HospitalManagementSystem.slnx
└── build-controls.log
