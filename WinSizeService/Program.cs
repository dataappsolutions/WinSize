using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Text.Json;
using WinSize;

namespace WinSizeService;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var builder = WebApplication.CreateBuilder(args);
        //builder.WebHost.UseUrls("http://localhost:5250");
        builder.WebHost.UseContentRoot(AppContext.BaseDirectory);
        var app = builder.Build();

        app.Urls.Add("http://localhost:5250");
        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.MapGet("/api/windows", () =>
                WinSizeShared.GetOpenWindows()
                .Select(w => new { 
                    handle = w.Handle.ToInt64(),
                    title = w.Title, 
                    processName = w.ProcessName
                }));

        //app.MapPost("/api/resize", (ResizeRequest req) =>
        //{
        //    var hWnd = new IntPtr(req.Handle);
        //    var ok = WinSizeShared.TryResizeWindow(hWnd, req.Width, req.Height, out int err);
        //    return ok ? Results.Ok() : Results.NotFound();
        //});

        app.MapPost("/api/resize", async (HttpContext ctx) =>
        {
            var opts = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var req = await ctx.Request.ReadFromJsonAsync<ResizeRequest>(opts);
            if (req is null) return Results.BadRequest("Invalid request body.");
            var hWnd = new IntPtr(req.Handle);
            var ok = WinSizeShared.TryResizeWindow(hWnd, req.Width, req.Height, out int err, req.Center);
            return ok ? Results.Ok() : Results.Problem($"Win32 error: {err}");
        });

        app.MapPost("/api/ping", () => Results.Ok("pong"));

        new Thread(() => app.Run()) { IsBackground = true }.Start();

        using var trayIcon = new NotifyIcon
        {
            Icon = SystemIcons.Application,
            Text = "Winize Service",
            Visible = true
        };

        var menu = new ContextMenuStrip();
        menu.Items.Add("Open in Browser", null, (_, _) =>
            Process.Start(new ProcessStartInfo("Http://localhost:5250") { UseShellExecute = true }));
        menu.Items.Add("Exit", null, (_, _) => Application.Exit());
        trayIcon.ContextMenuStrip = menu;
        trayIcon.DoubleClick += (_, _) =>
           Process.Start(new ProcessStartInfo("http://localhost:5250") { UseShellExecute = true });

        Application.Run();
    }
}

record ResizeRequest(long Handle, int Width, int Height, bool Center);
