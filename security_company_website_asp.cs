// PROJECT: SecurityCompany_Website
// Minimal ASP.NET Core 7 project with static frontend (wwwroot)
// Files included below. Save each block into the matching path and run with `dotnet run`.

// FILE: SecurityCompany_Website.csproj
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net7.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
</Project>

// FILE: Program.cs
using Microsoft.AspNetCore.Mvc;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();

// FILE: Controllers/ContactController.cs
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    [HttpPost]
    public IActionResult Post(ContactRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name) || string.IsNullOrWhiteSpace(req.Email))
            return BadRequest(new { message = "Name and Email are required." });

        // TODO: Replace with real email sending or database persistence
        Console.WriteLine($"Contact request from {req.Name} <{req.Email}>: {req.Message}");

        return Ok(new { message = "Thanks — we received your request." });
    }
}

// FILE: Controllers/HealthController.cs
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok(new { status = "healthy", time = DateTime.UtcNow });
}

// FILE: Models/ContactRequest.cs
public class ContactRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

// FILE: wwwroot/index.html
<!doctype html>
<html lang="en">
<head>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1" />
  <title>Chicago SecureTech — Security Camera & Access Control</title>
  <link rel="stylesheet" href="/css/site.css" />
</head>
<body>
  <header class="hero">
    <div class="container">
      <h1>Chicago SecureTech</h1>
      <p>Professional security camera installation, low-voltage cabling, access control & electronic door programming.</p>
      <a class="cta" href="#contact">Get a Quote</a>
    </div>
  </header>

  <main class="container">
    <section id="services">
      <h2>Our Services</h2>
      <div class="grid">
        <article>
          <h3>Security Camera Installation</h3>
          <p>Indoor/outdoor CCTV, IP cameras, NVR/DVR setup, remote monitoring.</p>
        </article>
        <article>
          <h3>Low-Voltage Cabling</h3>
          <p>Cat6/Cat6a backbone, fiber patching, neat labeling and documentation.</p>
        </article>
        <article>
          <h3>Access Control Systems</h3>
          <p>Design and install access control panels, readers, and integrations.</p>
        </article>
        <article>
          <h3>Fob / Key Card Systems</h3>
          <p>Card programming, credential management, system provisioning.</p>
        </article>
        <article>
          <h3>Electronic Door Lock Programming</h3>
          <p>Smart locks, door controllers, scheduling and event logging.</p>
        </article>
      </div>
    </section>

    <section id="about">
      <h2>Why Choose Us</h2>
      <ul>
        <li>Licensed & Insured technicians</li>
        <li>Transparent pricing & free site surveys</li>
        <li>Commercial & residential experience</li>
      </ul>
    </section>

    <section id="contact">
      <h2>Contact Us</h2>
      <form id="contactForm">
        <label>Name<input type="text" id="name" required></label>
        <label>Email<input type="email" id="email" required></label>
        <label>Phone<input type="tel" id="phone"></label>
        <label>Message<textarea id="message"></textarea></label>
        <button type="submit">Send Request</button>
        <p id="formStatus" aria-live="polite"></p>
      </form>
    </section>
  </main>

  <footer class="site-footer">
    <div class="container">
      <p>© <span id="year"></span> Chicago SecureTech — All rights reserved.</p>
    </div>
  </footer>

  <script src="/js/site.js"></script>
</body>
</html>

// FILE: wwwroot/css/site.css
:root{--accent:#0b5cff;--bg:#f7f8fb;--card:#ffffff}
*{box-sizing:border-box}
body{font-family:Inter,system-ui,Segoe UI,Roboto,Arial,sans-serif;margin:0;background:var(--bg);color:#111}
.container{max-width:980px;margin:0 auto;padding:24px}
.hero{background:linear-gradient(135deg,#0b5cff20,#0bffe020);padding:48px 0;text-align:center}
.hero h1{margin:0;font-size:2.2rem}
.cta{display:inline-block;margin-top:12px;padding:10px 18px;border-radius:8px;background:var(--accent);color:white;text-decoration:none}
.grid{display:grid;grid-template-columns:repeat(auto-fit,minmax(220px,1fr));gap:16px}
article{background:var(--card);padding:16px;border-radius:10px;box-shadow:0 6px 18px rgba(10,10,10,0.06)}
#contactForm{display:grid;gap:8px;max-width:520px}
#contactForm label{display:flex;flex-direction:column;font-size:0.9rem}
#contactForm input,#contactForm textarea{padding:8px;border-radius:6px;border:1px solid #ddd}
button[type=submit]{padding:10px 14px;border:none;border-radius:8px;background:var(--accent);color:white;cursor:pointer}
.site-footer{padding:18px 0;text-align:center;color:#555}

// FILE: wwwroot/js/site.js
document.getElementById('year').textContent = new Date().getFullYear();
const form = document.getElementById('contactForm');
const status = document.getElementById('formStatus');
form.addEventListener('submit', async (e)=>{
  e.preventDefault();
  status.textContent = 'Sending...';
  const payload = {
    name: document.getElementById('name').value,
    email: document.getElementById('email').value,
    phone: document.getElementById('phone').value,
    message: document.getElementById('message').value
  };
  try{
    const res = await fetch('/api/contact', {method:'POST',headers:{'Content-Type':'application/json'},body:JSON.stringify(payload)});
    const json = await res.json();
    if (!res.ok) throw new Error(json?.message || 'Network error');
    status.textContent = json.message || 'Message sent!';
    form.reset();
  }catch(err){
    status.textContent = 'Error: ' + err.message;
  }
});

// FILE: README.md
# Chicago SecureTech - Minimal ASP.NET Core Website

## How to run
1. Install .NET 7 SDK: https://dotnet.microsoft.com/download
2. Save all files preserving folder structure (Controllers, Models, wwwroot...)
3. In project folder run:
   ```bash
   dotnet restore
   dotnet run
   ```
4. Open http://localhost:5000 (or the URL shown in console)

## Next steps / Customization ideas
- Integrate real email (SMTP) or SendGrid in ContactController
- Add database (SQLite / SQL Server) for leads
- Add authentication for admin UI to manage jobs/credentials
- Add portfolio, pricing, and testimonials sections

// END OF PROJECT
