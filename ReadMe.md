# Travel Expenses - Manage Your Moolah
### Build Procedures and Deployment
---
##### Building the client for deployment to Azure
- The client gets tested and built from the development environment and then the build artifacts are checked in to GIT and pushed
- This will get detected by the DevOps Continuous Integration Pipeline and the build of the server will start
### Tasks
---
##### Connect SQL Server Management Studio to the Azure SQL Server database
- The server name is travel-expenses-server.database.windows.net

##### Development procedure
1. Visual Studio 2017 has TravelExpense.sln loaded
2. Visual Studio Code has the TravelExpenses.Client folder loaded
3. Start (F5) the WebAPI solution by self hosting TravelExpenses.WebAPI and not IIS Express
   - It just runs faster
   - Debugging works just as normal
4. Open a terminal in VS Code and type `npm run serve`
   - This will run the client in its own web server from the Vue CLI on port 8080
   - Port 8080 is allowed by CORS in the Startup.cs file, but only in development
   - The client is watched for changes so you just have to refresh the page
5. Open SQL Server Management Studio and connect to `(localdb)\MSSQLLocalDB` with Windows authentication

##### Commands
- Update the nuget packages with this [dotnet tools](https://github.com/natemcmaster/dotnet-tools) command
    ```
    dotnet-outdated
    ```
- Created a new migration for changes that are made to either the entities or the context
    ```
    add-migration <name>
    ```
- Run migrations into the development database (uses the connection string in appsettings.Development.json)
    ```
    update-database <name>
    ```
    - *Note that migration are automatically run into the production database when Startup runs*
### Todos
---
- Client
  - [x] Create Login page
  - [x] Create registration page
  - [ ] Create forgot password procedure
    - [ ] Create send email page
    - [ ] Server to talk to SendGrid to send email
    - [ ] Server to record reset password record
    - [ ] Create password reset page
  - [x] Build to wwwroot
  - [x] Enable CORS for development
  - [ ] Make Setup data records scroll when there are too many
  - [ ] Create Recent Transactions page
  - [ ] Get GPS coordinates when creating transaction
  - [x] Add transaction to wire up to the stores (locations, currencies, categories, keywords)
- Server
  - [x] Add integration tests
    - [x] Happy path auth
    - [x] Wrong password
    - [x] Unknown user
    - [x] Happy path create user
    - [x] User already exists
    - [x] Validation errors
  - [ ] Create import API
    - [ ] Export data from Access database
  - [x] Complete database
  - [x] Add memory caches for currencies and countries
  - [x] Create basic health checks
  - [x] Move connection string to correct location
  - [ ] Double logging of requests (I think this is due to OPTIONS calls in development for CORS)
  - [x] External configuration
  - [x] Add user id to the requests
  - [ ] Add user id to the logging
  - [x] Cleanup logging in Startup.cs
  - [x] Turn off developer exception page
  - [ ] Logging format on Azure
  - [x] Add environment health check
- Azure
  - [x] Get build to work
  - [x] Deploy database
  - [x] Deploy client code
  - [x] Logging setup
  - [ ] Execute unit tests