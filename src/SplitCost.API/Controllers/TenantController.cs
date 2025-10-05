//using Microsoft.AspNetCore.Mvc;
//using SplitCost.Application.Common.Interfaces;
//using SplitCost.Application.Common.Repositories;
//using SplitCost.Application.Dtos;
//using SplitCost.Domain.Entities;

//namespace SplitCost.API.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//public class TenantController : ControllerBase
//{
//    private readonly ITenantService _tenantService;
//    private readonly IMemberRepository _memberRepository;
//    private readonly IResidenceRepository _residenceRepository;

//    public TenantController(
//        ITenantService tenantService,
//        IMemberRepository memberRepository,
//        IResidenceRepository residenceRepository)
//    {
//        _tenantService = tenantService;
//        _memberRepository = memberRepository;
//        _residenceRepository = residenceRepository;
//    }

//    /// <summary>
//    /// Resolve a residência do usuário no primeiro login.
//    /// Se não houver residência, cria uma nova ou entra em uma existente via código.
//    /// </summary>
//    [HttpPost("resolve")]
//    public async Task<IActionResult> ResolveTenant([FromBody] ResolveTenantInput input)
//    {
//        var userId = _tenantService.GetCurrentUserId();

//        // Verifica se o usuário já está associado a alguma residência
//        var existingMember = await _memberRepository.GetByExpression(x => x.UserId == userId);
//        if (existingMember != null)
//        {
//            // Já possui residência
//            _tenantService.SetCurrentTenantId(existingMember.ResidenceId);
//            return Ok(new { existingMember.ResidenceId });
//        }

//        Residence residence = null!;

//        if (!string.IsNullOrWhiteSpace(input.InviteCode))
//        {
//            // Entrar em residência existente via código
//            residence = await _residenceRepository.GetByInviteCodeAsync(input.InviteCode);
//            if (residence == null)
//                return BadRequest("Código de residência inválido.");
//        }
//        else
//        {
//            // Criar nova residência
//            if (string.IsNullOrWhiteSpace(input.ResidenceName))
//                return BadRequest("Nome da residência é obrigatório para criar uma nova.");

//            residence = new Residence(
//                input.ResidenceName,
//                userId,
//                input.Street ?? string.Empty,
//                input.Number ?? string.Empty,
//                input.Apartment ?? string.Empty,
//                input.City ?? string.Empty,
//                input.Prefecture ?? string.Empty,
//                input.Country ?? string.Empty,
//                input.PostalCode ?? string.Empty
//            );

//            await _residenceRepository.AddAsync(residence);
//        }

//        // Cria o Member que vincula usuário à residência
//        var member = new Member(DateTime.UtcNow)
//            .SetUserId(userId)
//            .SetResidenceId(residence.Id);

//        await _memberRepository.AddAsync(member);

//        // Seta o tenant atual
//        _tenantService.SetCurrentTenantId(residence.Id);

//        return Ok(new { residence.Id, residence.Name });
//    }
//}

