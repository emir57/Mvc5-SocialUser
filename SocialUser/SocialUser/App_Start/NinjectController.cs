using BusinessLayer.DependencyResolvers.Ninject;
using BusinessLayer.Utilities;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SocialUser.App_Start
{
    public class NinjectController : DefaultControllerFactory
    {
        private IKernel _kernel;
        public NinjectController()
        {
            _kernel = new StandardKernel(new BusinessModule());
        }
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)_kernel.Get(controllerType);
        }
    }
}