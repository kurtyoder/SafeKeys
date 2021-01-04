using AutoMapper;
using Caliburn.Micro;
using SafeKeys.Library;
using SafeKeys.Library.API;
using SafeKeys.Library.Models;
using SafeKeys.Models;
using SafeKeys.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace SafeKeys
{
    public class Bootstrapper : BootstrapperBase
    {
        //Dependency injection
        private readonly SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();
        }

        private IMapper ConfigureAutoMapper()
        {
            //Add automapper
            //Tell it which models to map together
            //This will use reflection only once when we start application
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<KeyModel, KeyDisplayModel>();
                cfg.CreateMap<CategoryModel, CategoryDisplayModel>();
            });

            //Takes automapper configuration and will create the mapper we can add to dependency inejction
            IMapper mapper = config.CreateMapper();

            return mapper;
        }

        protected override void Configure()
        {
            //This puts the automapper in as a singleton
            _ = _container.Instance(ConfigureAutoMapper());

            //Dependency injection
            _ = _container.Instance(_container)
                .PerRequest<IDataAccess, XamlDataAccess>()
                .PerRequest<ICrypto, Crypto>()
                .PerRequest<ISettingHandler, SettingHandler>()
                .PerRequest<IGenerateStrings, GenerateStrings>()
                ;

            _ = _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()                
                .Singleton<IApiHelper, ApiHelper>()
                ;

            GetType().Assembly.GetTypes()
                 .Where(type => type.IsClass)
                 .Where(type => type.Name.EndsWith("ViewModel"))
                 .ToList()
                 .ForEach(viewModelType => _container.RegisterPerRequest(
                     viewModelType, viewModelType.ToString(), viewModelType));
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            _ = DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}