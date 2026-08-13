# Hospital Management System — Database Backup & Restore

Criterion #8 deliverable for SQL Server database **malakallah13137**.

This project uses a **documented SQL Server backup/restore procedure** (not an in-app backup button). That is the safest approach for a local .NET Framework 4.8 ASP.NET Web Forms application: no elevated SQL permissions in the web app, no risk of accidental overwrite from the UI, and no large `.bak` files inside the deployment ZIP.

---

## A. Database name

| Item | Value |
|------|--------|
| Database | `malakallah13137` |
| Typical instance (local) | `MALAKALLAH\SQLEXPRESS` (replace with your server\instance) |
| Authentication for demos | Windows Integrated Security (preferred) |

Do **not** put SQL passwords or SMTP secrets in this document or in scripts you share.

---

## B. Backup procedure (FULL)

1. Choose a folder that **exists** and that the SQL Server service account can write to.  
   On many SQL Express installs the default is:

   `C:\Program Files\Microsoft SQL Server\MSSQL<version>.SQLEXPRESS\MSSQL\Backup`

2. Replace `<BACKUP_PATH>` below with that folder (no trailing backslash required if you include it consistently).

3. Run in **SQL Server Management Studio (SSMS)** or `sqlcmd`:

```sql
BACKUP DATABASE [malakallah13137]
TO DISK = N'<BACKUP_PATH>\HospitalManagementSystem.bak'
WITH INIT, FORMAT, NAME = N'HospitalManagementSystem-Full',
     SKIP, NOREWIND, NOUNLOAD, STATS = 10;
```

