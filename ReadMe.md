# Travel Expenses - Manage Your Moolah
### Build Procedures
### Tasks
Updated the nuget packages with this [dotnet tools](https://github.com/natemcmaster/dotnet-tools) command
```
dotnet-outdated
```
### Todos
- Client
  - [ ] Create Login page
  - [ ] Create registration page
  - [x] Build to wwwroot
- Server
  - [x] Add integration tests
    - [x] Happy path auth
    - [x] Wrong password
    - [x] Unknown user
    - [x] Happy path create user
    - [x] User already exists
    - [x] Validation errors
  - [ ] Complete database
  - [x] Create basic health checks
  - [x] Move connection string to correct location
  - [ ] Double logging of requests
  - [ ] External configuration
  - [ ] Add user id to the requests and logging
  - [ ] Cleanup logging in Startup.cs
  - [x] Turn off developer exception page
- Azure
  - [ ] Get build to work
  - [ ] Deploy database
  - [ ] Deploy client code
  - [ ] Logging setup