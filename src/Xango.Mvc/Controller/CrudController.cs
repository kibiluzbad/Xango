using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using AutoMapper;
using Ifa.Model;
using Xango.Common.String;
using Xango.Data;
using Xango.Data.NHibernate.Filters;
using Xango.Data.Query;
using Xango.Mvc.AjaxAntiForgery;
using Xango.Mvc.ViewModel;
using Xango.Common.Extensions;

namespace Xango.Mvc.Controller
{
    public abstract class CrudController<TRepository, TEntity, TViewModel, TKey>
        : System.Web.Mvc.Controller
        where TRepository : IRepository<TEntity>
        where TEntity : Entity
        where TViewModel : ViewModelBase
    {
        protected readonly TRepository Repository;

        protected CrudController(TRepository postRepository)
        {
            Repository = postRepository;
        }

        //
        // GET: /Controller/
        [HttpGet]
        [NeedsPersistence(Order = 1)]
        public ActionResult Index([DefaultValue(10)] int itemsPerPage, [DefaultValue(1)] int pageNumber)
        {
            IPagedQuery<TEntity> query = GetSearchQuery();

            query.ItemsPerPage = itemsPerPage;
            query.PageNumber = pageNumber;

            PagedResult<TEntity> model = query.Execute();

            BeforeViewModelConvertion(model);

            var result = new PagedResultViewModel<TViewModel>(query.ItemsPerPage,
                query.PageNumber,
                model.TotalItems, 
                Mapper.Map<IEnumerable<TEntity>, IEnumerable<TViewModel>>(model.PageOfResults));

            AfterViewModelConvertion(result);

            return !HttpContext.IsAjaxRequest()
                ? (ActionResult)View(result)
                : PartialView(GetPartialViewName(typeof(TEntity)), result);
        }

        protected virtual string GetPartialViewName(Type type)
        {
            return Inflector.Pluralize(type.Name);
        }

        protected abstract IPagedQuery<TEntity> GetSearchQuery();

        public virtual void BeforeViewModelConvertion(PagedResult<TEntity> list)
        { }

        public virtual void AfterViewModelConvertion(PagedResultViewModel<TViewModel> list)
        { }

        //
        // GET: /Controller/{id}
        [HttpGet]
        [NeedsPersistence(Order = 1)]
        public ActionResult Details(TKey id)
        {
            TEntity model = GetEntityById(id);

            BeforeViewModelConvertion(model);

            var result = Mapper.Map<TEntity, TViewModel>(model);

            AfterViewModelConvertion(result);

            return View(result);
        }

        public virtual void BeforeViewModelConvertion(TEntity model)
        { }

        public virtual void AfterViewModelConvertion(TViewModel viewModel)
        { }

        protected abstract TEntity GetEntityById(TKey id);

        //
        // GET: /Controller/Create
        [HttpGet]
        [NeedsPersistence]
        public ActionResult Create()
        {
            TEntity model = CreateEmptyModel();
            return View(Mapper.Map<TEntity, TViewModel>(model));
        }

        protected virtual TEntity CreateEmptyModel()
        {
            return Activator.CreateInstance<TEntity>();
        }

        //
        // POST: /Controller/Create
        [HttpPost]
        [ValidateAntiForgeryToken(Order = 1)]
        [NeedsPersistence(Order = 2)]
        public ActionResult Create(TViewModel viewModel)
        {
            TEntity model = Mapper.Map<TViewModel, TEntity>(viewModel);

            BeforeValidateModelState(model);

            if (ModelState.IsValid)
            {
                BeforeRepositoryAdd(model);
                Repository.Add(model);
                AfterRepositoryAdd(model);
            }
            else
            {
                AfterAssertModelStateIsInvalid(model);
            }

            return RedirectToAction("Index");
        }

        public virtual void BeforeRepositoryAdd(TEntity model)
        { }

        public virtual void AfterRepositoryAdd(TEntity model)
        { }

        public virtual void BeforeValidateModelState(TEntity model)
        { }

        public virtual void AfterAssertModelStateIsInvalid(TEntity model)
        { }

        //
        // GET: /Controller/{id}/Ediat
        [HttpGet]
        [NeedsPersistence(Order = 1)]
        public ActionResult Edit(TKey id)
        {
            TEntity model = GetEntityById(id);

            return View(Mapper.Map<TEntity, TViewModel>(model));
        }

        //
        // PUT: /Controller/{id}
        [HttpPut]
        [ValidateAntiForgeryToken(Order = 1)]
        [NeedsPersistence(Order=2)]
        public ActionResult Update(TKey id, TViewModel viewModel)
        {
            UpdateModelData(id, viewModel);

            return RedirectToAction("Index");
        }

        protected abstract void UpdateModelData(TKey id, TViewModel viewModel);

        //
        // DELETE: /Controller/{id}
        [HttpDelete]
        [AjaxValidateAntiForgeryToken(Order = 1)]
        [NeedsPersistence(Order = 2)]
        public void Delete(TKey id)
        {
            TEntity model = GetEntityById(id);

            BeforeRepositoryRemove(model);
            
            Repository.Remove(model);

            AfterRepositoryRemove(model);
        }

        protected abstract TEntity LoadFormViewModel(TViewModel viewModel);

        public virtual void BeforeRepositoryRemove(TEntity model)
        { }

        public virtual void AfterRepositoryRemove(TEntity model)
        { }
    }
}