using System.ComponentModel.DataAnnotations;

namespace Bibliotheques.ApplicationCore.Entites
{
    public abstract class BaseEntite
    {
        [Key]
        public virtual int Id { get; set; }
    }
}