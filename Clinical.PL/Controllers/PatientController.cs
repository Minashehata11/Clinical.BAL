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
    [Authorize(Roles = "Patient")]
    public class PatientController : Controller
	{
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toast;
        private readonly IUnitWork _unitofWork;


        public PatientController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,IMapper mapper ,
            IToastNotification toast,IUnitWork unitWork)
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
                    var patient=_unitofWork.PatientRepository.GetByIdWithInclude(user.Id);
                    ViewBag.patientName = patient.User.UserName;
                    ViewBag.patientEmail = patient.User.Email;
                    var appointment= await _unitofWork.appointmentRepository.GetAppointmentByPatientIdAsync(user.Id); 
                   
                    var patientModel=_mapper.Map<List<PatientProfileViewModel>>(appointment);

                    return View(patientModel);
                }
                _toast.AddAlertToastMessage("No User");
                ModelState.AddModelError(string.Empty, "No user Found");
                return RedirectToAction("SignIn","Account");
            }
            _toast.AddAlertToastMessage("No User Please Login");
            return RedirectToAction("SignIn", "Controller");
        }

       
    //Todo: Time reservation constrain 
        public async Task<IActionResult> Reservation()
        {
            if (User.Identity.IsAuthenticated)
            {
                
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return NotFound();
              
                    ViewBag.doctores = _unitofWork.DoctorRepository.GetAllWithInclude();
                    ViewBag.patientId = user.Id;
                    return View(new ReservationViewModel());

              
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> Reservation(ReservationViewModel data)
        {
                if (ModelState.IsValid)
                {
                if(data.DateReserve<DateTime.Now || data.DateReserve.TimeOfDay < new TimeSpan(10, 0, 0) || // Before 10:00 AM
			data.DateReserve.TimeOfDay > new TimeSpan(18, 0, 0))
                {
                    ModelState.AddModelError(string.Empty, "Not Valid Time");
					ViewBag.doctores = _unitofWork.DoctorRepository.GetAllWithInclude();
					return View(data);
                }
                    var appointemnts=_unitofWork.appointmentRepository.GetAll();  
                      
                    var reseve = _mapper.Map<Appointment>(data);
                     foreach (Appointment appointment in appointemnts)
                             {
                                if(reseve.DateReserve==appointment.DateReserve && reseve.DoctorId==appointment.DoctorId)
                                      {
                         ModelState.AddModelError(string.Empty, "Time is already Token");
                        ViewBag.doctores = _unitofWork.DoctorRepository.GetAllWithInclude();
                        return View(data);
                                      }

                             }
                    _unitofWork.appointmentRepository.Add(reseve);
                    _toast.AddSuccessToastMessage("Reservition Has Orderd");
                    _unitofWork.Commit();
                    return RedirectToAction(nameof(Profile));
                }
                ModelState.AddModelError(string.Empty, "Data not Falied");
                return View(data);
     
           
        }
    }
}
