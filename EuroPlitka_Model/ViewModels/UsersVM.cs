namespace EuroPlitka_Model.ViewModels
{
    public class UsersVM
    {
        public AplicationUser aplicationUser { get; set; }
        public ChangeRoleViewModel ChangeRoles { get; set; }
        public IEnumerable<AplicationUser> aplicationUsers { get; set; }

        
    }
}
