namespace NagCode.ViewModels
{
    using CommunityToolkit.Mvvm.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection;

    public class ViewModelLocator
    {
        static ViewModelLocator()
        {

            Ioc.Default.ConfigureServices(
              new ServiceCollection()
              .AddSingleton<NageCodeModel>()
              .AddSingleton<EditViewModel>()
              .BuildServiceProvider());
        }

        // Returns singleton Models
        public NageCodeModel MainViewModel => Ioc.Default.GetService<NageCodeModel>();
        public EditViewModel EditViewModel => Ioc.Default.GetService<EditViewModel>();
    }
}
