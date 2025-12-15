# Spectrum API - User Authentication & Department Management

## Overview
This API provides user authentication and management functionality with JWT token-based authentication and BCrypt password hashing, along with department and role management capabilities.

## Recent DB Migration Notes
The `tblVisitorEntry` table schema was changed:
- Renamed column `VisitorEntry_isApproval` -> `VisitorEntryAdmin_isApproval` (admin approval flag)
- Added column `VisitorEntryuser_isApproval` BIT NOT NULL DEFAULT 0 (user approval flag)

API adjustments:
- Create/Update endpoints for visitor entries accept `VisitorEntryAdmin_isApproval` and `VisitorEntryuser_isApproval`.
- For backward compatibility the API still accepts `VisitorEntry_isApproval` (combined flag). If `VisitorEntryAdmin_isApproval`/`VisitorEntryuser_isApproval` are not provided, the combined flag is used to set both.

## Features
- User Registration
- User Login with JWT Token
- User Profile Update
- Password Hashing using BCrypt
- JWT Authentication
- Get User Details
- Get All Users
- Department Management (CRUD operations)
- Role Management (CRUD operations)

## Tech Stack
- .NET 10
- Dapper (Database access)
- BCrypt.Net-Next (Password hashing)
- JWT Bearer Authentication
- SQL Server

## Database Configuration
Connection string is configured in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=Sanku;Database=Spectrum;User Id=sa;Password=jmn!@#;TrustServerCertificate=True;"
}
```

## API Endpoints

### Authentication APIs

#### 1. Register User
**POST** `/api/auth/register`

Register a new user with hashed password.

**Request Body:**
```json
{
  "username": "john_doe",
  "password": "SecurePass123",
  "u_Name": "John Doe",
  "u_Mobile": "1234567890",
  "u_Email": "john@example.com",
  "u_Address": "123 Main St",
  "u_RoleId": 1,
  "u_DepartmentID": 1,
  "u_ReportingToId": null
}
```

**Response:**
```json
{
  "message": "User registered successfully",
  "userId": 1
}
```

---

#### 2. Login
**POST** `/api/auth/login`

Authenticate user and receive JWT token.

**Request Body:**
```json
{
  "username": "john_doe",
  "password": "SecurePass123"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "userId": 1,
  "username": "john_doe",
  "u_Name": "John Doe",
  "u_Email": "john@example.com",
  "u_RoleId": 1,
  "u_DepartmentID": 1
}
```

---

#### 3. Update Current User
**PUT** `/api/auth/update`

Update the currently logged-in user's profile (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Request Body:**
```json
{
  "u_Name": "John Updated",
  "u_Mobile": "9876543210",
  "u_Email": "john.updated@example.com",
  "u_Address": "456 New St",
  "u_RoleId": 2,
  "u_DepartmentID": 2,
  "u_ReportingToId": 5,
  "newPassword": "NewSecurePass456"
}
```

**Response:**
```json
{
  "message": "User updated successfully",
  "user": {
    "userId": 1,
    "username": "john_doe",
    "u_Name": "John Updated",
    "u_Mobile": "9876543210",
    "u_Email": "john.updated@example.com",
    "u_Address": "456 New St",
    "u_RoleId": 2,
    "u_DepartmentID": 2,
    "u_ReportingToId": 5,
    "createdDate": "2024-01-15T10:30:00",
    "updatedDate": "2024-01-20T14:45:00"
  }
}
```

---

#### 4. Update Specific User
**PUT** `/api/auth/update/{userId}`

Update a specific user by ID (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Request Body:** (Same as Update Current User)

---

#### 5. Get Current User
**GET** `/api/auth/me`

Get the currently logged-in user's details (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Response:**
```json
{
  "userId": 1,
  "username": "john_doe",
  "u_Name": "John Doe",
  "u_Mobile": "1234567890",
  "u_Email": "john@example.com",
  "u_Address": "123 Main St",
  "u_RoleId": 1,
  "u_DepartmentID": 1,
  "u_ReportingToId": null,
  "createdDate": "2024-01-15T10:30:00",
  "updatedDate": null
}
```

---

#### 6. Get User By ID
**GET** `/api/auth/{userId}`

Get a specific user's details by ID (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Response:** (Same as Get Current User)

---

#### 7. Get All Users
**GET** `/api/auth/all`

Get all users in the system (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Response:**
```json
[
  {
    "userId": 1,
    "username": "john_doe",
    "u_Name": "John Doe",
    ...
  },
  {
    "userId": 2,
    "username": "jane_smith",
    "u_Name": "Jane Smith",
    ...
  }
]
```

---

#### 8. Delete User
**DELETE** `/api/auth/{userId}`

Delete a user by ID (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Success Response (200 OK):**
```json
{
  "message": "User deleted successfully"
}
```

**Error Response (404 Not Found):**
```json
{
  "message": "User not found"
}
```

---

### Department APIs

#### 1. Get All Departments
**GET** `/api/department/all`

Get all departments in the system (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Response:**
```json
[
  {
    "departmentId": 1,
    "department": "IT",
    "createdDate": "2024-01-15T10:00:00",
    "updatedDate": null,
    "isActive": true
  },
  {
    "departmentId": 2,
    "department": "HR",
    "createdDate": "2024-01-15T10:05:00",
    "updatedDate": "2024-01-20T14:30:00",
    "isActive": true
  }
]
```

---

#### 2. Get Active Departments
**GET** `/api/department/active`

Get only active departments (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Response:**
```json
[
  {
    "departmentId": 1,
    "department": "IT",
    "createdDate": "2024-01-15T10:00:00",
    "updatedDate": null,
    "isActive": true
  },
  {
    "departmentId": 2,
    "department": "HR",
    "createdDate": "2024-01-15T10:05:00",
    "updatedDate": null,
    "isActive": true
  }
]
```

---

#### 3. Get Department By ID
**GET** `/api/department/{departmentId}`

Get a specific department by ID (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Response:**
```json
{
  "departmentId": 1,
  "department": "IT",
  "createdDate": "2024-01-15T10:00:00",
  "updatedDate": null,
  "isActive": true
}
```

---

#### 4. Create Department
**POST** `/api/department`

Create a new department (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Request Body:**
```json
{
  "departmentName": "HOD"
}
```

**Response:**
```json
{
  "message": "Department created successfully",
  "departmentId": 3
}
```

---

#### 5. Update Department
**PUT** `/api/department/{departmentId}`

Update an existing department (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Request Body:**
```json
{
  "department": "Finance & Accounts",
  "isActive": true
}
```

**Response:**
```json
{
  "message": "Department updated successfully",
  "department": {
    "departmentId": 3,
    "department": "Finance & Accounts",
    "createdDate": "2024-01-20T10:00:00",
    "updatedDate": "2024-01-20T15:30:00",
    "isActive": true
  }
}
```

---

#### 6. Delete Department
**DELETE** `/api/department/{departmentId}`

Delete a department by ID (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Success Response (200 OK):**
```json
{
  "message": "Department deleted successfully"
}
```

**Error Response (404 Not Found):**
```json
{
  "message": "Department not found"
}
```

---

### Role API

**Note:** All Role endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer {your_jwt_token}
```

#### 1. Get All Roles
**GET** `/api/role`

