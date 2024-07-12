namespace basicApp
{
    public interface IAuthenticationService
    {
        User Authenticate(string email, string password);
    }

    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _context;

        public AuthenticationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public User Authenticate(string email, string password)
        {
            var user = _context.Users.SingleOrDefault(u => u.Email == email);

            // Password check
            if (user != null && VerifyPasswordHash(password, user.PasswordHash))
            {
                return user; // Authentication successful
            }

            return null; // Authentication failed
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            return password == storedHash;
        }
    }
}
