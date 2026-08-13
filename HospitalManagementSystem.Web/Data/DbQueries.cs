namespace HospitalManagementSystem.Web.Data
{
    public static class DbQueries
    {
        // Patients
        public const string PatientsSelectAll = "SELECT PatientID, NationalID, FirstName, LastName, Gender, DateOfBirth, Phone, Email, Address, BloodType, EmergencyContact, CreatedDate FROM Patients ORDER BY PatientID";
        public const string PatientsSelectById = "SELECT PatientID, NationalID, FirstName, LastName, Gender, DateOfBirth, Phone, Email, Address, BloodType, EmergencyContact, CreatedDate FROM Patients WHERE PatientID = @PatientID";
        public const string PatientsInsert = "INSERT INTO Patients (NationalID, FirstName, LastName, Gender, DateOfBirth, Phone, Email, Address, BloodType, EmergencyContact, CreatedDate) VALUES (@NationalID, @FirstName, @LastName, @Gender, @DateOfBirth, @Phone, @Email, @Address, @BloodType, @EmergencyContact, @CreatedDate)";
        public const string PatientsUpdate = "UPDATE Patients SET NationalID = @NationalID, FirstName = @FirstName, LastName = @LastName, Gender = @Gender, DateOfBirth = @DateOfBirth, Phone = @Phone, Email = @Email, Address = @Address, BloodType = @BloodType, EmergencyContact = @EmergencyContact WHERE PatientID = @PatientID";
        public const string PatientsDelete = "DELETE FROM Patients WHERE PatientID = @PatientID";

        // Doctors
        public const string DoctorsSelectAll = "SELECT DoctorID, FirstName, LastName, Specialization, Phone, Email, DepartmentID FROM Doctors ORDER BY DoctorID";
        public const string DoctorsSelectById = "SELECT DoctorID, FirstName, LastName, Specialization, Phone, Email, DepartmentID FROM Doctors WHERE DoctorID = @DoctorID";
        public const string DoctorsInsert = "INSERT INTO Doctors (FirstName, LastName, Specialization, Phone, Email, DepartmentID) VALUES (@FirstName, @LastName, @Specialization, @Phone, @Email, @DepartmentID)";
        public const string DoctorsUpdate = "UPDATE Doctors SET FirstName = @FirstName, LastName = @LastName, Specialization = @Specialization, Phone = @Phone, Email = @Email, DepartmentID = @DepartmentID WHERE DoctorID = @DoctorID";
        public const string DoctorsDelete = "DELETE FROM Doctors WHERE DoctorID = @DoctorID";

        // Departments
        public const string DepartmentsSelectAll = "SELECT DepartmentID, DepartmentName, Description, CreatedDate FROM Departments ORDER BY DepartmentID";
        public const string DepartmentsSelectById = "SELECT DepartmentID, DepartmentName, Description, CreatedDate FROM Departments WHERE DepartmentID = @DepartmentID";
        public const string DepartmentsInsert = "INSERT INTO Departments (DepartmentName, Description, CreatedDate) VALUES (@DepartmentName, @Description, @CreatedDate)";
        public const string DepartmentsUpdate = "UPDATE Departments SET DepartmentName = @DepartmentName, Description = @Description WHERE DepartmentID = @DepartmentID";
        public const string DepartmentsDelete = "DELETE FROM Departments WHERE DepartmentID = @DepartmentID";

        // Appointments
        public const string AppointmentsSelectAll = "SELECT AppointmentID, PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Notes, CreatedDate FROM Appointments ORDER BY AppointmentID";
        public const string AppointmentsSelectById = "SELECT AppointmentID, PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Notes, CreatedDate FROM Appointments WHERE AppointmentID = @AppointmentID";
        public const string AppointmentsInsert = "INSERT INTO Appointments (PatientID, DoctorID, AppointmentDate, AppointmentTime, Status, Notes, CreatedDate) VALUES (@PatientID, @DoctorID, @AppointmentDate, @AppointmentTime, @Status, @Notes, @CreatedDate)";
        public const string AppointmentsUpdate = "UPDATE Appointments SET PatientID = @PatientID, DoctorID = @DoctorID, AppointmentDate = @AppointmentDate, AppointmentTime = @AppointmentTime, Status = @Status, Notes = @Notes WHERE AppointmentID = @AppointmentID";
        public const string AppointmentsDelete = "DELETE FROM Appointments WHERE AppointmentID = @AppointmentID";

        // MedicalRecords
        public const string MedicalRecordsSelectAll = "SELECT RecordID, PatientID, DoctorID, Diagnosis, Symptoms, Treatment, Notes, RecordDate FROM MedicalRecords ORDER BY RecordID";
        public const string MedicalRecordsSelectById = "SELECT RecordID, PatientID, DoctorID, Diagnosis, Symptoms, Treatment, Notes, RecordDate FROM MedicalRecords WHERE RecordID = @RecordID";
        public const string MedicalRecordsInsert = "INSERT INTO MedicalRecords (PatientID, DoctorID, Diagnosis, Symptoms, Treatment, Notes, RecordDate) VALUES (@PatientID, @DoctorID, @Diagnosis, @Symptoms, @Treatment, @Notes, @RecordDate)";
        public const string MedicalRecordsUpdate = "UPDATE MedicalRecords SET PatientID = @PatientID, DoctorID = @DoctorID, Diagnosis = @Diagnosis, Symptoms = @Symptoms, Treatment = @Treatment, Notes = @Notes, RecordDate = @RecordDate WHERE RecordID = @RecordID";
        public const string MedicalRecordsDelete = "DELETE FROM MedicalRecords WHERE RecordID = @RecordID";

        // Prescriptions
        public const string PrescriptionsSelectAll = "SELECT PrescriptionID, PatientID, DoctorID, Medication, Dosage, Frequency, Duration, Instructions, PrescriptionDate FROM Prescriptions ORDER BY PrescriptionID";
        public const string PrescriptionsSelectById = "SELECT PrescriptionID, PatientID, DoctorID, Medication, Dosage, Frequency, Duration, Instructions, PrescriptionDate FROM Prescriptions WHERE PrescriptionID = @PrescriptionID";
        public const string PrescriptionsInsert = "INSERT INTO Prescriptions (PatientID, DoctorID, Medication, Dosage, Frequency, Duration, Instructions, PrescriptionDate) VALUES (@PatientID, @DoctorID, @Medication, @Dosage, @Frequency, @Duration, @Instructions, @PrescriptionDate)";
        public const string PrescriptionsUpdate = "UPDATE Prescriptions SET PatientID = @PatientID, DoctorID = @DoctorID, Medication = @Medication, Dosage = @Dosage, Frequency = @Frequency, Duration = @Duration, Instructions = @Instructions, PrescriptionDate = @PrescriptionDate WHERE PrescriptionID = @PrescriptionID";
        public const string PrescriptionsDelete = "DELETE FROM Prescriptions WHERE PrescriptionID = @PrescriptionID";

        // Bills
        public const string BillsSelectAll = "SELECT BillID, PatientID, Amount, PaymentStatus, PaymentMethod, Description, BillDate FROM Bills ORDER BY BillID";
        public const string BillsSelectById = "SELECT BillID, PatientID, Amount, PaymentStatus, PaymentMethod, Description, BillDate FROM Bills WHERE BillID = @BillID";
        public const string BillsInsert = "INSERT INTO Bills (PatientID, Amount, PaymentStatus, PaymentMethod, Description, BillDate) VALUES (@PatientID, @Amount, @PaymentStatus, @PaymentMethod, @Description, @BillDate)";
        public const string BillsUpdate = "UPDATE Bills SET PatientID = @PatientID, Amount = @Amount, PaymentStatus = @PaymentStatus, PaymentMethod = @PaymentMethod, Description = @Description, BillDate = @BillDate WHERE BillID = @BillID";
        public const string BillsDelete = "DELETE FROM Bills WHERE BillID = @BillID";

        // Users
        public const string UsersSelectByUserName = "SELECT UserID, UserName, PasswordHash, PasswordSalt, CreatedDate FROM Users WHERE UserName = @UserName";
        public const string UsersInsert = "INSERT INTO Users (UserName, PasswordHash, PasswordSalt, CreatedDate) VALUES (@UserName, @PasswordHash, @PasswordSalt, @CreatedDate)";
        public const string UsersExistsByUserName = "SELECT COUNT(1) FROM Users WHERE UserName = @UserName";
    }
}
