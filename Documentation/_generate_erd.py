from PIL import Image, ImageDraw, ImageFont
import os

OUT_DIR = r"c:\Users\malak\Desktop\myprojects\HospitalManagementSystem\Documentation"
os.makedirs(OUT_DIR, exist_ok=True)

# Schema verified from live DB malakallah13137
tables = {
    "Departments": {
        "cols": [
            ("DepartmentID", "int", "PK"),
            ("DepartmentName", "nvarchar(100)", "NN"),
            ("Description", "nvarchar(250)", ""),
            ("CreatedDate", "datetime", "NN"),
        ],
    },
    "Patients": {
        "cols": [
            ("PatientID", "int", "PK"),
            ("NationalID", "nvarchar(20)", "NN UQ"),
            ("FirstName", "nvarchar(50)", "NN"),
            ("LastName", "nvarchar(50)", "NN"),
            ("Gender", "nvarchar(10)", "NN"),
            ("DateOfBirth", "date", ""),
            ("Phone", "nvarchar(20)", ""),
            ("Email", "nvarchar(100)", ""),
            ("Address", "nvarchar(200)", ""),
            ("BloodType", "nvarchar(5)", ""),
            ("EmergencyContact", "nvarchar(100)", ""),
            ("CreatedDate", "datetime", "NN"),
        ],
    },
    "Doctors": {
        "cols": [
            ("DoctorID", "int", "PK"),
            ("FirstName", "nvarchar(50)", "NN"),
            ("LastName", "nvarchar(50)", "NN"),
            ("Specialization", "nvarchar(100)", "NN"),
            ("Phone", "nvarchar(20)", ""),
            ("Email", "nvarchar(100)", ""),
            ("DepartmentID", "int", "FK NN"),
        ],
    },
    "Appointments": {
        "cols": [
            ("AppointmentID", "int", "PK"),
            ("PatientID", "int", "FK NN"),
            ("DoctorID", "int", "FK NN"),
            ("AppointmentDate", "date", "NN"),
            ("AppointmentTime", "time", "NN"),
            ("Status", "nvarchar(20)", "NN"),
            ("Notes", "nvarchar(500)", ""),
            ("CreatedDate", "datetime", "NN"),
        ],
    },
    "MedicalRecords": {
        "cols": [
            ("RecordID", "int", "PK"),
            ("PatientID", "int", "FK NN"),
            ("DoctorID", "int", "FK NN"),
            ("Diagnosis", "nvarchar(200)", "NN"),
            ("Symptoms", "nvarchar(500)", ""),
            ("Treatment", "nvarchar(500)", ""),
            ("Notes", "nvarchar(1000)", ""),
            ("RecordDate", "datetime", "NN"),
        ],
    },
    "Prescriptions": {
        "cols": [
            ("PrescriptionID", "int", "PK"),
            ("PatientID", "int", "FK NN"),
            ("DoctorID", "int", "FK NN"),
            ("Medication", "nvarchar(200)", "NN"),
            ("Dosage", "nvarchar(100)", "NN"),
            ("Frequency", "nvarchar(100)", "NN"),
            ("Duration", "nvarchar(100)", "NN"),
            ("Instructions", "nvarchar(500)", ""),
            ("PrescriptionDate", "datetime", "NN"),
        ],
    },
    "Bills": {
        "cols": [
            ("BillID", "int", "PK"),
            ("PatientID", "int", "FK NN"),
            ("Amount", "decimal(10,2)", "NN"),
            ("PaymentStatus", "nvarchar(20)", "NN"),
            ("PaymentMethod", "nvarchar(30)", ""),
            ("Description", "nvarchar(300)", ""),
            ("BillDate", "datetime", "NN"),
        ],
    },
    "Users": {
        "cols": [
            ("UserID", "int", "PK"),
            ("UserName", "nvarchar(50)", "NN UQ"),
            ("PasswordHash", "nvarchar(128)", "NN"),
            ("PasswordSalt", "nvarchar(128)", "NN"),
            ("CreatedDate", "datetime", "NN"),
        ],
    },
}

