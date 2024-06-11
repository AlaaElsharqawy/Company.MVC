using BLL.Interfaces;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
      
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
           
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ////Binding=>2 Methods

            ////1) ViewData=>Dictionary Object(KeyPairsValue)
            ////transfer data from action to view
            ////.net framework 3.5
            //ViewData["Message"] = "Hello From ViewData";

            ////2)ViewBag=>Dynamic Property
            ////transfer data from action to view
            ////.net framework 4.0

            //ViewBag.Message= "Hello From ViewBag";//overload for ViewData

            return View(departments);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)//server side validation
            {
              await _unitOfWork.DepartmentRepository.AddAsync(department);//Add in DB
                int Result = await _unitOfWork.CompleteAsync();
                if (Result > 0)
                {

                    TempData["Message"] = "Department is Created";
                }
                //TempData=>Dictionary Object
                //transfer data from Action to Action

                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }





     

        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {
            if (id == null)
                return BadRequest();//status code 400

            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);

            if (department == null)
                return NotFound();
            return View(ViewName, department);

        }

        [HttpGet]
        public Task<IActionResult> Edit(int? id)
        {
            //if (id == null)
            //    return BadRequest();//status code 400
            //var department = _departmentRepository.GetById(id.Value);

            //if (department == null)
            //    return NotFound();
            //return View(department);
            return Details(id, "Edit");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]//more security
        public async Task<IActionResult> Edit(Department department, [FromRoute] int id)
        {
            if (id != department.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                  _unitOfWork.DepartmentRepository.Update(department);
                  await   _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    // 1)Log Exception
                    //2)show Exception
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(department);



        }






        [HttpGet]
        public Task<IActionResult> Delete(int? id)
        {
            return Details(id, "Delete");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]//more security
        public async Task<IActionResult> Delete(Department department, [FromRoute] int id)
        {
            if (id != department.Id)
                return BadRequest();
            try
            {
               
               _unitOfWork.DepartmentRepository.Delete(department);
              await   _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(department);
            }
            
          

        }

    }
}
