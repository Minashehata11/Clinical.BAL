using AutoMapper;
using Clinical.BAL.Interfaces;
using Clinical.BAL.Repository;
using Clinical.DAL.Entities;
using Clinical.DAL.Entities.Identity;
using Clinical.PL.Helper;
using Clinical.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using System.Reflection.Metadata;

namespace Clinical.PL.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly IUnitWork _unitWork;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AdminController> _logger;
        private readonly IToastNotification _toast;

        public AdminController(IUnitWork unitWork,IMapper mapper,UserManager<ApplicationUser> userManager,ILogger<AdminController> logger,IToastNotification toast)
        {
            _unitWork = unitWork;
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
            _toast = toast;
        }
        public IActionResult Index(string name="")
        {
            List<DoctorViewModel> doctorData;
            IEnumerable<Doctor> entityData;
            if (string.IsNullOrEmpty(name))
           
                entityData = _unitWork.DoctorRepository.GetAllWithInclude();
         
            else
                 entityData = _unitWork.DoctorRepository.Search(name);

            doctorData = _mapper.Map<List<DoctorViewModel>>(entityData);
            
            return View(doctorData);
        }

        [HttpGet]
        public IActionResult Create()
        { 
            return View( new DoctorCreateViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create( DoctorCreateViewModel data)
        {
            
            if(ModelState.IsValid)
            {
               var user=await _userManager.FindByEmailAsync(data.Email);
                if(user != null)
                {
                    
                    ModelState.AddModelError(string.Empty,"User Already Taken");
                    return View(data);
                }
                var applicationUser = new ApplicationUser
                {
                    Email = data.Email,
                    UserName=data.Email.Split("@")[0],
					FirstName = data.Email.Split("@")[0],

				};
                var result= await _userManager.CreateAsync(applicationUser,data.Password);
                if (result.Succeeded)
                {
                    Doctor doc = new Doctor()
                    {
                        DoctorId=applicationUser.Id,
                        Specialty=data.Specialty,
                        Image = DocumnetSetting.UploadFile(data.File, "Images")
                    };
                    _unitWork.DoctorRepository.Add(doc);
                    await _userManager.AddToRoleAsync(applicationUser, "Doctor");
                    try
                    {
                         _unitWork.Commit();
                        _toast.AddSuccessToastMessage("Doctor Has Added");
                        return RedirectToAction("Index");
                    }
                    catch (DbUpdateException ex)
                    {
                        // Handle potential database saving errors
                        ModelState.AddModelError(string.Empty, "An error occurred during registration. Please try again.");
                        // Log the exception details for further investigation
                        _logger.LogError(ex, "Error saving user and admin data");
                        return View(data);
                    }

                }
                else
                {
                    // Handle user creation errors (e.g., add error messages to ModelState)
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, item.Description);
                    }

                    return View(data);
                }

            }
            return View(data) ;
            }

        [HttpGet]
        public async Task<IActionResult> Details(string id,string ActionName="Details")
        {
            if (id == null)
                return NotFound();
            var doctor =  _unitWork.DoctorRepository.GetByIdWithInclude(id);
            if(doctor == null)
                return BadRequest();
           var viewData= _mapper.Map<DoctorDetailsViewModel>(doctor);
            return View(ActionName,viewData);
        
        }


		public async Task<IActionResult> Delete(string id)
        {
			if (id == null)
				return NotFound();
			
            try
            {
                var doctor = _unitWork.DoctorRepository.GetById(id);
                var user=  await _userManager.FindByIdAsync(id);
                _unitWork.DoctorRepository.Delete(doctor);
                if(doctor.Image != null)
                   DocumnetSetting.DeleteFile(doctor.Image, "Images");
                 await _userManager.DeleteAsync(user);
                 _unitWork.Commit();
                _toast.AddSuccessToastMessage("Doctor Deleted");
                return RedirectToAction(nameof(Index));

            }
            catch (Exception ex) { _logger.LogError(ex.Message); }
               
            return RedirectToAction(nameof(Index));

		}

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
			if (id == null)
				return NotFound();
			var doctor = _unitWork.DoctorRepository.GetByIdWithInclude(id);
			if (doctor == null)
				return BadRequest();
            DoctorEditViewModel view = new DoctorEditViewModel
            {
                Email=doctor.User.Email,
                Specialty=doctor.Specialty,
                UserName=doctor.User.UserName,
                Image=doctor.Image 
                
                
            };

            return View(view);
		}
        [HttpPost]
		public async Task<IActionResult> Edit(string id, DoctorEditViewModel data)
		{
            if (ModelState.IsValid)
            {

                //var ApplicationUser = await _userManager.FindByIdAsync(id);
                var doctor = _unitWork.DoctorRepository.GetByIdWithInclude(id);

                doctor.Specialty = data.Specialty;
                if(data.File != null)
                {
                    if (data.Image != null)
                        doctor.Image = DocumnetSetting.UpdateFile(data.File, data.Image, "Images");
                    else
                        doctor.Image = DocumnetSetting.UploadFile(data.File, "Images");


				}
                doctor.User.Email = data.Email;
				doctor.User.UserName = data.UserName;
                _unitWork.DoctorRepository.Update(doctor);
                _unitWork.Commit();
                _toast.AddAlertToastMessage("Doctor Has Updated");
                return RedirectToAction(nameof(Index));

			}
            return View(data);
		}

		[HttpGet]
		public IActionResult AllPatient(string name = "")
		{
			List<PatientViewModel> PatientData;
			IEnumerable<Patient> entityData;
			if (string.IsNullOrEmpty(name))

				entityData = _unitWork.PatientRepository.GetAllWithInclude();

			else
				entityData = _unitWork.PatientRepository.Search(name);

			PatientData = _mapper.Map<List<PatientViewModel>>(entityData);

			return View(PatientData);
		}
	}

    }

