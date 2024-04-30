using AspNetCore;
using AutoMapper;
using Clinical.BAL.Interfaces;
using Clinical.DAL.Entities;
using Clinical.DAL.Entities.Identity;
using Clinical.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Clinical.PL.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toast;
        private readonly IUnitWork _unitofWork;


        public DoctorController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IMapper mapper,
            IToastNotification toast, IUnitWork unitWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _toast = toast;
            _unitofWork = unitWork;

        }


        public async Task<IActionResult> Profile()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    var Doctor = _unitofWork.DoctorRepository.GetByIdWithInclude(user.Id);
                    ViewBag.doctorName = Doctor.User.UserName;
                    ViewBag.doctorEmail = Doctor.User.Email;
                    var appointment = await _unitofWork.appointmentRepository.GetAppointmentByDoctorIdAsync(user.Id);

                    var DoctorModel = _mapper.Map<List<DoctorProfileViewModel>>(appointment);

                    return View(DoctorModel);
                }
                _toast.AddAlertToastMessage("No User");
                ModelState.AddModelError(string.Empty, "No user Found");
                return RedirectToAction("SignIn", "Account");
            }
            _toast.AddAlertToastMessage("No User Please Login");
            return RedirectToAction("SignIn", "Controller");
        }

        public async Task<IActionResult> MakeChecked(int? id)
        {
            if (id == null)
                return NotFound();

            var appointment = await _unitofWork.appointmentRepository.GetAppointmentWithIncudeByIdAsync(id);
            if (appointment == null)
                return NotFound();
            var viewdata = _mapper.Map<DoctorProfileViewModel>(appointment);

            return View(viewdata);
        }

        [HttpPost]
        public async Task<IActionResult> MakeChecked(int id, DoctorProfileViewModel data)
        {

            var Appoint = await _unitofWork.appointmentRepository.GetAppointmentWithIncudeByIdAsync(id);
            if (data.IsChecked)
            {
                Appoint.IsChecked = data.IsChecked;
                _unitofWork.appointmentRepository.Update(Appoint);
                _unitofWork.Commit();
                _toast.AddSuccessToastMessage("Checked");
                return RedirectToAction(nameof(Profile));
            }
            _toast.AddInfoToastMessage("Not Ckecked");
            return RedirectToAction(nameof(Profile));


        }

        public async Task<IActionResult> ChangePassword()
        {
            return View(new ChangePassword());
        }
        [HttpPost]
		public async Task<IActionResult> ChangePassword(ChangePassword data)
		{
			if (User.Identity.IsAuthenticated)
			{
				var user = await _userManager.GetUserAsync(User);

				if (user != null)
				{
					if (await _userManager.CheckPasswordAsync(user, data.OldPassword))
					{
						user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, data.Password);
						await _userManager.UpdateAsync(user);
						_toast.AddSuccessToastMessage($"Password changed successfully!");
					}
					else
					{
						ModelState.AddModelError(string.Empty, "Incorrect current password.");
						return View(data);
					}

					return RedirectToAction("Index", "Home"); // Redirect to homepage or desired location
				}

				return BadRequest();
			}

			return RedirectToAction("SignIn", "Account");

		}
	}
}
