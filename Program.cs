using Microsoft.EntityFrameworkCore;
using ServerWater.APIs;
using ServerWater.Model;

namespace ServerWater;
public class Program
{
    public static MyRole api_role = new MyRole();
    public static MyUser api_user = new MyUser();
    public static MyCustomer api_customer = new MyCustomer();
    public static MyArea api_area = new MyArea();
    public static MyAction api_action = new MyAction();
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.WebHost.ConfigureKestrel((context, option) => {
            option.ListenAnyIP(50000, listenOptions =>
            {

            });
            option.Limits.MaxConcurrentConnections = 1000;
            option.Limits.MaxRequestBodySize = null;
            option.Limits.MaxRequestBufferSize = null;
        });

        builder.Services.AddCors(options => {
            options.AddPolicy("HTTPSystem", builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().SetIsOriginAllowed(origin => true).WithExposedHeaders("Grpc-Status", "Grpc-Encoding", "Grpc-Accept-Encoding");
            });
        });

        builder.Services.AddSignalR(options =>
        {
            options.EnableDetailedErrors = true;
            options.KeepAliveInterval = TimeSpan.MaxValue;
            options.MaximumReceiveMessageSize = 10 * 1024 * 1024;
            options.StreamBufferCapacity = 10 * 1024 * 1024;
            options.EnableDetailedErrors = true;
        }).AddMessagePackProtocol();

        // Add services to the container.

        builder.Services.AddDbContext<DataContext>(options => options.UseNpgsql(DataContext.configSql));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
        app.UseSwagger();
        app.UseSwaggerUI();
        //}

        app.UseDeveloperExceptionPage();
        app.UseMigrationsEndPoint();

        using (var scope = app.Services.CreateScope())
        {
            IServiceProvider services = scope.ServiceProvider;
            DataContext datacontext = services.GetRequiredService<DataContext>();
            datacontext.Database.EnsureCreated();
            //await datacontext.Database.MigrateAsync();
        }
        await api_role.initAsync();
        await api_user.initAsync();
        await api_area.initAsync();
        await api_customer.initAsync();

        app.UseCors("HTTPSystem");
        app.UseRouting();

        //app.UseHttpsRedirection();

        app.UseAuthorization();
            
        app.MapControllers();
        app.MapGet("/", () => "ServerWater of STVG");
        app.Run();
    }
}

