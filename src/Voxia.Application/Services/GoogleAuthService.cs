using Google.Apis.Auth;

namespace Voxia.Application.Services
{
    public class GoogleAuthService
    {
        private readonly string _clientId;

        public GoogleAuthService(string clientId)
        {
            _clientId = clientId;
        }

        public async Task<GoogleJsonWebSignature.Payload?> ValidateTokenAsync(string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new[] { _clientId }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
                return payload;
            }
            catch
            {
                return null;
            }
        }
    }
}
