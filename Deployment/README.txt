Hospital Management System — Deployment Guide
==============================================

Application name:
  Hospital Management System

Project:
  HospitalManagementSystem.Web

Target framework:
  .NET Framework 4.8

Package contents:
  Deployment/HospitalManagementSystem_Web/     (folder)
  Deployment/HospitalManagementSystem_Web.zip  (same content, zipped)

------------------------------------------------
1) IIS / ASP.NET REQUIREMENTS
------------------------------------------------
- Windows Server or Windows 10/11 with IIS enabled
- IIS with ASP.NET 4.x support
- .NET Framework 4.8 installed on the host
- Application Pool settings:
    * .NET CLR Version: v4.0
    * Managed Pipeline Mode: Integrated
    * Identity: ApplicationPoolIdentity (or a Windows account that can access SQL Server if using Integrated Security)
- Enable ASP.NET features (IIS):
    * ASP.NET 4.8
    * .NET Extensibility 4.8
    * ISAPI Extensions / ISAPI Filters

------------------------------------------------
2) SQL SERVER REQUIREMENTS
------------------------------------------------
- SQL Server Express / SQL Server
- Database name used by the application: malakallah13137
- Schema script included in the package:
    Database/HospitalDatabase.sql
- Create the database on the target SQL Server before first run (run the script),
  or restore from a SQL Server .bak using the Backup & Restore guide (see section 7).
- Ensure the IIS app-pool identity (or SQL login) can connect and read/write the database.

------------------------------------------------
3) CONFIGURE Web.config (REQUIRED ON TARGET)
------------------------------------------------
Edit the deployed Web.config (NOT the development copy).

A) Connection string
  Replace YOUR_SQL_SERVER\SQLEXPRESS with your real SQL instance, for example:

  Data Source=YOUR_SQL_SERVER\SQLEXPRESS;Initial Catalog=malakallah13137;Integrated Security=True;TrustServerCertificate=True;

  Or use SQL authentication if required by the host (provide User ID/Password securely; do not share them in the package).

B) ASP.NET WebForms validation
  The package includes:
    ValidationSettings:UnobtrusiveValidationMode = None
  Do not remove this setting unless you also register a jQuery ScriptResourceMapping.

C) SMTP (Gmail App Password recommended)
  Set these appSettings keys:
    smtpHost      = smtp.gmail.com
    smtpPort      = 587
    smtpEnableSsl = true
    smtpUser      = your Gmail address
    smtpPassword  = your Gmail App Password (NOT your normal Gmail password)
    smtpFrom      = your Gmail address

  The package ships with placeholders only:
    __SMTP_USER_HERE__
    __SMTP_PASSWORD_HERE__
    __SMTP_FROM_HERE__

  Replace placeholders on the server only. Do not commit or redistribute real passwords.

D) Security settings already configured
    compilation debug="false"
    customErrors mode="RemoteOnly"
    Forms Authentication + deny anonymous users
    Login.aspx and Register.aspx allow anonymous

------------------------------------------------
4) HOW TO DEPLOY TO IIS
------------------------------------------------
1. Install .NET Framework 4.8 and enable IIS ASP.NET 4.8 features.
2. Create/restore database malakallah13137 using Database/HospitalDatabase.sql (or SQL backup).
3. Copy the package folder HospitalManagementSystem_Web to the server, e.g.:
     C:\inetpub\wwwroot\HospitalManagementSystem
4. In IIS Manager:
     - Add Website (or Application under Default Web Site)
     - Physical path = the folder containing Web.config and bin\
     - Binding: http (or https with certificate)
5. Set Application Pool to .NET CLR v4.0 / Integrated.
6. Grant the app-pool identity Modify on the site folder if needed (usually Read/Execute is enough).
7. Edit Web.config connection string + SMTP settings for the target environment.
8. Browse the site.

------------------------------------------------
5) HOW TO TEST AFTER DEPLOYMENT
------------------------------------------------
Login:
  - Open /Login.aspx
  - Register a user (or use an existing Users table account)
  - Confirm login redirects to Dashboard

CRUD:
  - Patients / Doctors / Departments / Appointments /
    Medical Records / Prescriptions / Bills
  - Perform Add, Edit, Delete, List/Search on each module

Email:
  - Open Patients List
  - Click Send Email for a patient that has an email address
  - Confirm inbox delivery (SMTP must be configured)

Reports / Export:
  - Open Reports.aspx
  - Export Excel, Word, PDF and confirm downloads open correctly

Security:
  - Sign out
  - Confirm protected pages redirect to Login.aspx

------------------------------------------------
6) BACKUP & RESTORE (Criterion #8)
------------------------------------------------
SQL Server backup/restore is documented in the package:

  Database/Database-Backup-Restore.md
  Database/Backup-Restore-README.txt

Also available in the project repository:

  Documentation/Database-Backup-Restore.md

Use a temporary restore-test database for demos.
Do NOT overwrite the live database during testing.
Do NOT place .bak files inside this web deployment ZIP.

------------------------------------------------
7) NOTES
------------------------------------------------
- This package is for rubric deployment readiness (IIS web package).
- It is NOT an automatic cloud publish.
- Source code (.cs) is compiled into bin\HospitalManagementSystem.Web.dll
- Do not deploy obj/, .vs/, .git/, .pdb, .mdf, .ldf, or .bak files.
- Do not deploy development secrets.
- ERD files remain in the project Documentation/ folder (ERD.png, ERD.pdf, ERD-Documentation.txt).
