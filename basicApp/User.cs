using Microsoft.AspNetCore.Identity;

namespace basicApp
{
    public class User : IdentityUser
    {

        //** This is for displaying any info taken from Atlas database 
        public string Info { get; set; }

        //** these are for registering the credentials when any microsoft account is LINKED. they will be used for authentication when the user logs in with microsoft account
        public string MicrosoftAccountId { get; set; }
        public string MicrosoftAccountEmail { get; set; }
    }
}
