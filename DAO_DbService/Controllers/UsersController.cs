﻿using DAO_DbService.Contexts;
using DAO_DbService.Models;
using Helpers.Models.DtoModels.MainDbDto;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAO_DbService.Mapping;
using static DAO_DbService.Mapping.AutoMapperBase;
using Helpers.Models.SharedModels;
using PagedList;
using Microsoft.EntityFrameworkCore;
using static Helpers.Constants.Enums;

namespace DAO_DbService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        [Route("Get")]
        [HttpGet]
        public IEnumerable<UserDto> Get()
        {
            List<User> model = new List<User>();

            try
            {
                using (dao_maindb_context db = new dao_maindb_context())
                {
                    model = db.Users.ToList();
                }
            }
            catch (Exception ex)
            {
                model = new List<User>();
                Program.monitizer.AddException(ex, LogTypes.ApplicationError, true);
            }

            return _mapper.Map<List<User>, List<UserDto>>(model).ToArray();
        }

        [Route("UserSearch")]
        [HttpGet]
        public IEnumerable<UserDto> UserSearch(string query)
        {
            List<User> res = new List<User>();

            try
            {
                using (dao_maindb_context db = new dao_maindb_context())
                {
                    res = db.Users.Where(x => x.NameSurname.Contains(query) || x.Email.Contains(query)).ToList();
                }

            }
            catch (Exception ex)
            {
                res = new List<User>();
                Program.monitizer.AddException(ex, LogTypes.ApplicationError, true);
            }
            return _mapper.Map<List<User>, List<UserDto>>(res).ToArray();
        }

        [Route("GetId")]
        [HttpGet]
        public UserDto GetId(int id)
        {
            User model = new User();

            try
            {
                using (dao_maindb_context db = new dao_maindb_context())
                {
                    model = db.Users.Find(id);
                }
            }
            catch (Exception ex)
            {
                model = new User();
                Program.monitizer.AddException(ex, LogTypes.ApplicationError, true);
            }

            return _mapper.Map<User, UserDto>(model);
        }

        [Route("Post")]
        [HttpPost]
        public UserDto Post([FromBody] UserDto model)
        {
            try
            {
                User item = _mapper.Map<UserDto, User>(model);
                using (dao_maindb_context db = new dao_maindb_context())
                {
                    db.Users.Add(item);
                    db.SaveChanges();
                }
                return _mapper.Map<User, UserDto>(item);
            }
            catch (Exception ex)
            {
                Program.monitizer.AddException(ex, LogTypes.ApplicationError, true);
                return new UserDto();
            }
        }

        [Route("PostMultiple")]
        [HttpPost]
        public List<UserDto> PostMultiple([FromBody] List<UserDto> model)
        {
            try
            {
                List<User> item = _mapper.Map<List<UserDto>, List<User>>(model);
                using (dao_maindb_context db = new dao_maindb_context())
                {
                    db.Users.AddRange(item);
                    db.SaveChanges();
                }
                return _mapper.Map<List<User>, List<UserDto>>(item);
            }
            catch (Exception ex)
            {
                Program.monitizer.AddException(ex, LogTypes.ApplicationError, true);
                return new List<UserDto>();
            }
        }

        [Route("Delete")]
        [HttpDelete]
        public bool Delete(int? ID)
        {
            try
            {
                using (dao_maindb_context db = new dao_maindb_context())
                {
                    User item = db.Users.FirstOrDefault(s => s.UserId == ID);
                    db.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                    db.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                Program.monitizer.AddException(ex, LogTypes.ApplicationError, true);
                return false;
            }
        }

        [Route("Update")]
        [HttpPut]
        public UserDto Update([FromBody] UserDto model)
        {
            try
            {
                User item = _mapper.Map<UserDto, User>(model);
                using (dao_maindb_context db = new dao_maindb_context())
                {
                    db.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                }
                return _mapper.Map<User, UserDto>(item);
            }
            catch (Exception ex)
            {
                Program.monitizer.AddException(ex, LogTypes.ApplicationError, true);
                return new UserDto();
            }
        }

        [Route("GetPaged")]
        [HttpGet]
        public PaginationEntity<UserDto> GetPaged(int page = 1, int pageCount = 30)
        {
            PaginationEntity<UserDto> res = new PaginationEntity<UserDto>();

            try
            {
                using (dao_maindb_context db = new dao_maindb_context())
                {

                    IPagedList<UserDto> lst = AutoMapperBase.ToMappedPagedList<User, UserDto>(db.Users.OrderByDescending(x => x.UserId).ToPagedList(page, pageCount));

                    res.Items = lst;
                    res.MetaData = new PaginationMetaData() { Count = lst.Count, FirstItemOnPage = lst.FirstItemOnPage, HasNextPage = lst.HasNextPage, HasPreviousPage = lst.HasPreviousPage, IsFirstPage = lst.IsFirstPage, IsLastPage = lst.IsLastPage, LastItemOnPage = lst.LastItemOnPage, PageCount = lst.PageCount, PageNumber = lst.PageNumber, PageSize = lst.PageSize, TotalItemCount = lst.TotalItemCount };



                    return res;
                }
            }
            catch (Exception ex)
            {
                Program.monitizer.AddException(ex, LogTypes.ApplicationError, true);
            }

            return res;
        }

        [Route("GetByEmail")]
        [HttpGet]
        public UserDto GetByEmail(string email)
        {
            UserDto model = new UserDto();
            try
            {
                using (dao_maindb_context db = new dao_maindb_context())
                {
                    model = _mapper.Map<User, UserDto>(db.Users.FirstOrDefault(x => x.Email == email));
                }
            }
            catch (Exception ex)
            {
                model = new UserDto();
                Program.monitizer.AddException(ex, LogTypes.ApplicationError, true);
            }
            return model;
        }
      
        [Route("Search")]
        [HttpGet]
        public PaginationEntity<UserDto> Search(string query, int page = 1, int pageCount = 30)
        {
            PaginationEntity<UserDto> res = new PaginationEntity<UserDto>();

            try
            {
                using (dao_maindb_context db = new dao_maindb_context())
                {
                    IPagedList<UserDto> lst = AutoMapperBase.ToMappedPagedList<User, UserDto>(db.Users.Where(x => x.Email.Contains(query) || x.NameSurname.Contains(query)).ToPagedList(page, pageCount));

                    res.Items = lst;
                    res.MetaData = new PaginationMetaData() { Count = lst.Count, FirstItemOnPage = lst.FirstItemOnPage, HasNextPage = lst.HasNextPage, HasPreviousPage = lst.HasPreviousPage, IsFirstPage = lst.IsFirstPage, IsLastPage = lst.IsLastPage, LastItemOnPage = lst.LastItemOnPage, PageCount = lst.PageCount, PageNumber = lst.PageNumber, PageSize = lst.PageSize, TotalItemCount = lst.TotalItemCount };
                }

                return res;
            }
            catch (Exception ex)
            {
                Program.monitizer.AddException(ex, LogTypes.ApplicationError, true);
                return res;
            }

        }

        
    }
}
