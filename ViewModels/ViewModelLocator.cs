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
              .AddSingleton<NagCodeViewModel>()
              .AddSingleton<EditViewModel>()
              .BuildServiceProvider());
        }

        // Returns singleton Models
        public NagCodeViewModel NagCodeViewModel => Ioc.Default.GetService<NagCodeViewModel>();
        public EditViewModel EditViewModel => Ioc.Default.GetService<EditViewModel>();
    }
}
 