# Relationships verified from live FK constraints (parent -> child)
relationships = [
    ("Departments", "Doctors", "DepartmentID", "DepartmentID", "1", "N"),
    ("Patients", "Appointments", "PatientID", "PatientID", "1", "N"),
    ("Doctors", "Appointments", "DoctorID", "DoctorID", "1", "N"),
    ("Patients", "MedicalRecords", "PatientID", "PatientID", "1", "N"),
    ("Doctors", "MedicalRecords", "DoctorID", "DoctorID", "1", "N"),
    ("Patients", "Prescriptions", "PatientID", "PatientID", "1", "N"),
    ("Doctors", "Prescriptions", "DoctorID", "DoctorID", "1", "N"),
    ("Patients", "Bills", "PatientID", "PatientID", "1", "N"),
]

W, H = 3000, 2200
positions = {
    "Departments": (100, 120),
    "Doctors": (620, 120),
    "Appointments": (1240, 120),
    "Patients": (100, 620),
    "MedicalRecords": (1240, 620),
    "Prescriptions": (1240, 1180),
    "Bills": (620, 1180),
    "Users": (100, 1450),
}

BOX_W = 420
ROW_H = 28
HEADER_H = 40
PAD = 6

font_paths = [
    r"C:\Windows\Fonts\arialbd.ttf",
    r"C:\Windows\Fonts\arial.ttf",
    r"C:\Windows\Fonts\segoeui.ttf",
]

def load_font(name_candidates, size):
    for path in name_candidates:
        if os.path.exists(path):
            try:
                return ImageFont.truetype(path, size)
            except Exception:
                pass
    return ImageFont.load_default()

font_title = load_font([r"C:\Windows\Fonts\arialbd.ttf"], 30)
font_header = load_font([r"C:\Windows\Fonts\arialbd.ttf"], 17)
font_col = load_font([r"C:\Windows\Fonts\arial.ttf", r"C:\Windows\Fonts\segoeui.ttf"], 14)
font_small = load_font([r"C:\Windows\Fonts\arial.ttf"], 13)
font_badge = load_font([r"C:\Windows\Fonts\arialbd.ttf"], 13)

img = Image.new("RGB", (W, H), (245, 248, 252))
draw = ImageDraw.Draw(img)

draw.rectangle([0, 0, W, 78], fill=(11, 61, 92))
draw.text((40, 20), "Hospital Management System — Entity Relationship Diagram", fill="white", font=font_title)
draw.text(
    (40, 90),
    "Database: malakallah13137  |  SQL Server Express  |  Verified against live PK/FK metadata",
    fill=(40, 70, 100),
    font=font_small,
)

# Legend
lx, ly = 2200, 120
draw.rounded_rectangle([lx, ly, lx + 720, ly + 180], radius=8, outline=(13, 110, 253), width=2, fill=(255, 255, 255))
draw.text((lx + 16, ly + 12), "Legend", fill=(11, 61, 92), font=font_header)
draw.rectangle([lx + 16, ly + 50, lx + 40, ly + 70], fill=(255, 236, 150))
draw.text((lx + 52, ly + 50), "PK — Primary Key (yellow)", fill=(30, 30, 30), font=font_small)
draw.rectangle([lx + 16, ly + 82, lx + 40, ly + 102], fill=(190, 225, 255))
draw.text((lx + 52, ly + 82), "FK — Foreign Key (blue)", fill=(30, 30, 30), font=font_small)
draw.text((lx + 16, ly + 116), "Cardinality: 1 (parent) —— N (child) via FK", fill=(30, 30, 30), font=font_small)
draw.text((lx + 16, ly + 144), "UQ = UNIQUE  |  NN = NOT NULL", fill=(30, 30, 30), font=font_small)

box_geom = {}
for name, meta in tables.items():
    h = HEADER_H + len(meta["cols"]) * ROW_H + PAD
    x, y = positions[name]
    box_geom[name] = (x, y, x + BOX_W, y + h)


