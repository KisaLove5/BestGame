using BestGameUI;
using Microsoft.Extensions.DependencyInjection;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // 1) создаём коллекцию и регистрируем всё, что нужно приложению
        var services = ConfigureServices();

        // 2) строим провайдер
        using var provider = services.BuildServiceProvider();

        // 3) получаем корневую форму из DI
        var mainForm = provider.GetRequiredService<MainMenuForm>();
        Application.Run(mainForm);
    }

    private static IServiceCollection ConfigureServices()
    {
        var services = new ServiceCollection();

        // ---- CORE-зависимости -------------------------------
        services.AddSingleton<IRandomService, RandomService>();               // Infrastructure
        services.AddSingleton<IArmyFactory, RegistryArmyFactory>();           // Infrastructure

        //  Автоматически зарегистрируем все Unit-builders
        services.Scan(scan => scan
            .FromAssemblyOf<SwordsmanBuilder>()   // любая сборка Infrastructure
            .AddClasses(c => c.AssignableTo<IUnitBuilder>())
            .AsImplementedInterfaces()
            .WithSingletonLifetime());

        // ---- UI-формы ---------------------------------------
        // Важное правило: Forms → Transient (или Singleton, если точно одна)
        services.AddTransient<MainMenuForm>();
        services.AddTransient<ArmyBuildForm>();

        return services;
    }
}
