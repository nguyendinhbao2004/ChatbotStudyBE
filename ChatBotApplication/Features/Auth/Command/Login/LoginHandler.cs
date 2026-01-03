using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatBotApplication.Dto.Auth;
using Domain.Common;
using Domain.Entity;
using Domain.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ChatBotApplication.Features.Auth.Command.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, Result<AuthResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public LoginHandler(UserManager<User> userManager, IJwtTokenGenerator jwtTokenGenerator)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user == null)
                return Result<AuthResponse>.Failure("Invalid email or password.");
            
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);
            if(!isPasswordValid)
                return Result<AuthResponse>.Failure("Invalid email or password.");
            
            var roles = await _userManager.GetRolesAsync(user);

            var accessToken =  _jwtTokenGenerator.GenerateToken(user, roles);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken();
            // 5. Lưu Refresh Token
            // Vì User là Aggregate Root, ta thêm token vào User
            // Lấy Jti từ Access Token để map cặp (nếu cần thiết)
            var jwtId = Guid.NewGuid().ToString(); // Giả sử lấy Jti từ Access Token

            user.AddRefreshToken(refreshToken, jwtId);

            await _userManager.UpdateAsync(user);

            return Result<AuthResponse>.Success(
                "Login successful.",
                new AuthResponse
            {
                Id = user.Id.ToString(),
                FullName = user.FullName,
                Email = user.Email,
                Roles = roles.ToList(),
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }
    }
}