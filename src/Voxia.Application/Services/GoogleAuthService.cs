using Google.Apis.Auth;

namespace Voxia.Application.Services
{
    public class GoogleAuthService
    {
        private readonly string[] _clientIds;

        // Agora aceita vários IDs
        public GoogleAuthService(string[] clientIds)
        {
            _clientIds = clientIds;
        }

        public async Task<GoogleJsonWebSignature.Payload?> ValidateTokenAsync(string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = _clientIds // aceita tokens de qualquer um dos IDs
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