Get all roles in the system (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Response:**
```json
{
  "message": "Roles retrieved successfully",
  "roles": [
    {
      "roleId": 1,
      "roleName": "Admin",
      "createdDate": "2024-01-15T10:30:00",
      "updatedDate": "2024-01-20T14:45:00"
    },
    {
      "roleId": 2,
      "roleName": "Manager",
      "createdDate": "2024-01-16T09:00:00",
      "updatedDate": null
    }
  ]
}
```

---

#### 2. Get Role by ID
**GET** `/api/role/{roleId}`

Get a specific role by ID (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Response:**
```json
{
  "message": "Role retrieved successfully",
  "role": {
    "roleId": 1,
    "roleName": "Admin",
    "createdDate": "2024-01-15T10:30:00",
    "updatedDate": "2024-01-20T14:45:00"
  }
}
```

---

#### 3. Create Role
**POST** `/api/role`

Create a new role (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Request Body:**
```json
{
  "roleName": "Supervisor"
}
```

**Response:**
```json
{
  "message": "Role created successfully",
  "roleId": 4
}
```

---

#### 4. Update Role
**PUT** `/api/role/{roleId}`

Update an existing role by ID (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Request Body:**
```json
{
  "roleName": "System Administrator"
}
```

**Response:**
```json
{
  "message": "Role updated successfully",
  "role": {
    "roleId": 1,
    "roleName": "System Administrator",
    "createdDate": "2024-01-15T10:30:00",
    "updatedDate": "2024-01-25T16:20:00"
  }
}
```

---

#### 5. Delete Role
**DELETE** `/api/role/{roleId}`

Delete a role by ID (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Response:**
```json
{
  "message": "Role deleted successfully"
}
```

---

### Location API

**Note:** All Location endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer {your_jwt_token}
```

#### 1. Get All Locations
**GET** `/api/location`

Get all locations in the system (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Response:**
```json
{
  "message": "Locations retrieved successfully",
  "locations": [
    {
      "locationId": 1,
      "locationName": "Head Office",
      "description": "Main office",
      "createdDate": "2025-11-28T10:34:29.633",
      "updatedDate": null,
      "isActive": true,
      "createdBy": 1
    },
    {
      "locationId": 2,
      "locationName": "Branch Office",
      "description": "Branch office description",
      "createdDate": "2025-11-28T10:35:00",
      "updatedDate": null,
      "isActive": true,
      "createdBy": 1
    }
  ]
}
```

---

#### 2. Get Location by ID
**GET** `/api/location/{locationId}`

Get a specific location by ID (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Response:**
```json
{
  "message": "Location retrieved successfully",
  "location": {
    "locationId": 1,
    "locationName": "Head Office",
    "description": "Main office",
    "createdDate": "2025-11-28T10:34:29.633",
    "updatedDate": null,
    "isActive": true,
    "createdBy": 1
  }
}
```

**Error Response (404 Not Found):**
```json
{
  "message": "Location not found"
}
```

---

#### 3. Create Location
**POST** `/api/location`

Create a new location (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Request Body:**
```json
{
  "locationName": "Branch Office",
  "description": "Branch office description",
  "isActive": true
}
```

**Response:**
```json
{
  "message": "Location created successfully",
  "locationId": 2
}
```

**Error Response (400 Bad Request):**
```json
{
  "message": "Location name already exists"
}
```

---

#### 4. Update Location
**PUT** `/api/location/{locationId}`

Update an existing location by ID (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Request Body:**
```json
{
  "locationName": "Regional Office",
  "description": "Updated description",
  "isActive": true
}
```

**Response:**
```json
{
  "message": "Location updated successfully",
  "location": {
    "locationId": 2,
    "locationName": "Regional Office",
    "description": "Updated description",
    "createdDate": "2025-11-28T10:34:29.633",
    "updatedDate": "2025-11-28T12:00:00",
    "isActive": true,
    "createdBy": 1
  }
}
```

**Error Response (400 Bad Request):**
```json
{
  "message": "Location name already exists"
}
```

---

#### 5. Delete Location
**DELETE** `/api/location/{locationId}`

Delete a location by ID (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Success Response (200 OK):**
```json
{
  "message": "Location deleted successfully"
}
```

**Error Response (404 Not Found):**
```json
{
  "message": "Location not found"
}
```

---

### Vendor API

**Note:** All Vendor endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer {your_jwt_token}
```

#### Get All Vendors
**GET** `/api/vendor`

Get all vendors in the system (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Response:**
```json
{
  "message": "Vendors retrieved successfully",
  "vendors": [
    {
      "vendorID": 1,
      "vendorCode": "V001",
      "vendorName": "ABC Supplies",
      "vendorMobile": "1234567890",
      "idProofType": "PAN",
      "idProof": "ABCDE1234F",
      "vendorAddress": "123 Market St",
      "company": "ABC Pvt Ltd",
      "createdDate": "2025-11-28T10:34:29.633",
      "updatedDate": null,
      "isActive": true
    }
  ]
}
```

---

#### Get Vendor by ID
**GET** `/api/vendor/{vendorId}`

Get a specific vendor by ID (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Response:**
```json
{
  "message": "Vendor retrieved successfully",
  "vendor": {
    "vendorID": 1,
    "vendorCode": "V001",
    "vendorName": "ABC Supplies",
    "vendorMobile": "1234567890",
    "idProofType": "PAN",
    "idProof": "ABCDE1234F",
    "vendorAddress": "123 Market St",
    "company": "ABC Pvt Ltd",
    "createdDate": "2025-11-28T10:34:29.633",
    "updatedDate": null,
    "isActive": true
  }
}
```

---

#### Create Vendor
**POST** `/api/vendor`

Create a new vendor (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Request Body:**
```json
{
  "vendorCode": "V002",
  "vendorName": "XYZ Traders",
  "vendorMobile": "0987654321",
  "idProofType": "Aadhar",
  "idProof": "1234-5678-9012",
  "vendorAddress": "456 Market St",
  "company": "XYZ Co",
  "isActive": true
}
```

**Response:**
```json
{
  "message": "Vendor created successfully",
  "vendorId": 2
}
```

---

#### Update Vendor
**PUT** `/api/vendor/{vendorId}`

Update an existing vendor by ID (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Request Body:**
```json
{
  "vendorName": "XYZ Traders Pvt Ltd",
  "vendorMobile": "0987654321",
  "isActive": true
}
```

**Response:**
```json
{
  "message": "Vendor updated successfully",
  "vendor": {
    "vendorID": 2,
    "vendorCode": "V002",
    "vendorName": "XYZ Traders Pvt Ltd",
    "vendorMobile": "0987654321",
    "idProofType": "Aadhar",
    "idProof": "1234-5678-9012",
    "vendorAddress": "456 Market St",
    "company": "XYZ Co",
    "createdDate": "2025-11-28T10:34:29.633",
    "updatedDate": "2025-11-28T12:00:00",
    "isActive": true
  }
}
```

---

#### Delete Vendor
**DELETE** `/api/vendor/{vendorId}`

Delete a vendor by ID (requires JWT token).

**Headers:**
```
Authorization: Bearer {your_jwt_token}
```

**Response:**
```json
{
  "message": "Vendor deleted successfully"
}
```

---

### Vendor Appointment API

**Note:** All VendorAppointment endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer {your_jwt_token}
```

#### Get All Appointments
**GET** `/api/vendorappointment`

#### Get Appointment by ID
**GET** `/api/vendorappointment/{id}`

#### Create Appointment
**POST** `/api/vendorappointment`

**Request Body:**
```json
{
  "vendorA_VendorID": 1,
  "vendorA_Getpass": "PASS123",
  "vendorA_FromDate": "2025-12-01T10:00:00",
  "vendorA_ToDate": "2025-12-01T12:00:00",
  "vendorA_VehicleNO": "MH12AB1234",
  "vendorA_IdProofType": "Aadhar",
  "vendorA_IdProofNo": "1234-5678-9012",
  "vendorA_UserId": 1
}
```

#### Update Appointment
**PUT** `/api/vendorappointment/{id}`

#### Delete Appointment
**DELETE** `/api/vendorappointment/{id}`

Responses follow the same pattern as other APIs in this README.

---

### Vendor Employee API

**Note:** All Vendor Employee endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer {your_jwt_token}
```

#### Get All Vendor Employees
**GET** `/api/vendoremp`

**Success Response (200 OK):**
```json
{
  "message": "Employees retrieved successfully",
  "employees": [
    {
      "vendorEmpId": 1,
      "vendorEmp_VendorID": 1,
      "vendorEmp_Name": "John Vendor",
      "vendorEmp_IDProofType": "Aadhar",
      "vendorEmp_IDProofNo": "1234-5678-9012",
      "vendorEmp_mobile": "9876543210",
      "vendorEmp_VenderAID": 1,
      "createdDate": "2025-11-28T10:34:29.633",
      "updatedDate": null
    }
  ]
}
```

---

#### Get Vendor Employee by ID
**GET** `/api/vendoremp/{id}`

**Success Response (200 OK):**
```json
{
  "message": "Employee retrieved successfully",
  "employee": {
    "vendorEmpId": 1,
    "vendorEmp_VendorID": 1,
    "vendorEmp_Name": "John Vendor",
    "vendorEmp_IDProofType": "Aadhar",
    "vendorEmp_IDProofNo": "1234-5678-9012",
    "vendorEmp_mobile": "9876543210",
    "vendorEmp_VenderAID": 1,
    "createdDate": "2025-11-28T10:34:29.633",
    "updatedDate": null
  }
}
```

**Error Response (404 Not Found):**
```json
{
  "message": "Employee not found"
}
```

---

#### Create Vendor Employee
**POST** `/api/vendoremp`

**Request Body:**
```json
{
  "vendorEmp_VendorID": 1,
  "vendorEmp_Name": "John Vendor",
  "vendorEmp_IDProofType": "Aadhar",
  "vendorEmp_IDProofNo": "1234-5678-9012",
  "vendorEmp_mobile": "9876543210",
  "vendorEmp_VenderAID": 1
}
```

**Success Response (201 Created):**
```json
{
  "message": "Employee created successfully",
  "empId": 2
}
```

---

#### Update Vendor Employee
**PUT** `/api/vendoremp/{id}`

**Request Body:**
```json
{
  "vendorEmp_Name": "John Vendor Updated",
  "vendorEmp_mobile": "0123456789"
}
```

**Success Response (200 OK):**
```json
{
  "message": "Employee updated successfully",
  "employee": {
    "vendorEmpId": 2,
    "vendorEmp_VendorID": 1,
    "vendorEmp_Name": "John Vendor Updated",
    "vendorEmp_mobile": "0123456789",
    "vendorEmp_VenderAID": 1,
    "createdDate": "2025-11-28T10:34:29.633",
    "updatedDate": "2025-11-28T12:00:00"
  }
}
```

---

#### Delete Vendor Employee
**DELETE** `/api/vendoremp/{id}`

**Success Response (200 OK):**
```json
{
  "message": "Employee deleted successfully"
}
```

**Error Response (404 Not Found):**
```json
{
  "message": "Employee not found"
}
```

---

### Visitor API

**Note:** All Visitor endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer {your_jwt_token}
```

#### Get All Visitors
**GET** `/api/visitor`

#### Get Visitor by ID
**GET** `/api/visitor/{id}`

#### Create Visitor
**POST** `/api/visitor`

**Request Body (example):**
```json
{
  "visitor_Name": "John Doe",
  "visitor_mobile": "9876543210",
  "visitor_Address": "123 Main St",
  "visitor_CompanyName": "ABC Corp",
  "visitor_Purposeofvisit": "Meeting",
  "visitor_Idprooftype": "Aadhar",
  "visitor_idproofno": "1234-5678-9012",
  "visitor_MeetingDate": "2025-12-01T10:00:00"
}
```

#### Update Visitor
**PUT** `/api/visitor/{id}`

#### Delete Visitor
**DELETE** `/api/visitor/{id}`

Responses follow the same patterns as other APIs in this README.

---

### Visitor Entry API

**Note:** All VisitorEntry endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer {your_jwt_token}
```

#### Get All Visitor Entries
**GET** `/api/visitorentry`

#### Get Visitor Entry by ID
**GET** `/api/visitorentry/{id}`

#### Create Visitor Entry
**POST** `/api/visitorentry`

**Request Body (example):**
```json
{
  "VisitorEntry_visitorId": 1,
  "VisitorEntry_Gatepass": "GP12345",
  "VisitorEntry_Vehicletype": "Car",
  "VisitorEntry_Vehicleno": "MH12AB1234",
  "VisitorEntry_Date": "2025-12-01T10:00:00",
  "VisitorEntry_Intime": "2025-12-01T10:05:00",
  "VisitorEntry_Outtime": null,
  "VisitorEntry_Userid": 1,
  "VisitorEntryAdmin_isApproval": false,
  "VisitorEntryuser_isApproval": false,
  "VisitorEntry_Remark": "Meeting",
  "VisitorEntry_isCanteen": false,
  "VisitorEntry_isStay": false
}
```

#### Update Visitor Entry
**PUT** `/api/visitorentry/{id}`

#### Delete Visitor Entry
**DELETE** `/api/visitorentry/{id}`

Responses follow the same patterns as other APIs in this README.

---

### Approval Status API

**Note:** All ApprovalStatus endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer {your_jwt_token}
```

#### Get All Approval Statuses
**GET** `/api/approvalstatus`

#### Get Approval Status by ID
**GET** `/api/approvalstatus/{id}`

#### Create Approval Status
**POST** `/api/approvalstatus`

**Request Body (example):**
```json
{
  "approvalStatus_Gatepass": "GP12345",
  "approvalStatus_VisitorEntryId": 1,
  "approvalStatus_TransactionDate": "2025-12-01T10:00:00",
  "approvalStatus_ApprovalDate": "2025-12-01T11:00:00",
  "approvalStatus_ApprovalStatus": true,
  "approvalStatus_Remark": "Approved",
  "approvalStatus_ApprovalPersonRoleID": 2,
  "approvalStatus_UserId": 1
}
```

#### Update Approval Status
**PUT** `/api/approvalstatus/{id}`

#### Delete Approval Status
**DELETE** `/api/approvalstatus/{id}`

Responses follow the same patterns as other APIs in this README.

---

## Security Features

### Password Hashing
- Passwords are hashed using BCrypt before storing in the database
- BCrypt automatically handles salting and multiple hashing rounds
- Verification is done using BCrypt's built-in verify method

### JWT Authentication
- JWT tokens are issued upon successful login
- Tokens expire after 1440 minutes (24 hours) by default
- Token contains user claims: UserId, Username, U_Name, Email, RoleId, DepartmentID
- Protected endpoints require valid JWT token in Authorization header

### Token Configuration
In `appsettings.json`:
```json
"Jwt": {
  "Key": "A9f8#3dF_lK92@!xP0qZ88*LmQw7VrTnB4",
  "Issuer": "SpectrumApi",
  "Audience": "SpectrumClients",
  "ExpiresMinutes": 1440
}
```

## How to Use

### Authentication Examples

#### 1. Register a new user
```bash
curl -X POST https://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"username":"john_doe","password":"SecurePass123","u_Name":"John Doe","u_Mobile":"1234567890","u_Email":"john@example.com","u_Address":"123 Main St","u_RoleId":1,"u_DepartmentID":1,"u_ReportingToId":null}'
```

#### 2. Login to get JWT token
```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"john_doe","password":"SecurePass123"}'
```

#### 3. Use token for authenticated requests
```bash
curl -X GET https://localhost:5001/api/auth/me \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 4. Update user profile
```bash
curl -X PUT https://localhost:5001/api/auth/update \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"u_Name":"John Updated","u_Email":"john.updated@example.com","u_Mobile":"9876543210"}'
```

### Department Examples

#### 1. Get all departments
```bash
curl -X GET https://localhost:5001/api/department/all \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 2. Get active departments
```bash
curl -X GET https://localhost:5001/api/department/active \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 3. Create a new department
```bash
curl -X POST https://localhost:5001/api/department \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"department":"Finance"}'
```

#### 4. Update a department
```bash
curl -X PUT https://localhost:5001/api/department/3 \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"department":"Finance & Accounts","isActive":true}'
```

#### 5. Get department by ID
```bash
curl -X GET https://localhost:5001/api/department/1 \
  -H "Authorization: Bearer {your_jwt_token}"
```

### Role Examples

#### 1. Get all roles
```bash
curl -X GET https://localhost:5001/api/role \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 2. Get role by ID
```bash
curl -X GET https://localhost:5001/api/role/1 \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 3. Create a new role
```bash
curl -X POST https://localhost:5001/api/role \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"roleName":"Supervisor"}'
```

#### 4. Update a role
```bash
curl -X PUT https://localhost:5001/api/role/1 \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"roleName":"System Administrator"}'
```

#### 5. Delete a role
```bash
curl -X DELETE https://localhost:5001/api/role/4 \
  -H "Authorization: Bearer {your_jwt_token}"
```

### Location Examples

#### 1. Get all locations
```bash
curl -X GET https://localhost:5001/api/location \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 2. Get location by ID
```bash
curl -X GET https://localhost:5001/api/location/1 \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 3. Create a new location
```bash
curl -X POST https://localhost:5001/api/location \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"locationName":"Branch Office","description":"Branch office description","isActive":true}'
```

#### 4. Update a location
```bash
curl -X PUT https://localhost:5001/api/location/1 \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"locationName":"Regional Office","description":"Updated description","isActive":true}'
```

#### 5. Delete a location
```bash
curl -X DELETE https://localhost:5001/api/location/2 \
  -H "Authorization: Bearer {your_jwt_token}"
```

### Vendor Examples

#### 1. Get all vendors
```bash
curl -X GET https://localhost:5001/api/vendor \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 2. Get vendor by ID
```bash
curl -X GET https://localhost:5001/api/vendor/1 \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 3. Create a new vendor
```bash
curl -X POST https://localhost:5001/api/vendor \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"vendorCode":"V002","vendorName":"XYZ Traders","vendorMobile":"0987654321","idProofType":"Aadhar","idProof":"1234-5678-9012","vendorAddress":"456 Market St","company":"XYZ Co","isActive":true}'
```

#### 4. Update a vendor
```bash
curl -X PUT https://localhost:5001/api/vendor/1 \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"vendorName":"XYZ Traders Pvt Ltd","vendorMobile":"0987654321","isActive":true}'
```

#### 5. Delete a vendor
```bash
curl -X DELETE https://localhost:5001/api/vendor/2 \
  -H "Authorization: Bearer {your_jwt_token}"
```

### Vendor Appointment Examples

#### 1. Get all appointments
```bash
curl -X GET https://localhost:5001/api/vendorappointment \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 2. Get appointment by ID
```bash
curl -X GET https://localhost:5001/api/vendorappointment/1 \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 3. Create a new appointment
```bash
curl -X POST https://localhost:5001/api/vendorappointment \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"vendorA_VendorID":1,"vendorA_Getpass":"PASS123","vendorA_FromDate":"2025-12-01T10:00:00","vendorA_ToDate":"2025-12-01T12:00:00","vendorA_VehicleNO":"MH12AB1234","vendorA_IdProofType":"Aadhar","vendorA_IdProofNo":"1234-5678-9012","vendorA_UserId":1}'
```

#### 4. Update an appointment
```bash
curl -X PUT https://localhost:5001/api/vendorappointment/1 \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"vendorA_ToDate":"2025-12-01T13:00:00"}'
```

#### 5. Delete an appointment
```bash
curl -X DELETE https://localhost:5001/api/vendorappointment/1 \
  -H "Authorization: Bearer {your_jwt_token}"
```

### Vendor Employee Examples

#### 1. Get all vendor employees
```bash
curl -X GET https://localhost:5001/api/vendoremp \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 2. Get vendor employee by ID
```bash
curl -X GET https://localhost:5001/api/vendoremp/1 \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 3. Create a new vendor employee
```bash
curl -X POST https://localhost:5001/api/vendoremp \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"vendorEmp_VendorID":1,"vendorEmp_Name":"John Vendor","vendorEmp_IDProofType":"Aadhar","vendorEmp_IDProofNo":"1234-5678-9012","vendorEmp_mobile":"9876543210","vendorEmp_VenderAID":1}'
```

#### 4. Update a vendor employee
```bash
curl -X PUT https://localhost:5001/api/vendoremp/1 \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"vendorEmp_Name":"John Vendor Updated","vendorEmp_mobile":"0123456789"}'
```

#### 5. Delete a vendor employee
```bash
curl -X DELETE https://localhost:5001/api/vendoremp/1 \
  -H "Authorization: Bearer {your_jwt_token}"
```

---

### Visitor API

**Note:** All Visitor endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer {your_jwt_token}
```

#### Get All Visitors
**GET** `/api/visitor`

#### Get Visitor by ID
**GET** `/api/visitor/{id}`

#### Create Visitor
**POST** `/api/visitor`

**Request Body (example):**
```json
{
  "visitor_Name": "John Doe",
  "visitor_mobile": "9876543210",
  "visitor_Address": "123 Main St",
  "visitor_CompanyName": "ABC Corp",
  "visitor_Purposeofvisit": "Meeting",
  "visitor_Idprooftype": "Aadhar",
  "visitor_idproofno": "1234-5678-9012",
  "visitor_MeetingDate": "2025-12-01T10:00:00"
}
```

#### Update Visitor
**PUT** `/api/visitor/{id}`

#### Delete Visitor
**DELETE** `/api/visitor/{id}`

Responses follow the same patterns as other APIs in this README.

---

### Visitor Entry API

**Note:** All VisitorEntry endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer {your_jwt_token}
```

#### Get All Visitor Entries
**GET** `/api/visitorentry`

#### Get Visitor Entry by ID
**GET** `/api/visitorentry/{id}`

#### Create Visitor Entry
**POST** `/api/visitorentry`

**Request Body (example):**
```json
{
  "visitorEntry_visitorId": 1,
  "visitorEntry_Gatepass": "GP12345",
  "visitorEntry_Vehicletype": "Car",
  "visitorEntry_Vehicleno": "MH12AB1234",
  "visitorEntry_Date": "2025-12-01T10:00:00",
  "visitorEntry_Intime": "2025-12-01T10:05:00",
  "visitorEntry_Userid": 1,
  "visitorEntry_isCanteen": false,
  "visitorEntry_isStay": false
}
```

#### Update Visitor Entry
**PUT** `/api/visitorentry/{id}`

#### Delete Visitor Entry
**DELETE** `/api/visitorentry/{id}`

Responses follow the same patterns as other APIs in this README.

---

### Approval Status API

**Note:** All ApprovalStatus endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer {your_jwt_token}
```

#### Get All Approval Statuses
**GET** `/api/approvalstatus`

#### Get Approval Status by ID
**GET** `/api/approvalstatus/{id}`

#### Create Approval Status
**POST** `/api/approvalstatus`

**Request Body (example):**
```json
{
  "approvalStatus_Gatepass": "GP12345",
  "approvalStatus_VisitorEntryId": 1,
  "approvalStatus_TransactionDate": "2025-12-01T10:00:00",
  "approvalStatus_ApprovalDate": "2025-12-01T11:00:00",
  "approvalStatus_ApprovalStatus": true,
  "approvalStatus_Remark": "Approved",
  "approvalStatus_ApprovalPersonRoleID": 2,
  "approvalStatus_UserId": 1
}
```

#### Update Approval Status
**PUT** `/api/approvalstatus/{id}`

#### Delete Approval Status
**DELETE** `/api/approvalstatus/{id}`

Responses follow the same patterns as other APIs in this README.

---

## Security Features

### Password Hashing
- Passwords are hashed using BCrypt before storing in the database
- BCrypt automatically handles salting and multiple hashing rounds
- Verification is done using BCrypt's built-in verify method

### JWT Authentication
- JWT tokens are issued upon successful login
- Tokens expire after 1440 minutes (24 hours) by default
- Token contains user claims: UserId, Username, U_Name, Email, RoleId, DepartmentID
- Protected endpoints require valid JWT token in Authorization header

### Token Configuration
In `appsettings.json`:
```json
"Jwt": {
  "Key": "A9f8#3dF_lK92@!xP0qZ88*LmQw7VrTnB4",
  "Issuer": "SpectrumApi",
  "Audience": "SpectrumClients",
  "ExpiresMinutes": 1440
}
```

## How to Use

### Authentication Examples

#### 1. Register a new user
```bash
curl -X POST https://localhost:5001/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{"username":"john_doe","password":"SecurePass123","u_Name":"John Doe","u_Mobile":"1234567890","u_Email":"john@example.com","u_Address":"123 Main St","u_RoleId":1,"u_DepartmentID":1,"u_ReportingToId":null}'
```

#### 2. Login to get JWT token
```bash
curl -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"john_doe","password":"SecurePass123"}'
```

#### 3. Use token for authenticated requests
```bash
curl -X GET https://localhost:5001/api/auth/me \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 4. Update user profile
```bash
curl -X PUT https://localhost:5001/api/auth/update \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"u_Name":"John Updated","u_Email":"john.updated@example.com","u_Mobile":"9876543210"}'
```

### Department Examples

#### 1. Get all departments
```bash
curl -X GET https://localhost:5001/api/department/all \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 2. Get active departments
```bash
curl -X GET https://localhost:5001/api/department/active \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 3. Create a new department
```bash
curl -X POST https://localhost:5001/api/department \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"department":"Finance"}'
```

#### 4. Update a department
```bash
curl -X PUT https://localhost:5001/api/department/3 \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"department":"Finance & Accounts","isActive":true}'
```

#### 5. Get department by ID
```bash
curl -X GET https://localhost:5001/api/department/1 \
  -H "Authorization: Bearer {your_jwt_token}"
```

### Role Examples

#### 1. Get all roles
```bash
curl -X GET https://localhost:5001/api/role \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 2. Get role by ID
```bash
curl -X GET https://localhost:5001/api/role/1 \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 3. Create a new role
```bash
curl -X POST https://localhost:5001/api/role \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"roleName":"Supervisor"}'
```

#### 4. Update a role
```bash
curl -X PUT https://localhost:5001/api/role/1 \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"roleName":"System Administrator"}'
```

#### 5. Delete a role
```bash
curl -X DELETE https://localhost:5001/api/role/4 \
  -H "Authorization: Bearer {your_jwt_token}"
```

### Location Examples

#### 1. Get all locations
```bash
curl -X GET https://localhost:5001/api/location \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 2. Get location by ID
```bash
curl -X GET https://localhost:5001/api/location/1 \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 3. Create a new location
```bash
curl -X POST https://localhost:5001/api/location \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"locationName":"Branch Office","description":"Branch office description","isActive":true}'
```

#### 4. Update a location
```bash
curl -X PUT https://localhost:5001/api/location/1 \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"locationName":"Regional Office","description":"Updated description","isActive":true}'
```

#### 5. Delete a location
```bash
curl -X DELETE https://localhost:5001/api/location/2 \
  -H "Authorization: Bearer {your_jwt_token}"
```

### Vendor Examples

#### 1. Get all vendors
```bash
curl -X GET https://localhost:5001/api/vendor \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 2. Get vendor by ID
```bash
curl -X GET https://localhost:5001/api/vendor/1 \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 3. Create a new vendor
```bash
curl -X POST https://localhost:5001/api/vendor \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"vendorCode":"V002","vendorName":"XYZ Traders","vendorMobile":"0987654321","idProofType":"Aadhar","idProof":"1234-5678-9012","vendorAddress":"456 Market St","company":"XYZ Co","isActive":true}'
```

#### 4. Update a vendor
```bash
curl -X PUT https://localhost:5001/api/vendor/1 \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"vendorName":"XYZ Traders Pvt Ltd","vendorMobile":"0987654321","isActive":true}'
```

#### 5. Delete a vendor
```bash
curl -X DELETE https://localhost:5001/api/vendor/2 \
  -H "Authorization: Bearer {your_jwt_token}"
```

### Vendor Appointment Examples

#### 1. Get all appointments
```bash
curl -X GET https://localhost:5001/api/vendorappointment \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 2. Get appointment by ID
```bash
curl -X GET https://localhost:5001/api/vendorappointment/1 \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 3. Create a new appointment
```bash
curl -X POST https://localhost:5001/api/vendorappointment \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"vendorA_VendorID":1,"vendorA_Getpass":"PASS123","vendorA_FromDate":"2025-12-01T10:00:00","vendorA_ToDate":"2025-12-01T12:00:00","vendorA_VehicleNO":"MH12AB1234","vendorA_IdProofType":"Aadhar","vendorA_IdProofNo":"1234-5678-9012","vendorA_UserId":1}'
```

#### 4. Update an appointment
```bash
curl -X PUT https://localhost:5001/api/vendorappointment/1 \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"vendorA_ToDate":"2025-12-01T13:00:00"}'
```

#### 5. Delete an appointment
```bash
curl -X DELETE https://localhost:5001/api/vendorappointment/1 \
  -H "Authorization: Bearer {your_jwt_token}"
```

### Vendor Employee Examples

#### 1. Get all vendor employees
```bash
curl -X GET https://localhost:5001/api/vendoremp \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 2. Get vendor employee by ID
```bash
curl -X GET https://localhost:5001/api/vendoremp/1 \
  -H "Authorization: Bearer {your_jwt_token}"
```

#### 3. Create a new vendor employee
```bash
curl -X POST https://localhost:5001/api/vendoremp \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"vendorEmp_VendorID":1,"vendorEmp_Name":"John Vendor","vendorEmp_IDProofType":"Aadhar","vendorEmp_IDProofNo":"1234-5678-9012","vendorEmp_mobile":"9876543210","vendorEmp_VenderAID":1}'
```

#### 4. Update a vendor employee
```bash
curl -X PUT https://localhost:5001/api/vendoremp/1 \
  -H "Authorization: Bearer {your_jwt_token}" \
  -H "Content-Type: application/json" \
  -d '{"vendorEmp_Name":"John Vendor Updated","vendorEmp_mobile":"0123456789"}'
```

#### 5. Delete a vendor employee
```bash
curl -X DELETE https://localhost:5001/api/vendoremp/1 \
  -H "Authorization: Bearer {your_jwt_token}"
```

---

### Visitor API

**Note:** All Visitor endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer {your_jwt_token}
```

#### Get All Visitors
**GET** `/api/visitor`

#### Get Visitor by ID
**GET** `/api/visitor/{id}`

#### Create Visitor
**POST** `/api/visitor`

**Request Body (example):**
```json
{
  "visitor_Name": "John Doe",
  "visitor_mobile": "9876543210",
  "visitor_Address": "123 Main St",
  "visitor_CompanyName": "ABC Corp",
  "visitor_Purposeofvisit": "Meeting",
  "visitor_Idprooftype": "Aadhar",
  "visitor_idproofno": "1234-5678-9012",
  "visitor_MeetingDate": "2025-12-01T10:00:00"
}
```

#### Update Visitor
**PUT** `/api/visitor/{id}`

#### Delete Visitor
**DELETE** `/api/visitor/{id}`

Responses follow the same patterns as other APIs in this README.

---

### Visitor Entry API

**Note:** All VisitorEntry endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer {your_jwt_token}
```

#### Get All Visitor Entries
**GET** `/api/visitorentry`

#### Get Visitor Entry by ID
**GET** `/api/visitorentry/{id}`

#### Create Visitor Entry
**POST** `/api/visitorentry`

**Request Body (example):**
```json
{
  "visitorEntry_visitorId": 1,
  "visitorEntry_Gatepass": "GP12345",
  "visitorEntry_Vehicletype": "Car",
  "visitorEntry_Vehicleno": "MH12AB1234",
  "visitorEntry_Date": "2025-12-01T10:00:00",
  "visitorEntry_Intime": "2025-12-01T10:05:00",
  "visitorEntry_Userid": 1,
  "visitorEntry_isCanteen": false,
  "visitorEntry_isStay": false
}
```

#### Update Visitor Entry
**PUT** `/api/visitorentry/{id}`

#### Delete Visitor Entry
**DELETE** `/api/visitorentry/{id}`

Responses follow the same patterns as other APIs in this README.

---

### Approval Status API

**Note:** All ApprovalStatus endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer {your_jwt_token}
```

#### Get All Approval Statuses
**GET** `/api/approvalstatus`

#### Get Approval Status by ID
**GET** `/api/approvalstatus/{id}`

#### Create Approval Status
**POST** `/api/approvalstatus`

**Request Body (example):**
```json
{
  "approvalStatus_Gatepass": "GP12345",
  "approvalStatus_VisitorEntryId": 1,
  "approvalStatus_TransactionDate": "2025-12-01T10:00:00",
  "approvalStatus_ApprovalDate": "2025-12-01T11:00:00",
  "approvalStatus_ApprovalStatus": true,
  "approvalStatus_Remark": "Approved",
  "approvalStatus_ApprovalPersonRoleID": 2,
  "approvalStatus_UserId": 1
}
```

#### Update Approval Status
**PUT** `/api/approvalstatus/{id}`

#### Delete Approval Status
**DELETE** `/api/approvalstatus/{id}`

Responses follow the same patterns as other APIs in this README.

---

## Project Structure
```
Spectrum/
??? Controllers/
?   ??? AuthController.cs          # Authentication API endpoints
?   ??? DepartmentController.cs    # Department API endpoints
?   ??? RoleController.cs          # Role API endpoints
?   ??? LocationController.cs      # Location API endpoints
?   ??? VendorController.cs        # Vendor API endpoints
?   ??? VendorAppointmentController.cs  # Vendor Appointment API endpoints
?   ??? VendorEmpController.cs          # Vendor Employee API endpoints
?   ??? VisitorController.cs         # Visitor API endpoints
?   ??? VisitorEntryController.cs
??? DTOs/
?   ??? RegisterDTO.cs             # Registration request model
?   ??? LoginDTO.cs                # Login request model
?   ??? UpdateUserDTO.cs           # Update user request model
?   ??? LoginResponseDTO.cs        # Login response model
?   ??? UserResponseDTO.cs         # User details response model
?   ??? CreateDepartmentDTO.cs     # Create department request model
?   ??? UpdateDepartmentDTO.cs     # Update department request model
?   ??? DepartmentResponseDTO.cs   # Department response model
?   ??? CreateRoleDTO.cs          # Create role request model
?   ??? UpdateRoleDTO.cs          # Update role request model
?   ??? RoleResponseDTO.cs        # Role response model
?   ??? CreateLocationDTO.cs      # Create location request model
?   ??? UpdateLocationDTO.cs      # Update location request model
?   ??? LocationResponseDTO.cs    # Location response model
?   ??? CreateVendorDTO.cs        # Create vendor request model
?   ??? UpdateVendorDTO.cs        # Update vendor request model
?   ??? VendorResponseDTO.cs      # Vendor response model
?   ??? CreateVendorAppointmentDTO.cs  # Create vendor appointment request model
?   ??? UpdateVendorAppointmentDTO.cs  # Update vendor appointment request model
?   ??? VendorAppointmentResponseDTO.cs  # Vendor appointment response model
?   ??? CreateVendorEmpDTO.cs          # Create vendor employee request model
?   ??? UpdateVendorEmpDTO.cs          # Update vendor employee request model
?   ??? VendorEmpResponseDTO.cs          # Vendor employee response model
?   ??? CreateVisitorDTO.cs            # Create visitor request model
?   ??? UpdateVisitorDTO.cs            # Update visitor request model
?   ??? VisitorResponseDTO.cs            # Visitor response model
?   ??? CreateVisitorEntryDTO.cs
?   ??? UpdateVisitorEntryDTO.cs
?   ??? VisitorEntryResponseDTO.cs
??? Models/
?   ??? User.cs                    # User entity model
?   ??? Department.cs              # Department entity model
?   ??? Role.cs                    # Role entity model
?   ??? Location.cs                # Location entity model
?   ??? Vendor.cs                  # Vendor entity model
?   ??? VendorAppointment.cs        # Vendor appointment entity model
?   ??? VendorEmp.cs                  # Vendor employee entity model
?   ??? Visitor.cs                   # Visitor entity model
?   ??? VisitorEntry.cs
??? Repositories/
?   ??? IUserRepository.cs         # User repository interface
?   ??? UserRepository.cs          # User database operations using Dapper
?   ??? IDepartmentRepository.cs   # Department repository interface
?   ??? DepartmentRepository.cs    # Department database operations using Dapper
?   ??? IRoleRepository.cs         # Role repository interface
?   ??? RoleRepository.cs          # Role database operations using Dapper
?   ??? ILocationRepository.cs     # Location repository interface
?   ??? LocationRepository.cs      # Location database operations using Dapper
?   ??? IVendorRepository.cs       # Vendor repository interface
?   ??? VendorRepository.cs        # Vendor database operations using Dapper
?   ??? IVendorAppointmentRepository.cs  # Vendor appointment repository interface
?   ??? VendorAppointmentRepository.cs  # Vendor appointment database operations using Dapper
?   ??? IVendorEmpRepository.cs          # Vendor employee repository interface
?   ??? VendorEmpRepository.cs          # Vendor employee database operations using Dapper
?   ??? IVisitorRepository.cs          # Visitor repository interface
?   ??? VisitorRepository.cs          # Visitor database operations using Dapper
?   ??? IVisitorEntryRepository.cs
?   ??? VisitorEntryRepository.cs
??? Services/
?   ??? IAuthService.cs            # Auth service interface
?   ??? AuthService.cs             # Business logic for authentication
?   ??? IJwtService.cs             # JWT service interface
?   ??? JwtService.cs              # JWT token generation
?   ??? IDepartmentService.cs      # Department service interface
?   ??? DepartmentService.cs       # Business logic for departments
?   ??? IRoleService.cs            # Role service interface
?   ??? RoleService.cs             # Business logic for roles
?   ??? ILocationService.cs        # Location service interface
?   ??? LocationService.cs         # Business logic for locations
?   ??? IVendorService.cs          # Vendor service interface
?   ??? VendorService.cs           # Business logic for vendors
?   ??? IVendorAppointmentService.cs  # Vendor appointment service interface
?   ??? VendorAppointmentService.cs  # Business logic for vendor appointments
?   ??? IVendorEmpService.cs          # Vendor employee service interface
?   ??? VendorEmpService.cs          # Business logic for vendor employees
?   ??? IVisitorService.cs            # Visitor service interface
?   ??? VisitorService.cs            # Business logic for visitors
?   ??? IVisitorEntryService.cs
?   ??? VisitorEntryService.cs
??? Program.cs                     # App configuration & DI setup
??? appsettings.json              # Configuration file
```

## Database Schema

### tblUsers
```sql
CREATE TABLE tblUsers
(
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password_hash NVARCHAR(255) NOT NULL,
    U_Name NVARCHAR(100) NOT NULL,
    U_Mobile NVARCHAR(15),
    U_Email NVARCHAR(100),
    U_Address NVARCHAR(255),
    U_RoleId INT,
    U_DepartmentID INT,
    U_ReportingToId INT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME NULL
);
```

### tblDepartment
```sql
CREATE TABLE tblDepartment
(
    DepartmentId INT IDENTITY(1,1) PRIMARY KEY,
    Department NVARCHAR(100) NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME NULL,
    IsActive BIT NOT NULL DEFAULT 1
);
```

### tblRole
```sql
CREATE TABLE tblRole
(
    RoleId INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(100) NOT NULL UNIQUE,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME NULL
);
```

### tblLocation
```sql
CREATE TABLE tblLocation
(
    LocationId INT IDENTITY(1,1) PRIMARY KEY,
    LocationName NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(255),
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedBy INT NOT NULL
);
```

### tblVendor
```sql
CREATE TABLE tblVendor
(
    VendorID INT IDENTITY(1,1) PRIMARY KEY,
    VendorCode NVARCHAR(50) NOT NULL UNIQUE,
    VendorName NVARCHAR(100) NOT NULL,
    VendorMobile NVARCHAR(15),
    IdProofType NVARCHAR(50),
    IdProof NVARCHAR(100),
    VendorAddress NVARCHAR(255),
    Company NVARCHAR(100),
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME NULL,
    IsActive BIT NOT NULL DEFAULT 1
);
```

### tblVendorAppointment
```sql
CREATE TABLE tblVendorAppointment
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    VendorA_VendorID INT NOT NULL,
    VendorA_Getpass NVARCHAR(50) NOT NULL,
    VendorA_FromDate DATETIME NOT NULL,
    VendorA_ToDate DATETIME NOT NULL,
    VendorA_VehicleNO NVARCHAR(15) NOT NULL,
    VendorA_IdProofType NVARCHAR(50) NOT NULL,
    VendorA_IdProofNo NVARCHAR(100) NOT NULL,
    VendorA_UserId INT NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME NULL,
    FOREIGN KEY (VendorA_VendorID) REFERENCES tblVendor(VendorID),
    FOREIGN KEY (VendorA_UserId) REFERENCES tblUsers(UserId)
);
```

### tblVendorEmp
```sql
CREATE TABLE tblVendorEmp
(
    VendorEmpId INT IDENTITY(1,1) PRIMARY KEY,
    VendorEmp_VendorID INT NOT NULL,
    VendorEmp_Name NVARCHAR(100) NOT NULL,
    VendorEmp_IDProofType NVARCHAR(50) NOT NULL,
    VendorEmp_IDProofNo NVARCHAR(100) NOT NULL,
    VendorEmp_mobile NVARCHAR(15) NOT NULL,
    VendorEmp_VenderAID INT,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME NULL,
    FOREIGN KEY (VendorEmp_VendorID) REFERENCES tblVendor(VendorID)
);
```

### tblVisitor
```sql
CREATE TABLE tblVisitor
(
    VisitorId INT IDENTITY(1,1) PRIMARY KEY,
    Visitor_Name NVARCHAR(100) NOT NULL,
    Visitor_Mobile NVARCHAR(15) NOT NULL,
    Visitor_Address NVARCHAR(255),
    Visitor_CompanyName NVARCHAR(100),
    Visitor_Purposeofvisit NVARCHAR(255),
    Visitor_Idprooftype NVARCHAR(50),
    Visitor_idproofno NVARCHAR(100),
    Visitor_MeetingDate DATETIME NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME NULL,
    IsActive BIT NOT NULL DEFAULT 1
);
```

### tblVisitorEntry
```sql
CREATE TABLE tblVisitorEntry
(
    VisitorEntryId INT IDENTITY(1,1) PRIMARY KEY,
    VisitorEntry_VisitorId INT NOT NULL,
    VisitorEntry_Gatepass NVARCHAR(50) NOT NULL,
    VisitorEntry_Vehicletype NVARCHAR(50) NOT NULL,
    VisitorEntry_Vehicleno NVARCHAR(15) NOT NULL,
    VisitorEntry_Date DATETIME NOT NULL,
    VisitorEntry_Intime DATETIME NOT NULL,
    VisitorEntry_Userid INT NOT NULL,
    VisitorEntry_isCanteen BIT NOT NULL DEFAULT 0,
    VisitorEntry_isStay BIT NOT NULL DEFAULT 0,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME NULL,
    FOREIGN KEY (VisitorEntry_VisitorId) REFERENCES tblVisitor(VisitorId),
    FOREIGN KEY (VisitorEntry_Userid) REFERENCES tblUsers(UserId)
);
```

### tblMaterial
```sql
CREATE TABLE tblMaterial
(
    MaterialId INT IDENTITY(1,1) PRIMARY KEY,
    Material_Name NVARCHAR(100) NOT NULL,
    Material_Code NVARCHAR(50) NOT NULL,
    Material_Status NVARCHAR(50) NOT NULL,
    Material_VisitorId INT,
    Material_EntryDate DATETIME NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME NULL,
    FOREIGN KEY (Material_VisitorId) REFERENCES tblVisitor(VisitorId)
);
```

### tblAminities
```sql
CREATE TABLE tblAminities
(
    AminitiesId INT IDENTITY(1,1) PRIMARY KEY,
    Aminities_Name NVARCHAR(100) NOT NULL,
    Aminities_isActive BIT NOT NULL DEFAULT 1
);
```

### tblMeetingRoom
```sql
CREATE TABLE tblMeetingRoom
(
    MeetingRoomId INT IDENTITY(1,1) PRIMARY KEY,
    MeetingRoom_Name NVARCHAR(100) NOT NULL,
    MeetingRoom_Floor INT NOT NULL,
    MeetingRoom_Capacity INT NOT NULL,
    MeetingRoom_AminitiesId INT,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME NULL,
    FOREIGN KEY (MeetingRoom_AminitiesId) REFERENCES tblAminities(AminitiesId)
);
```

### tblRoomBooking
```sql
CREATE TABLE tblRoomBooking
(
    RoomBookingId INT IDENTITY(1,1) PRIMARY KEY,
    RoomBooking_MeetingroomId INT NOT NULL,
    RoomBooking_UserID INT NOT NULL,
    RoomBooking_VisitorID INT NOT NULL,
    RoomBooking_MeetingDate DATETIME NOT NULL,
    RoomBooking_Starttime TIME NOT NULL,
    RoomBooking_Endtime TIME NOT NULL,
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedDate DATETIME NULL,
    FOREIGN KEY (RoomBooking_MeetingroomId) REFERENCES tblMeetingRoom(MeetingRoomId),
    FOREIGN KEY (RoomBooking_UserID) REFERENCES tblUsers(UserId),
    FOREIGN KEY (RoomBooking_VisitorID) REFERENCES tblVisitor(VisitorId)
);
```

## Error Responses

### 400 Bad Request
```json
{
  "message": "Username already exists"
}
```
```json
{
  "message": "Department name already exists"
}
```
```json
{
  "message": "Role name already exists"
}
```
```json
{
  "message": "Location name already exists"
}
```
```json
{
  "message": "Vendor code already exists"
}
```
```json
{
  "message": "Vendor appointment details are incomplete"
}
```
```json
{
  "message": "Vendor employee details are incomplete"
}
```
```json
{
  "message": "Visitor details are required"
}
```
```json
{
  "message": "Visitor entry details are required"
}
```
```json
{
  "message": "Material code must be unique"
}
```
```json
{
  "message": "Aminity name must be unique"
}
```
```json
{
  "message": "Meeting room name must be unique"
}
```

### 401 Unauthorized
```json
{
  "message": "Invalid username or password"
}
```

### 404 Not Found
```json
{
  "message": "User not found"
}
```
```json
{
  "message": "Department not found"
}
```
```json
{
  "message": "Role not found"
}
```
```json
{
  "message": "Location not found"
}
```
```json
{
  "message": "Vendor not found"
}
```
```json
{
  "message": "Vendor appointment not found"
}
```
```json
{
  "message": "Employee not found"
}
```
```json
{
  "message": "Visitor not found"
}
```
```json
{
  "message": "Visitor entry not found"
}
```
```json
{
  "message": "Material not found"
}
```
```json
{
  "message": "Aminity not found"
}
```
```json
{
  "message": "Meeting room not found"
}
```

## Notes
- All password fields are automatically hashed before storage
- Passwords are never returned in API responses
- JWT tokens should be included in the Authorization header as: `Bearer {token}`
- All update fields in UpdateUserDTO, UpdateDepartmentDTO, UpdateRoleDTO, and UpdateLocationDTO are optional - only provided fields will be updated
- NewPassword field in UpdateUserDTO allows password changes
- All department, role, location, vendor, and employee endpoints require authentication (JWT token)
- Department, role, and location names must be unique across the system
- Departments and roles can be marked as inactive using the IsActive flag (only for departments and roles)
- Vendor codes must be unique across the system
- Vendor appointments require valid VendorA_VendorID, VendorA_UserId, and corresponding proof details
- Vendor employees require valid VendorEmp_VendorID and corresponding proof details
- Visitor details are required for visitor creation and updates
- Visitor entry details are required for visitor entry creation and updates
- Material codes, aminities names, and meeting room names must be unique across their respective entities

---

### Material API

**Note:** All Material endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer {your_jwt_token}
```

#### Get All Materials
**GET** `/api/material`

#### Get Material by ID
**GET** `/api/material/{id}`

#### Create Material
**POST** `/api/material`

**Request Body (example):**
```json
{
  "material_Name": "Laptop",
  "material_Code": "MT-001",
  "material_Status": "Checked-In",
  "material_VisitorId": 1,
  "material_EntryDate": "2025-12-01T10:00:00"
}
```

#### Update Material
**PUT** `/api/material/{id}`

#### Delete Material
**DELETE** `/api/material/{id}`

Responses follow standard patterns used elsewhere in README.

---

### Aminities API

**Note:** All Aminities endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer {your_jwt_token}
```

#### Get All Aminities
**GET** `/api/aminities`

#### Get Aminity by ID
**GET** `/api/aminities/{id}`

#### Create Aminity
**POST** `/api/aminities`

**Request Body (example):**
```json
{
  "aminities_Name": "Conference Room",
  "aminities_isActive": true
}
```

#### Update Aminity
**PUT** `/api/aminities/{id}`

#### Delete Aminity
**DELETE** `/api/aminities/{id}`

Responses follow the same patterns as other APIs in this README.

---

### Meeting Room API

**Note:** All MeetingRoom endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer {your_jwt_token}
```

#### Get All Meeting Rooms
**GET** `/api/meetingroom`

#### Get Meeting Room by ID
**GET** `/api/meetingroom/{id}`

#### Create Meeting Room
**POST** `/api/meetingroom`

**Request Body (example):**
```json
{
  "meetingRoom_Name": "Conference A",
  "meetingRoom_Floor": 2,
  "meetingRoom_Capacity": 20,
  "meetingRoom_AminitiesId": 1
}
```

#### Update Meeting Room
**PUT** `/api/meetingroom/{id}`

#### Delete Meeting Room
**DELETE** `/api/meetingroom/{id}`

Responses follow the same patterns as other APIs in this README.

---

### Room Booking API

**Note:** All RoomBooking endpoints require JWT authentication. Include the token in the Authorization header:
```
Authorization: Bearer {your_jwt_token}
```

#### Get All Room Bookings
**GET** `/api/roombooking`

#### Get Room Booking by ID
**GET** `/api/roombooking/{id}`

#### Create Room Booking
**POST** `/api/roombooking`

**Request Body (example):**
```json
{
  "roomBooking_MeetingroomId": 1,
  "roomBooking_UserID": 1,
  "roomBooking_VisitorID": 1,
  "roomBooking_MeetingDate": "2025-12-01T10:00:00",
  "roomBooking_Starttime": "10:00:00",
  "roomBooking_Endtime": "11:00:00"
}
```

#### Update Room Booking
**PUT** `/api/roombooking/{id}`

#### Delete Room Booking
**DELETE** `/api/roombooking/{id}`

Responses follow the same patterns as other APIs in this README.


---

### Parcel Api

#### Get all Parcel
**GET** `/api/parcel`

#### Post PArcel 
**POST** `/api/parcel`

**Request Body (example):**
```json
{
  "parcelBarcode": "PCL-20251234",
  "parcelCompanyName": "Acme Logistics",
  "userId": 42,
  "isActive": true
}
```
#### PUT Parcel
**PUT** `/api/parcel/{id}`

#### DELETE Parcel
**DELETE** `/api/parcel/{id}`