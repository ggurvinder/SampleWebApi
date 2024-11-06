namespace SampleWebApi.Data.Repositories
{

    public interface ICurrentUser
    {
        void GetLoggedInUser();

    }
    public class CurrentUser : ICurrentUser
    {
        public void GetLoggedInUser()
        {

            throw new NotImplementedException();
        }
    }
}
