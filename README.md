# A simple Todo App to Create Tasks 

This code represents single entity .NET MVC project to add, edit, read and delete todo tasks. It includes search functionality and you can categorize your tasks based on their current status  
This project uses Postgres database, so in order to run it, you need to set up your db.  
I have separated project for `DbContext` and `Migration` stuff, so database migration commands must use project references as well. 

With Postgres database, what I like is that when you go to your pgAdmin console, you can look for all the databases you have for different projects. For me pgAdmin looks like this:

![13_Database view](https://github.com/user-attachments/assets/adeae3f0-8a60-4f9a-87bf-8fa9a543494e)

So different databases. When you have succesfully set up your database you should see our database for yourself aswell.  

## Setting up this project 
- Download or clone this project
- If you dont have, download postgres [here](https://www.postgresql.org/download/)
- Open this app with your favourite C# text editor. I'm using Visual Studio that you can download [here](https://visualstudio.microsoft.com/)
- Verify ConnectionString that you have in `WebApp/appsettings.json`. Currently it is like this:
```
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=myMedia-todoApp;Username=postgres;Password=qwerty123"
    },
```
You might need to change username and password for your postgres root user credentials.   
If connectionString has been checked, we should think about database creation. I have already created Migrations, so you can skip step number one. But anyway, 3 step migration commands to run for this project are as follows:
```
// 1. use this one to create new Migration structure to EFCore folder
dotnet ef migrations --project EFCore --startup-project WebApp add Initial

// 2. this one creates new database that you can verify in pgAdmin console
dotnet ef database --project EFCore --startup-project WebApp update

// 3. use this one to delete your existing db
dotnet ef database --project EFCore --startup-project WebApp drop
```
Try step number 2 first and if it is successfull then you are ready to go. If step 2 did not work, delete Migrations folder, check connectionString and start again from step 1. Enjoy.   
Some pictures about this application:
![03final_view](https://github.com/user-attachments/assets/46b260ba-bcff-4f01-970d-a23f13d44076)


![all-tests-pass](https://github.com/user-attachments/assets/66f96061-07ed-41b7-8e95-269f176b04d5)


