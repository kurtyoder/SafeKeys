using Caliburn.Micro;
using SafeKeys.EventModels;
using SafeKeys.Library.API;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SafeKeys.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<OpenPasswordEditorEvent>, IHandle<OpenPasswordsViewEvent>, 
        IHandle<OpenCategoryEditorEvent>, IHandle<LogoutEvent>, IHandle<OpenAccountEditorEvent>, IHandle<OpenLoginEvent>,
        IHandle<OpenSettingsEvent>
    {
        private readonly IEventAggregator events;
        private readonly IApiHelper apiHelper;

        public ShellViewModel(IEventAggregator events, IApiHelper apiHelper)
        {
            this.events = events;
            this.apiHelper = apiHelper;

            //So event aggregator knows to send us events
            this.events.SubscribeOnPublishedThread(this);

            //IoC allows us to contact the simple container to bring in an instance
            _ = ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
        }

        public async Task HandleAsync(OpenPasswordEditorEvent message, CancellationToken cancellationToken)
        {
            //Load password editor
            KeyEditorViewModel editor = IoC.Get<KeyEditorViewModel>();
            editor.LoadKey(message.key);

            await ActivateItemAsync(editor, new CancellationToken());
        }

        public async Task HandleAsync(OpenPasswordsViewEvent message, CancellationToken cancellationToken)
        {
            
            await ActivateItemAsync(IoC.Get<KeysViewModel>(), new CancellationToken());
        }

        public async Task HandleAsync(OpenCategoryEditorEvent message, CancellationToken cancellationToken)
        {
            
            await ActivateItemAsync(IoC.Get<CategoryEditorViewModel>(), new CancellationToken());
        }

        public async Task HandleAsync(LogoutEvent message, CancellationToken cancellationToken)
        {
            apiHelper.Logout();
            await ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
        }

        public async Task HandleAsync(OpenAccountEditorEvent message, CancellationToken cancellationToken)
        {
            
            await ActivateItemAsync(IoC.Get<NewAccountViewModel>(), new CancellationToken());
        }

        public async Task HandleAsync(OpenLoginEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(IoC.Get<LoginViewModel>(), new CancellationToken());
        }

        public async Task HandleAsync(OpenSettingsEvent message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(IoC.Get<SettingsViewModel>(), new CancellationToken());
        }
    }
}