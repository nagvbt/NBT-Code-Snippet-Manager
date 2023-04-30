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
              .AddSingleton<NagCodeModel>()
              .AddSingleton<EditViewModel>()
              .BuildServiceProvider());
        }

        // Returns singleton Models
        public NagCodeModel MainViewModel => Ioc.Default.GetService<NagCodeModel>();
        public EditViewModel EditViewModel => Ioc.Default.GetService<EditViewModel>();
    }
}
 