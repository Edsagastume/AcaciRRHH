using AcaciRRHH.Web.Data;
using AcaciRRHH.Web.Models;
using AcaciRRHH.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;

namespace AcaciRRHH.Web.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;

        public UsersController(ApplicationDbContext context, IPasswordHasher<ApplicationUser> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.ApplicationUsers
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .ToListAsync();
            return View(users);
        }

        // GET: Users/Create
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create()
        {
            var allRoles = await _context.ApplicationRoles.ToListAsync();
            var viewModel = new UserCreateViewModel
            {
                Roles = allRoles.Select(role => new RoleSelection
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = false
                }).ToList()
            };
            return View(viewModel);
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Create(UserCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == viewModel.UserName);
                if (existingUser != null)
                {
                    ModelState.AddModelError("UserName", "El nombre de usuario ya está en uso.");
                }
                else
                {
                    var user = new ApplicationUser { UserName = viewModel.UserName };
                    user.PasswordHash = _passwordHasher.HashPassword(user, viewModel.Password);

                    foreach (var roleSelection in viewModel.Roles)
                    {
                        if (roleSelection.IsSelected)
                        {
                            user.UserRoles.Add(new ApplicationUserRole
                            {
                                RoleId = roleSelection.RoleId
                            });
                        }
                    }

                    _context.ApplicationUsers.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            var allRoles = await _context.ApplicationRoles.ToListAsync();
            viewModel.Roles = allRoles.Select(role => new RoleSelection
            {
                RoleId = role.Id,
                RoleName = role.Name,
                IsSelected = viewModel.Roles.Any(rs => rs.RoleId == role.Id && rs.IsSelected)
            }).ToList();
            return View(viewModel);
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "Administrador")] // Only Administrators can edit users
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var allRoles = await _context.ApplicationRoles.ToListAsync();
            var viewModel = new UserEditViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Roles = allRoles.Select(role => new RoleSelection
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = user.UserRoles.Any(ur => ur.RoleId == role.Id)
                }).ToList()
            };
            return View(viewModel);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")] // Only Administrators can edit users
        public async Task<IActionResult> Edit(int id, UserEditViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var userToUpdate = await _context.ApplicationUsers
                    .Include(u => u.UserRoles)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (userToUpdate == null)
                {
                    return NotFound();
                }

                var existingUser = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == viewModel.UserName && u.Id != viewModel.Id);
                if (existingUser != null)
                {
                    ModelState.AddModelError("UserName", "El nombre de usuario ya está en uso.");
                }
                else
                {
                    userToUpdate.UserName = viewModel.UserName;

                    if (!string.IsNullOrEmpty(viewModel.NewPassword))
                    {
                        userToUpdate.PasswordHash = _passwordHasher.HashPassword(userToUpdate, viewModel.NewPassword);
                    }

                    // Update User Roles
                    _context.ApplicationUserRoles.RemoveRange(userToUpdate.UserRoles);
                    foreach (var roleSelection in viewModel.Roles)
                    {
                        if (roleSelection.IsSelected)
                        {
                            userToUpdate.UserRoles.Add(new ApplicationUserRole
                            {
                                UserId = userToUpdate.Id,
                                RoleId = roleSelection.RoleId
                            });
                        }
                    }

                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserExists(userToUpdate.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            // If ModelState is not valid, re-populate roles and return view
            var allRoles = await _context.ApplicationRoles.ToListAsync();
            viewModel.Roles = allRoles.Select(role => new RoleSelection
            {
                RoleId = role.Id,
                RoleName = role.Name,
                IsSelected = viewModel.Roles.Any(rs => rs.RoleId == role.Id && rs.IsSelected) // Maintain selected state
            }).ToList();
            return View(viewModel);
        }

        private bool UserExists(int id)
        {
            return _context.ApplicationUsers.Any(e => e.Id == id);
        }

        // GET: Users/Block/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Block(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Block/5
        [HttpPost, ActionName("Block")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> BlockConfirmed(int id)
        {
            var user = await _context.ApplicationUsers.FindAsync(id);
            if (user != null)
            {
                user.IsBlocked = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Users/Unblock/5
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Unblock(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Unblock/5
        [HttpPost, ActionName("Unblock")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> UnblockConfirmed(int id)
        {
            var user = await _context.ApplicationUsers.FindAsync(id);
            if (user != null)
            {
                user.IsBlocked = false;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}