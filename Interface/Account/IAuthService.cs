using VigilanceClearance.Models.DTOs;
using VigilanceClearance.Models.Modal_Properties.Account;

namespace VigilanceClearance.Interface.Account
{
    public interface IAuthService
    {
        Task<TokenResponse?> LoginAsync(LoginDto loginDto);
    }
}
