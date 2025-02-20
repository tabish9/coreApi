Test Project
This project includes:

API (ASP.NET Core) – Handles lead submissions and retrieval. ( api is works fine, will add the screenshot )
Frontend (Angular) – Displays leads in a simple dashboard. ( because of cors issue, using hardcode data)
API Tests – Tests for the backend API. ( using MOQ, project build but issue with one dependency)

CoreAPI/
  ├── API/        <-- Backend (ASP.NET Core 8)
  ├── API.Test/   <-- API Tests
  ├── frontend/   <-- Frontend (Angular 19)
  ├── README.md   <-- Project Documentation

API Endpoints
GET	/leads	--Get all leads
GET	/leads/{id}	--Get a lead by ID
POST	/leads	--Submit a new lead
GET	/swagger	--Open API docs
