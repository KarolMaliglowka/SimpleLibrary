using Library.Application.Services;

namespace Library.Api.EndPoints;

public static class DashboardEndpoints
{
    public static void MapDashboardEndpoints(this WebApplication app)
    {
        app.MapGet("/dashboard", async (IDashboardService dashboardService) =>
        {
            var dashboard = await dashboardService.GetDashboardAsync();
            return Results.Ok(dashboard);
        });
    }
}