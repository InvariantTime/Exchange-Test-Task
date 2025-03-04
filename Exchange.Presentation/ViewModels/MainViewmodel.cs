namespace Exchange.Presentation.ViewModels
{
    public class MainViewmodel
    {
        public ProfileViewModel Profile { get; }

        public RestViewModel Rest { get; }

        public SocketViewModel Socket { get; }

        public MainViewmodel(ProfileViewModel profile, RestViewModel rest, SocketViewModel socket)
        {
            Profile = profile;
            Rest = rest;
            Socket = socket;
        }
    }
}
