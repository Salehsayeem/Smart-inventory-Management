using Microsoft.IdentityModel.Tokens;
using Sims.Api.Context;
using Sims.Api.Dto;
using Sims.Api.Dto.AuthDto;
using Sims.Api.IRepositories;
using Sims.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Sims.Api.Helper;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Sims.Api.Dto.Auth;
namespace Sims.Api.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ILogger<AuthRepository> _logger;
        private readonly IConfiguration _configuration;

        private readonly ApplicationDbContext _context;

        public AuthRepository(ILogger<AuthRepository> logger, IConfiguration configuration, ApplicationDbContext context)
        {
            _logger = logger;
            _configuration = configuration;
            _context = context;
        }

        public async Task<CommonResponseDto> Register(RegistrationDto model)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                List<RegisteredShopsDto> shops = new List<RegisteredShopsDto>();
                if (!string.Equals(model.Password, model.ConfirmPassword))
                {
                    return new CommonResponseDto()
                    {
                        Message = "Password doesn't match",
                        Data = null,
                        StatusCode = 400,
                    };
                }
                if (await _context.Users.AnyAsync(u => u.Email == model.Email))
                {
                    return new CommonResponseDto()
                    {
                        Message = "Email is already in use.",
                        Data = null,
                        StatusCode = 400,
                    };
                }
                var user = new User
                {
                    Id = Ulid.NewUlid(),
                    Email = model.Email,
                    PasswordHash = HashPassword(model.Password),
                    RoleId = (int)RoleEnums.Admin,
                };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                if (!string.IsNullOrEmpty(model.BusinessName))
                {
                    var shop = new Shop
                    {
                        Name = model.BusinessName,
                        CreatedBy = user.Id,
                    };

                    await _context.Shops.AddAsync(shop);
                    await _context.SaveChangesAsync();
                    var userShop = new UserShop
                    {
                        RoleId = (int)RoleEnums.Admin,
                        UserId = user.Id,
                        ShopId = shop.Id,
                        CreatedBy = user.Id,
                    };

                    await _context.UserShops.AddAsync(userShop);
                    await _context.SaveChangesAsync();

                    shops.Add(new RegisteredShopsDto
                    {
                        Id = shop.Id,
                        Name = shop.Name,
                        Address = shop.Address,
                    });
                }
                await SetPermissions(user.RoleId, user.Id);

                string token = GenerateJwtToken(user, shops);
                await transaction.CommitAsync();
                return new CommonResponseDto()
                {
                    Message = "User registered successfully.",
                    Data = token,
                    StatusCode = 200,
                };
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw new Exception(e.Message);
            }
        }

        public async Task<CommonResponseDto> Login(LoginDto model)
        {
            try
            {
                var response = new CommonResponseDto();
                var shops = new List<RegisteredShopsDto>();
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == model.Email && u.IsActive);
                if (user == null || !VerifyPassword(model.Password, user.PasswordHash))
                {
                    response.Message = "Invalid email or password.";
                    response.StatusCode = 400;
                    return response;
                }

                if (user.RoleId == (int)RoleEnums.SuperAdmin)
                {
                    shops = await _context.Shops
                       .Select(s => new RegisteredShopsDto
                       {
                           Id = s.Id,
                           Name = s.Name,
                           Address = s.Address,
                       })
                       .ToListAsync();
                }
                else if (user.RoleId == (int)RoleEnums.Admin)
                {
                    shops = await _context.Shops
                       .Where(s => s.CreatedBy == user.Id && s.IsActive)
                       .Select(s => new RegisteredShopsDto
                       {
                           Id = s.Id,
                           Name = s.Name,
                           Address = s.Address,
                       })
                       .ToListAsync();
                }
                else
                {
                    shops = await (from us in _context.UserShops
                                   join s in _context.Shops on us.ShopId equals s.Id
                                   where us.UserId == user.Id && s.IsActive
                                   select new RegisteredShopsDto
                                   {
                                       Id = s.Id,
                                       Name = s.Name,
                                       Address = s.Address
                                   }).ToListAsync();
                }
                string token = GenerateJwtToken(user, shops);
                response.Message = "Logged in Successfully";
                response.StatusCode = 200;
                response.Data = token;
                return response;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CommonResponseDto> GetProfile(Ulid userId, Ulid currentUserId)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.Id == userId)
                    .Select(u => new GetProfileDto()
                    {
                        Id = u.Id,
                        Email = u.Email,
                        FullName = u.FullName,
                        RoleName = ((RoleEnums)u.RoleId).ToString(),
                        RegisteredShops = _context.Shops
                            .Where(s => s.CreatedBy == u.Id && s.IsActive)
                            .Select(s => new RegisteredShopsDto()
                            {
                                Id = s.Id,
                                Name = s.Name,
                                Address = s.Address,
                            })
                            .ToList(),
                    }).AsNoTracking()
                    .FirstOrDefaultAsync();
                return new CommonResponseDto()
                {
                    Message = "",
                    Data = user,
                    StatusCode = 200,
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<CommonResponseDto> UpdateProfile(UpdateProfileDto model)
        {
            try
            {
                var user = await _context.Users.FindAsync(CommonHelper.StringToUlidConverter(model.Id!));
                if (user == null)
                {
                    return new CommonResponseDto()
                    {
                        Message = "User not found.",
                        Data = null,
                        StatusCode = 404,
                    };
                }
                user.FullName = model.FullName;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return new CommonResponseDto()
                {
                    Message = "User profile updated successfully.",
                    Data = null,
                    StatusCode = 200,
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<CommonResponseDto> GetAllUsersByShopId(long shopId)
        {
            try
            {
                var query = await (
                    from user in _context.Users
                    join userShop in _context.UserShops on user.Id equals userShop.UserId
                    where userShop.ShopId == shopId && user.IsActive && userShop.IsActive
                    select new GetProfileDto()
                    {
                        Id = user.Id,
                        FullName = user.FullName,
                        Email = user.Email,
                        RoleName = ((RoleEnums)user.RoleId).ToString()
                    }
                ).ToListAsync();
                return new CommonResponseDto()
                {
                    Data = query,
                    Message = "",
                    StatusCode = 200,
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<CommonResponseDto> GetPermissionsOfUser(Ulid userId)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.Id == userId)
                    .Select(u => new GetProfileDto()
                    {
                        Id = u.Id,
                    }).AsNoTracking()
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return new CommonResponseDto()
                    {
                        Message = "User not found.",
                        Data = null,
                        StatusCode = 404,
                    };
                }
                var data = new PermissionDto();
                data.UserId = userId;
                data.RoleName = user.RoleName;
                data.PermissionDetails = await _context.Permissions
                    .Where(p => p.UserId == userId && p.IsActive)
                    .Select(p => new PermissionDetailsDto
                    {
                        Id = p.Id,
                        ModuleId = p.ModuleId,
                        ModuleName = _context.Modules.Where(a => a.Id == p.ModuleId).Select(a => a.Name).FirstOrDefault() ?? String.Empty,
                        IsCreate = p.IsCreate,
                        IsView = p.IsView,
                        IsEdit = p.IsEdit,
                        IsList = p.IsList,
                        IsDelete = p.IsDelete,
                        IsActive = p.IsActive
                    })
                    .OrderBy(p => p.ModuleId)
                    .AsNoTracking()
                    .ToListAsync();
                return new CommonResponseDto()
                {
                    Message = "",
                    Data = data,
                    StatusCode = 200,
                };

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<CommonResponseDto> UpdatePermissionsOfUser(List<UpdatePermissionOfUserDto> model, Ulid userId)
        {
            try
            {
                List<Permission> permissionsToUpdate = new List<Permission>();
                foreach (var item in model)
                {
                    var permission =
                        await _context.Permissions.Where(a => a.Id == item.Id && a.UserId == userId && a.IsActive)
                            .FirstOrDefaultAsync();
                    if (permission == null) continue;
                    permission.IsCreate = item.IsCreate;
                    permission.IsView = item.IsView;
                    permission.IsEdit = item.IsEdit;
                    permission.IsList = item.IsList;
                    permission.IsDelete = item.IsDelete;
                    permissionsToUpdate.Add(permission);
                }

                _context.Permissions.UpdateRange(permissionsToUpdate);
                await _context.SaveChangesAsync();
                return new CommonResponseDto()
                {
                    Message = "Permissions updated successfully.",
                    Data = null,
                    StatusCode = 200,
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task SetPermissions(int roleId, Ulid userId)
        {
            try
            {
                var modules = await _context.Modules.ToListAsync();
                var permissions = modules.Select(module => new Permission()
                {
                    ModuleId = module.Id,
                    UserId = userId,
                    RoleId = roleId,
                    IsCreate = roleId == (int)RoleEnums.Admin || module.Id == 1 || module.Id == 2,
                    IsView = roleId == (int)RoleEnums.Admin || module.Id == 1 || module.Id == 2,
                    IsEdit = roleId == (int)RoleEnums.Admin || module.Id == 1 || module.Id == 2,
                    IsList = roleId == (int)RoleEnums.Admin || module.Id == 1 || module.Id == 2,
                    IsDelete = roleId == (int)RoleEnums.Admin || module.Id == 1 || module.Id == 2,
                }).ToList();

                await _context.Permissions.AddRangeAsync(permissions);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public PermissionDto GetPermissions(Ulid userId)
        {
            try
            {
                var permissions = _context.Permissions
                    .Where(p => p.UserId == userId && p.IsActive)
                    .ToList();
                var modules = _context.Modules.ToList();
                return new PermissionDto
                {
                    UserId = permissions.FirstOrDefault()?.UserId ?? Ulid.Empty,
                    RoleId = permissions.FirstOrDefault()?.RoleId ?? 0,
                    PermissionDetails = permissions.Select(p => new PermissionDetailsDto
                    {
                        Id = p.Id,
                        ModuleId = p.ModuleId,
                        ModuleName = modules.Where(a => a.Id == p.ModuleId).Select(a => a.Name).FirstOrDefault() ?? String.Empty,
                        MenuIcon = modules.Where(a => a.Id == p.ModuleId).Select(a => a.ModuleIcon).FirstOrDefault() ?? String.Empty,
                        Path = modules.Where(a => a.Id == p.ModuleId).Select(a => a.Path).FirstOrDefault() ?? String.Empty,
                        IsCreate = p.IsCreate,
                        IsView = p.IsView,
                        IsEdit = p.IsEdit,
                        IsList = p.IsList,
                        IsDelete = p.IsDelete,
                        IsActive = p.IsActive
                    }).OrderBy(p => p.ModuleId).ToList()
                };
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public CommonResponseDto GetAvailableRoles()
        {
            var data = Enum.GetValues(typeof(RoleEnums))
                .Cast<int>()
                .Where(x => x != 0 && x != 1)
                .Select(x => new KeyValuePair<int, string>(x, Enum.GetName(typeof(RoleEnums), x)))
                .ToList();
            return new CommonResponseDto()
            {
                Message = "",
                Data = data,
                StatusCode = 200,
            };
        }

        public async Task<CommonResponseDto> CreateUser(CreateUserDto model, Ulid currentUserId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var checkUserOnSameShop = await (
                    from u in _context.Users
                    join us in _context.UserShops on u.Id equals us.UserId
                    where u.Email == model.Email && us.ShopId == model.ShopId && u.IsActive && us.IsActive
                    select u.Id
                ).FirstOrDefaultAsync();

                if (checkUserOnSameShop != default)
                {
                    return new CommonResponseDto
                    {
                        Message = "User already exists in the shop.",
                        Data = null,
                        StatusCode = 400,
                    };
                }

                var user = new User
                {
                    Id = Ulid.NewUlid(),
                    FullName = model.FullName,
                    Email = model.Email,
                    PasswordHash = HashPassword("123456"),
                    RoleId = model.RoleId,
                };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                var userShop = new UserShop
                {
                    RoleId = model.RoleId,
                    UserId = user.Id,
                    ShopId = model.ShopId,
                    CreatedBy = currentUserId,
                };

                await _context.UserShops.AddAsync(userShop);
                await _context.SaveChangesAsync();

                await SetPermissions(user.RoleId, user.Id);

                await transaction.CommitAsync();
                return new CommonResponseDto
                {
                    Message = "User created successfully & default password is 123456",
                    Data = null,
                    StatusCode = 200,
                };
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                throw new Exception(e.Message);
            }
        }

        public async Task<CommonResponseDto> DeleteUser(Ulid userId, Ulid currentUserId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null)
                {
                    return new CommonResponseDto
                    {
                        Message = "User not found.",
                        Data = null,
                        StatusCode = 404,
                    };
                }
                _context.Users.Remove(user);
                var userShop = await _context.UserShops
                    .Where(us => us.UserId == userId && us.IsActive)
                    .FirstOrDefaultAsync();
                if (userShop != null)
                {
                    _context.UserShops.Remove(userShop);
                }
                var permissions = await _context.Permissions
                    .Where(p => p.UserId == userId && p.IsActive)
                    .ToListAsync();
                _context.RemoveRange(permissions);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new CommonResponseDto
                {
                    Message = "User deleted successfully.",
                    Data = null,
                    StatusCode = 200,
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string GenerateJwtToken(User user, List<RegisteredShopsDto>? shops)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JwtConfig:Key"] ?? string.Empty);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim(JwtRegisteredClaimNames.Name, user.FullName),
                        new Claim("Shops", JsonSerializer.Serialize(shops )),
                        new Claim("Role", Enum.GetName(typeof(RoleEnums), user.RoleId) ?? string.Empty),
                        new Claim("Permissions", JsonSerializer.Serialize(GetPermissions(user.Id)))
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(
                        int.Parse(_configuration["JwtConfig:DurationInMinutes"] ?? "60")),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _configuration["JwtConfig:Issuer"],
                    Audience = _configuration["JwtConfig:Audience"]
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
        private bool VerifyPassword(string enteredPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, hashedPassword);
        }


    }
}
