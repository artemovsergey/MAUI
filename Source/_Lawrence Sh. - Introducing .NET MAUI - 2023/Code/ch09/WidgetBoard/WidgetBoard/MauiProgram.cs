﻿using Microsoft.Extensions.Logging;
using WidgetBoard.Data;
using WidgetBoard.Pages;
using WidgetBoard.ViewModels;
using WidgetBoard.Views;

namespace WidgetBoard;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.Services.AddTransient<AppShell>();

        AddPage<BoardDetailsPage, BoardDetailsPageViewModel>(builder.Services, "boarddetails");
        AddPage<FixedBoardPage, FixedBoardPageViewModel>(builder.Services, "fixedboard");

        builder.Services.AddTransient<AppShellViewModel>();

		builder.Services.AddSingleton(FileSystem.Current);
        builder.Services.AddSingleton(Preferences.Default);
        builder.Services.AddSingleton(SecureStorage.Default);
        builder.Services.AddSingleton(SemanticScreenReader.Default);

        builder.Services.AddSingleton<WidgetFactory>();
        builder.Services.AddSingleton<WidgetTemplateSelector>();

        builder.Services.AddTransient<IBoardRepository, LiteDBBoardRepository>();

        WidgetFactory.RegisterWidget<ClockWidgetView, ClockWidgetViewModel>("Clock");
        builder.Services.AddTransient<ClockWidgetView>();
        builder.Services.AddTransient<ClockWidgetViewModel>();

        return builder.Build();
	}

    private static IServiceCollection AddPage<TPage, TViewModel>(
        IServiceCollection services,
        string route)
        where TPage : Page
        where TViewModel : BaseViewModel
    {
        services
            .AddTransient(typeof(TPage))
            .AddTransient(typeof(TViewModel));

        Routing.RegisterRoute(route, typeof(TPage));

        return services;
    }
}
