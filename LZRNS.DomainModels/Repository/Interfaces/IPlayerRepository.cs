﻿using LZRNS.DomainModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LZRNS.DomainModels.Repository.Interfaces
{
    public interface IPlayerRepository : IRepositoryBase<Player>
    {

        Player GetPlayerByName(string firstName, string lastName, string middleName = "");
    }
}
