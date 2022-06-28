using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SlipeServer.Server;
using SlipeServer.Server.Elements;
using SlipeServer.Server.ServerBuilders;
using SlipeServer.Server.Loggers;

namespace SlipeServer.BlankTemplate;

public partial class Program
{
    public static void Main(string[] args)
    {
        Program? program = null;
        try
        {
            program = new Program(args);
            program.Start();
        }
        catch (Exception exception)
        {
            if (program != null)
            {
                program.Logger.LogCritical(exception, "{message}", exception.Message);
            } else
            {
                System.Console.WriteLine($"Error in startup {exception.Message}");
            }
            System.Console.WriteLine("Press any key to exit...");
            System.Console.ReadKey();
        }
    }

    private readonly EventWaitHandle waitHandle = new(false, EventResetMode.AutoReset);
    private readonly MtaServer server;
    private readonly Configuration configuration;

    public ILogger Logger { get; }

    public Program(string[] args)
    {
        this.configuration =  new Configuration()
        {
            IsVoiceEnabled = true
        };

        this.server = MtaServer.CreateWithDiSupport<Player>(
            (builder) =>
            {
                builder.UseConfiguration(this.configuration);

#if DEBUG
                builder.AddDefaults(exceptBehaviours: ServerBuilderDefaultBehaviours.MasterServerAnnouncementBehaviour);
                builder.AddNetWrapper(dllPath: "net_d", port: (ushort)(this.configuration.Port + 1));
#else
                    builder.AddDefaults();
#endif

                builder.ConfigureServices(services =>
                {
                    services.AddSingleton<ILogger, ConsoleLogger>();
                });
            }
        );

        this.server.GameType = "Slipe Server";
        this.server.MapName = "N/A";

        this.Logger = this.server.GetRequiredService<ILogger>();

        System.Console.CancelKeyPress += (sender, args) =>
        {
            this.server.Stop();
            this.waitHandle.Set();
        };
    }

    public void Start()
    {
        this.server.Start();
        this.Logger.LogInformation("Server started.");
        this.waitHandle.WaitOne();
    }
}
