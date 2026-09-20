# Security Policy

AdExcellence is a final-year academic project (ASP.NET Web Forms, .NET Framework
4.7.2, SQL Server). It is provided for educational and demonstration purposes.

## Reporting a Vulnerability

If you discover a security issue, please **do not open a public issue**. Instead,
report it privately using GitHub's
[private vulnerability reporting](https://github.com/laveshparyani/AdExcellence/security/advisories/new),
or contact the maintainers:

- Lavesh Paryani - laveshparyani01@gmail.com
- Kapil Lund - kapillund29@gmail.com

Please include a description, steps to reproduce, and potential impact.

## Notes for anyone deploying this project

This project was built as a learning exercise. Before deploying it to any
public or production environment, review and harden the following:

- **Use parameterized queries** everywhere (protects against SQL injection).
- **Hash passwords** (e.g. PBKDF2/BCrypt) instead of storing them in plain text.
- **Do not enable `debug="true"`** in production (the Release build already
  removes it via `Web.Release.config`).
- **Use SQL authentication with a strong password** (not `Integrated Security`)
  when connecting to a hosted database, and keep secrets out of source control.
- **Validate and encode all user input/output** to mitigate XSS and CSRF.
