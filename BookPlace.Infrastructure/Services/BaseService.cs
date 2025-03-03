using AutoMapper;
using BookPlace.Infrastructure.Data;

namespace BookPlace.Infrastructure.Services
{
    public abstract class BaseService
    {
        protected readonly AppDbContext _dbContext;
        protected readonly IMapper _mapper;


        public BaseService(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
    }
}
