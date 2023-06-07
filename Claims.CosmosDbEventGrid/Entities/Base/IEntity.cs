using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Claims.CosmosDbEventGrid.Entities.Base
{
    public interface IEntity<TId>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public TId Id { get; set; }
    }
}
