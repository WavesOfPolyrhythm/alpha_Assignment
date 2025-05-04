
"Alpha Project Manager"

This is a project built in ASP.NET Core MVC. The purpose is to allow users to:
-Create an account
-Log in
-View the "project portal"

All views are protected and only accessible after logging in.

"Admin users" have extended permissions and can:
-Create, edit and delete projects (CRUD)
-Manage user roles

Good to know!
-An SQL query is included to quickly insert clients and project statuses. It is located under the "Databases" folder
-All new projects get the status "STARTED" by default

Roles & Permissions
-Admin and User roles are created automatically at startup
-An admin user (admin@domain.com) is created automatically at first startup and is automatically assigned the Admin role
-All following users get the User role

In User Manager page, Admin can:
-View users
-Change roles between Admin and User
-Only Admin has access to manage CRUD on projects and manage roles

Kind regards
Tim

