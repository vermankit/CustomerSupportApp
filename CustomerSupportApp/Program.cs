
using CustomerSupportApp.Model;
using CustomerSupportApp.Services;
using CustomerSupportApp.Services.Interface;

namespace CustomerSupportApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllersWithViews();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<Queue<UserSession>>();
            builder.Services.AddSingleton<List<ChatSession>>();
            var agent = InitializeAgents();
            builder.Services.AddSingleton(agent);
            builder.Services.AddSingleton<IAgentService,AgentService>();
            builder.Services.AddSingleton<ISessionManagerService, SessionManagerService>();
            builder.Services.AddHostedService<ShiftManagerService>();
            builder.Services.AddHostedService<AgentCoordinatorService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
               ;
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();


            app.MapControllerRoute(
                name: "default",
                pattern: "{controller}/{action=Index}/{id?}");

            app.MapFallbackToFile("index.html");

            app.Run();
        }

        private static List<Agent> InitializeAgents()
        {
            return new List<Agent>
                        {

                            // In real it will come from database

                            // Team A
                            new() { Id = "Agent1", IsAvailable = false, Seniority = Seniority.Lead, Team = "TeamA", Shift = "Morning" },
                            new() { Id = "Agent2", IsAvailable = false, Seniority = Seniority.Mid, Team = "TeamA", Shift = "Morning" },
                            new() { Id = "Agent3", IsAvailable = false, Seniority = Seniority.Mid, Team = "TeamA", Shift = "Morning" },
                            new() { Id = "Agent4", IsAvailable = false, Seniority = Seniority.Junior, Team = "TeamA", Shift = "Morning" },

                            // Team B
                            new() { Id = "Agent5", IsAvailable = false, Seniority = Seniority.Senior, Team = "TeamB", Shift = "Afternoon" },
                            new() { Id = "Agent6", IsAvailable = false, Seniority = Seniority.Mid, Team = "TeamB", Shift = "Afternoon" },
                            new() { Id = "Agent7", IsAvailable = false, Seniority = Seniority.Junior, Team = "TeamB", Shift = "Afternoon" },
                            new() { Id = "Agent8", IsAvailable = false, Seniority = Seniority.Junior, Team = "TeamB", Shift = "Afternoon" }, 

                            // Team C (Night Shift)
                            new() { Id = "Agent9", IsAvailable = false, Seniority = Seniority.Mid, Team = "TeamC", Shift = "Night" },
                            new() { Id = "Agent10", IsAvailable = false, Seniority = Seniority.Mid, Team = "TeamC", Shift = "Night" },

                            // Overflow Team
                            new() { Id = "Agent11", IsAvailable = false, Seniority = Seniority.Junior, Team = "Overflow", Shift = "Morning,Afternoon" },
                            new() { Id = "Agent12", IsAvailable = false, Seniority = Seniority.Junior, Team = "Overflow", Shift = "Morning,Afternoon" },
                            new() { Id = "Agent13", IsAvailable = false, Seniority = Seniority.Junior, Team = "Overflow", Shift = "Morning,Afternoon" },
                            new() { Id = "Agent14", IsAvailable = false, Seniority = Seniority.Junior, Team = "Overflow", Shift = "Morning,Afternoon" },
                            new() { Id = "Agent15", IsAvailable = false, Seniority = Seniority.Junior, Team = "Overflow", Shift = "Morning,Afternoon" },
                            new() { Id = "Agent16", IsAvailable = false, Seniority = Seniority.Junior, Team = "Overflow", Shift = "Morning,Afternoon" },
                        };
        }
    }
}