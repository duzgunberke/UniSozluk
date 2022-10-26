using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniSozluk.Common.Models.RequestModels
{
    public class UpdateUserCommand:IRequest<Guid>
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAdress { get; set; }
        public string UserName { get; set; }
    }
}
