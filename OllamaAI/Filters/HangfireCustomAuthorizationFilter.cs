using Hangfire.Dashboard;

public class HangfireCustomAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // Add your authorization logic here
        // For development, return true
        // For production, implement proper authentication
        return true;
    }
} 