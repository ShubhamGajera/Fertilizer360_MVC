using Fertilizer360.Models;
using Fertilizer360.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;
using Fertilizer360.ViewModels;


namespace Fertilizer360.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        // Corrected constructor (Removed duplicate)
        public AccountController(UserManager<Users> userManager, SignInManager<Users> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // Login
        public async Task<IActionResult> Login(LoginViewModel model, string Role)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, false);

                    if (result.Succeeded)
                    {
                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles.Contains(Role))
                        {
                            if (Role == "Admin")
                                return RedirectToAction("Index", "Admin");
                            if (Role == "Shop Keeper")
                                return RedirectToAction("Index", "Shopkeeper");
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid role selection.");
                        }
                    }
                }
                ModelState.AddModelError("", "Email or password is incorrect.");
            }
            return View(model);
        }

        // Register
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Users
                {
                    FullName = model.Name,
                    Email = model.Email,
                    UserName = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Ensure role exists
                    if (!await _roleManager.RoleExistsAsync(model.Role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(model.Role));
                    }

                    // Assign role to user
                    await _userManager.AddToRoleAsync(user, model.Role);

                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(model);
        }

        // Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }






        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var model = new UpdateProfileViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            // Update Full Name
            user.FullName = model.FullName;

            // If changing password
            if (model.ChangePassword)
            {
                if (string.IsNullOrWhiteSpace(model.CurrentPassword) || string.IsNullOrWhiteSpace(model.NewPassword))
                {
                    ModelState.AddModelError("", "Both current and new password are required.");
                    return View(model);
                }

                var passwordChangeResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!passwordChangeResult.Succeeded)
                {
                    foreach (var error in passwordChangeResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction("UpdateProfile");
        }























































        // ✅ Show Users List
        public async Task<IActionResult> UsersList()
        {
            var users = _userManager.Users.ToList();
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();

            var model = users.Select(user => new UserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault()
            }).ToList();

            ViewBag.Roles = roles;
            return View(model);
        }

        // ✅ Change User Role
        [HttpPost]
        [ValidateAntiForgeryToken] // ✅ Protects against CSRF attacks
        public async Task<IActionResult> ChangeUserRole(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("UsersList");
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles); // ✅ Remove old roles

            var roleExists = await _roleManager.RoleExistsAsync(newRole);
            if (!roleExists)
            {
                TempData["Error"] = "Role does not exist.";
                return RedirectToAction("UsersList");
            }

            var result = await _userManager.AddToRoleAsync(user, newRole); // ✅ Assign new role
            if (!result.Succeeded)
            {
                TempData["Error"] = "Failed to update user role.";
                return RedirectToAction("UsersList");
            }

            TempData["Success"] = "User role updated successfully!";
            return RedirectToAction("UsersList");
        }


        // ✅ Edit User (GET)
        public async Task<IActionResult> EditUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return NotFound();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            var model = new EditUserViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email
            };

            return View(model);
        }


        // ✅ Edit User (POST)
        [HttpPost]
        [ValidateAntiForgeryToken] // ✅ Protects against CSRF attacks
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return NotFound();

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.UserName = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

            TempData["Success"] = "User updated successfully!";
            return RedirectToAction("UsersList");
        }


        // ✅ Delete User
        [HttpPost]
        [ValidateAntiForgeryToken] // ✅ Protects against CSRF attacks
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("UsersList");
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                TempData["Error"] = "Failed to delete user.";
                return RedirectToAction("UsersList");
            }

            TempData["Success"] = "User deleted successfully!";
            return RedirectToAction("UsersList");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); // Return with validation errors
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // Log this for development but don't reveal to the user
                Console.WriteLine("User not found with email: " + model.Email);
                TempData["SuccessMessage"] = "If your email exists in our system, we’ve sent you a password reset link.";
                return RedirectToAction("ForgotPassword"); // Redirect to show success message
            }

            if (!(await _userManager.IsEmailConfirmedAsync(user)))
            {
                Console.WriteLine("User email not confirmed: " + user.Email);
                TempData["SuccessMessage"] = "If your email is confirmed, we’ve sent you a password reset link.";
                return RedirectToAction("ForgotPassword"); // Redirect to show success message
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword", "Account", new
            {
                email = user.Email,
                code = Uri.EscapeDataString(token)
            }, protocol: Request.Scheme);

            try
            {
                // Send email using SMTP client
                var smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("shubhamgajera122@gmail.com", "ujjo xwmb uyia arfc"), // Use an App Password here
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("shubhamgajera122@gmail.com"),
                    Subject = "Reset Your Password",
                    Body = $"Hi {user.FullName},\n\nClick the following link to reset your password:\n\n{callbackUrl}\n\nIf you did not request a reset, please ignore this message.",
                    IsBodyHtml = false
                };

                mailMessage.To.Add(user.Email);
                await smtpClient.SendMailAsync(mailMessage);

                // Success message
                TempData["SuccessMessage"] = "A password reset link has been sent to your email.";
                return RedirectToAction("Login"); // Redirect to login page after successful email sending
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email error: " + ex.Message);
                TempData["ErrorMessage"] = "Failed to send the email. Please try again later.";
                return RedirectToAction("ForgotPassword"); // Redirect to show error message
            }
        }



        [HttpGet]
        public IActionResult ResetPassword(string email, string code)
        {
            if (email == null || code == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new ResetPasswordViewModel { Email = email, Code = code };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }
























    }
}
