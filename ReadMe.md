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
- Server
  - [x] Add integration tests
    - [x] Happy path auth
    - [ ] Wrong password
    - [ ] Unknown user
    - [ ] Happy path create user
    - [ ] User already exists
    - [ ] Validation errors
  - [ ] Complete database
  - [x] Create basic health checks
  - [x] Move connection string to correct location
- Azure
  - [ ] Get build to work
  - [ ] Deploy database
  - [ ] Deploy client code
  - [ ] Logging setup