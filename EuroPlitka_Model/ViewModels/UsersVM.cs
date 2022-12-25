namespace EuroPlitka_Model.ViewModels
{
    public class UsersVM
    {
        public AplicationUser? aplicationUser { get; set; }

        public string? OldPasswd { get; set; } = string.Empty;
        public ChangeRoleViewModel? ChangeRoles { get; set; }
        public IEnumerable<AplicationUser>? aplicationUsers { get; set; }
        public EditUserVM? EditUserVM { get; set; }

        public UsersVM()
        {
           EditUserVM =  new EditUserVM();
            
        }


    }
}
