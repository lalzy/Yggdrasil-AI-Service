// GlobalUsing.cs
// Personally don't like a lot of import statements, especially ones that are normalized for the 'entire' project.
// Such as AspNet in an AspNet program (We may as well have the entire .Net STDLib be gated behind an using as well, it is not, so neither should Aspnet).

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
