using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarHub.Armoury.GodMode.Modules.DataAccess.Views
{
    using ViewModels;

    public static class ViewModelLocator
    {
        public static RemoteDataIndexViewModel RemoteDataIndexViewModel => Resolve<RemoteDataIndexViewModel>();

        private static TService Resolve<TService>() where TService : class => null;
    }
}
