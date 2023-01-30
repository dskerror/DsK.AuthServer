using Microsoft.AspNetCore.Components;

namespace BlazorWASMCustomAuth.Client.Components
{
    public partial class Notification
    {
        private string _modalDisplay;
        private string _modalClass;
        private string _modalTitle;
        private string _modalBody;
        private bool _showBackdrop;
        [Inject] public NavigationManager Navigation { get; set; }
        public void Show(string header, string message)
        {
            _modalTitle = header;
            _modalBody = message;
            _modalDisplay = "block;";
            _modalClass = "show";
            _showBackdrop = true;
            StateHasChanged();
        }
        private void Hide()
        {
            _modalTitle = "";
            _modalBody = "";
            _modalDisplay = "none;";
            _modalClass = "";
            _showBackdrop = false;
            StateHasChanged();
        }

        private void HideAndNavigateTo(string route)
        {
            _modalTitle = "";
            _modalBody = "";
            _modalDisplay = "none;";
            _modalClass = "";
            _showBackdrop = false;
            StateHasChanged();
            Navigation.NavigateTo(route);
        }
    }
}
