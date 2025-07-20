using Paynext.Domain.Errors;
using Paynext.Domain.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paynext.Domain.Entities._bases
{
    public abstract class EntityBase
    {
        public int Id { get; set; }
        public Guid UUID { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }


        public virtual void Validate()
        {
            //InitializeEntity(this.UUID, this.CreateAt, this.Id);

            RuleValidator.Build()
                .When(Id < 0, Error.INVALID_ID)
                .When(UUID == Guid.Empty, Error.INVALID_UUID)
                .When(Id > 0 && CreateAt == default, Error.INVALID_DATE)
                .ThrowExceptionIfExists();
        }
    }
}
