### **Project Plan: AI-Powered Resume-to-Portfolio Converter (Blazor Server App)**

---

#### **Project Overview**
A no-login Blazor Server application where users upload resumes (PDF/DOC) to generate a single-page portfolio. Includes feedback and donation screens, SQLite database, and AI-driven content parsing.

---

### **Phase 1: Setup & Infrastructure (Week 1-2)**
#### **Tasks**
1. **Project Setup**
   - Create Blazor Server App (.NET 6+).
   - Install NuGet packages:
     - `Syncfusion.Blazor` (UI components)
     - `IronPdf`, `DocX` (file processing)
     - `Microsoft.EntityFrameworkCore.Sqlite` (database)
     - `OpenAI` (text analysis)
   - Configure dependency injection for services.

2. **Database Design**
   - SQLite schema with tables:
     - **Resumes**: `Id` (GUID), `FileName`, `FileContent`, `CreatedAt`, `ParsedJson`, `PortfolioHtml`
     - **Feedbacks**: `Id`, `Name`, `Email`, `Message`, `Rating` (1-5), `CreatedAt`
     - **Donations**: `Id`, `Amount`, `Email`, `TransactionId`, `CreatedAt`
   - Use EF Core Code-First migrations.

3. **Basic UI Layout**
   - Create `MainLayout.razor` with navigation menu (Home, Feedback, Donate).
   - Style using `Syncfusion` themes or custom CSS.

---

### **Phase 2: Core Functionality (Week 3-4)**
#### **Tasks**
1. **File Upload Component**
   - Razor component with `<InputFile>` accepting PDF/DOC/DOCX.
   - Client-side validation (file size ≤5MB, correct format).
   - Server-side processing:
     - Convert PDF to text with `IronPdf`.
     - Extract DOC/DOCX text with `DocX`.

2. **AI-Powered Resume Parsing**
   - Use OpenAI API to analyze extracted text:
     - Prompt: "Extract name, summary, skills, experience, education, and contact info from this resume."
   - Map parsed data to a `ResumeModel` class.

3. **Portfolio Generator**
   - Razor component template with sections (Summary, Skills, Experience).
   - Dynamic data binding using parsed `ResumeModel`.
   - Add "Download as HTML/PDF" button (use `IronPdf`).

---

### **Phase 3: Feedback & Donation Screens (Week 5)**
#### **Tasks**
1. **Feedback Screen**
   - Form with fields: Name (optional), Email (optional), Rating (dropdown), Message.
   - Submit to `FeedbackService` (stores in SQLite).

2. **Donation Screen**
   - Integrate PayPal/Stripe payment links (no backend processing).
   - Simple UI with "Support Us" message and redirect buttons.

---

### **Phase 4: Testing & Debugging (Week 6)**
#### **Tasks**
1. **Unit Tests**
   - Test file conversion (mock PDF/DOC files).
   - Validate AI parsing logic with sample resumes.
   - Test database CRUD operations.

2. **UI Testing**
   - Test upload, error handling, and mobile responsiveness.
   - Use Selenium for automated browser tests.

3. **User Testing**
   - Recruit 5-10 users to test end-to-end flow.
   - Fix issues (e.g., slow AI response, layout bugs).

---

### **Phase 5: Deployment (Week 7)**
#### **Tasks**
1. **Azure Hosting**
   - Deploy to Azure App Service (Windows/Linux).
   - Configure SQLite database path (`appsettings.json`).

2. **CI/CD Pipeline**
   - Set up GitHub Actions for automated deployment.
   - Add scripts for EF Core migrations.

3. **Monitoring**
   - Integrate Azure Application Insights for logging/analytics.

---

### **Phase 6: Post-Launch (Week 8+)**
#### **Tasks**
1. **User Feedback Loop**
   - Monitor feedback for common issues (e.g., parsing errors).

2. **Maintenance**
   - Schedule monthly database cleanup (delete records >30 days old).
   - Update OpenAI prompts for better accuracy.

---

### **Timeline**
| Phase       | Duration | Deliverables                          |
|-------------|----------|----------------------------------------|
| Setup       | 2 weeks  | Project repo, database, basic UI      |
| Core Features | 2 weeks | Upload, AI parsing, portfolio generator |
| Feedback & Donate | 1 week | Functional screens, payment links |
| Testing     | 1 week   | Test reports, bug fixes                |
| Deployment  | 1 week   | Live Azure app, CI/CD pipeline         |
| Post-Launch | Ongoing  | Analytics, updates                     |

---

### **Risk Mitigation**
- **Large File Uploads**: Restrict to 5MB; compress files if needed.
- **AI Errors**: Fallback to manual text parsing for key sections.
- **Payment Failures**: Use trusted providers (PayPal/Stripe) with HTTPS.

---

### **Budget**
- **Development**: $5k (2 months part-time)
- **Hosting**: $50/month (Azure B1 tier)
- **OpenAI API**: $100/month (based on usage)

---
