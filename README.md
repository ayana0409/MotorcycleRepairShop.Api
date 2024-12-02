### README: Motorcycle Repair Shop API  

---

#### **Project Overview**  
The **Motorcycle Repair Shop API** is a robust web application designed to streamline and enhance the management of motorcycle repair services. It enables efficient operations for administrators, managers, staff, and customers with comprehensive features tailored to the repair shop's needs.  

---

#### **Features**  

1. **Account Management**  
   - **Role:** Admin  
   - Add, edit, and manage system accounts for managers and staff.  

2. **Brand Management**  
   - **Role:** Manager  
   - Perform CRUD operations on motorcycle brand information.  

3. **Vehicle Management**  
   - **Role:** Manager  
   - Add, edit, delete, and search vehicle records.  

4. **Parts Management**  
   - **Role:** Manager  
   - Manage spare parts inventory, including adding, editing, and removing parts.  

5. **Service Management**  
   - **Role:** Manager  
   - Manage services offered, including inventory control and CRUD operations.  

6. **Problem Management**  
   - **Role:** Manager  
   - Manage common motorcycle issues, including adding and editing details.  

7. **Service Request Management**  
   - **Role:** Staff  
   - Handle repair requests, update details, and create payment records.  

8. **Statistics and Reporting**  
   - **Role:** Manager  
   - View detailed statistics on revenue, sales, and inventory.  

9. **Customer Features**  
   - **Service Request Creation:** Customers can request services online.  
   - **Status Inquiry:** Customers can view their repair request status.  
   - **Pricing Inquiry:** Check service and spare part prices.  

---

#### **Technologies Used**  

- **Frontend:** //  
- **Backend:** ASP.NET Core WebAPI (C#)  
- **Database:** MySQL  

---

#### **Installation**  

1. **Clone the Repository:**  
   ```bash
   git clone https://github.com/ayana0409/MotorcycleRepairShop.Api
   cd motorcycle-repair-shop-api
   ```  

2. **Setup the Database:**  
   - Ensure MySQL is installed and running.  
   - Create a database named `MotorcycleRepairDB`.  

3. **Configure the Connection String:**  
   Update `appsettings.json`:  
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnectionString": "Server=productdb;Database=MotorcycleRepairDB;Uid=root;Pwd=Passw0rd!"
     }
   }
   ```  

4. **Run Migrations:**  
   ```bash
   dotnet ef database update
   ```  

5. **Run the Application:**  
   ```bash
   dotnet run
   ```  

6. **Access the API:**  
   - Localhost: `https://localhost:7073/swagger/index.html`  
   - Public URL: `http://26.139.159.129:5000/swagger/index.html`  

---

#### **Usage**  

1. **Authentication:**  
   - The system uses role-based access control (RBAC). Obtain a token for authentication.  

2. **API Endpoints:**  
   Use the Swagger UI to explore available endpoints.  

3. **Development Environment:**  
   - Visual Studio 2022 or higher  
   - .NET 8.0 SDK  

---

#### **Future Enhancements**  

- **Automated Notifications:** Notify customers about status updates via email/SMS.  
- **Mobile App Support:** Extend the system to include mobile applications.  
- **Payment Integrations:** Enhance support for PayPal and VNPay for seamless transactions.  

---

#### **Contributing**  

1. Fork the repository.  
2. Create a feature branch:  
   ```bash
   git checkout -b feature-name
   ```  
3. Commit your changes:  
   ```bash
   git commit -m "Add new feature"
   ```  
4. Push to the branch:  
   ```bash
   git push origin feature-name
   ```  
5. Create a pull request.  

---

#### **Docker**  
1. Path:  MotorcycleRepairShop.Api
2. cmd:  
```
   docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --remove-orphans --build
```

---

#### **License**  

This project is licensed under the MIT License.  

---

#### **Contact**  

For inquiries or support, please contact:  
- **Email:** duongdoanthuan2003@gmail.com  

Enjoy using the **Motorcycle Repair Shop API**! 🚀  
