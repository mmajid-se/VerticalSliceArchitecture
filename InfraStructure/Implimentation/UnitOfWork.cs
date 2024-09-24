using MeesageService.Data.Interfaces;
using MeesageService.InfraStructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeesageService.InfraStructure.Implimentation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CommitAsync()
        {
            try
            {
                return await _context.SaveChangesAsync().ConfigureAwait(false) > 0;
            }
            catch (Exception ex)
            {
                // log it for analysis
                throw new Exception("An error occured.");
            }
        }
    }
}
