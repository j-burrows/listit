using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using System.Web.Routing;
using Ninject.Modules;
using Domain.Abstract;
using Domain.Concrete;
using System.Configuration;

namespace LISTIT.Infastructure
{
    public class NControllerFactory : DefaultControllerFactory
    {
        private IKernel kernel = new StandardKernel(new LISTITServices());

        protected override IController GetControllerInstance(RequestContext context,
                                                            Type controllerType) {
            if(controllerType==null){
                return null;
            }
            return (IController)kernel.Get(controllerType);
        }

        private class LISTITServices:NinjectModule {
            /*
             +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
             |  Load
             +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
             |  Purpose:    If no parameters are passed to the factory, a new controller is
             |              created with bindings to the applications main database.
             +-- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- --- ---
             */
            public override void Load()
            {
                // The item and wlist repository is set to the sql by default, and will be
                // populated with items from the applications main database.
                Bind<IItemRepository>().To<SqlItemRepository>()
                                    .WithConstructorArgument("dbConnection",
                                    ConfigurationManager.ConnectionStrings["AppDb"]
                                    .ConnectionString);
                Bind<IWListRepository>().To<SqlWListRepository>()
                                    .WithConstructorArgument("dbConnection",
                                    ConfigurationManager.ConnectionStrings["AppDb"]
                                    .ConnectionString);
            }
        }
    }
}
