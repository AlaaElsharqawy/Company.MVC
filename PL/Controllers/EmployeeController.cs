using AutoMapper;
using BLL.Interfaces;
using BLL.Repositories;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PL.Helpers;
using PL.ViewModels;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PL.Controllers
{

	[Authorize]
	public class EmployeeController : Controller
    {
       
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController( IUnitOfWork unitOfWork,IMapper mapper)//ask clr for object from class implement interface IUnitOfWork
        {
            
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(string SearchValue)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchValue))
            {
                 employees =await  _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                employees = _unitOfWork.EmployeeRepository.GetEmployeesByName(SearchValue);
            }

            var MappedEmployees=_mapper.Map<IEnumerable<Employee>,IEnumerable<EmployeeViewModel>>(employees);
            return View(MappedEmployees);






            //ViewData["Message"] = "Hello";

            //ViewBag.Message = "Welcome";


          
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {

            ViewBag.Departments=await  _unitOfWork.DepartmentRepository.GetAllAsync();
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
           
            if(ModelState.IsValid)
            {
                if (employeeVM.Image is not null)
                {

                    employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");
                }

                var MappedEmployee=   _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
             await  _unitOfWork.EmployeeRepository.AddAsync(MappedEmployee);
               int Result= await _unitOfWork.CompleteAsync();
                if (Result > 0) 
                {

                    TempData["Message"] = "Employee is Added";
                }

               
              return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(employeeVM);
        }


        //await
        public async Task<IActionResult> Details(int?id,string ViewModel="Details") 
        {
            if (id == null)
                BadRequest();
            
            var employee= await _unitOfWork.EmployeeRepository.GetByIdAsync(id.Value);
           

            if (employee == null)
                return NotFound();
            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee);


            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAllAsync();

            return View(ViewModel,MappedEmployee);
        
        
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)

        {
            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAllAsync();

            return await Details(id,"Edit");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]//more security
        public async Task<IActionResult> Edit(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (id != employeeVM.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    if (employeeVM.Image is not null)
                    { 
                      
                     employeeVM.ImageName=  DocumentSettings.UploadFile(employeeVM.Image,"Images");
                    }

                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Update(MappedEmployee);
                   await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (System.Exception ex)
                {
                    // 1)Log Exception
                    //2)show Exception
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(employeeVM);

        }




        [HttpGet]
        public async  Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]//more security
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {


                try
                {
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Delete(MappedEmployee);
                    int Result = await _unitOfWork.CompleteAsync();
                    if (Result > 0 && employeeVM.ImageName is not null)
                    {
                        DocumentSettings.DeleteFile(employeeVM.ImageName, "Images");
                    }

                    return RedirectToAction(nameof(Index));

                }
                catch (System.Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(employeeVM);
            



        }




    }
}
