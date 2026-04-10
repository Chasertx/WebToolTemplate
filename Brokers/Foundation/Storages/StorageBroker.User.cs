using System.Linq;
using System.Threading.Tasks;
using Template.Api.Models.Foundation.User;
using Microsoft.EntityFrameworkCore;

namespace Template.Api.Brokers.Foundation.Storages
{
    /** Implementation of the book functions
    for the storage broker. 
    All functions should delegate to generic 
    functionalities found in StorageBroker.cs **/

    internal partial class StorageBroker : DbContext, IStorageBroker
    {
        public DbSet<User> Users { get; set; }

        public IQueryable<User> SelectAllUsers() =>
            SelectAll<User>();

        public async ValueTask<User?> SelectUserByIdAsync(Guid userId) =>
            await Users.FindAsync(userId);

        public async ValueTask<User?> SelectUserByEmailAsync(string email) =>
            await Users.FirstOrDefaultAsync(u => u.Email == email);

        public async ValueTask<User> InsertUserAsync(User user) =>
            await InsertAsync(user);
    }
}