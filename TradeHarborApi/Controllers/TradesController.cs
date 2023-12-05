using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TradeHarborApi.Data;
using TradeHarborApi.Models;
using TradeHarborApi.Services;

namespace TradeHarborApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TradesController : ControllerBase
    {
        private readonly TradeService _tradeService;
        private readonly AuthService _authService;

        public TradesController(TradeService tradeService, AuthService authService)
        {
            _tradeService = tradeService;
            _authService = authService;
        }


        [HttpGet]
        public async Task<IEnumerable<TradePost>> GetTrades()
        {
            var tradePosts = await _tradeService.GetTrades();
            return tradePosts;
        }

        [HttpPost]
        public async Task CreateTradePost([FromBody] CreateTradePostRequest request)
        {
            await _tradeService.CreateTradePost(request);
        }
    }
}