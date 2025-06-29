<div align="center">
  <a href="https://github.com/antonio-backnotfront/linear-regression/stargazers">
    <img src="https://img.shields.io/github/stars/antonio-backnotfront/linear-regression?style=for-the-badge" alt="GitHub stars">
  </a>
  <a href="https://github.com/antonio-backnotfront/linear-regression/issues">
    <img src="https://img.shields.io/github/issues/antonio-backnotfront/linear-regression.svg?style=for-the-badge" alt="GitHub issues">
  </a>
  <a href="https://github.com/antonio-backnotfront/linear-regression/blob/main/LICENSE.txt">
    <img src="https://img.shields.io/github/license/antonio-backnotfront/linear-regression.svg?style=for-the-badge" alt="License">
  </a>
<br>
<a href="https://linkedin.com/in/anton-solianyk-906453221">
  <img src="https://img.shields.io/badge/🔗%20LinkedIn-Connect-blue?style=for-the-badge&logo=linkedin&logoColor=white" alt="LinkedIn">
</a>

  <a href="mailto:solyanicks@gmail.com">
    <img src="https://img.shields.io/badge/Email-solyanicks%40gmail.com-D14836?style=for-the-badge&logo=gmail&logoColor=white" alt="Gmail">
  </a>
</div>




<h1 align="center">💰 Revenue Recognition System</h1>


This application tackles a classic challenge in finance: the **revenue recognition problem**.

> Revenue recognition is about determining when you can legitimately record received money in your books. For straightforward sales—like selling a cup of coffee—you record the transaction immediately. But with complex contracts, it's not that simple.

Imagine you pay a retainer upfront for a year of service. Even if the full amount is paid today, you can't recognize all that revenue immediately since the service unfolds over months. Often, revenue is recognized proportionally (e.g., one-twelfth monthly), accounting for scenarios like early contract cancellations.

The rules governing revenue recognition are diverse and ever-changing—shaped by regulations, accounting standards, and company policies—making accurate tracking a complex but crucial task.

Historically, flawed revenue recognition has fueled major corporate scandals, including Enron and WorldCom, causing severe financial and legal repercussions. Reliable revenue recognition systems ensure transparency and uphold trust in financial markets.

---

<h2 align="center"> 📑 Table of Contents </h2>

<p align="center">
    <a href="https://github.com/antonio-backnotfront/revenue-recognition?tab=readme-ov-file#-database-schema">Database Schema</a> •
    <a href="https://github.com/antonio-backnotfront/revenue-recognition?tab=readme-ov-file#-api-endpoints">API Endpoints</a> •
    <a href="https://github.com/antonio-backnotfront/revenue-recognition?tab=readme-ov-file#-request-and-response-bodies">Request and Response Bodies</a> •
    <a href="https://github.com/antonio-backnotfront/revenue-recognition?tab=readme-ov-file#-usage">Usage</a> •
    <a href="https://github.com/antonio-backnotfront/revenue-recognition?tab=readme-ov-file#-technologies">Technologies</a> •
    <a href="https://github.com/antonio-backnotfront/revenue-recognition?tab=readme-ov-file#-license">Licence</a>

</p>


---

## 📌 Database Schema

![Database Schema](.github/image/revenue-recognition-schema.png)

---

## 🚀 API Endpoints

<!-- List your API endpoints here, e.g.: -->
- `POST /contracts` – Create a new contract

---

## 📬 Request and Response Bodies

### Example: Create Contract Request

```json
```

---

## 📝 Usage
> 1) Clone the repository 
```bash
   git clone https://github.com/antonio-backnotfront/revenue-recognition.git
```  
<br><br>
> 2) Create appsettings.json in the API folder and your database connection string according to this template:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultDatabase": "server = your server; user id = your user id; password = your password; TrustServerCertificate = True; database= your database"
  },
  "CurrencyApi": {
    "key": "your api key"
  },
  "JwtConfig": {
    "Issuer": "issuer",
    "Audience": "audience",
    "Key": "unique key consisting of 32 chars [a-zA-Z]",
    "ValidInMinutes": "integer"
  }
}


```

---
## 💻 Technologies

---

## 📄 License