Example (local Criterion #8 verification — adjust path to your machine):

```sql
BACKUP DATABASE [malakallah13137]
TO DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL17.SQLEXPRESS\MSSQL\Backup\HospitalManagementSystem_Criterion8.bak'
WITH INIT, FORMAT, NAME = N'HospitalManagementSystem-Full',
     SKIP, NOREWIND, NOUNLOAD, STATS = 10;
```

Expected result: message similar to `BACKUP DATABASE successfully processed ... pages`.

**Do not** run BACKUP to a path that does not exist or that SQL Server cannot write.

---

## C. Backup verification (VERIFYONLY)

After the `.bak` is created:

```sql
RESTORE VERIFYONLY
FROM DISK = N'<BACKUP_PATH>\HospitalManagementSystem.bak';
```

Expected result: `The backup set on file 1 is valid.`

---

## D. Restore procedure (production / instructor machine)

**Warning:** Restoring **over** `malakallah13137` replaces the live database. Only do this when you intentionally want to recover that database.

1. Discover logical file names (required for MOVE):

```sql
RESTORE FILELISTONLY
FROM DISK = N'<BACKUP_PATH>\HospitalManagementSystem.bak';
```

Typical logical names for this project:

| LogicalName | Type |
|-------------|------|
| `malakallah13137` | Data |
| `malakallah13137_log` | Log |

2. Set the live database to single-user (disconnects other sessions), then restore:

```sql
USE [master];
GO

ALTER DATABASE [malakallah13137]
SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO

RESTORE DATABASE [malakallah13137]
FROM DISK = N'<BACKUP_PATH>\HospitalManagementSystem.bak'
WITH REPLACE,
     MOVE N'malakallah13137'
       TO N'<DATA_PATH>\malakallah13137.mdf',
     MOVE N'malakallah13137_log'
       TO N'<DATA_PATH>\malakallah13137_log.ldf',
     STATS = 10;
GO

ALTER DATABASE [malakallah13137]
SET MULTI_USER;
GO
```

Replace `<DATA_PATH>` with the instance data directory (often under `...\MSSQL\DATA`).

---

## E. Do NOT overwrite the live database during coursework testing

For demos and student verification, **restore into a temporary database** (see section F).  
Never DROP or REPLACE `malakallah13137` unless performing a real recovery.

---

## F. Safe restore test (recommended demonstration)

Creates **`HospitalManagementSystem_RestoreTest`**, verifies it, then deletes **only** that database.

```sql
USE [master];
GO

-- Optional: remove a previous test DB
IF DB_ID(N'HospitalManagementSystem_RestoreTest') IS NOT NULL
BEGIN
    ALTER DATABASE [HospitalManagementSystem_RestoreTest]
      SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [HospitalManagementSystem_RestoreTest];
END
GO

RESTORE DATABASE [HospitalManagementSystem_RestoreTest]
FROM DISK = N'<BACKUP_PATH>\HospitalManagementSystem.bak'
WITH MOVE N'malakallah13137'
       TO N'<DATA_PATH>\HospitalManagementSystem_RestoreTest.mdf',
     MOVE N'malakallah13137_log'
       TO N'<DATA_PATH>\HospitalManagementSystem_RestoreTest_log.ldf',
     REPLACE, STATS = 10;
GO
```

### Verify tables exist

```sql
USE [HospitalManagementSystem_RestoreTest];
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME;
```

Expected tables:

- Appointments  
- Bills  
- Departments  
- Doctors  
- MedicalRecords  
- Patients  
- Prescriptions  
- Users  

### Verify row counts (compare to live)

```sql
SELECT 'Patients' AS t, COUNT(*) AS c FROM Patients
UNION ALL SELECT 'Doctors', COUNT(*) FROM Doctors
UNION ALL SELECT 'Departments', COUNT(*) FROM Departments
UNION ALL SELECT 'Appointments', COUNT(*) FROM Appointments
UNION ALL SELECT 'MedicalRecords', COUNT(*) FROM MedicalRecords
UNION ALL SELECT 'Prescriptions', COUNT(*) FROM Prescriptions
UNION ALL SELECT 'Bills', COUNT(*) FROM Bills
UNION ALL SELECT 'Users', COUNT(*) FROM Users;
```

### Verify foreign keys preserved

```sql
SELECT name
FROM sys.foreign_keys
ORDER BY name;
```

Expected FK names (8):

- FK_Appointments_Doctors  
- FK_Appointments_Patients  
- FK_Bills_Patients  
- FK_Doctors_Departments  
- FK_MedicalRecords_Doctors  
- FK_MedicalRecords_Patients  
- FK_Prescriptions_Doctors  
- FK_Prescriptions_Patients  

### Cleanup — delete ONLY the temporary database

```sql
USE [master];
ALTER DATABASE [HospitalManagementSystem_RestoreTest]
  SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE [HospitalManagementSystem_RestoreTest];
```

Confirm live DB still present:

```sql
SELECT name, state_desc
FROM sys.databases
WHERE name = N'malakallah13137';
```

---

## G. Evidence from local Criterion #8 verification (2026-08-12)

| Step | Result |
|------|--------|
| FULL backup of `malakallah13137` | **SUCCESS** (`BACKUP DATABASE successfully processed 722 pages`) |
| `RESTORE VERIFYONLY` | **SUCCESS** (`The backup set on file 1 is valid.`) |
| Restore into `HospitalManagementSystem_RestoreTest` | **SUCCESS** |
| Tables in restore-test DB | All 8 expected tables present |
| Row counts (restore-test) | Patients 8, Doctors 5, Departments 5, Appointments 6, MedicalRecords 5, Prescriptions 5, Bills 7, Users 8 |
| Foreign keys in restore-test DB | **8** FKs present |
| Live `malakallah13137` after test | Still **ONLINE**; same row counts; **not** overwritten |
| Drop `HospitalManagementSystem_RestoreTest` | **SUCCESS**; live DB remains |

Backup file used for verification (local SQL Backup folder; **not** packaged in the web deployment ZIP):

`...\MSSQL\Backup\HospitalManagementSystem_Criterion8.bak`

---

## H. How an instructor can demonstrate Backup & Restore

1. Open SSMS → connect to the student SQL instance.  
2. Run the **BACKUP** script (section B) to a writable `<BACKUP_PATH>`.  
3. Run **RESTORE VERIFYONLY** (section C).  
4. Run the **safe restore test** into `HospitalManagementSystem_RestoreTest` (section F).  
5. Show tables, row counts, and foreign keys.  
6. Drop **only** `HospitalManagementSystem_RestoreTest`.  
7. Confirm `malakallah13137` is still online.

Optional: show that the ASP.NET app still logs in and lists Patients against the live database after the test.

---

## I. Deployment notes

- Do **not** include large `.bak` files inside `Deployment/HospitalManagementSystem_Web.zip`.  
- Do **not** embed SQL passwords in this document or in Web.config samples you distribute.  
- Application connection string remains separate (`HospitalDb` in Web.config); backup/restore is a DBA/SQL Server operation.

---

## Quick reference — sqlcmd examples

Replace server and paths as needed:

```bat
sqlcmd -S YOUR_SERVER\SQLEXPRESS -E -C -Q "BACKUP DATABASE [malakallah13137] TO DISK = N'<BACKUP_PATH>\HospitalManagementSystem.bak' WITH INIT, FORMAT, NAME = N'HospitalManagementSystem-Full';"

sqlcmd -S YOUR_SERVER\SQLEXPRESS -E -C -Q "RESTORE VERIFYONLY FROM DISK = N'<BACKUP_PATH>\HospitalManagementSystem.bak';"
```

(`-E` = Windows auth, `-C` = Trust Server Certificate for local Driver 18 scenarios.)
