# Contributing to AdExcellence

Thanks for your interest in AdExcellence! This is an academic (final-year) project,
but contributions, bug reports, and suggestions are welcome.

## Ways to contribute

- Report a bug or request a feature via an [issue](https://github.com/laveshparyani/AdExcellence/issues).
- Submit a pull request for a fix or improvement.

## Development setup

**Prerequisites**
- Visual Studio 2022/2026 (Community is fine) with the *"ASP.NET and web development"* workload
- SQL Server + SQL Server Management Studio (SSMS)
- .NET Framework 4.7.2

**Steps**
1. Clone the repo and open `AdExcellence.sln` in Visual Studio.
2. Create a database named `adexcel` in SQL Server and import the schema/data.
3. Update the connection string in `AdExcellence/Web.config` (replace the `LAVESH`
   placeholder with your local/hosted password — do **not** commit real secrets).
4. Press **F5** to run locally.

See [HOSTING.md](../HOSTING.md) for the full deployment/CI-CD details.

## Pull request guidelines

1. Create a feature branch from `main` (e.g. `fix/booking-validation`).
2. Keep changes focused and small where practical.
3. Make sure the solution **builds** in Release before opening the PR (CI runs CodeQL + deploy).
4. Use **parameterized SQL** and never commit secrets (DB password, machineKey, deploy password).
5. Describe what changed and why, and link any related issue.

## Reporting security issues

Please do not open public issues for security problems - see [SECURITY.md](../SECURITY.md).