def col_anchor(table, col_name, side="right"):
    x1, y1, x2, y2 = box_geom[table]
    cols = [c[0] for c in tables[table]["cols"]]
    idx = cols.index(col_name)
    cy = y1 + HEADER_H + idx * ROW_H + ROW_H // 2
    if side == "right":
        return (x2, cy)
    if side == "left":
        return (x1, cy)
    if side == "bottom":
        return ((x1 + x2) // 2, y2)
    return ((x1 + x2) // 2, y1)


rel_sides = {
    ("Departments", "Doctors"): ("right", "left"),
    ("Patients", "Appointments"): ("right", "left"),
    ("Doctors", "Appointments"): ("right", "left"),
    ("Patients", "MedicalRecords"): ("right", "left"),
    ("Doctors", "MedicalRecords"): ("bottom", "left"),
    ("Patients", "Prescriptions"): ("right", "left"),
    ("Doctors", "Prescriptions"): ("bottom", "left"),
    ("Patients", "Bills"): ("bottom", "top"),
}


def draw_relation(parent, child, pcol, ccol, c1, cN):
    ps, cs = rel_sides.get((parent, child), ("right", "left"))
    p = col_anchor(parent, pcol, ps)
    c = col_anchor(child, ccol, cs)
    color = (13, 110, 253)
    mx = (p[0] + c[0]) // 2
    points = [p, (mx, p[1]), (mx, c[1]), c]
    draw.line(points, fill=color, width=3)
    ax, ay = c
    if cs == "left":
        draw.polygon([(ax, ay), (ax - 12, ay - 7), (ax - 12, ay + 7)], fill=color)
    elif cs == "top":
        draw.polygon([(ax, ay), (ax - 7, ay - 12), (ax + 7, ay - 12)], fill=color)
    elif cs == "right":
        draw.polygon([(ax, ay), (ax + 12, ay - 7), (ax + 12, ay + 7)], fill=color)
    else:
        draw.polygon([(ax, ay), (ax - 7, ay + 12), (ax + 7, ay + 12)], fill=color)
    # cardinality
    if ps == "right":
        draw.text((p[0] + 8, p[1] - 18), c1, fill=(180, 40, 40), font=font_badge)
    elif ps == "bottom":
        draw.text((p[0] + 8, p[1] + 4), c1, fill=(180, 40, 40), font=font_badge)
    else:
        draw.text((p[0] - 14, p[1] - 18), c1, fill=(180, 40, 40), font=font_badge)
    if cs == "left":
        draw.text((c[0] - 22, c[1] - 18), cN, fill=(180, 40, 40), font=font_badge)
    elif cs == "top":
        draw.text((c[0] + 8, c[1] - 20), cN, fill=(180, 40, 40), font=font_badge)
    else:
        draw.text((c[0] + 8, c[1] - 18), cN, fill=(180, 40, 40), font=font_badge)


for parent, child, pcol, ccol, c1, cN in relationships:
    draw_relation(parent, child, pcol, ccol, c1, cN)

for name, meta in tables.items():
    x1, y1, x2, y2 = box_geom[name]
    draw.rounded_rectangle([x1 + 5, y1 + 5, x2 + 5, y2 + 5], radius=8, fill=(220, 226, 234))
    draw.rounded_rectangle([x1, y1, x2, y2], radius=8, outline=(11, 61, 92), width=2, fill=(255, 255, 255))
    draw.rounded_rectangle([x1, y1, x2, y1 + HEADER_H], radius=8, fill=(11, 61, 92))
    draw.rectangle([x1, y1 + HEADER_H - 10, x2, y1 + HEADER_H], fill=(11, 61, 92))
    draw.text((x1 + 14, y1 + 10), name, fill="white", font=font_header)

    for i, (col, typ, flags) in enumerate(meta["cols"]):
        cy = y1 + HEADER_H + i * ROW_H
        if "PK" in flags:
            draw.rectangle([x1 + 2, cy, x2 - 2, cy + ROW_H], fill=(255, 236, 150))
        elif "FK" in flags:
            draw.rectangle([x1 + 2, cy, x2 - 2, cy + ROW_H], fill=(190, 225, 255))
        draw.line([(x1 + 2, cy + ROW_H), (x2 - 2, cy + ROW_H)], fill=(230, 235, 240), width=1)
        label = f"{col}  :  {typ}"
        if flags:
            label = f"{label}   [{flags}]"
        draw.text((x1 + 12, cy + 6), label, fill=(25, 35, 45), font=font_col)

draw.text(
    (40, H - 50),
    "Note: Users is an authentication table with no foreign keys to clinical tables. "
    "Only the 8 live foreign-key relationships are shown. No invented relationships.",
    fill=(80, 90, 100),
    font=font_small,
)

png_path = os.path.join(OUT_DIR, "ERD.png")
pdf_path = os.path.join(OUT_DIR, "ERD.pdf")
img.save(png_path, "PNG", optimize=True)
img.convert("RGB").save(pdf_path, "PDF", resolution=100.0)
print("PNG", png_path, os.path.getsize(png_path))
print("PDF", pdf_path, os.path.getsize(pdf_path))
print("OK")
