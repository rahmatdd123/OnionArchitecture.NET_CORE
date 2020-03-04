using OnionCore.Core.IApplicationService;
using OnionCore.Core.Interfaces;
using OnionCore.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnionCore.Core.Services
{
    public class DataService : IDataService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IDataRepo dataRepo;

        public DataService(IDataRepo dataRepo,
                              IUnitOfWork unitOfWork)
        {
            this.dataRepo = dataRepo;
            this.unitOfWork = unitOfWork;
        }
        public IEnumerable<ListData> listDatas()
        {
            return dataRepo.listDatas();
        }
    }
}
