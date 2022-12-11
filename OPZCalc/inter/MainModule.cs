using inter.Views;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inter
{
    public class MainModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
   //         var region = containerProvider.Resolve<IRegionManager>();
    //        region.RegisterViewWithRegion("ContentRegion", typeof(Calculator));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
