using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxia.Domain.DTOs;
public class JwtTokenDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}
