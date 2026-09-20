# Hosting & Deployment Guide

AdExcellence is an **ASP.NET Web Forms** app (**.NET Framework 4.7.2** + **SQL Server**).
It is deployed on **MonsterASP.NET** (free plan) and auto-deploys from GitHub via a
CI/CD workflow.

- **Live site:** http://adexcellence.runasp.net/
- **Host:** MonsterASP.NET — site `site92695`, database `db69212`

> Because this is a compiled .NET Framework Web Forms app (not static files and not
> .NET Core), it **cannot** be hosted on GitHub Pages / Netlify / Vercel. It needs a
> Windows/IIS host with SQL Server.

---

## 1. Prerequisites (local)

- **Visual Studio 2022/2026 Community** (free) with the *"ASP.NET and web development"* workload
- **SQL Server** + **SSMS**
- The database `adexcel` restored locally

## 2. Hosting setup (one-time, in the MonsterASP panel)

1. Create a **free website** → gives the `runasp.net` subdomain (`site92695`).
2. Create a **free MSSQL database** (`db69212`) → note **server, database, user, password**.
3. **Set the runtime:** Site → **Scripting → ASP.NET / .NET** → **.NET Framework 4.8 [v4.0]**, **Integrated** pipeline.
   - ⚠️ **Critical.** The default is a .NET Core runtime, which does **not** execute `.aspx`
     pages — every page 404s while static files still load. This must be .NET Framework.

## 3. Database migration

1. In SSMS: right-click `adexcel` → **Tasks → Generate Scripts** → *Advanced →
   "Types of data to script" = **Schema and data*** → save `adexcel.sql`.
2. Remove any `USE [...]` / `CREATE DATABASE` / `ALTER DATABASE` statements (you import
   into the existing `db69212`).
3. Ensure `Login.Password` is `varchar(255)` (PBKDF2 hashes are ~76 chars):
   ```sql
   ALTER TABLE dbo.Login ALTER COLUMN [Password] varchar(255);
   ```
4. MonsterASP → **Databases → db69212 → Import SQL** → upload the script.

## 4. Connection string

`Web.config` keeps a placeholder password (`LAVESH`) so no secret is committed. The real
value is injected at deploy time (see CI/CD). Format:

```
Server=db69212.databaseasp.net;Database=db69212;User Id=db69212;Password=<password>;Encrypt=False;MultipleActiveResultSets=True;
```

## 5. CI/CD auto-deploy (GitHub Actions → Web Deploy)

Every push to `main` runs `.github/workflows/deploy.yml`:
1. Builds the project with MSBuild on a Windows runner.
2. Injects secrets into `Web.config` (DB password + `machineKey`).
3. Deploys via **Web Deploy** using the `site92695-WebDeploy` publish profile.

**Required GitHub Secrets** (Settings → Secrets and variables → Actions):

| Secret | What it is |
| --- | --- |
| `MONSTERASP_PASSWORD` | Web Deploy / hosting password (user `site92695`) |
| `DB_PASSWORD` | Database password for `db69212` |
| `MACHINEKEY_VALIDATION` | 128-hex ViewState validation key (see below) |
| `MACHINEKEY_DECRYPTION` | 48-hex ViewState decryption key (see below) |

Generate the machineKey secrets locally (PowerShell) — never commit them:

```powershell
$rng = New-Object System.Security.Cryptography.RNGCryptoServiceProvider
$b1 = New-Object byte[] 64; $rng.GetBytes($b1); $vk = ($b1 | % { $_.ToString("X2") }) -join ''
$b2 = New-Object byte[] 24; $rng.GetBytes($b2); $dk = ($b2 | % { $_.ToString("X2") }) -join ''
gh secret set MACHINEKEY_VALIDATION -b $vk -R laveshparyani/AdExcellence
gh secret set MACHINEKEY_DECRYPTION -b $dk -R laveshparyani/AdExcellence
```

## 6. Manual publish (alternative to CI/CD)

Right-click the project in Visual Studio → **Publish** → import the MonsterASP
`.PublishSettings` → set **Release** → **Publish**. Replace `LAVESH` with the real
password in the local `Web.config` first (do not commit it).

---

## Gotchas we hit (so you don't have to)

1. **Runtime was .NET Core** → all `.aspx` returned 404. Fix: set the site to **.NET Framework 4.8**.
2. **`.csproj` referenced missing images** (`images/aadhar/*`, `images/top-rept.jpg`) → publish
   packaging failed. Fix: removed the dead `<Content>` references.
3. **Root URL showed the host placeholder** → added a `defaultDocument` (`home.aspx`) in `Web.config`.
4. **Register crashed on page load** → a leftover `<%@ Register assembly="EO.Web" %>` directive
   referenced a DLL that isn't deployed. Fix: removed the directive + the broken project reference.
5. **Register crashed on submit** → `FileUpload.SaveAs` ran before the `HasFile` check into a
   folder that didn't exist. Fix: guard + auto-create the folder.
6. **Every postback failed with "Validation of viewstate MAC failed"** → shared hosting recycles
   the app pool, so the auto-generated `machineKey` changed between requests. Fix: a **stable
   `machineKey`** (injected from secrets).
7. **Password hash overflowed the `mobile` column** → the positional `INSERT` mapped the hash to
   `mobile (varchar 50)`. Fix: a **column-explicit `INSERT`** so the hash goes into `Password (varchar 255)`.

## Security notes

- Passwords are hashed with PBKDF2 (`App_Code/SecurityHelper.cs`).
- All SQL is parameterized.
- Secrets (DB password, Web Deploy password, machineKey) are injected from GitHub Secrets,
  never committed.
- See [SECURITY.md](SECURITY.md) for reporting and hardening notes.
