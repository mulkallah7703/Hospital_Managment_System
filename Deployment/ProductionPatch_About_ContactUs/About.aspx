<%@ Page Title="About Our Team" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="About.aspx.cs" Inherits="HospitalManagementSystem.Web.About" %>

    <asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
        <style>
            .about-page {
                max-width: 1100px;
                margin: 0 auto;
            }

            .about-hero {
                background: linear-gradient(135deg, #0b3d5c 0%, #0d6efd 100%);
                color: #fff;
                border-radius: 12px;
                padding: 28px 28px 26px;
                margin-bottom: 28px;
                box-shadow: 0 8px 24px rgba(11, 61, 92, 0.18);
            }

            .about-hero h1 {
                margin: 0 0 8px 0;
                font-size: 30px;
                font-weight: bold;
                letter-spacing: 0.01em;
            }

            .about-hero .subtitle {
                margin: 0 0 10px 0;
                font-size: 18px;
                font-weight: 600;
                opacity: 0.95;
            }

            .about-hero .description {
                margin: 0;
                font-size: 14px;
                opacity: 0.9;
                max-width: 640px;
                line-height: 1.5;
            }

            .team-grid {
                display: grid;
                grid-template-columns: repeat(auto-fit, minmax(320px, 1fr));
                gap: 20px;
            }

            .member-card {
                background: #fff;
                border: 1px solid #d9e4ef;
                border-radius: 12px;
                box-shadow: 0 2px 10px rgba(13, 61, 92, 0.07);
                overflow: hidden;
                display: flex;
                flex-direction: column;
                transition: box-shadow 0.15s ease, transform 0.15s ease;
            }

            .member-card:hover {
                box-shadow: 0 8px 20px rgba(13, 61, 92, 0.12);
                transform: translateY(-2px);
            }

            .member-card-header {
                background: #f0f6fb;
                border-bottom: 1px solid #d9e4ef;
                padding: 16px 18px;
                display: flex;
                align-items: center;
                gap: 14px;
            }

            .member-number {
                flex-shrink: 0;
                width: 46px;
                height: 46px;
                border-radius: 10px;
                background: linear-gradient(135deg, #0b3d5c 0%, #0d6efd 100%);
                color: #fff;
                font-weight: bold;
                font-size: 16px;
                display: flex;
                align-items: center;
                justify-content: center;
                letter-spacing: 0.04em;
            }

            .member-card-header h2 {
                margin: 0;
                color: #0b3d5c;
                font-size: 17px;
                line-height: 1.35;
            }

            .member-card-body {
                padding: 6px 0 10px;
            }

            .member-section {
                padding: 14px 18px;
                border-bottom: 1px solid #eef2f6;
            }

            .member-section:last-child {
                border-bottom: none;
            }

            .member-section h3 {
                margin: 0 0 10px 0;
                font-size: 12px;
                text-transform: uppercase;
                letter-spacing: 0.06em;
                color: #0d6efd;
                font-weight: bold;
            }

            .info-row {
                display: grid;
                grid-template-columns: 140px 1fr;
                gap: 8px 12px;
                margin-bottom: 8px;
                font-size: 14px;
                line-height: 1.45;
            }

            .info-row:last-child {
                margin-bottom: 0;
            }

            .info-label {
                color: #5a6a7a;
                font-weight: bold;
            }

            .info-value {
                color: #243447;
                word-break: break-word;
            }

            .info-value a {
                color: #0d6efd;
                text-decoration: none;
                font-weight: 600;
            }

            .info-value a:hover {
                text-decoration: underline;
            }

            @media (max-width: 520px) {
                .about-hero {
                    padding: 22px 18px;
                }

                .about-hero h1 {
                    font-size: 24px;
                }

                .info-row {
                    grid-template-columns: 1fr;
                    gap: 2px;
                    margin-bottom: 10px;
                }
            }
        </style>

        <div class="about-page">
            <div class="about-hero">
                <h1>About Our Team</h1>
                <p class="subtitle">Hospital Management System</p>
                <p class="description">Cooperative Training Project — College of Computer Science and Engineering</p>
            </div>

            <div class="team-grid">

                <!-- TEAM MEMBER 01 -->
                <article class="member-card">
                    <div class="member-card-header">
                        <div class="member-number">01</div>
                        <h2>Malak Allah Ali Ali Mohammed Alsadi</h2>
                    </div>
                    <div class="member-card-body">
                        <section class="member-section">
                            <h3>Student Information</h3>
                            <div class="info-row">
                                <span class="info-label">Student Name</span>
                                <span class="info-value">Malak Allah Ali Ali Mohammed Alsadi</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Student ID</span>
                                <span class="info-value">2240007312</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Email</span>
                                <span class="info-value"><a
                                        href="mailto:malakallahalsadi6@gmail.com">malakallahalsadi6@gmail.com</a></span>
                            </div>
                        </section>
                        <section class="member-section">
                            <h3>Academic Information</h3>
                            <div class="info-row">
                                <span class="info-label">Department</span>
                                <span class="info-value">Software Engineering</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">College</span>
                                <span class="info-value">College of Computer Science and Engineering</span>
                            </div>
                        </section>
                        <section class="member-section">
                            <h3>Supervision</h3>
                            <div class="info-row">
                                <span class="info-label">Academic Supervisor</span>
                                <span class="info-value">Albraa Abo Ubaidah</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Academic Supervisor Email</span>
                                <span class="info-value"><a
                                        href="mailto:albarraa@uhb.edu.sa">albarraa@uhb.edu.sa</a></span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Field Supervisor</span>
                                <span class="info-value">Ali Humaid Al-Din</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Field Supervisor Email</span>
                                <span class="info-value"><a
                                        href="mailto:Ahameed@kfmc.med.sa">Ahameed@kfmc.med.sa</a></span>
                            </div>
                        </section>
                        <section class="member-section">
                            <h3>Training Information</h3>
                            <div class="info-row">
                                <span class="info-label">Training Period</span>
                                <span class="info-value">Six Months</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Academic Semester</span>
                                <span class="info-value">Summer Semester</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Academic Year</span>
                                <span class="info-value">2026</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Date</span>
                                <span class="info-value">August 12, 2026</span>
                            </div>
                        </section>
                    </div>
                </article>

                <!-- TEAM MEMBER 02 -->
                <article class="member-card">
                    <div class="member-card-header">
                        <div class="member-number">02</div>
                        <h2>Salih Masheal Al-Anazi</h2>
                    </div>
                    <div class="member-card-body">
                        <section class="member-section">
                            <h3>Student Information</h3>
                            <div class="info-row">
                                <span class="info-label">Student Name</span>
                                <span class="info-value">Salih Masheal Al-Anazi</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Student ID</span>
                                <span class="info-value">2240005622</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Email</span>
                                <span class="info-value"><a
                                        href="mailto:ksasalih11@gmail.com">ksasalih11@gmail.com</a></span>
                            </div>
                        </section>
                        <section class="member-section">
                            <h3>Academic Information</h3>
                            <div class="info-row">
                                <span class="info-label">Department</span>
                                <span class="info-value">Computer Science and Engineering</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">College</span>
                                <span class="info-value">College of Computer Science and Engineering</span>
                            </div>
                        </section>
                        <section class="member-section">
                            <h3>Supervision</h3>
                            <div class="info-row">
                                <span class="info-label">Academic Supervisor</span>
                                <span class="info-value">Mohammed Al-Suwaiket</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Academic Supervisor Email</span>
                                <span class="info-value"><a
                                        href="mailto:dr.alsuwaiket@uhb.edu.sa">dr.alsuwaiket@uhb.edu.sa</a></span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Field Supervisor</span>
                                <span class="info-value">Ali Humaid Al-Din</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Field Supervisor Email</span>
                                <span class="info-value"><a
                                        href="mailto:Ahameed@kfmc.med.sa">Ahameed@kfmc.med.sa</a></span>
                            </div>
                        </section>
                        <section class="member-section">
                            <h3>Training Information</h3>
                            <div class="info-row">
                                <span class="info-label">Training Period</span>
                                <span class="info-value">Six Months</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Academic Semester</span>
                                <span class="info-value">Summer Semester</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Academic Year</span>
                                <span class="info-value">2026</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Date</span>
                                <span class="info-value">August 12, 2026</span>
                            </div>
                        </section>
                    </div>
                </article>

                <!-- TEAM MEMBER 03 -->
                <article class="member-card">
                    <div class="member-card-header">
                        <div class="member-number">03</div>
                        <h2>Talal Ghathith AlShammari</h2>
                    </div>
                    <div class="member-card-body">
                        <section class="member-section">
                            <h3>Student Information</h3>
                            <div class="info-row">
                                <span class="info-label">Student Name</span>
                                <span class="info-value">Talal Ghathith AlShammari</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Student ID</span>
                                <span class="info-value">2240003720</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Email</span>
                                <span class="info-value"><a
                                        href="mailto:talal.178cx@gmail.com">talal.178cx@gmail.com</a></span>
                            </div>
                        </section>
                        <section class="member-section">
                            <h3>Academic Information</h3>
                            <div class="info-row">
                                <span class="info-label">Department</span>
                                <span class="info-value">Software Engineering</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">College</span>
                                <span class="info-value">College of Computer Science and Engineering</span>
                            </div>
                        </section>
                        <section class="member-section">
                            <h3>Supervision</h3>
                            <div class="info-row">
                                <span class="info-label">Academic Supervisor</span>
                                <span class="info-value">Albaraa Abuobieda</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Academic Supervisor Email</span>
                                <span class="info-value"><a
                                        href="mailto:albarraa@uhb.edu.sa">albarraa@uhb.edu.sa</a></span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Field Supervisor</span>
                                <span class="info-value">Ali Humaid Al-Din</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Field Supervisor Email</span>
                                <span class="info-value"><a
                                        href="mailto:Ahameed@kfmc.med.sa">Ahameed@kfmc.med.sa</a></span>
                            </div>
                        </section>
                        <section class="member-section">
                            <h3>Training Information</h3>
                            <div class="info-row">
                                <span class="info-label">Training Period</span>
                                <span class="info-value">Six Months</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Academic Semester</span>
                                <span class="info-value">Summer Semester</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Academic Year</span>
                                <span class="info-value">2026</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Date</span>
                                <span class="info-value">August 12, 2026</span>
                            </div>
                        </section>
                    </div>
                </article>

                <!-- TEAM MEMBER 04 -->
                <article class="member-card">
                    <div class="member-card-header">
                        <div class="member-number">04</div>
                        <h2>Mohammad Mubarak Al-Rashed</h2>
                    </div>
                    <div class="member-card-body">
                        <section class="member-section">
                            <h3>Student Information</h3>
                            <div class="info-row">
                                <span class="info-label">Student Name</span>
                                <span class="info-value">Mohammad Mubarak Al-Rashed</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Student ID</span>
                                <span class="info-value">2230001123</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Email</span>
                                <span class="info-value"><a
                                        href="mailto:Mhammd263@icloud.com">Mhammd263@icloud.com</a></span>
                            </div>
                        </section>
                        <section class="member-section">
                            <h3>Academic Information</h3>
                            <div class="info-row">
                                <span class="info-label">Department</span>
                                <span class="info-value">Software Engineering</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">College</span>
                                <span class="info-value">College of Computer Science and Engineering</span>
                            </div>
                        </section>
                        <section class="member-section">
                            <h3>Supervision</h3>
                            <div class="info-row">
                                <span class="info-label">Academic Supervisor</span>
                                <span class="info-value">Ahmed Harouna</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Academic Supervisor Email</span>
                                <span class="info-value"><a
                                        href="mailto:aaharuna@uhb.edu.sa">aaharuna@uhb.edu.sa</a></span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Field Supervisor</span>
                                <span class="info-value">Ali Humaid Al-Din</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Field Supervisor Email</span>
                                <span class="info-value"><a
                                        href="mailto:Ahameed@kfmc.med.sa">Ahameed@kfmc.med.sa</a></span>
                            </div>
                        </section>
                        <section class="member-section">
                            <h3>Training Information</h3>
                            <div class="info-row">
                                <span class="info-label">Training Period</span>
                                <span class="info-value">Six Months</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Academic Semester</span>
                                <span class="info-value">Summer Semester</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Academic Year</span>
                                <span class="info-value">2026</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Date</span>
                                <span class="info-value">August 12, 2026</span>
                            </div>
                        </section>
                    </div>
                </article>

                <!-- TEAM MEMBER 05 -->
                <article class="member-card">
                    <div class="member-card-header">
                        <div class="member-number">05</div>
                        <h2>Shatha Abdulrahman Alaqeel</h2>
                    </div>
                    <div class="member-card-body">
                        <section class="member-section">
                            <h3>Student Information</h3>
                            <div class="info-row">
                                <span class="info-label">Student Name</span>
                                <span class="info-value">Shatha Abdulrahman Alaqeel</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Student ID</span>
                                <span class="info-value">2241006720</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Email</span>
                                <span class="info-value"><a
                                        href="mailto:shdhyalq@gmail.com">shdhyalq@gmail.com</a></span>
                            </div>
                        </section>
                        <section class="member-section">
                            <h3>Academic Information</h3>
                            <div class="info-row">
                                <span class="info-label">Department</span>
                                <span class="info-value">Computer Science and Engineering</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">College</span>
                                <span class="info-value">College of Computer Science and Engineering</span>
                            </div>
                        </section>
                        <section class="member-section">
                            <h3>Supervision</h3>
                            <div class="info-row">
                                <span class="info-label">Academic Supervisor</span>
                                <span class="info-value">Aminat Ajibols</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Academic Supervisor Email</span>
                                <span class="info-value"><a
                                        href="mailto:aajibola@uhb.edu.sa">aajibola@uhb.edu.sa</a></span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Field Supervisor</span>
                                <span class="info-value">Ali Humaid Al-Din</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Field Supervisor Email</span>
                                <span class="info-value"><a
                                        href="mailto:Ahameed@kfmc.med.sa">Ahameed@kfmc.med.sa</a></span>
                            </div>
                        </section>
                        <section class="member-section">
                            <h3>Training Information</h3>
                            <div class="info-row">
                                <span class="info-label">Training Period</span>
                                <span class="info-value">Six Months</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Academic Semester</span>
                                <span class="info-value">Summer Semester</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Academic Year</span>
                                <span class="info-value">2026</span>
                            </div>
                            <div class="info-row">
                                <span class="info-label">Date</span>
                                <span class="info-value">August 11, 2026</span>
                            </div>
                        </section>
                    </div>
                </article>

            </div>
        </div>
    </asp:Content>