# Izenda Mvc5Starterkit (Back-end Standalone)

 
## Overview
This Starterkit showcases how to embed the front-end of Izenda into a MVC5 application that uses a stand-alone API back-end.

### Q. What is in this repository?

### A. This is a simple example using a Visual Studio project template with Izenda Embedded into it. This repository is only an example of integrating Izenda into another application. The Visual Studio project template used in this scenario is used as a substitute for your application. This repository shows examples of how you might embed Izenda into your application.

 :warning: The MVC Kit is designed for demonstration purposes and should not be used as an “as-is” fully-integrated solution. You can use the kit for reference or a baseline but ensure that security and customization meet the standards of your company.
 
 :warning: This deployment is currently using version 2.4.4. If you wish to update to a later version, ensure that you run the appropriate schema migration scripts.

### Deploying the standalone API and Izenda Configuration Database

- Download and deploy the <a href="https://downloads.izenda.com/v2.4.4/API.zip">Izenda API</a> to IIS.

- Run the <a href="https://github.com/Izenda7Series/Mvc5StarterKit_BE_Standalone/blob/master/Mvc5StarterKit/DBScript/mvc5_izenda.sql">DBScripts/mvc5_izenda.sql</a> to create a database named 'MVC5_Izenda' (This is the database for the Izenda configuration. It contains report definitions, dashboards,etc.). You may use any name of your choosing, just be sure to modify the script to USE the new database name.

- Download a copy of the <a href="https://github.com/Izenda7Series/Mvc5StarterKit/blob/master/Mvc5StarterKit/izendadb.config">izendadb.config</a> file, and copy it to the root of your API deployment. Then modify the file with a valid connection string to this new database. If the connection string contains a ‘/’, ensure that you escape it ‘//’

- In the IzendaSystemSettings table, update AuthValidateAccessTokenUrl to be fully qualified with the Starterkit's base address. e.g. api/account/validateIzendaAuthToken --> http://localhost:14809/api/account/validateIzendaAuthToken

- In the IzendaSystemSettings table, update AuthGetAccessTokenUrl to be fully qualified with the Starterkit's base address. e.g. api/getAccessToken --> http://localhost:14809/api/GetIzendaAccessToken

### Deploying the MVC Starter Kit Database

- Run the <a href="https://github.com/Izenda7Series/Mvc5StarterKit_BE_Standalone/blob/master/Mvc5StarterKit/DBScript/mvc5.sql">DBScripts/mvc5.sql</a> to create a database named 'mvc5'. This is the database for the .NET application. It contains the users, roles, tenants used to login. You may use any name of your choosing, just be sure to modify the script to USE the new database name.

### Deploying the Retail Database (optional)

Create the Retail database with the <a  href="https://github.com/Izenda7Series/Angular2Starterkit/blob/master/DbScripts/RetailDbScript.sql">RetailDbScript.sql</a> file.

 

### Deploying the MVC Kit

izenda.integrate.js

- Modify the hostApi to point to the port of your Izenda API.

Web.Config

- Update the connection string to point to your mvc5 database.

- Update the IzendaApiUrl to point to the port of your Izenda API.

Download the <a href="https://downloads.izenda.com/v2.4.4/EmbeddedUI.zip">Izenda Embedded UI</a>, and copy/store the files into the Scripts/izenda folder.

### Update RSA Keys

- Use Izenda's RSA Key Generator Utility Located at http://downloads.izenda.com/Utilities/Izenda.Synergy.RSATool.zip

  1. AuthRSAPublicKey value in the IzendaSystemSettings table of the Izenda database (note: only use keysize < 1024 to generate because max-length for this field in database is 256) . This value is your public key and should be in XML format.
  2. And RSAPrivateKey value in Web.config file of the MVC Kit. This value is your private key and should be in PEM format.

 

### Initial Log in

- Log in as the System User First. Navigate to the Settings page to update your license key and add database connections. Log out of Izenda to allow these changes to take effect.

Initial User for logging in.

System User: <br />
- Tenant:
- Username: IzendaAdmin@system.com <br />
- Password: Izenda@123 <br />

**DELDG**: <br />
Employee Role: <br />
- Tenant: DELDG
- Username: employee@deldg.com
- Password: Izenda@123

Manager Role:<br />
- Tenant: DELDG
- Username: manager@deldg.com
- Password: Izenda@123

VP Role:<br />
- Tenant: DELDG
- Username: vp@deldg.com
- Password: Izenda@123

**NATWR** <br />
Employee Role:<br />
- Tenant: NATWR
- Username: employee@natwr.com
- Password: Izenda@123

Manager Role:<br />
- Tenant: NATWR
- Username: manager@natwr.com
- Password: Izenda@123

VP Role:<br />
- Tenant: NATWR
- Username: vp@natwr.com
- Password: Izenda@123

**RETCL** <br />
Employee Role:<br />
- Tenant: RETCL
- Username: employee@retcl.com
- Password: Izenda@123

Manager Role:<br />
- Tenant: RETCL
- Username: manager@retcl.com
- Password: Izenda@123

VP Role:<br />
- Tenant: RETCL
- Username: vp@retcl.com
- Password: Izenda@123


## Post Installation

 :warning: In order to ensure smooth operation of this kit, the items below should be reviewed.
 
 
### Exporting

Update the WebUrl value in the IzendaSystemSetting table with the URL for your front-end. You can use the script below to accomplish this. As general best practice, we recommend backing up your database before making any manual updates.

```sql

UPDATE [IzendaSystemSetting]
SET [Value] = '<your url here including the trailing slash>'
WHERE [Name] = 'WebUrl'

``` 

If you do not update this setting, charts and other visualizations may not render correctly when emailed or exported. This will also be evident in the log files as shown below:

`[ERROR][ExportingLogic ] Convert to image:
System.Exception: HTML load error. The remote content was not found at the server - HTTP error 404`

</br>

### Authentication Routes

Ensure that the AuthValidateAccessTokenUrl and AuthGetAccessTokenUrl values in the IzendaSystemSetting table use the fully qualified path to those API endpoints. 

Examples:

| Name                       | Value                                                     | 
| -------------------------- |:----------------------------------------------------------|
| AuthValidateAccessTokenUrl |http://localhost:14809/api/account/validateIzendaAuthToken |
| AuthGetAccessTokenUrl      |http://localhost:14809/api/GetIzendaAccessToken            |

</br>

You can use the script below to accomplish this. As general best practice, we recommend backing up your database before making any manual updates.

```sql

UPDATE [IzendaSystemSetting]
SET [Value] = '<your url here>'
WHERE [Name] = 'AuthValidateAccessTokenUrl'

UPDATE [IzendaSystemSetting]
SET [Value] = '<your url here>'
WHERE [Name] = 'AuthGetAccessTokenUrl'

``` 

:no_entry: If these values are not set, the authentication will not work properly.
