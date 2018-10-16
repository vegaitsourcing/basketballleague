using LZRNS.DomainModels.Repository.Interfaces;
using LZRNS.Models.DocumentTypes.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using umbraco.NodeFactory;
using Umbraco.Web.Mvc;

namespace LZRNS.Web.Controllers.RenderMvc
{
    public class TeamManagmentController : RenderMvcController
    {
		private ITeamRepository _teamRepo;
		public TeamManagmentController(ITeamRepository teamRepo)
		{
			_teamRepo = teamRepo;
		}

		public ActionResult Index(TeamManagmentModel model)
        {
            return View(_teamRepo.GetAll());
        }
	}
}