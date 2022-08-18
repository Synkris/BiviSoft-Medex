using System.ComponentModel.DataAnnotations;

namespace Medex.Models
{
    public class DefualtBaseModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        
    }   
}
