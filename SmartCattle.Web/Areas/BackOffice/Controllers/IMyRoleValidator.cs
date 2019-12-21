using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using SmartCattle.DomainClass;

namespace SmartCattle.Web.Areas.BackOffice.Controllers
{
    public interface IMyRoleValidator
    {
        Task<IdentityResult> ValidateAsync(UserRole item);
    }
}