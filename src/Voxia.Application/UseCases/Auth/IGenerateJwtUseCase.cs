using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxia.Domain.DTOs;
using Voxia.Domain.Entities;

namespace Voxia.Application.UseCases.Auth;
public interface IGenerateJwtUseCase
{
    JwtTokenDto Execute(Usuario usuario);
}
