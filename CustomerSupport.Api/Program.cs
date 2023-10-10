using CustomerSupport.Api.Common;
using CustomerSupport.Api.Entity;
using CustomerSupport.Api.Hub;
using CustomerSupport.Api.Services;
using CustomerSupport.Api.Services.HostedService;
using CustomerSupport.Api.Services.Interface;

namespace CustomerSupport.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost", builder =>
                {
                    builder.WithOrigins("http://127.0.0.1:5173")
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            builder.Services.AddSignalR();
            //builder.Services.AddSingleton<Queue<UserSession>>();
            //builder.Services.AddSingleton<List<ChatSession>>();
            var agent = InitializeAgents();
            builder.Services.AddSingleton(agent);
            builder.Services.AddSingleton<IAgentService, AgentService>();
            builder.Services.AddSingleton<ISessionManagerService, SessionManagerService>();
            builder.Services.AddHostedService<ShiftManagerService>();
            builder.Services.AddHostedService<AgentCoordinatorService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            // Allow CORS for all endpoints
            app.UseCors("AllowLocalhost");
            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapHub<ChatHub>("/Chathub");
            app.MapControllers();

            app.Run();
        }
        private static List<Agent> InitializeAgents()
        {
            return new List<Agent>
                        {

                            // In real it will come from database

                            // Team A
                            new() { Name = "Agent1", IsPresent = false, Seniority = Seniority.Lead, Team = "TeamA", Shift = "Morning" },
                            new() { Name = "Agent2", IsPresent = false, Seniority = Seniority.Mid, Team = "TeamA", Shift = "Morning" },
                            new() { Name = "Agent3", IsPresent = false, Seniority = Seniority.Mid, Team = "TeamA", Shift = "Morning" },
                            new() { Name = "Agent4", IsPresent = false, Seniority = Seniority.Junior, Team = "TeamA", Shift = "Morning" },

                            // Team B
                            new() { Name = "Agent5", IsPresent = false, Seniority = Seniority.Senior, Team = "TeamB", Shift = "Afternoon" },
                            new() { Name = "Agent6", IsPresent = false, Seniority = Seniority.Mid, Team = "TeamB", Shift = "Afternoon" },
                            new() { Name = "Agent7", IsPresent = false, Seniority = Seniority.Junior, Team = "TeamB", Shift = "Afternoon" },
                            new() { Name = "Agent8", IsPresent = false, Seniority = Seniority.Junior, Team = "TeamB", Shift = "Afternoon" }, 

                            // Team C (Night Shift)
                            new() { Name = "Agent9", IsPresent = false, Seniority = Seniority.Mid, Team = "TeamC", Shift = "Night" },
                            new() { Name = "Agent10", IsPresent = false, Seniority = Seniority.Mid, Team = "TeamC", Shift = "Night" },

                            // Overflow Team
                            new() { Name = "Agent11", IsPresent = false, Seniority = Seniority.Junior, Team = "Overflow", Shift = "Morning,Afternoon" },
                            new() { Name = "Agent12", IsPresent = false, Seniority = Seniority.Junior, Team = "Overflow", Shift = "Morning,Afternoon" },
                            new() { Name = "Agent13", IsPresent = false, Seniority = Seniority.Junior, Team = "Overflow", Shift = "Morning,Afternoon" },
                            new() { Name = "Agent14", IsPresent = false, Seniority = Seniority.Junior, Team = "Overflow", Shift = "Morning,Afternoon" },
                            new() { Name = "Agent15", IsPresent = false, Seniority = Seniority.Junior, Team = "Overflow", Shift = "Morning,Afternoon" },
                            new() { Name = "Agent16", IsPresent = false, Seniority = Seniority.Junior, Team = "Overflow", Shift = "Morning,Afternoon" },
                        };
        }
    }
}